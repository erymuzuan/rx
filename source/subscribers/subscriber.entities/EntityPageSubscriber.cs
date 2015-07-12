using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Jsbeautifier;

namespace subscriber.entities
{
    public class EntityPageSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName => "ed_page_gen";
        public override string[] RoutingKeys => new[] {
            nameof(EntityDefinition) + ".changed.Publish",
            nameof(EntityDefinition) + ".changed.Dashboard",
            nameof(EntityDefinition) + ".changed.PublishDashboard",
            nameof(EntityDefinition) + ".changed.DashboardPublish"
        };

        protected async override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.IsDefault
                && f.EntityDefinitionId == item.Id);
            var vm = new
            {
                Definition = item,
                Form = form
            };

            var template = context.LoadOneFromSources<ViewTemplate>(x => x.Name == item.DashboardTemplate)
                           ?? GetDefaultTemplate();

            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + item.Name.ToLower() + ".html");
            var markup = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(template.Html, vm);
            File.WriteAllText(html, markup);

            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + item.Name.ToLower() + ".js");
            var script = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(template.Js, vm);
            var beau = new Beautifier();
            File.WriteAllText(js, beau.Beautify(script));

        }

        private ViewTemplate GetDefaultTemplate()
        {
            string html;
            string js;
            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "subscriber.entities.entity.page";

            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME + ".html"))
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();

            }


            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME + ".js"))
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
            {
                js = reader.ReadToEnd();

            }
            return new ViewTemplate(html, js);
        }
    }
}
