using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs.Javascripts;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormRenderers
{
    [Export("ViewModelRenderer", typeof(FormRenderer))]
    [FormRendererMetadata(FormType = typeof(ScreenActivityForm))]
    public class ScreenActivityFormJsViewModelRenderer : FormRenderer
    {
        [Import]
        public ExpressionCompiler ExpressionCompiler { get; set; }

        public override async Task<string> GenerateCodeAsync(IForm form, IProjectProvider project)
        {
            var saf = form as ScreenActivityForm;
            var wd = project as WorkflowDefinition;
            if (null == saf) return null;
            if (null == wd) return null;
            var @class = new ClassDeclaration
            {
                Name = form.Route
            };
            @class.AddDependency("services/datacontext", "context");
            @class.AddDependency("services/logger", "logger");
            @class.AddDependency("plugins/router", "router");
            @class.AddDependency("durandal/system", "system");
            @class.AddDependency("services/validation", "validation");
            @class.AddDependency("services/jsonimportexport", "eximp");
            @class.AddDependency("plugins/dialog", "dialog");
            @class.AddDependency("services/watcher", "watcher");
            @class.AddDependency("services/config", "config");
            @class.AddDependency("durandal/app", "app");
            //if (!string.IsNullOrWhiteSpace(saf.Partial))
            //    @class.AddDependency(saf.Partial, "partial");

            // fields
            @class.AddField("item", "ko.observable(new bespoke.sph.domain." + project.Name + "(system.guid()))");
            @class.AddField("errors", "ko.observableArray()");
            @class.AddField("watching", "ko.observable(false)");
            @class.AddField("id", "ko.observable()");
            @class.AddField("form", "ko.observable(new bespoke.sph.domain.EntityForm())");

            // functions
            @class.AddFunction(this.CreateActivate(wd, saf));
            @class.AddFunction("attached", this.CreateAttached(saf), "view");

            @class.AddFunction("save", this.CreateSave(wd), "$data");

            // button command

            foreach (var btn in form.FormDesign.FormElementCollection.OfType<Button>().Where(x => !string.IsNullOrWhiteSpace(x.CommandName)))
            {
                var snippet = await this.ExpressionCompiler.CompileAsync<object>(btn.Command, project);
                @class.AddFunction(btn.CommandName, "return " + snippet.Code + ";");
            }


            @class.AddReturn("activate");
            @class.AddReturn("config");
            @class.AddReturn("attached");
            @class.AddReturn("item");
            @class.AddReturn("errors");
            @class.AddReturn("save");

            @class.AddReturn("toolbar", this.CreateToolbar(wd, saf));


            return @class.ToString();
        }


        private string CreateSave(WorkflowDefinition entity)
        {
            var saveUrl = string.Format("/{0}/Save", entity.Name);
            var code = new StringBuilder();
            code.AppendLine("    if (!validation.valid()) {");
            code.AppendLine("           return Task.fromResult(false);");
            code.AppendLine("       }");
            //       

            code.AppendLine("     var tcs = new $.Deferred(),");
            code.AppendLine("       data = ko.toJSON(item);");
            //       
            //           

                code.AppendLinf("       context.post(data, \"{0}\")", saveUrl);
                code.AppendLine("               .then(function(result) {");
                code.AppendLine("                   tcs.resolve(result);");
                code.AppendLine("                   item().Id(result.id);");
                code.AppendLinf("                   app.showMessage(\"Your {0} has been successfully saved\", \"{1}\", [\"Ok\"]);", entity.Name, ConfigurationManager.ApplicationFullName);

                code.AppendLine("        });");
            

            code.AppendLine("       return tcs.promise();");


            return code.ToString();
        }


        private FunctionDeclaration CreateActivate(WorkflowDefinition wd, ScreenActivityForm form)
        {
            var code = new StringBuilder();
            //
            code.AppendLine("   id(itemId);");

            code.AppendLine("       var query = String.format(\"Id eq '{0}'\", itemId),");
            code.AppendLine("           tcs = new $.Deferred(),");
            code.AppendLinf("           itemTask = context.loadOneAsync(\"{0}\", query),", wd.Name);
            code.AppendLinf("           formTask = context.loadOneAsync(\"EntityForm\", \"Route eq '{0}'\"),", form.Route);
            code.AppendLinf("           watcherTask = watcher.getIsWatchingAsync(\"{0}\", itemId);", wd.Name);


            code.AppendLine("       $.when(itemTask, formTask, watcherTask).done(function(b,f,w) {");
            code.AppendLine("           if (b) { ");
            code.AppendLine("               var item2 = context.toObservable(b);");
            code.AppendLine("               item(item2);");
            code.AppendLine("           }");
            code.AppendLine("           else {");
            code.AppendLinf("               item(new bespoke.sph.domain.{0}(system.guid()));", wd.WorkflowTypeName);
            code.AppendLine("           }");
            code.AppendLine("           form(f);");
            code.AppendLine("           watching(w);");


            code.AppendLine("       tcs.resolve(true);");

            code.AppendLine("       ");
            code.AppendLine("       });");
            code.AppendLine("       return tcs.promise();");

            var activate = new FunctionDeclaration { Name = "activate", Body = code.ToString() };
            activate.ArgumentCollection.Add("itemId");
            return activate;
        }
        private string CreateAttached(ScreenActivityForm form)
        {
            var code = new StringBuilder();
            code.AppendLine("        // validation");
            code.AppendLinf("        validation.init($('#{0}-form'), form());", form.Route);
            
            return code.ToString();
        }
        private string CreateToolbar(WorkflowDefinition ed, ScreenActivityForm form)
        {
            var code = new StringBuilder();
            code.AppendLine("{");
            
            var buttonOperations = form.FormDesign.FormElementCollection.OfType<Button>()
                .Where(b => b.IsToolbarItem)
                .Where(b => !string.IsNullOrWhiteSpace(b.Operation))
                .Select(b => string.Format("{{ caption :\"{0}\", command : {1}, icon:\"{2}\" }}", b.Label, b.Operation.ToCamelCase(), b.IconClass));

            var commands = form.FormDesign.FormElementCollection.OfType<Button>()
                .Where(b => b.IsToolbarItem)
                .Where(b => !string.IsNullOrWhiteSpace(b.CommandName))
                .Select(b => string.Format("{{ caption :\"{0}\", command : {1}, icon:\"{2}\" }}", b.Label, b.CommandName, b.IconClass));
            var commandsJs = string.Format("[{0}]", string.Join(",", commands.Concat(buttonOperations)));
            code.AppendLinf("       commands : ko.observableArray({0}) ", commandsJs);

            code.Append("      }");
            return code.ToString();
        }


    }
}