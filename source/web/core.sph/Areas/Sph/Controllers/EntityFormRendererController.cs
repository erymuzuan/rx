using System;
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
            var vm = new FormRendererViewModel(ed, form);

            return View(layout, vm);
        }

        [RazorScriptFilter]
        public async Task<ActionResult> Js(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.Route == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Id == form.EntityDefinitionId);

            var model = new FormRendererViewModel(ed, form);

            var ns = ConfigurationManager.ApplicationName + "_" + ed.Name.ToCamelCase();
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
                partial = partial || {{}},
                i18n = null,
                headers = {{}},
                activate = function (entityId) {{
                    id(entityId);
                    var tcs = new $.Deferred();
                    context.loadOneAsync(""EntityForm"", ""Route eq '{form.Route}'"")
                       .then(function (f) {{
                           form(f);
                           return watcher.getIsWatchingAsync(""{ed.Name}"", entityId);
                       }})
                       .then(function (w) {{
                           watching(w);
                           return $.getJSON(""i18n/"" + config.lang + ""/{form.Route}"");
                       }})
                       .then(function (n) {{
                           i18n = n[0];
                            if(!entityId || entityId === ""0""){{
                                return Task.fromResult({{ WebId: system.guid() }}); 
                            }}
                           return context.get(""/api/{ed.Plural.ToIdFormat()}/"" + entityId);
                       }}).then(function (b,  textStatus, xhr) {{
                            
                            if(xhr) {{
 				                var etag = xhr.getResponseHeader(""ETag""),
                                    lastModified = xhr.getResponseHeader(""Last-Modified"");
                                if(etag){{
                                    headers[""If-Match""] = etag;
                                }}
                                if(lastModified){{
                                    headers[""If-Modified-Since""] = lastModified;
                                }}
                            }}    
                             entity(new bespoke.{ns}.domain.{ed.Name}(b[0]||b));
                       }}, function (e) {{
                         if (e.status == 404) {{
                            app.showMessage(""Sorry, but we cannot find any {ed.Name} with location : "" + ""/api/{ed.Plural.ToIdFormat()}/"" + entityId, ""{ConfigurationManager.ApplicationFullName}"", [""OK""]);
                         }}
                       }}).always(function () {{
                           if (typeof partial.activate === ""function"") {{
                               partial.activate(ko.unwrap(entity))
                                        .done(tcs.resolve)
                                        .fail(tcs.reject);
                           }}
                           else{{
                            tcs.resolve(true);
                           }}
                       }});
                       return tcs.promise();
            
                }},");



            var operation = context.LoadOneFromSources<OperationEndpoint>(x => x.Name == form.Operation && x.Entity == ed.Name);
            if (null != operation)
            {
                var api = GenerateApiOperationCode(operation, form.OperationMethod);
                script.Append(api);
            }
            if (model.Form.IsRemoveAvailable)
            {
                var deleteOperation = context.LoadOneFromSources<OperationEndpoint>(x => x.Name == form.DeleteOperation && x.Entity == form.Entity);
                var route = !string.IsNullOrWhiteSpace(deleteOperation.Route) ? $"{deleteOperation.Route.ToLowerInvariant()}/" : "";
                route = $"/api/{deleteOperation.Resource}/{route}/".Replace("//", "/");
                script.AppendLine($@"remove = function() {{
                        return context.sendDelete(""{route}"" + ko.unwrap(entity().Id))
                                       .then(function(result){{
                                             return app.showMessage(""{form.DeleteOperationSuccessMesage}"",""{ConfigurationManager.ApplicationFullName}"",[""OK""]);
                                        }})
                                        .then(function(result){{
                                             router.navigate(""{form.DeleteOperationSuccessNavigateUrl}"");
                                        }});
                    }},");

            }
            // end of operation
            script.AppendLine($@"
                attached = function (view) {{
                    // validation
                    validation.init($('#{formId}'), form());");

            script.AppendLine(@"
                    if(typeof partial.attached === ""function""){
                        partial.attached(view);
                    }");
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

            var commandButtons = model.Form.FormDesign.FormElementCollection.OfType<Button>()
                .Where(x => !string.IsNullOrWhiteSpace(x.CommandName)).ToArray();
            var operationButtons = model.Form.FormDesign.FormElementCollection.OfType<Button>()
                .Where(x => x.Operation != form.Operation) // struck out form's operation
                .Where(x => !string.IsNullOrWhiteSpace(x.Operation)).ToArray();
            foreach (var btn in commandButtons)
            {
                var function = btn.CommandName;
                script.AppendLine($@"
                    {function} = function(){{
                        {btn.Command}
                }},");
            }

            foreach (var btn in operationButtons)
            {
                var oe = context.LoadOneFromSources<OperationEndpoint>(x => x.Name == btn.Operation && x.Entity == ed.Name);
                var apiCallScript = GenerateApiOperationCode(oe, btn.OperationMethod);
                script.Append(apiCallScript);
                var operationScript = this.GetOperationScript(new EntityForm
                {
                    Operation = btn.Operation,
                    DeleteOperationSuccessNavigateUrl = btn.DeleteOperationSuccessNavigateUrl,
                    DeleteOperationSuccessMesage = btn.DeleteOperationSuccessMesage,
                    DeleteOperation = btn.DeleteOperation,
                    OperationMethod = btn.OperationMethod,
                    OperationFailureCallback = btn.OperationFailureCallback,
                    OperationSuccessCallback = btn.OperationSuccessCallback,
                    OperationSuccessNavigateUrl = btn.OperationSuccessNavigateUrl,
                    OperationSuccessMesage = btn.OperationSuccessMesage
                }, $"{btn.OperationMethod}{btn.Operation}Command".ToCamelCase());
                script.Append(operationScript);
                script.Append(",");
            }

            script.Append(@"
                compositionComplete = function() {
                    $(""[data-i18n]"").each(function (i, v) {
                        var $label = $(v),
                            text = $label.data(""i18n"");
                        if (i18n && typeof i18n[text] === ""string"") {
                            $label.text(i18n[text]);
                        }
                    });
                }");
            if (!string.IsNullOrWhiteSpace(form.Operation))
            {
                var operationScript = this.GetOperationScript(form);
                script.Append(",");
                script.Append(operationScript);
            }
            script.Append(";");

            // viewmodel
            script.AppendLine(@"
            var vm = {
                    partial: partial,");

            foreach (var rule in model.Form.Rules)
            {
                var function = rule.Dehumanize().ToCamelCase();
                script.AppendLine($"   {function} : {function},");

            }
            script.AppendLine(@"    activate: activate,
                                        config: config,
                                        attached: attached,
                                        compositionComplete:compositionComplete,
                                        entity: entity,
                                        errors: errors,");
            foreach (var btn in commandButtons)
            {
                script.AppendLine($"   {btn.CommandName} : {btn.CommandName},");
            }

            foreach (var btn in operationButtons)
            {
                var operationCommand = $"{btn.OperationMethod}{btn.Operation}Command".ToCamelCase();
                script.AppendLine($"   {operationCommand} : {operationCommand},");
            }

            script.AppendLine("     toolbar : {");
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
            if (!string.IsNullOrWhiteSpace(saveOperation))
            {
                script.AppendLine("saveCommand : saveCommand,");
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

        private string GetOperationScript(EntityForm form, string functionName = "saveCommand")
        {
            var script = new StringBuilder();
            script.AppendLine($@"
                {functionName} = function() {{
                    return {form.Operation.ToCamelCase()}Command()");
            if (!string.IsNullOrWhiteSpace(form.OperationSuccessCallback))
            {
                script.AppendLine($@"  .then(function(result){{
                        {form.OperationSuccessCallback}
                    }})");
            }

            var successMessage = form.OperationSuccessMesage;
            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                successMessage = successMessage.Trim().StartsWith("=") ? successMessage.Remove(0, 1) : $"\"{successMessage}\"";
                script.AppendLine($@"  .then(function(result){{
                          if(result.success) {{
                                return app.showMessage({successMessage}, [""OK""]);
                          }}
                          else {{
                               return Task.fromResult(false);
                          }}
                    }})");
            }
            var url = form.OperationSuccessNavigateUrl;
            if (!string.IsNullOrWhiteSpace(url))
            {
                url = url.Trim().StartsWith("=") ? url.Remove(0, 1) : $"\"{url}\"";
                script.AppendLine($@"  .then(function(result){{
                        if(result){{
                            router.navigate({url});
                        }}
                    }})");
            }
            if (!string.IsNullOrWhiteSpace(form.OperationFailureCallback))
            {
                script.AppendLine($@"  .fail(function(){{
                        {form.OperationFailureCallback}
                    }})");
            }

            script.AppendLine(@"; 
                }");

            return script.ToString();
        }

        private static string GenerateApiOperationCode(OperationEndpoint operation, string method)
        {
            var opFunc = operation.Name.ToCamelCase();
            var route = operation.Route.StartsWith("~/") ?
                        operation.Route.Replace("~/", "/") :
                        $"/api/{operation.Resource}/{operation.Route}";
            // TODO : replace {id} in route with ko.unwrap(entity().Id)
            route = route.Replace("{id}", "\" + ko.unwrap(entity().Id) + \"");
            var putOrPatchWithDefaultRoute = (method.Equals("PUT", StringComparison.InvariantCultureIgnoreCase) 
                     || method.Equals("PATCH", StringComparison.OrdinalIgnoreCase)) &&
                    !operation.Route.Contains("{id");
            if (putOrPatchWithDefaultRoute)
                route += "\" + ko.unwrap(entity().Id) + \"";
            return $@"
                {opFunc}Command = function(){{

                     if (!validation.valid()) {{
                         return Task.fromResult(false);
                     }}

                     var data = ko.mapping.toJSON(entity),
                        tcs = new $.Deferred();
                      
                     context.{method}(data, ""{route}"", headers)
                         .fail(function(response){{ 
                            var result = response.responseJSON;
                            errors.removeAll();
                            if(response.status === 428){{
                                // out of date conflict
                                logger.error(result.message);
                            }}
                            if(response.status === 422 && _(result.rules).isArray()){{                            
                                _(result.rules).each(function(v){{
                                    errors(v.ValidationErrors);
                                }});
                            }}
                            logger.error(""There are errors in your entity, !!!"");
                            tcs.resolve(false);
                         }})
                         .then(function (result) {{
                            logger.info(result.message);
                            entity().Id(result.id);
                            errors.removeAll();
                            tcs.resolve(result);
                         }});
                     return tcs.promise();
                 }},";
        }
    }
}