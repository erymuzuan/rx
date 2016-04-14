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
    public class WorkflowFormRendererController : BaseSphController
    {
        public async Task<ActionResult> Html(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<WorkflowForm>(f => f.Route == id);
            var wd = await context.LoadOneAsync<WorkflowDefinition>(f => f.Id == form.WorkflowDefinitionId);

            var layout = string.IsNullOrWhiteSpace(form.Layout) ? "Html2ColsWithAuditTrail" : form.Layout;
            var vm = new FormRendererViewModel(wd, form, null);

            return View(layout, vm);
        }

        [RazorScriptFilter]
        public async Task<ActionResult> Js(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<WorkflowForm>(f => f.Route == id);
            var wd = await context.LoadOneAsync<WorkflowDefinition>(f => f.Id == form.WorkflowDefinitionId);


            var buttonOperations = form.FormDesign.FormElementCollection.OfType<Button>()
                .Where(b => b.IsToolbarItem)
                .Where(b => !string.IsNullOrWhiteSpace(b.Operation))
                .Select(b => $"{{ caption :\"{b.Label}\", command : {b.Operation.ToCamelCase()}, icon:\"{b.IconClass}\" }}");

            var commands = form.FormDesign.FormElementCollection.OfType<Button>()
                .Where(b => b.IsToolbarItem)
                .Where(b => !string.IsNullOrWhiteSpace(b.CommandName))
                .Select(b => $"{{ caption :\"{b.Label}\", command : {b.CommandName}, icon:\"{b.IconClass}\" }}");
            var commandsJs = $"[{string.Join(",", commands.Concat(buttonOperations))}]";


            var formId = form.Route + "-form";
            var saveOperation = form.Operation;
            var partialPath = string.IsNullOrWhiteSpace(form.Partial) ? string.Empty : ",'" + form.Partial + "'";
            var partialVariable = string.IsNullOrWhiteSpace(form.Partial) ? string.Empty : ",partial";

            var script = new StringBuilder();
            script.AppendLine(
                $@"
define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
        objectbuilders.system, objectbuilders.validation,
        objectbuilders.dialog, objectbuilders.config, objectbuilders.app {partialPath}],
        function (context, logger, router, system, validation, dialog, config, app {partialVariable}) {{

            var message = ko.observable(),
                errors = ko.observableArray(),
                form = ko.observable(new bespoke.sph.domain.WorkflowForm()),
                partial = partial || {{}},
                i18n = null,
                activate = function () {{
                    var tcs = new $.Deferred();
                    context.loadOneAsync(""WorkflowForm"", ""Route eq '{form.Route}'"")
                       .then(function (f) {{
                           form(f);
                           return $.getJSON(""i18n/"" + config.lang + ""/{form.Route}"");
                       }})
                       .then(function (n) {{
                           i18n = n[0];

                           return context.get(""api/workflow-forms/{form.Id}/activities/{form.Operation}/schema"");
                       }}).then(function (b) {{
                             message(ko.mapping.fromJS(b));
                       }}, function (e) {{
                         if (e.status == 404) {{
                            app.showMessage(""Sorry, but we cannot find any {wd.Name} with location : /api/{wd.Id}/v{wd.Version}"", ""{ConfigurationManager.ApplicationFullName}"", [""OK""]);
                         }}
                       }}).always(function () {{
                           if (typeof partial.activate === ""function"") {{
                               partial.activate(ko.unwrap(message))
                                        .done(tcs.resolve)
                                        .fail(tcs.reject);
                           }}
                           else{{
                            tcs.resolve(true);
                           }}
                       }});
                       return tcs.promise();
            
                }},");



            var operation = wd.GetActivity<ReceiveActivity>(form.Operation);
            if (null != operation)
            {
                var api = GenerateApiOperationCode(wd, operation, form);
                script.Append(api);
                script.Append(",");
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

            foreach (var rule in form.Rules)
            {
                var function = rule.Dehumanize().ToCamelCase();
                script.AppendLine($@"
                    {function} = function(){{

                        var data = ko.mapping.toJSON(message);
                        return context.post(data, ""/api/businessRule/validate?{function}"" );
                        
                }},");
            }

            foreach (var btn in form.FormDesign.FormElementCollection.OfType<Button>().Where(x => !string.IsNullOrWhiteSpace(x.CommandName)))
            {
                var function = btn.CommandName;
                script.AppendLine($@"
                    {function} = function(){{
                        {btn.Command}
                }},");
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

            script.Append(";");

            // viewmodel
            script.AppendLine(@"
            var vm = {
                    partial: partial,");

            foreach (var rule in form.Rules)
            {
                var function = rule.Dehumanize().ToCamelCase();
                script.AppendLine($"   {function} : {function},");

            }
            script.AppendLine(@"    activate: activate,
                                        config: config,
                                        attached: attached,
                                        compositionComplete:compositionComplete,
                                        message: message,
                                        errors: errors,");
            foreach (
                var btn in
                    form.FormDesign.FormElementCollection.OfType<Button>()
                        .Where(b => !string.IsNullOrWhiteSpace(b.CommandName)))
            {
                script.AppendLine($"   {btn.CommandName} : {btn.CommandName},");

            }

            script.AppendLine("     toolbar : {");

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

        private string GenerateSaveCommand(WorkflowForm form, string operationName)
        {
            var script = new StringBuilder();
            script.Append($@"
                saveCommand = function() {{
                    return {operationName}()");
            if (!string.IsNullOrWhiteSpace(form.OperationSuccessCallback))
            {
                script.Append($@".then(function(result){{
                        {form.OperationSuccessCallback}
                        return Task.fromResult(result);
                    }})");
            }
            if (!string.IsNullOrWhiteSpace(form.OperationSuccessMesage))
            {
                script.Append($@".then(function(result){{
                          if(result.success)
                                return app.showMessage(""{form.OperationSuccessMesage}"", [""OK""]);
                            else
                               return Task.fromResult(false);
                    }})");
            }
            if (!string.IsNullOrWhiteSpace(form.OperationSuccessNavigateUrl))
            {
                script.Append($@".then(function(result){{
                        if(result) router.navigate(""{form.OperationSuccessNavigateUrl}"");
                    }})");
            }
            if (!string.IsNullOrWhiteSpace(form.OperationFailureCallback))
            {
                script.Append($@".fail(function(){{
                        {form.OperationFailureCallback}
                    }})");
            }

            script.AppendLine(@"; 
                }");

            return script.ToString();
        }

        private string GenerateApiOperationCode(WorkflowDefinition wd, ReceiveActivity activity, WorkflowForm form)
        {
            var opFunc = activity.Name.ToCamelCase();
            var route = $"/wf/{wd.Id}/v{wd.Version}/{activity.Operation}";

            var script = new StringBuilder();
            script.Append($@"
                {opFunc} = function(){{

                     if (!validation.valid()) {{
                         return Task.fromResult(false);
                     }}

                     var data = ko.mapping.toJSON(message),
                        tcs = new $.Deferred();
                      
                     context.{form.OperationMethod ?? "post"}(data, ""{route}"" )
                         .fail(function(response){{ 
                            var result = response.responseJSON;
                            errors.removeAll();
                            _(result.rules).each(function(v){{
                                errors(v.ValidationErrors);
                            }});
                            logger.error(""There are errors in your message, !!!"");
                            tcs.resolve(false);
                         }})
                         .then(function (result) {{
                            logger.info(result.message);
                            errors.removeAll();
                            tcs.resolve(result);
                         }});
                     return tcs.promise();
                 }},");

            if (!string.IsNullOrWhiteSpace(form.Operation))
            {
                var save = this.GenerateSaveCommand(form, opFunc);
                script.Append(save);
            }
            return script.ToString();
        }
    }
}