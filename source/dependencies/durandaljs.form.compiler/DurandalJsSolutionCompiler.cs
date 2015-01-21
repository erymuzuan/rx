using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export("SolutionCompiler", typeof(SolutionCompiler))]
    [SolutionCompilerMetadata(Name = "DurandalJs")]
    public class DurandalJsSolutionCompiler : SolutionCompiler
    {
        public override async Task<WorkflowCompilerResult> CompileAsync(Solution solution)
        {
            var result = new WorkflowCompilerResult();
            var models = new List<string>();
            foreach (var pm in solution.ProjectMetadataCollection)
            {
                var project = await solution.LoadProjectAsync(pm);
                var model = await project.GetModelAsync();
                if (null == model) continue;
                var javascriptModel = this.GenerateModel(model);
                models.Add(javascriptModel);
            }
            result.Outputs = models.ToArray();
            return result;
        }

        private string GenerateModel(IProjectModel model)
        {
            var jsNamespace = ConfigurationManager.ApplicationName + "_" + model.Id;
            var assemblyName = ConfigurationManager.ApplicationName + "." + model.Name;
            var script = new StringBuilder();
            script.AppendLine("var bespoke = bespoke ||{};");
            script.AppendLinf("bespoke.{0} = bespoke.{0} ||{{}};", jsNamespace);
            script.AppendLinf("bespoke.{0}.domain = bespoke.{0}.domain ||{{}};", jsNamespace);

            script.AppendLinf("bespoke.{0}.domain.{1} = function(optionOrWebid){{", jsNamespace, model.Name);
            script.AppendLine(" var system = require('services/system'),");
            script.AppendLine(" model = {");
            script.AppendLinf("     $type : ko.observable(\"{0}.{1}, {2}\"),", model.DefaultNamespace, model.Name, assemblyName);
            script.AppendLine("     Id : ko.observable(\"0\"),");
            foreach (var item in model.Members)
            {
                if (item.Type == typeof(Array))
                    script.AppendLinf("     {0}: ko.observableArray([]),", item.Name);
                else if (item.Type == typeof(object))
                    script.AppendLinf("     {0}: ko.observable(new bespoke.{1}.domain.{0}()),", item.Name, jsNamespace);
                else
                    script.AppendLinf("     {0}: ko.observable(),", item.Name);
            }
            script.AppendFormat(@"
    addChildItem : function(list, type){{
                        return function(){{                          
                            list.push(new type(system.guid()));
                        }}
                    }},
            
   removeChildItem : function(list, obj){{
                        return function(){{
                            list.remove(obj);
                        }}
                    }},
");
            script.AppendLine("     WebId: ko.observable()");

            script.AppendLine(" };");

            script.AppendLine(@" 
             if (optionOrWebid && typeof optionOrWebid === ""object"") {
                for (var n in optionOrWebid) {
                    if (typeof model[n] === ""function"") {
                        model[n](optionOrWebid[n]);
                    }
                }
            }
            if (optionOrWebid && typeof optionOrWebid === ""string"") {
                model.WebId(optionOrWebid);
            }");

            script.AppendFormat(@"

                if (bespoke.{0}.domain.{1}Partial) {{
                    return _(model).extend(new bespoke.{0}.domain.{1}Partial(model));
                }}", jsNamespace, model.Name);

            script.AppendLine(" return model;");
            script.AppendLine("};");
            foreach (var item in model.Members.Where(m => m.Type == typeof(object) || m.Type == typeof(Array)))
            {
                var code = item.GenerateJavascriptClass(jsNamespace, model.DefaultNamespace, assemblyName);
                script.AppendLine(code);
            }
            return script.ToString();
        }

    }

}