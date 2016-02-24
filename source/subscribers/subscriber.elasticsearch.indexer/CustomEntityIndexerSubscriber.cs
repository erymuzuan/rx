using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.ElasticSearch
{
    public class CustomEntityIndexerSubscriber : Subscriber<Entity>
    {

        public override string QueueName => this.GetType().FullName;
        public override string[] RoutingKeys => new[] { "#.added.#", "#.changed.#", "#.delete.#" };

        private readonly HttpClient m_client = new HttpClient();
        protected override async Task ProcessMessage(Entity item, MessageHeaders headers)
        {
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(item, setting);

            var content = new StringContent(json);
            if (item.IsSystemType()) return;// just custom entity


            var type1 = item.GetType();
            var type = type1.Name.ToLowerInvariant();
            var index = ConfigurationManager.ElasticSearchIndex;
            var url = $"{ConfigurationManager.ElasticSearchHost}/{index}/{type}/{item.Id}";


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
                var delay = ConfigurationManager.EsIndexingDelay;
                var maxTry = ConfigurationManager.EsIndexingMaxTry;
                if ((headers.TryCount ?? 0) < maxTry)
                {
                    await RequeueMessageAsync(item, headers, e, delay);
                }
                else
                {
                    this.WriteMessage("Error in {0}", this.GetType().Name);
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
            this.WriteMessage("{0} retry on HttpRequestException : {1}", count.Ordinalize(), e.Message);

            var ph = headers.GetRawHeaders();
            ph.AddOrReplace(MessageHeaders.SPH_DELAY, delay);
            ph.AddOrReplace(MessageHeaders.SPH_TRYCOUNT, count);

            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
            await publisher.PublishChanges(headers.Operation, new[] {item}, new AuditTrail[] {}, ph);
        }

        protected override void OnStop()
        {
            m_client.Dispose();
            base.OnStop();
        }
    }
}