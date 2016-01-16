using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.ViewModels;
using Humanizer;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EntityFormRendererController : BaseSphController
    {
        public async Task<ActionResult> Html(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.Route == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Id == form.EntityDefinitionId);

            var layout = string.IsNullOrWhiteSpace(form.Layout) ? "Html2ColsWithAuditTrail" : form.Layout;
            var vm = new FormRendererViewModel { Form = form, EntityDefinition = ed };

            return View(layout, vm);
        }

        [RazorScriptFilter]
        public async Task<ActionResult> Js(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.Route == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Id == form.EntityDefinitionId);

            var model = new FormRendererViewModel { Form = form, EntityDefinition = ed };

            var ns = ConfigurationManager.ApplicationName + "_" + model.EntityDefinition.Id;
            var typeCtor = $"bespoke.{ns}.domain.{model.EntityDefinition.Name}(system.guid())";
            var buttonOperations = model.Form.FormDesign.FormElementCollection.OfType<Button>()
                .Where(b => b.IsToolbarItem)
                .Where(b => !string.IsNullOrWhiteSpace(b.Operation))
                .Select(b => $"{{ caption :\"{b.Label}\", command : {b.Operation.ToCamelCase()}, icon:\"{b.IconClass}\" }}");

            var commands = model.Form.FormDesign.FormElementCollection.OfType<Button>()
                .Where(b => b.IsToolbarItem)
                .Where(b => !string.IsNullOrWhiteSpace(b.CommandName))
                .Select(b => $"{{ caption :\"{b.Label}\", command : {b.CommandName}, icon:\"{b.IconClass}\" }}");
            var commandsJs = $"[{string.Join(",", commands.Concat(buttonOperations))}]";


            var formId = model.Form.Route + "-form";
            var saveOperation = model.Form.Operation;
            var partialPath = string.IsNullOrWhiteSpace(model.Form.Partial) ? string.Empty : ",'" + model.Form.Partial + "'";
            var partialVariable = string.IsNullOrWhiteSpace(model.Form.Partial) ? string.Empty : ",partial";

            var script = new StringBuilder();
            script.AppendLine(
               $@"
define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
        objectbuilders.system, objectbuilders.validation, objectbuilders.eximp,
        objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config,
        objectbuilders.app {partialPath}],
        function (context, logger, router, system, validation, eximp, dialog, watcher,config,app {partialVariable}) {{

            var entity = ko.observable(new {typeCtor}),
                errors = ko.observableArray(),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                watching = ko.observable(false),
                id = ko.observable(),
                i18n = null,
                activate = function (entityId) {{
                    id(entityId);

                    var query = String.format(""Id eq '{{0}}'"", entityId),
                        tcs = new $.Deferred(),
                        itemTask = context.loadOneAsync(""{model.EntityDefinition.Name}"", query),
                        formTask = context.loadOneAsync(""EntityForm"", ""Route eq '{model.Form.Route}'""),
                        watcherTask = watcher.getIsWatchingAsync(""{model.EntityDefinition.Name}"", entityId),
                        i18nTask = $.getJSON(""i18n/"" + config.lang + ""/{model.Form.Route}"");

                    $.when(itemTask, formTask, watcherTask, i18nTask).done(function(b,f,w,n) {{
                        if (b) {{
                            var item = context.toObservable(b);
                            entity(item);
                        }}
                        else {{
                            entity(new {typeCtor});
                        }}
                        form(f);
                        watching(w);
                        i18n = n[0];");
            var hasPartial = !string.IsNullOrWhiteSpace(model.Form.Partial);
            if (hasPartial)
            {
                script.AppendLine(@"
                            if(typeof partial.activate === ""function""){
                                var pt = partial.activate(entity());
                                if(typeof pt.done === ""function""){
                                    pt.done(tcs.resolve);
                                }else{
                                    tcs.resolve(true);
                                }
                            }
                       ");
            }
            else
            {
                script.AppendLine("tcs.resolve(true);");

            }
            script.AppendLine(@"
                        
                    });

                    return tcs.promise();
                },");


            var operation = ed.EntityOperationCollection.SingleOrDefault(x => x.Name == form.Operation);
            if (null != operation)
            {
                var api = GenerateApiOperationCode(ed, operation, form.OperationMethod);
                script.Append(api);
            }
            if (model.Form.IsRemoveAvailable)
                script.AppendLine($@"remove = function {{
                        return context.sendDelete(""{ed.Name}/{form.DeleteOperation}/"" + ko.unwrap(entity().Id));
                    }},");
            // end of operation
            script.AppendLine($@"
                attached = function (view) {{
                    // validation
                    validation.init($('#{formId}'), form());");

            script.AppendLine(@"
                    if(typeof partial.attached === ""function""){
                        partial.attached(view);
                    }", hasPartial);
            script.AppendLine(@"
                },");

            foreach (var rule in model.Form.Rules)
            {
                var function = rule.Dehumanize().ToCamelCase();
                script.AppendLine($@"
                    {function} = function(){{

                        var data = ko.mapping.toJSON(entity);
                        return context.post(data, ""/Sph/BusinessRule/Validate?{function}"" );
                        
                }},");
            }

            foreach (var btn in model.Form.FormDesign.FormElementCollection.OfType<Button>().Where(x => !string.IsNullOrWhiteSpace(x.CommandName)))
            {
                var function = btn.CommandName;
                script.AppendLine($@"
                    {function} = function(){{
                        {btn.Command}
                }},");
            }

            script.AppendLine(@"
                compositionComplete = function() {
                    $(""[data-i18n]"").each(function (i, v) {
                        var $label = $(v),
                            text = $label.data(""i18n"");
                        if (typeof i18n[text] === ""string"") {
                            $label.text(i18n[text]);
                        }
                    });
                },");
            script.AppendLine($@"
                saveCommand = function() {{
                    return {form.Operation.ToCamelCase()}()");
            if (!string.IsNullOrWhiteSpace(form.OperationSuccessCallback))
            {
                script.AppendLine($@"  .then(function(result){{
                        {form.OperationSuccessCallback}
                        return Task.fromResult(result);
                    }})");
            }
            if (!string.IsNullOrWhiteSpace(form.OperationSuccessMesage))
            {
                script.AppendLine($@"  .then(function(result){{
                          if(result.success)
                                return app.showMessage(""{form.OperationSuccessMesage}"", [""OK""]);
                            else
                               return Task.fromResult(false);
                    }})");
            }
            if (!string.IsNullOrWhiteSpace(form.OperationSuccessNavigateUrl))
            {
                script.AppendLine($@"  .then(function(result){{
                        if(result) router.navigate(""{form.OperationSuccessNavigateUrl}"");
                    }})");
            }
            if (!string.IsNullOrWhiteSpace(form.OperationFailureCallback))
            {
                script.AppendLine($@"  .fail(function(){{
                        {form.OperationFailureCallback}
                    }})");
            }

            script.AppendLine($@"; 
                }};");

            // viewmodel
            var partialMemberDeclaration = (string.IsNullOrWhiteSpace(model.Form.Partial) ? "" : "partial: partial,");
            script.AppendLine($@"
            var vm = {{
                 {partialMemberDeclaration}");

            foreach (var rule in model.Form.Rules)
            {
                var function = rule.Dehumanize().ToCamelCase();
                script.AppendLine($@"   {function} : {function},");

            }
            script.AppendLine(@"    activate: activate,
                                        config: config,
                                        attached: attached,
                                        compositionComplete:compositionComplete,
                                        entity: entity,
                                        errors: errors,");
            foreach (var op in model.EntityDefinition.EntityOperationCollection)
            {
                var function = op.Name.ToCamelCase();
                script.AppendLine($"    {function} : {function},");
            }
            foreach (
                var btn in
                    model.Form.FormDesign.FormElementCollection.OfType<Button>()
                        .Where(b => !string.IsNullOrWhiteSpace(b.CommandName)))
            {
                script.AppendLine($@"   {btn.CommandName} : {btn.CommandName},");

            }

            script.AppendLine(@"     toolbar : {");
            if (model.Form.IsEmailAvailable)
                script.AppendLine($@"   emailCommand : {{
                        entity : ""{model.EntityDefinition.Name}"",
                        id :id
                    }},");

            if (model.Form.IsPrintAvailable)
                script.AppendLine($@"
                    printCommand :{{
                        entity : '{model.EntityDefinition.Name}',
                        id : id
                    }},");
            if (model.Form.IsRemoveAvailable)
                script.AppendLine(@"removeCommand :remove,
                    canExecuteRemoveCommand : ko.computed(function(){
                        return entity().Id();
                    }),");
            if (model.Form.IsWatchAvailable)
                script.AppendLine($@"
                    watchCommand: function() {{
                        return watcher.watch(""{model.EntityDefinition.Name}"", entity().Id())
                            .done(function(){{
                                watching(true);
                            }});
                    }},
                    unwatchCommand: function() {{
                        return watcher.unwatch(""{model.EntityDefinition.Name}"", entity().Id())
                            .done(function(){{
                                watching(false);
                            }});
                    }},
                    watching: watching,");
            if (!string.IsNullOrWhiteSpace(@saveOperation))
            {
                script.AppendLine("saveCommand : saveCommand,");
                if (hasPartial)
                    script.AppendLine(@"canExecuteSaveCommand : ko.computed(function(){
                        if(typeof partial.canExecuteSaveCommand === ""function""){
                            return partial.canExecuteSaveCommand();
                        }
                        return true;
                    }),");
            }
            script.AppendLine(@"                    
                },// end toolbar");
            script.AppendLine($@"
                    commands : ko.observableArray({commandsJs})
            }};

            return vm;
        }});");

            return Content(script.ToString(), MimeMapping.GetMimeMapping("some.js"), Encoding.UTF8);


        }

        private static string GenerateApiOperationCode(EntityDefinition ed, EntityOperation operation, string method)
        {
            var opFunc = operation.Name.ToCamelCase();
            var route = string.IsNullOrWhiteSpace(operation.Route) ? operation.Name : operation.Route;
            return $@"
                {opFunc} = function(){{

                     if (!validation.valid()) {{
                         return Task.fromResult(false);
                     }}

                     var data = ko.mapping.toJSON(entity),
                        tcs = new $.Deferred();
                      
                     context.{method}(data, ""/{ed.Name}/{route}"" )
                         .then(function (result) {{
                             if (result.success) {{
                                 logger.info(result.message);
                                 entity().Id(result.id);
                                 errors.removeAll();

                             }} else {{
                                 errors.removeAll();
                                 _(result.rules).each(function(v){{
                                     errors(v.ValidationErrors);
                                 }});
                                 logger.error(""There are errors in your entity, !!!"");
                             }}
                             tcs.resolve(result);
                         }});
                     return tcs.promise();
                 }},";
        }
    }
}