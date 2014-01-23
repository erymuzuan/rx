using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class EntityViewSubscriber : Subscriber<EntityView>
    {
        public override string QueueName
        {
            get { return "ed_view_gen"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityView).Name + ".changed.Publish" }; }

        }

        protected async override Task ProcessMessage(EntityView item, MessageHeaders header)
        {
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.EntityDefinitionId == item.EntityDefinitionId);
            var vm = new
            {
                EntityDefinition = ed,
                EntityView = item
            };

            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "subscriber.entities.entity.view";


            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + item.Route.ToLower() + ".html");
            using (var stream = assembly.GetManifestResourceStream(resourceName + ".html"))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                var markup = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, vm);
                if (File.Exists(html))
                {
                    File.Move(html, string.Format("{0}_{1:yyyyMMdd_HHmmss}.html", html.Replace(".html",""), DateTime.Now));
                }
                File.WriteAllText(html, markup);

            }


            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + item.Route.ToLower() + ".js");
            using (var stream = assembly.GetManifestResourceStream(resourceName + ".js"))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                var script = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, vm);
                if (File.Exists(js))
                {
                    File.Move(js, string.Format("{0}_{1:yyyyMMdd_HHmmss}.js", js.Replace(".js",""), DateTime.Now));
                }
                File.WriteAllText(js, script);

            }

        }



    }
}
