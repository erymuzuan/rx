using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class FormDialogRendererController : BaseSphController
    {
        public async Task<ActionResult> Html(string id)
        {
            var context = new SphDataContext();
            var dialog = await context.LoadOneAsync<FormDialog>(f => f.Route == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Name == dialog.Entity);

            var vm = new DialogRendererViewModel { Dialog = dialog, EntityDefinition = ed };
            return View(vm);
        }

        [RazorScriptFilter]
        public async Task<ActionResult> Js(string id)
        {
            var context = new SphDataContext();
            var dlg = await context.LoadOneAsync<FormDialog>(f => f.Route == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Name == dlg.Entity);

            var model = new DialogRendererViewModel { Dialog = dlg, EntityDefinition = ed };
            var ns = ConfigurationManager.ApplicationName + "_" + model.EntityDefinition.Name.ToCamelCase();
            var typeCtor = $"bespoke.{ns}.domain.{model.EntityDefinition.Name}(system.guid())";

            var script = new StringBuilder();
            script.AppendLine(
                $@"
define([""plugins/dialog"", objectbuilders.datacontext, objectbuilders.system],
        function (dialog, context, system) {{

            var entity = ko.observable(new {typeCtor}),
                errors = ko.observableArray(),
                activate = function () {{
                    // activation, you can also return a promise
                    return true;
                }},");

            foreach (var btn in model.Dialog.DialogButtonCollection)
            {
                script.AppendLine($@"
                    {btn.Text.ToCamelCase()}Click = function(){{
                            dialog.close(this, ""{btn.Text}"");
                }},");
            }
            script.AppendLine(@"
                attached = function(view){
                    // DOM manipulation
                }; ");


            // viewmodel
            script.AppendLine(@"
            var vm = {");
            foreach (var btn in model.Dialog.DialogButtonCollection.Select(x => x.Text.ToCamelCase()))
            {
                script.AppendLine($"    {btn}Click : {btn}Click,");
            }
            script.AppendLine(@"    activate: activate,
                                        attached: attached,
                                        entity : entity,
                                        errors: errors
                    };");


            script.AppendLine(@"
            return vm;
            });");

            return Content(script.ToString(), MimeMapping.GetMimeMapping($"{dlg.Route}.js"), Encoding.UTF8);


        }

    }
}