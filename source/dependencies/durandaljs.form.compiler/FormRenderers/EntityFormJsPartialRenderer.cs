using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs.Javascripts;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormRenderers
{
    [Export("PartialRenderer", typeof(FormRenderer))]
    [FormRendererMetadata(FormType = typeof(EntityForm))]
    public class EntityFormJsPartialRenderer : FormRenderer
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
            var activate = await this.CreateActivate(entity, ef);
            if (null != activate)
                @class.AddFunction(activate);

            @class.AddFunction("attached", this.CreateAttached(ef), "view");


            @class.AddReturn("activate");
            @class.AddReturn("attached");

            return @class.ToString();
        }


        private async Task<FunctionDeclaration> CreateActivate(EntityDefinition ed, EntityForm form)
        {
            var code = "TODO : compiled activate method for " + ed.Name + form.Route;

            var cs = form.PartialActivate;
            if (string.IsNullOrWhiteSpace(cs)) return null;

            var compiler = new StatementCompiler();
            ObjectBuilder.ComposeMefCatalog(compiler);
            if (cs.Contains("await "))
            {
                var result = await compiler.CompileAsync<Task<bool>>(cs, ed);
                if (result.Success)
                    code = result.Code;
                else
                    result.DiagnosticCollection.ForEach(Console.WriteLine);
            }
            else
            {
                var result = await compiler.CompileAsync<bool>(cs, ed);
                if (result.Success)
                    code = result.Code;
                else
                    result.DiagnosticCollection.ForEach(Console.WriteLine);

            }

            var activate = new FunctionDeclaration { Name = "activate", Body = code };
            activate.ArgumentCollection.Add("item");
            return activate;
        }
        private string CreateAttached(EntityForm form)
        {
            var code = "TODO : compiled attached method " + form;

            return code;
        }
    }
}