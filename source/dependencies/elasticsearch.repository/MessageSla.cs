using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespokse.Sph.ElasticsearchRepository
{
    public class MessageSla : IMessageDeliverySla
    {
        public bool IsEnabled { get; }
        public string ElasticsearchHost { get; }
        private readonly HttpClient m_client;

        public MessageSla(bool isEnabled, string elasticsearchHost)
        {
            IsEnabled = isEnabled;
            ElasticsearchHost = elasticsearchHost;
            if (null == m_client)
                m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };
        }
        public async Task RegisterAcceptanceAsync(SlaEvent eventData)
        {
            if (!this.IsEnabled) return;
            eventData.Event = "Register";
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(eventData, setting);

            var host = this.ElasticsearchHost ?? ConfigurationManager.ElasticSearchHost;
            var content = new StringContent(json);
            var index = ConfigurationManager.ElasticSearchIndex;
            var url = $"{host}/{index}_sla/event/{eventData.MessageId}";

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

        public Task RegisterStartProcessingAsync(SlaEvent eventData)
        {
            throw new NotImplementedException();
        }

        public Task RegisterCompletedAsync(SlaEvent eventData)
        {
            throw new NotImplementedException();
        }

        public Task RegisterDlqedAsync(SlaEvent eventData)
        {
            throw new NotImplementedException();
        }

        public Task RegisterRetriedAsync(SlaEvent eventData)
        {
            throw new NotImplementedException();
        }

        public Task RegisterDelayedAsync(SlaEvent eventData)
        {
            throw new NotImplementedException();
        }
    }
}
