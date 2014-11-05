using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.ElasticSearch
{
    public abstract class EsEntityIndexer<T> : Subscriber<T> where T : Entity
    {

        public override string QueueName
        {
            get { return typeof(T).Name.ToLowerInvariant() + "_es"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(T).Name + ".#.#" }; }
        }

        protected virtual string GetTypeName(T item)
        {
            return typeof(T).Name.ToLowerInvariant();

        }

        protected async override Task ProcessMessage(T item, MessageHeaders headers)
        {
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(item, setting);

            var content = new StringContent(json);
            var id = item.Id;


            var url = string.Format("{0}/{1}/{2}", ConfigurationManager.ElasticSearchIndex, this.GetTypeName(item), id);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
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
                        publisher.PublishChanges(headers.Operation, new Entity[] { item }, new AuditTrail[] { }, ph)
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