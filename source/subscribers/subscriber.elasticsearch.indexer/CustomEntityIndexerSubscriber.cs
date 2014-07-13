using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.ElasticSearch
{
    public class CustomEntityIndexerSubscriber : Subscriber<Entity>
    {

        public override string QueueName
        {
            get { return "custom_entities_es_indexer"; }
        }

        // subscribe to all , not so efficient
        public override string[] RoutingKeys
        {
            get { return new[] { "#.added.#", "#.changed.#", "#.delete.#" }; }
        }



        protected async override Task ProcessMessage(Entity item, MessageHeaders headers)
        {
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(item, setting);

            var content = new StringContent(json);
            var id = item.GetId();
            if (item.GetType().Namespace == typeof(Entity).Namespace) return;// just custom entity


            var url = string.Format("{0}/{1}/{2}/{3}", ConfigurationManager.ElasticSearchHost,
                ConfigurationManager.ApplicationName.ToLowerInvariant(),
                item.GetType().Name.ToLowerInvariant(),
                id);

            using (var client = new HttpClient())
            {
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