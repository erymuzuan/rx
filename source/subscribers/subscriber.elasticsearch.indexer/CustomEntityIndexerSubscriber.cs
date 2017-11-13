using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.ElasticSearch
{
    public class CustomEntityIndexerSubscriber : Subscriber<Entity>
    {
        public override string QueueName => this.GetType().FullName;
        public override string[] RoutingKeys => new[] {"#.added.#", "#.changed.#", "#.deleted.#"};

        private readonly HttpClient m_client = new HttpClient();

        protected override async Task ProcessMessage(Entity item, MessageHeaders headers)
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
                var delay = EsConfigurationManager.IndexingDelay;
                var maxTry = EsConfigurationManager.IndexingMaxTry;
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


            if (null != response)
            {
                Debug.Write(".");
            }
        }

        private async Task RequeueMessageAsync(Entity item, MessageHeaders headers, HttpRequestException e, long delay)
        {
            var count = (headers.TryCount ?? 0) + 1;
            this.WriteMessage($"{count.Ordinalize()} retry on HttpRequestException : {e.Message}");

            var ph = headers.GetRawHeaders();
            ph.AddOrReplace(MessageHeaders.SPH_DELAY, delay);
            ph.AddOrReplace(MessageHeaders.SPH_TRYCOUNT, count);

            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
            await publisher.PublishChanges(headers.Operation, new[] {item}, new AuditTrail[] { }, ph);
        }

        protected override void OnStop()
        {
            m_client.Dispose();
            base.OnStop();
        }
    }
}