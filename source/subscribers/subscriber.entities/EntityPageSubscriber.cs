using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class EntityPageSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName
        {
            get { return "ed_page_gen"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityDefinition).Name + ".changed.Publish" }; }

        }

        protected async override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "subscriber.entities.entitypage";

            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + item.Name.ToLower() + ".html");
            using (var page = assembly.GetManifestResourceStream(resourceName + ".html"))
            using (var fs = new FileStream(html, FileMode.Create))
            {
                if (null != page)
                    page.CopyTo(fs);
            }

            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + item.Name.ToLower() + ".js");


            using (var stream = assembly.GetManifestResourceStream(resourceName + ".js"))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                var script = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, item);
                File.WriteAllText(js, script);

            }

        }



    }
}
