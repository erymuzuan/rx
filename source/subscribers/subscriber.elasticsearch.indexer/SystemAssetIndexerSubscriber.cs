using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.ElasticSearch
{
    public class SystemAssetIndexerSubscriber : Subscriber<Entity>
    {

        public override string QueueName
        {
            get { return "system_asset_es_indexer"; }
        }


        public override string[] RoutingKeys
        {
            get { return new[] { "#.added.#", "#.changed.#", "#.delete.#" }; }
        }



        protected async override Task ProcessMessage(Entity item, MessageHeaders headers)
        {
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(item, setting);

            var content = new StringContent(json);
            if (!item.GetType().IsSystemType()) return;// just custom entity
            if (item.GetType() == typeof(Tracker))
            {
                var trackerIndexer = new TrackerIndexer();
                await trackerIndexer.ProcessMessage((Tracker)item, headers);
                return;
            }


            var url = string.Format("{0}/{1}/{2}/{3}", ConfigurationManager.ElasticSearchHost,
                ConfigurationManager.ElasticSearchIndex,
                item.GetType().Name.ToLowerInvariant(),
                item.Id);

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = null;
                try
                {
                    if (headers.Crud == CrudOperation.Added)
                    {
                        response = await client.PutAsync(url, content);
                    }
                    if (headers.Crud == CrudOperation.Changed)
                    {
                        response = await client.PostAsync(url, content);
                    }
                    if (headers.Crud == CrudOperation.Deleted)
                    {
                        response = await client.DeleteAsync(url);
                    }
                }
                catch (HttpRequestException e)
                {
                    // republish the message to a delayed queue
                    var delay = ConfigurationManager.SqlPersistenceDelay;
                    var maxTry = ConfigurationManager.SqlPersistenceMaxTry;
                    if ((headers.TryCount ?? 0) < maxTry)
                    {
                        var count = (headers.TryCount ?? 0) + 1;
                        this.WriteMessage("{0} retry on HttpRequestException : {1}", count.Ordinalize(), e.Message);

                        var ph = headers.GetRawHeaders();
                        ph.AddOrReplace(MessageHeaders.SPH_DELAY, delay);
                        ph.AddOrReplace(MessageHeaders.SPH_TRYCOUNT, count);

                        var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
                        publisher.PublishChanges(headers.Operation, new[] { item }, new AuditTrail[] { }, ph)
                            .Wait();

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


        }

    }
}