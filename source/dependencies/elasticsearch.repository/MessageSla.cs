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
        private readonly HttpClient m_client;

        public MessageSla()
        {
            if (null == m_client)
                m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };
        }
        public async Task RegisterAcceptanceAsync(SlaEvent eventData)
        {
            eventData.Event = "Register";
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(eventData, setting);

            var content = new StringContent(json);
            var index = ConfigurationManager.ElasticSearchIndex;
            var url = $"{ConfigurationManager.ElasticSearchHost}/{index}_sla/event/{eventData.MessageId}";


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
