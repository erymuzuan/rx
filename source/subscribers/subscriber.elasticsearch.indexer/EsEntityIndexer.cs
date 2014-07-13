using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;
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
            var id = item.GetId();


            var url = string.Format("{0}/{1}/{2}",ConfigurationManager.ElasticSearchIndex, this.GetTypeName(item), id);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                HttpResponseMessage response = null;
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


                if (null != response)
                {
                    Debug.Write(".");
                }
            }
        }

    }
}