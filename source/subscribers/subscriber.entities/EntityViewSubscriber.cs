using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class EntityViewSubscriber : Subscriber<EntityView>
    {
        public override string QueueName => "ed_view_gen";

        public override string[] RoutingKeys => new[] { typeof(EntityView).Name + ".#.Publish" };

        protected async override Task ProcessMessage(EntityView view, MessageHeaders header)
        {
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Id == view.EntityDefinitionId);
            var form = await context.LoadOneAsync<EntityForm>(f => f.IsDefault == true
                && f.EntityDefinitionId == view.EntityDefinitionId);


            if (null == ed)
                this.WriteError(new NullReferenceException("EntityDefinition is null Id = " + view.EntityDefinitionId));
            if (null == form)
                this.WriteError(new NullReferenceException("Default EntityForm cannot be found for EntityDefinition = " + view.EntityDefinitionId));
            var vm = new
            {
                Definition = ed,
                View = view,
                Form = form,
                FilterDsl = Filter.GenerateElasticSearchFilterDsl(view, view.FilterCollection),
                SortDsl = view.GenerateEsSortDsl(),
                Routes = string.Join(",", view.RouteParameterCollection.Select(r => r.Name)),
                PartialArg = string.IsNullOrWhiteSpace(view.Partial) ? "" : ", partial",
                PartialPath = string.IsNullOrWhiteSpace(view.Partial) ? "" : ", \"" + view.Partial + "\""
            };

            var template = context.LoadOneFromSources<ViewTemplate>(t => t.Name == view.Template) ??
                           this.GetDefaultHtmlTemplate();

            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + view.Route.ToLower() + ".html");
            var markup = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(template.Html, vm);
            File.WriteAllText(html, markup);

            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + view.Route.ToLower() + ".js");
            var script = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(template.Js, vm);
            File.WriteAllText(js, script);



        }

        private ViewTemplate GetDefaultHtmlTemplate()
        {
            var html = "";
            var js = "";
            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "subscriber.entities.entity.view.html";
            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
            using (var reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();

            }

            const string RESOURCE_NAME_JS = "subscriber.entities.entity.view.js";
            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME_JS))
            using (var reader = new StreamReader(stream))
            {
                js = reader.ReadToEnd();

            }

            return new ViewTemplate(html, js);
        }



    }
}
