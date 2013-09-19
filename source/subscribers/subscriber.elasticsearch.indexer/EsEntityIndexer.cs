using System;
using System.Configuration;
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
            get { return new[] { typeof(T).Name + ".*" }; }
        }

        protected async override Task ProcessMessage(T item, MessageHeaders headers)
        {
            this.WriteMessage("INDEXING {0}", item);

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json);
            var id = item.GetId();
            var esServer = ConfigurationManager.AppSettings["es.server"];
            const string index = "sph";


            var url = string.Format("{0}/{1}/{2}/{3}", esServer, index, typeof(T).Name.ToLowerInvariant(), id);

            var client = new HttpClient();
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
                this.WriteMessage("RESPONSE CODE : {0}", response.StatusCode);
                this.WriteMessage(response.Content);
            }
        }

    }
}