using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Polly;

namespace Bespokse.Sph.ElasticsearchRepository
{
    public class CancelledMessageRepository : ICancelledMessageRepository
    {
        public string Host { get; }
        private readonly HttpClient m_client;
        public string DailyIndex = $"{ConfigurationManager.ApplicationName.ToLowerInvariant()}_sla_{DateTime.Today:yyyyMMdd}";
        public bool Profiled { get; set; }

        /// <summary>
        /// $"{Host}/{Index}/event"
        /// </summary>
        private string DailyTypeUri => $"{DailyIndex}/cancelledmessage";
        private string AliasTypeUri => $"{ConfigurationManager.ApplicationName.ToLowerInvariant()}_sla/cancelledmessage";

        public CancelledMessageRepository()
        {
            Host = ConfigurationManager.GetEnvironmentVariable("ElasticsearchMessageTrackingHost") ?? ConfigurationManager.ElasticSearchHost;
            m_client = new HttpClient { BaseAddress = new Uri(Host) };
        }

        public async Task<bool> CheckMessageAsync(string messageId, string worker)
        {
            var id = $"{messageId}{worker}".Replace("/", "");
            var response = await m_client.GetAsync($"{AliasTypeUri}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task PutAsync(string messageId, string worker)
        {

            var id = $"{messageId}{worker}".Replace("/", "");
            var json = $@"{{
        ""messageId"": ""{messageId}"",
        ""worker"": ""{worker}""
}}";
            var task = m_client.PostAsync($"{DailyTypeUri}/{id}", new StringContent(json));
            await Policy.Handle<Exception>()
               .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(100 * Math.Pow(2, c)))
               .ExecuteAsync(async () =>
               {
                   var r = await task;
                   r.EnsureSuccessStatusCode();
               });
        }

        public async Task RemoveAsync(string messageId, string worker)
        {
            var id = $"{messageId}{worker}".Replace("/", "");
            var task = m_client.DeleteAsync($"{AliasTypeUri}/{id}");

            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(100 * Math.Pow(2, c)))
                .ExecuteAsync(async () =>
                {
                    var r = await task;
                    r.EnsureSuccessStatusCode();
                });
        }


        private bool m_initialized;
        public async Task InitializeAsync()
        {
            if (m_initialized) return;

            // create index if not exist
            await m_client.PutAsync(DailyIndex, new StringContent(""));

            // create es mapping
            var content = new StringContent(Properties.Resources.CancelledMessageMapping);
            var response = await m_client.PutAsync($"{DailyIndex}/_mapping/cancelledmessage", content);
            response.EnsureSuccessStatusCode();

            m_initialized = true;

        }
    }
}
