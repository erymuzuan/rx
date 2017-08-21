using System;
using System.Diagnostics;
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
        public string[] EnabledQueues { get; set; }

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
            if (!this.IsEnabled) return;
            if (!IsSystemTypeEnabled && eventData.EntityNamespace == typeof(EntityDefinition).Namespace) return;
            if (!IsSystemTypeEnabled && eventData.EntityNamespace == typeof(Adapter).Namespace) return;

            if (null != this.EnabledEntities)
                if (!EnabledEntities.Contains(eventData.Entity)) return;
            if (null != this.EnabledQueues)
                if (!EnabledQueues.Contains(eventData.Worker)) return;

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

        private bool m_initialized;
        private async Task InitializeAsync()
        {
            if (m_initialized) return;
            m_initialized = true;

            var content = new StringContent(Properties.Resources.MessageTrackingMapping);
            var response = await m_client.PutAsync($"{Index}/_mapping/event", content);
            response.EnsureSuccessStatusCode();

        }
    }
}
