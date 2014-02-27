using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class EntityFormSubscriber : Subscriber<EntityForm>
    {
        public override string QueueName
        {
            get { return "ed_form_gen"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityForm).Name + ".changed.Publish" }; }

        }

        protected async override Task ProcessMessage(EntityForm item, MessageHeaders header)
        {
            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + item.Route.ToLower() + ".html");
            using (var client = new HttpClient())
            {
                var uri = ConfigurationManager.BaseUrl + "/App/EntityFormRenderer/Html/" + item.Route;
                this.WriteMessage("Rendering {0}", uri);
                var markup = await client.GetStringAsync(uri);
                File.WriteAllText(html, markup);
            }


            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + item.Route.ToLower() + ".js");
            using (var client = new HttpClient())
            {
                var script = await client.GetStringAsync(ConfigurationManager.BaseUrl + "/App/EntityFormRenderer/Js/" + item.Route);
                File.WriteAllText(js, script);
            }

        }



    }
}