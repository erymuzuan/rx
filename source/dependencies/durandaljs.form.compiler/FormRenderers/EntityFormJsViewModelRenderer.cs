using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs.Javascripts;
using Humanizer;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormRenderers
{
    [Export("ViewModelRenderer", typeof(FormRenderer))]
    [FormRendererMetadata(FormType = typeof(EntityForm))]
    public class EntityFormJsViewModelRenderer : FormRenderer
    {
        [Import]
        public ExpressionCompiler ExpressionCompiler { get; set; }

        public override async Task<string> GenerateCodeAsync(IForm form, IProjectProvider project)
        {
            var ef = form as EntityForm;
            var entity = project as EntityDefinition;
            if (null == ef) return null;
            if (null == entity) return null;
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
            if (!string.IsNullOrWhiteSpace(ef.Partial))
                @class.AddDependency(ef.Partial, "partial");

            // fields
            @class.AddField("item", "ko.observable(new bespoke.sph.domain." + project.Name + "(system.guid()))");
            @class.AddField("errors", "ko.observableArray()");
            @class.AddField("watching", "ko.observable(false)");
            @class.AddField("id", "ko.observable()");
            @class.AddField("form", "ko.observable(new bespoke.sph.domain.EntityForm())");

            // functions
            @class.AddFunction(this.CreateActivate(entity, ef));
            @class.AddFunction("attached", this.CreateAttached(ef), "view");
            if (ef.IsRemoveAvailable)
                @class.AddFunction("remove", this.Createremove(entity, ef), "$data");
            if (ef.IsPrintAvailable)
                @class.AddFunction("print", this.CreateAttached(ef));

            if (ef.IsImportAvailable)
                @class.FunctionCollection.Add(new FunctionDeclaration { Name = "import", Body = this.CreateImport(entity, ef) });
            if (ef.IsExportAvailable)
                @class.FunctionCollection.Add(new FunctionDeclaration { Name = "export", Body = this.CreateExport(entity, ef) });
            if (ef.IsEmailAvailable)
                @class.FunctionCollection.Add(new FunctionDeclaration { Name = "email", Body = this.CreateEmail(entity, ef) });

            foreach (var op in entity.EntityOperationCollection)
            {
                var operation = op.Name.ToCamelCase();
                var function = new FunctionDeclaration { Name = operation, Body = this.CreateOperationBody(project, op) };
                function.ArgumentCollection.Add("$data");

                @class.FunctionCollection.Add(function);
                @class.AddReturn(operation);
            }
            @class.AddFunction("save", this.CreateSave(entity, ef), "$data");

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

            @class.AddReturn("toolbar", this.CreateToolbar(entity, ef));
            if (!string.IsNullOrWhiteSpace(ef.Partial))
                @class.AddReturn("partial");

            return @class.ToString();
        }

        private string CreateExport(EntityDefinition entity, EntityForm ef)
        {
            return "//CreateExport" + entity + ef;
        }

        private string CreateEmail(EntityDefinition entity, EntityForm ef)
        {
            return "//CreateEmail" + entity + ef;
        }

        private string CreateImport(EntityDefinition entity, EntityForm ef)
        {

            return "//CreateImport" + entity + ef;
        }
        private string CreateSave(EntityDefinition entity, EntityForm ef)
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

            if (ef.Rules.Any())
            {
                var validateUrl = string.Format("/business-rule/{0}/validate/{1}", entity.Name, string.Join(";", ef.Rules.Select(r => r.Dehumanize())));
                //       
                code.AppendLinf("       context.post(data, \"{0}\")", validateUrl);
                code.AppendLine("           .then(function(result) {");
                code.AppendLine("               if(result.success){");
                code.AppendLinf("                   context.post(data, \"{0}\")", saveUrl);
                code.AppendLine("                       .then(function(sr) {");
                code.AppendLine("                           tcs.resolve(sr);");
                code.AppendLine("                           item().Id(sr.id);");
                code.AppendLinf("                           app.showMessage(\"Your {0} has been successfully saved\", \"{1}\", [\"Ok\"]);", entity.Name, ConfigurationManager.ApplicationFullName);

                code.AppendLine("                       });");

                code.AppendLine("               }");                //  
                code.AppendLine("               else {");
                code.AppendLine("                   var ve = _(result.validationErrors).map(function(v){ return { Message : v.message}; });");
                code.AppendLine("                   errors(ve);");
                code.AppendLine("                   logger.error(\"There are errors in your entity, !!!\");");
                code.AppendLine("                   tcs.resolve(result);");
                code.AppendLine("               }");
                code.AppendLine("           });");
            }
            else
            {
                code.AppendLinf("       context.post(data, \"{0}\")", saveUrl);
                code.AppendLine("               .then(function(result) {");
                code.AppendLine("                   tcs.resolve(result);");
                code.AppendLine("                   item().Id(result.id);");
                code.AppendLinf("                   app.showMessage(\"Your {0} has been successfully saved\", \"{1}\", [\"Ok\"]);", entity.Name, ConfigurationManager.ApplicationFullName);

                code.AppendLine("        });");
            }

            code.AppendLine("       return tcs.promise();");


            return code.ToString();
        }
        private string Createremove(EntityDefinition entity, EntityForm ef)
        {
            //   remove = function() {
            //       var tcs = new $.Deferred();
            //       $.ajax({
            //           type: "DELETE",
            //           url: "/@(Model.Project.Name)/Remove/" + entity().Id(),
            //           contentType: "application/json; charset=utf-8",
            //           dataType: "json",
            //           error: tcs.reject,
            //           success: function() {
            //               tcs.resolve(true);
            //               app.showMessage("Your item has been successfully removed", "Removed", ["OK"])
            //                 .done(function () {
            //                     window.location = "#@(Model.Project.Name.ToLowerInvariant())";
            //                 });
            //           }
            //       });


            //       return tcs.promise();
            //   }

            return "//Createremove" + entity + ef;
        }

        private FunctionDeclaration CreateActivate(EntityDefinition ed, EntityForm form)
        {
            var code = new StringBuilder();
            //
            code.AppendLine("   id(itemId);");

            code.AppendLine("       var query = String.format(\"Id eq '{0}'\", itemId),");
            code.AppendLine("           tcs = new $.Deferred(),");
            code.AppendLinf("           itemTask = context.loadOneAsync(\"{0}\", query),", ed.Name);
            code.AppendLinf("           formTask = context.loadOneAsync(\"EntityForm\", \"Route eq '{0}'\"),", form.Route);
            code.AppendLinf("           watcherTask = watcher.getIsWatchingAsync(\"{0}\", itemId);", ed.Name);


            code.AppendLine("       $.when(itemTask, formTask, watcherTask).done(function(b,f,w) {");
            code.AppendLine("           if (b) { ");
            code.AppendLine("               var item2 = context.toObservable(b);");
            code.AppendLine("               item(item2);");
            code.AppendLine("           }");
            code.AppendLine("           else {");
            code.AppendLinf("               item(new bespoke.sph.domain.{0}(system.guid()));", ed.Name);
            code.AppendLine("           }");
            code.AppendLine("           form(f);");
            code.AppendLine("           watching(w);");
            if (!string.IsNullOrWhiteSpace(form.Partial))
            {
                code.AppendLine("           if(typeof partial.activate === \"function\"){");
                code.AppendLine("               var pt = partial.activate(item());");
                code.AppendLine("               if(typeof pt.done === \"function\"){");
                code.AppendLine("                   pt.done(tcs.resolve);");
                code.AppendLine("               }");
                code.AppendLine("               else{");
                code.AppendLine("                   tcs.resolve(true);");
                code.AppendLine("               }");
                code.AppendLine("           }");
            }
            else
            {
                code.AppendLine("       tcs.resolve(true);");
            }
            code.AppendLine("       ");
            code.AppendLine("       });");
            code.AppendLine("       return tcs.promise();");

            var activate = new FunctionDeclaration { Name = "activate", Body = code.ToString() };
            activate.ArgumentCollection.Add("itemId");
            return activate;
        }
        private string CreateAttached(EntityForm form)
        {
            var code = new StringBuilder();
            code.AppendLine("        // validation");
            code.AppendLinf("        validation.init($('#{0}-form'), form());", form.Route);


            if (!string.IsNullOrWhiteSpace(form.Partial))
            {
                code.AppendLine("        if( typeof partial.attached === \"function\"){");
                code.AppendLine("           partial.attached(view);");
                code.AppendLine("         }");

            }
            return code.ToString();
        }
        private string CreateToolbar(EntityDefinition ed, EntityForm form)
        {
            var code = new StringBuilder();
            code.AppendLine("{");

            if (form.IsEmailAvailable)
            {
                code.AppendLine("       emailCommand : {");
                code.AppendLinf("           entity : \"{0}\",", ed.Name);
                code.AppendLine("           id :id");
                code.AppendLine("       },");
            }
            if (form.IsPrintAvailable)
            {
                code.AppendLine("      printCommand :{ ");
                code.AppendLinf("           entity : \"{0}\"", ed.Name);
                code.AppendLine("           id :id");
                code.AppendLine("       },");
            }
            if (form.IsRemoveAvailable)
            {
                code.AppendLine("      removeCommand :remove, ");
                code.AppendLine("      canExecuteRemoveCommand : ko.computed(function(){");
                code.AppendLine("           return item().Id();");
                code.AppendLine("       }),");
            }


            if (form.IsWatchAvailable)
            {
                code.AppendLine("       watchCommand: function() {");
                code.AppendLinf("           return watcher.watch(\"{0}\", item().Id())", ed.Name);
                code.AppendLine("               .done(function(){");
                code.AppendLine("                   watching(true);");
                code.AppendLine("               });");
                code.AppendLine("       },");


                code.AppendLine("       unwatchCommand: function() {");
                code.AppendLinf("           return watcher.unwatch(\"{0}\", item().Id())", ed.Name);
                code.AppendLine("               .done(function(){");
                code.AppendLine("                   watching(false);");
                code.AppendLine("               });");
                code.AppendLine("       },");
                code.AppendLine("       watching: watching,");


                if (!string.IsNullOrWhiteSpace(form.Operation))
                {
                    code.AppendLinf("       saveCommand : {0},", form.Operation.ToCamelCase());
                }
            }

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

        private string CreateOperationBody(IProjectProvider project, EntityOperation op)
        {
            var code = new StringBuilder();

            //         
            code.AppendLine("       if (!validation.valid()) {");
            code.AppendLine("          return Task.fromResult(false);");
            code.AppendLine("       }");
            code.AppendLine("       ");
            //         
            //             
            code.AppendLine("       var tcs = new $.Deferred(),");
            code.AppendLine("           data = ko.mapping.toJSON(item);");
            code.AppendLine("       ");

            code.AppendLinf("       context.post(data, \"{0}/{1}\")", project.Id, op.Name);
            code.AppendLine("       .then(function (result) {");
            code.AppendLine("           if (result.success) {");
            code.AppendLine("               logger.info(result.message);");
            code.AppendLine("               item().Id(result.id);");
            code.AppendLine("               errors.removeAll();");
            code.AppendLine(this.GetConfirmationMessage(op));
            code.AppendLine("           } else {");
            code.AppendLine("               errors.removeAll();");
            code.AppendLine("               _(result.rules).each(function(v){");
            code.AppendLine("               errors(v.ValidationErrors);");
            code.AppendLine("           });");
            code.AppendLine("           ");
            code.AppendLine("           logger.error(\"There are errors in your entity, !!!\");");
            code.AppendLine("           }");
            code.AppendLine("           tcs.resolve(result);");
            code.AppendLine("       });");
            code.AppendLine();
            code.AppendLine("       return tcs.promise();");
            return code.ToString();
        }

        public string GetConfirmationMessage(EntityOperation op)
        {
            var nav = string.Empty;
            if (!string.IsNullOrWhiteSpace(op.NavigateSuccessUrl))
            {
                nav = string.Format("window.location=\"/sph#{0}\";", op.NavigateSuccessUrl);
                if (op.NavigateSuccessUrl.StartsWith("="))
                {
                    nav = "window.location" + op.NavigateSuccessUrl;
                }
                if (string.IsNullOrWhiteSpace(op.SuccessMessage))
                    return nav;
            }

            if (string.IsNullOrWhiteSpace(op.SuccessMessage)
                && string.IsNullOrWhiteSpace(op.NavigateSuccessUrl))
                return string.Empty;

            if (!op.ShowSuccessMessage) return nav;

            return string.Format(@" 
                                    app.showMessage(""{0}"", ""{1}"", [""OK""])
	                                    .done(function () {{
                                            {2}
	                                    }});
                                 ", op.SuccessMessage, ConfigurationManager.ApplicationFullName, nav);
        }
    }
}