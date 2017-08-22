using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespokse.Sph.ElasticsearchRepository
{
    public class CancelledMessageRepository : ICancelledMessageRepository
    {
        public string Host { get; }
        private readonly HttpClient m_client;
        public string Index = $"{ConfigurationManager.ElasticSearchIndex}_sla";
        public bool Profiled { get; set; }

        /// <summary>
        /// $"{Host}/{Index}/event"
        /// </summary>
        private string TypeUrl => $"{Host}/{Index}/cancelledmessage";

        public CancelledMessageRepository()
        {
            Host = ConfigurationManager.GetEnvironmentVariable("ElasticsearchMessageTrackingHost") ?? ConfigurationManager.ElasticSearchHost;
            m_client = new HttpClient { BaseAddress = new Uri(Host) };
        }

        public async Task<bool> CheckMessageAsync(string messageId, string worker)
        {
            var id = $"{messageId}{worker}".Replace("/", "");
            var response = await m_client.GetAsync($"{TypeUrl}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task PutAsync(string messageId, string worker)
        {

            var id = $"{messageId}{worker}".Replace("/", "");
            var json = $@"{{
        ""messageId"": ""{messageId}"",
        ""worker"": ""{worker}""
}}";
            var response = await m_client.PostAsync($"{TypeUrl}/{id}", new StringContent(json));
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveAsync(string messageId, string worker)
        {
            var id = $"{messageId}{worker}".Replace("/", "");
            var response = await m_client.DeleteAsync($"{TypeUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }


        private bool m_initialized;
        public async Task InitializeAsync()
        {
            if (m_initialized) return;

            // create index if not exist
            await m_client.PutAsync(Index, new StringContent(""));

            // create es mapping
            var content = new StringContent(Properties.Resources.CancelledMessageMapping);
            var response = await m_client.PutAsync($"{Index}/_mapping/cancelledmessage", content);
            response.EnsureSuccessStatusCode();

            m_initialized = true;

        }
    }
}
