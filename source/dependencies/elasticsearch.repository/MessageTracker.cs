using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespokse.Sph.ElasticsearchRepository
{
    public class MessageTracker : IMessageTracker
    {
        public bool IsEnabled { get; }
        public string Host { get; }
        private readonly HttpClient m_client;
        public string Index = $"{ConfigurationManager.ElasticSearchIndex}_sla";
        public bool IsSystemTypeEnabled { get; set; } = false;
        public string[] EnabledEntities { get; set; }
        public string[] EnabledQueues { get; private set; }
        private WorkersConfig Options { get; set; }
        public string[] OnceAcceptedSlaEnabledEntities { get; private set; } = Array.Empty<string>();
        public string[] OncePersistedSlaEnabledEntities { get; private set; } = Array.Empty<string>();

        /// <summary>
        /// $"{Host}/{Index}/event"
        /// </summary>
        private string TypeUrl => $"{Host}/{Index}/event";

        public MessageTracker()
        {
            IsEnabled = ConfigurationManager.GetEnvironmentVariableBoolean("ElasticsearchMessageTrackingIsEnabled");
            Host = ConfigurationManager.GetEnvironmentVariable("ElasticsearchMessageTrackingHost") ?? ConfigurationManager.ElasticSearchHost;
            m_client = new HttpClient { BaseAddress = new Uri(Host) };
        }
        public MessageTracker(bool isEnabled, string host)
        {
            IsEnabled = isEnabled;
            Host = host;
            m_client = new HttpClient { BaseAddress = new Uri(host) };
        }


        public async Task RegisterAcceptanceAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "Accepted";
            await PostEventAsync(eventData);

            var should = this.OnceAcceptedSlaEnabledEntities.Contains(eventData.Entity);
            if (should)
            {
                await this.InitializeMessageSlaAsyc("Accepted", eventData);
            }
        }

        private async Task InitializeMessageSlaAsyc(string eventName, MessageTrackingEvent data)
        {
            var contents = (from o in this.Options.SubscriberConfigs
                            where o.ShouldProcessedOnceAccepted > 0
                            && o.Entity == data.Entity
                            select new
                            {
                                @event = eventName,
                                itemId = data.ItemId,
                                entity = data.Entity,
                                dateTime = data.DateTime,
                                messageId = data.MessageId,
                                expectedProcessed = data.DateTime.AddMilliseconds(o.ShouldProcessedOnceAccepted.Value),
                                timespan = o.ShouldProcessedOnceAccepted.Value,
                                worker = o.QueueName

                            }).ToArray();
            var tasks = from c in contents
                        select m_client.PostAsync($"{Index}/sla/", new StringContent(c.ToJson()));
            var responses = await Task.WhenAll(tasks);
            responses.ToList().ForEach(x => x.EnsureSuccessStatusCode());

            //
            var slaManager = ObjectBuilder.GetObject<IMessageSlaManager>();
            var monitorTasks = from c in contents
                               let @event = new MessageSlaEvent
                               {
                                   DateTime = c.dateTime,
                                   Entity = c.entity,
                                   Event = c.@event,
                                   ItemId = c.itemId,
                                   MessageId = c.messageId,
                                   ProcessingTimeSpanInMiliseconds = c.timespan,
                                   Worker = c.worker
                               }
                               select slaManager.PublishSlaOnAcceptanceAsync(@event);
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
             }},
             {{
                 ""term"": {{
                    ""Event"": {{
                       ""value"": ""ProcessingCompleted""
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
   ]
}}
";
            var request = new HttpRequestMessage(HttpMethod.Post, $"{TypeUrl}/_search") { Content = new StringContent(query) };
            var response = await m_client.SendAsync(request);

            var content = response.Content as StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es ");
            var responseString = await content.ReadAsStringAsync();

            var esJson = JObject.Parse(responseString);
            var total = esJson.SelectToken("$.hits.total").Value<int>();
            if (total == 0) return MessageTrackingStatus.NotStarted;

            var trackings = esJson.SelectTokens("$._source")
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
            await this.InitializeAsync();

            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(eventData, setting);

            var content = new StringContent(json);
            var url = $"{TypeUrl}";

            HttpResponseMessage response = null;
            try
            {
                response = await m_client.PostAsync(url, content);
            }
            catch (HttpRequestException)
            {
            }

            if (null != response)
            {
                Debug.Write(".");
            }
        }

        private bool CanTrack(MessageTrackingEvent eventData)
        {
            if (!this.IsEnabled) return false;
            if (!IsSystemTypeEnabled && eventData.EntityNamespace == typeof(EntityDefinition).Namespace) return false;
            if (!IsSystemTypeEnabled && eventData.EntityNamespace == typeof(Adapter).Namespace) return false;

            if (null != this.EnabledEntities)
                if (!EnabledEntities.Contains(eventData.Entity)) return false;
            if (null != this.EnabledQueues)
                if (!EnabledQueues.Contains(eventData.Worker)) return false;
            return true;
        }

        private bool m_initialized;
        private async Task InitializeAsync()
        {
            if (m_initialized) return;

            // get enabled queues
            var configFile = GetConfigFile();
            if (!File.Exists(configFile))
            {
                Console.WriteLine($@"Cannot find subscribers config in '{configFile}'");
                return;
            }
            this.Options = configFile.DeserializeFromJsonFile<WorkersConfig>();
            var enabledQueues = Options.SubscriberConfigs.Where(x => x.TrackingEnabled ?? false)
                .Select(x => x.QueueName)
                .ToArray();
            this.EnabledQueues = enabledQueues;

            var onceAcceptedSlaEnabledEntities = Options.SubscriberConfigs
                .Where(x => x.TrackingEnabled ?? false)
                .Where(x => x.ShouldProcessedOnceAccepted > 0)
                .Where(x => !string.IsNullOrWhiteSpace(x.Entity))
                .Select(x => x.Entity);
            this.OnceAcceptedSlaEnabledEntities = onceAcceptedSlaEnabledEntities.ToArray();

            var oncePersistedSlaEnabledEntities = Options.SubscriberConfigs
                .Where(x => x.TrackingEnabled ?? false)
                .Where(x => x.ShouldProcessedOncePersisted > 0)
                .Where(x => !string.IsNullOrWhiteSpace(x.Entity))
                .Select(x => x.Entity);
            this.OncePersistedSlaEnabledEntities = oncePersistedSlaEnabledEntities.ToArray();

            // create index if not exist
            await m_client.PutAsync(Index, new StringContent(""));

            // create es mapping
            var content = new StringContent(Properties.Resources.MessageTrackingMapping);
            var response = await m_client.PutAsync($"{Index}/_mapping/event", content);
            response.EnsureSuccessStatusCode();

            // create es mapping
            var slaMapping = new StringContent(Properties.Resources.SlaMapping);
            var slaResponse = await m_client.PutAsync($"{Index}/_mapping/sla", slaMapping);
            slaResponse.EnsureSuccessStatusCode();


            m_initialized = true;

        }

        private string GetConfigFile()
        {
            var envName = ConfigurationManager.GetEnvironmentVariable("Environment") ?? ParseArg("env") ?? "dev";
            var configName = ConfigurationManager.AppSettings["sph:WorkersConfig"] ?? ParseArg("config") ?? "all";
            var configFile = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}\\{envName}.{configName}.json";
            if (!File.Exists(configFile))
                configFile = ConfigurationManager.GetEnvironmentVariable("MessageTrackerWorkersConfig");
            return configFile;
        }


        public static string ParseArg(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            return val?.Replace("/" + name + ":", string.Empty);
        }
    }
}
