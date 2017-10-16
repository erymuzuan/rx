using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

namespace Bespokse.Sph.ElasticsearchRepository
{
    public class MessageTracker : IMessageTracker
    {
        public bool IsEnabled { get; }
        public string Host { get; }
        private readonly HttpClient m_client;
        private string LoweredApp => ConfigurationManager.ApplicationName.ToLowerInvariant();
        public string DailyIndex => $"{LoweredApp}_sla_{DateTime.Today:yyyyMMdd}";
        public string IndexAlias => $"{LoweredApp}_sla";
        public int HttpRequestRetryCount { get; set; } = 3;
        public int HttpRequestWaitTime { get; set; } = 100;
        public WaitAlgorithm HttpRequestWaitAlgorithm { get; set; } = WaitAlgorithm.Exponential;

        public bool IsSystemTypeEnabled { get; set; } = false;
        private string[] m_trackableEntities;
        private Trigger[] m_enabledTriggers;


        /// <summary>
        /// $"{Host}/{Index}/event"
        /// </summary>
        private string DailyTypeUrl => $"{DailyIndex}/event";

        public MessageTracker()
        {
            IsEnabled = ConfigurationManager.GetEnvironmentVariableBoolean("ElasticsearchMessageTrackingIsEnabled");
            Host = ConfigurationManager.GetEnvironmentVariable("ElasticsearchMessageTrackingHost") ??
                   ConfigurationManager.ElasticSearchHost;
            m_client = new HttpClient {BaseAddress = new Uri(Host)};
        }

        public MessageTracker(bool isEnabled, string host)
        {
            IsEnabled = isEnabled;
            Host = host;
            m_client = new HttpClient {BaseAddress = new Uri(host)};
        }


        public async Task RegisterAcceptanceAsync(MessageTrackingEvent @event)
        {
            ObjectBuilder.GetObject<ILogger>().WriteVerbose($"Accepted {@event.Entity}{{{@event.ItemId}}}");
            @event.Event = "Accepted";
            await PostEventAsync(@event);

            var should = m_trackableEntities.Contains(@event.Entity);
            if (should)
            {
                await this.InitializeMessageSlaAsyc("Accepted", @event);
            }
        }

        private async Task InitializeMessageSlaAsyc(string eventName, MessageTrackingEvent data)
        {
            var events = (from trg in m_enabledTriggers
                where trg.ShouldProcessedOnceAccepted > 0
                      && trg.Entity == data.Entity
                select new MessageSlaEvent
                {
                    Event = eventName,
                    ItemId = data.ItemId,
                    Entity = data.Entity,
                    DateTime = data.DateTime,
                    MessageId = data.MessageId,
                    ProcessingTimeSpanInMiliseconds = trg.ShouldProcessedOnceAccepted.Value,
                    Worker = $"trigger_subs_{trg.Id}"
                }).ToArray();
            //
            var slaManager = ObjectBuilder.GetObject<IMessageSlaManager>();
            var monitorTasks = events.Select(c => slaManager.PublishSlaOnAcceptanceAsync(c));
            await Task.WhenAll(monitorTasks);
        }

        public async Task RegisterSendingToWorkerAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "SendingToWorker";
            await PostEventAsync(eventData);
        }

