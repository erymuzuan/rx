using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json;

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
            // TODO : create mesage with delay queue for message monitoring
            var contents = from o in this.Options.SubscriberConfigs
                           where o.ShouldProcessedOnceAccepted > 0
                           && o.Entity == data.Entity
                           select new StringContent((new
                           {
                               @event = eventName,
                               itemId = data.ItemId,
                               entity = data.Entity,
                               dateTime = data.DateTime,
                               messageId = data.MessageId,
                               expectedProcessed = data.DateTime.AddMilliseconds(o.ShouldProcessedOnceAccepted.Value),
                               worker = o.QueueName

                           }).ToJson());
            var tasks = from c in contents
                        select m_client.PostAsync($"{Index}/sla/", c);
            var responses = await Task.WhenAll(tasks);
            responses.ToList().ForEach(x => x.EnsureSuccessStatusCode());
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
            m_initialized = true;

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

            // create es mapping
            var content = new StringContent(Properties.Resources.MessageTrackingMapping);
            var response = await m_client.PutAsync($"{Index}/_mapping/event", content);
            response.EnsureSuccessStatusCode();

            // create es mapping
            var slaMapping = new StringContent(Properties.Resources.SlaMapping);
            var slaResponse = await m_client.PutAsync($"{Index}/_mapping/sla", slaMapping);
            slaResponse.EnsureSuccessStatusCode();

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
