using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Polly;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class ReadOnlyRepositoryRepositorySyncManager : IReadOnlyRepositorySyncManager
    {

        private readonly HttpClient m_client;
        public ReadOnlyRepositoryRepositorySyncManager(string host)
        {
            m_client = new HttpClient { BaseAddress = new Uri(host) };

        }

        public ReadOnlyRepositoryRepositorySyncManager() : this(EsConfigurationManager.Host)
        {

        }
        public Task AddAsync(Entity item)
        {
            return SyncRepositoryAsync(item, "Add");
        }

        public Task DeleteAsync(Entity item)
        {
            return SyncRepositoryAsync(item, "Delete");
        }

        public Task UpdateAsync(Entity item)
        {
            return SyncRepositoryAsync(item, "Update");
        }

        public async Task BulkInsertAsync(Entity[] entities)
        {
            var retry = ConfigurationManager.GetEnvironmentVariableInt32("ElasticsearchRetry", 5);
            var wait = ConfigurationManager.GetEnvironmentVariableInt32("ElasticsearchWaitOnRetry", 1000);


            var items = entities.Select(x => new
            {
                x.Id,
                Type = x.GetType().Name.ToLowerInvariant(),
                JsonPayload = JsonConvert.SerializeObject(x)
            })
                .ToArray();

            var tasks = from item in items
                        let url = $"{EsConfigurationManager.Index}/{item.Type}/{item.Id}"
                        let content = new StringContent(item.JsonPayload)
                        select Policy.Handle<Exception>()
                            .WaitAndRetryAsync(retry, t => TimeSpan.FromMilliseconds(wait * t))
                            .ExecuteAsync(() => m_client.PutAsync(url, content));
            await Task.WhenAll(tasks);
        }

        private async Task SyncRepositoryAsync(Entity item, string crud)
        {

            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(item, setting);


            var content = new StringContent(json);
            if (item.IsSystemType()) return; // just custom entity

            var option = item.GetPersistenceOption();
            if (!option.IsElasticsearch) return;

            var type1 = item.GetType();
            var type = type1.Name.ToLowerInvariant();
            var index = EsConfigurationManager.Index;
            var url = $"{EsConfigurationManager.Host}/{index}/{type}/{item.Id}";


            HttpResponseMessage response = null;

            if (crud == "Add")
            {
                response = await m_client.PutAsync(url, content);
            }
            if (crud == "Update")
            {
                response = await m_client.PostAsync(url, content);
            }
            if (crud == "Delete")
            {
                response = await m_client.DeleteAsync(url);
            }

            if (null != response)
            {
                Debug.Write(".");
            }
        }
    }
}