        public async Task RegisterStartProcessingAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "StartProcessing";
            await PostEventAsync(eventData);
        }

        public async Task RegisterCompletedAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "ProcessingCompleted";
            await PostEventAsync(eventData);
        }

        public async Task RegisterCancelledAsync(MessageTrackingEvent @event)
        {
            @event.Event = "Cancelled";
            await PostEventAsync(@event);
        }

        public async Task RegisterCancelRequestedAsync(MessageTrackingEvent @event)
        {
            @event.Event = "CancelRequested";
            await PostEventAsync(@event);
        }

        public async Task RegisterDlqedAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "DLQ";
            await PostEventAsync(eventData);
        }

        public async Task RegisterRetriedAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "Retried";
            await PostEventAsync(eventData);
        }

        public async Task RegisterDelayedAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "Delayed";
            await PostEventAsync(eventData);
        }

        public async Task<MessageTrackingStatus> GetProcessStatusAsync(MessageSlaEvent @event)
        {
            var query = $@"
{{
   ""query"": {{
      ""bool"": {{
         ""must"": [
             {{
                 ""term"": {{
                    ""MessageId"": {{
                       ""value"": ""{@event.MessageId}""
                    }}
                 }}
             }},
             {{
                 ""term"": {{
                    ""Worker"": {{
                       ""value"": ""{@event.Worker}""
                    }}
                 }}
             }}
         
         ]
      }}
   }},
   ""sort"": [
      {{
         ""DateTime"": {{
            ""order"": ""desc""
         }}
      }}
   ],
   ""size"":50
}}
";
            var request = new HttpRequestMessage(HttpMethod.Post, $"{IndexAlias}/_search") {Content = new StringContent(query)};
            var response = await m_client.SendAsync(request);

            if (!(response.Content is StreamContent content)) throw new Exception("Cannot execute query on es ");
            var responseString = await content.ReadAsStringAsync();

            var esJson = JObject.Parse(responseString);
            var total = esJson.SelectToken("$.hits.total").Value<int>();
            if (total == 0) return MessageTrackingStatus.NotStarted;

            var trackings = esJson.SelectToken("$.hits.hits")
                .Select(x => x.SelectToken("$._source"))
                .Select(x => x.ToString().DeserializeFromJson<MessageTrackingEvent>())
                .ToArray();


            if (trackings.Any(x => x.Event == "ProcessingCompleted"))
                return MessageTrackingStatus.Started | MessageTrackingStatus.Completed;

            if (trackings.Any(x => x.Event == "StartProcessing") && trackings.Length == 1)
                return MessageTrackingStatus.Started;

            if (trackings.Any(x => x.Event == "StartProcessing") && trackings.Length > 1)
                return MessageTrackingStatus.Started | MessageTrackingStatus.Error;

            return MessageTrackingStatus.Terminated;
        }


        private async Task PostEventAsync(MessageTrackingEvent eventData)
        {
            if (!CanTrack(eventData)) return;

            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(eventData, setting);

            var content = new StringContent(json);
            var url = $"{DailyTypeUrl}";

            TimeSpan Wait(int c)
            {
                switch (HttpRequestWaitAlgorithm)
                {
                    case WaitAlgorithm.Linear:
                        return TimeSpan.FromMilliseconds(this.HttpRequestWaitTime * c);
                    case WaitAlgorithm.Exponential:
                        return TimeSpan.FromMilliseconds(this.HttpRequestWaitTime * Math.Pow(2, c));
                    case WaitAlgorithm.Constant:
                        return TimeSpan.FromMilliseconds(this.HttpRequestWaitTime);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(this.HttpRequestRetryCount, Wait)
                .ExecuteAsync(async () =>
                {
                    var response = await m_client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                });
        }

        private bool CanTrack(MessageTrackingEvent @event)
        {
            if (!this.IsEnabled) return false;
            if (!IsSystemTypeEnabled && @event.EntityNamespace == typeof(EntityDefinition).Namespace) return false;
            if (!IsSystemTypeEnabled && @event.EntityNamespace == typeof(Adapter).Namespace) return false;

            if (!m_trackableEntities.Contains(@event.Entity)) return false;

            var workerIsTracked = m_enabledTriggers.Any(x => $"trigger_subs_{x.Id}" == @event.Worker);
            if (!string.IsNullOrWhiteSpace(@event.Worker))
                return workerIsTracked;

            return true;
        }

        public void Initialize()
        {
            this.InitializeAsync().Wait(9000);
        }

        public async Task InitializeAsync()
        {
            var logger = new ConsoleLogger {TraceSwitch = Severity.Debug};
            // NOTE : do not use SphDataContext since it required the calls to RegistryContext, this is initialization code
            m_enabledTriggers = Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\Trigger", "*.json")
                .Select(x => x.DeserializeFromJsonFile<Trigger>())
                .Where(x => x.EnableTracking)
                .ToArray();

            m_trackableEntities = Directory
                .GetFiles($"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition", "*.json")
                .Select(x => x.DeserializeFromJsonFile<EntityDefinition>())
                .Where(x => x.EnableTracking)
                .Select(x => x.Name)
                .ToArray();
            logger.WriteInfo($"Tracking entities : {m_trackableEntities.ToString(", ")}");
            logger.WriteInfo($"Tracking triggers : {m_enabledTriggers.ToString(", ")}");

            const string TEMPLATE_URI = "_template/rx_sla";
            var templateStatus = await m_client.GetAsync(TEMPLATE_URI);
            if (templateStatus.IsSuccessStatusCode) return;

            var template = $@"{{
    ""template"" : ""{LoweredApp}_sla_*"",
    ""aliases"":{{
        ""{LoweredApp}_sla"":{{}}
    }},
    ""mappings"" : {{
        {Properties.Resources.CancelledMessageMapping},
        {Properties.Resources.MessageTrackingMapping}

    }}
}}";
            await m_client.PutAsync(TEMPLATE_URI, new StringContent(template));
        }
    }
}