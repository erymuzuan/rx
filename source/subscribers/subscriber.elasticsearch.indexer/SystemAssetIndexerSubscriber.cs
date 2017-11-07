using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
using Bespoke.Sph.SubscribersInfrastructure;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.ElasticSearch
{
    public class SystemAssetIndexerSubscriber : Subscriber<Entity>
    {
        public override string QueueName => "system_asset_es_indexer";
        public override string[] RoutingKeys => new[] { "#.added.#", "#.changed.#", "#.delete.#" };
        private HttpClient m_client;
        private TrackerIndexer m_trackerIndexer;
        protected override void OnStart()
        {
            m_client = new HttpClient { BaseAddress = new Uri(EsConfigurationManager.ElasticSearchHost) };
            m_trackerIndexer = new TrackerIndexer(m_client);
            base.OnStart();
        }

        protected override async Task ProcessMessage(Entity item, MessageHeaders headers)
        {

            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(item, setting);

            var content = new StringContent(json);
            var type = item.GetType();
            if (!item.IsSystemType()) return;// just custom entity
            var option = item.GetPersistenceOption();
            if (!option.IsElasticsearch) return;

            if (type == typeof(Tracker))
            {
                await m_trackerIndexer.ProcessMessage((Tracker)item, headers);
                return;
            }

            var url = $"{EsConfigurationManager.ElasticSearchHost}/{EsConfigurationManager.ElasticSearchSystemIndex}/{type.Name.ToLowerInvariant()}/{item.Id}";


            HttpResponseMessage response = null;
            try
            {
                if (headers.Crud == CrudOperation.Added)
                {
                    response = await m_client.PutAsync(url, content);
                }
                if (headers.Crud == CrudOperation.Changed)
                {
                    response = await m_client.PostAsync(url, content);
                }
                if (headers.Crud == CrudOperation.Deleted)
                {
                    response = await m_client.DeleteAsync(url);
                }
            }
            catch (HttpRequestException e)
            {
                // republish the message to a delayed queue
                var delay = EsConfigurationManager.EsIndexingDelay;
                var maxTry = EsConfigurationManager.EsIndexingMaxTry;
                if ((headers.TryCount ?? 0) < maxTry)
                {
                    await RequeueMessageAsync(item, headers, e, delay);
                }
                else
                {
                    this.WriteMessage($"Error in {this.GetType().Name}");
                    this.WriteError(e);
                    throw;
                }
            }
            response?.EnsureSuccessStatusCode();

        }

        private async Task RequeueMessageAsync(Entity item, MessageHeaders headers, HttpRequestException e, long delay)
        {

            var count = (headers.TryCount ?? 0) + 1;
            this.WriteMessage($"{count.Ordinalize()} retry on HttpRequestException : {e.Message}");

            var ph = headers.GetRawHeaders();
            ph.AddOrReplace(MessageHeaders.SPH_DELAY, delay);
            ph.AddOrReplace(MessageHeaders.SPH_TRYCOUNT, count);

            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
            await publisher.PublishChanges(headers.Operation, new[] { item }, new AuditTrail[] { }, ph);
        }

        protected override void OnStop()
        {
            m_trackerIndexer?.Dispose();
            m_client.Dispose();
            base.OnStop();
        }
    }
}