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


            if(null == ed)
                this.WriteError(new NullReferenceException("EntityDefinition is null Id = " + view.EntityDefinitionId));
            if(null == form)
                this.WriteError(new NullReferenceException("Default EntityForm cannot be found for EntityDefinition = " + view.EntityDefinitionId));
            var vm = new
            {
                Definition = ed,
                View = view, 
                Form = form,
                FilterDsl = Filter.GenerateElasticSearchFilterDsl(view, view.FilterCollection),
                SortDsl = view.GenerateEsSortDsl(),
                Routes = string.Join(",",view.RouteParameterCollection.Select(r => r.Name)),
                PartialArg = string.IsNullOrWhiteSpace(view.Partial) ? "" : ", partial",
                PartialPath = string.IsNullOrWhiteSpace(view.Partial) ? "" : ", \"" + view.Partial + "\""
            };


            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "subscriber.entities.entity.view";


            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + view.Route.ToLower() + ".html");
            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME + ".html"))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                var markup = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, vm);
                File.WriteAllText(html, markup);

            }


            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + view.Route.ToLower() + ".js");
            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME + ".js"))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                var script = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, vm);
                File.WriteAllText(js, script);

            }

        }



    }
}
