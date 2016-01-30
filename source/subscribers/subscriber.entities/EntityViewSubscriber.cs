using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Jsbeautifier;

namespace subscriber.entities
{
    public class EntityViewSubscriber : Subscriber<EntityView>
    {
        public override string QueueName => "ed_view_gen";

        public override string[] RoutingKeys => new[] { typeof(EntityView).Name + ".#.Publish" };

        protected override async Task ProcessMessage(EntityView view, MessageHeaders header)
        {
            await Task.Delay(2000);
            var context = new SphDataContext();
            var query = context.LoadOneFromSources<EntityQuery>(x => x.Id == view.Endpoint);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Id == view.EntityDefinitionId);
            var form = await context.LoadOneAsync<EntityForm>(f => f.IsDefault
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
                Query = query,
                Routes = string.Join(",", view.RouteParameterCollection.Select(r => r.Name)),
                PartialArg = string.IsNullOrWhiteSpace(view.Partial) ? "" : ", partial",
                PartialPath = string.IsNullOrWhiteSpace(view.Partial) ? "" : ", \"" + view.Partial + "\""
            };

            var template = context.LoadOneFromSources<ViewTemplate>(t => t.Name == view.Template) ??
                           this.GetDefaultHtmlTemplate();
            var beau = new Beautifier();

            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + view.Route.ToLower() + ".html");
            var markup = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(template.Html, vm);
            File.WriteAllText(html, markup.Tidy());

            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + view.Route.ToLower() + ".js");
            var script = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(template.Js, vm);
            
            var formattedScript = beau.Beautify(script);
            File.WriteAllText(js, formattedScript);



        }

        private string GenerateDefaultViewModelTemplate()
        {
          
            var code = new StringBuilder();
            code.Append(
                @"
@{
        var query = Model.Query;
        var route = query.GetLocation();
}
define([""services/datacontext"", ""services/logger"", ""plugins/router"", ""services/chart"", objectbuilders.config ,""services/_ko.list""],
    function (context, logger, router, chart,config ) {

        var isBusy = ko.observable(false),
            query = ""@route"",
            partial = partial || {},
            list = ko.observableArray([]),
            map = function(v) {
                if (typeof partial.map === ""function"") {
                    return partial.map(v);
                }
                return v;
            },
            activate = function () {
                if (typeof partial.activate === ""function"") {
                    return partial.activate(list);
                }
                return true;
            },
            attached = function (view) {
                if (typeof partial.attached === ""function"") {
                    partial.attached(view);
                }
            };

        var vm = {
            query: query,
            config: config,
            isBusy: isBusy,
            map: map,
            activate: activate,
            attached: attached,
            list: list,
            toolbar: {
                commands: ko.observableArray([])
            }
        };

        return vm;

    });");

            return code.ToString();
        }

        private ViewTemplate GetDefaultHtmlTemplate()
        {
            string html;
            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "subscriber.entities.entity.view.html";
            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();

            }
            var js = GenerateDefaultViewModelTemplate();
            return new ViewTemplate(html, js);
        }



    }
}
