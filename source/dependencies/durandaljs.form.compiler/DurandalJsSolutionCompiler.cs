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

        #region "code snippets"

        private const string OptionsCode = @" 
             if (optionOrWebid && typeof optionOrWebid === ""object"") {
                for (var n in optionOrWebid) {
                    if (typeof model[n] === ""function"") {
                        model[n](optionOrWebid[n]);
                    }
                }
            }
            if (optionOrWebid && typeof optionOrWebid === ""string"") {
                model.WebId(optionOrWebid);
            }";

        private const string AddRemoveChildItemFunctions = @"
    addChildItem : function(list, type){{
                        return function(){{
                            list.push(new childType(system.guid()));
                        }}
                    }},
            
   removeChildItem : function(list, obj){{
                        return function(){{
                            list.remove(obj);
                        }}
                    }},
";

        private const string ExtendWithPartial = @"

                if (bespoke.{0}.domain.{1}Partial) {{
                    return _(model).extend(new bespoke.{0}.domain.{1}Partial(model));
                }}";

        #endregion


        private string GenerateModel(IProjectModel model)
        {
            var jsNamespace = ConfigurationManager.ApplicationName + "_" + model.Id.Replace("-", "");
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
                if (item.AllowMultiple)
                    script.AppendLinf("     {0} : ko.observableArray([]),", item.Name);
                else if (item.IsComplex && !item.AllowMultiple && !string.IsNullOrWhiteSpace(item.InferredType))
                    script.AppendLinf("     {0} : ko.observable(new bespoke.{1}.domain.{0}()),", item.Name, jsNamespace);
                else if (Member.NativeTypes.Contains(item.Type))
                    script.AppendLinf("     {0}: ko.observable(),", item.Name);
            }
            script.AppendFormat(AddRemoveChildItemFunctions);
            script.AppendLine("     WebId: ko.observable()");

            script.AppendLine(" };");

            script.AppendLine(OptionsCode);
            script.AppendFormat(ExtendWithPartial, jsNamespace, model.Name);

            script.AppendLine(" return model;");
            script.AppendLine("};");
            foreach (var item in model.Members.Where(m => m.IsComplex && !string.IsNullOrWhiteSpace(m.InferredType)))
            {
                var code = this.GenerateJavascriptClass(item, jsNamespace, model.DefaultNamespace, assemblyName);
                script.AppendLine(code);
            }
            return script.ToString();
        }


        public string GenerateJavascriptClass(Member member, string jsNamespace, string codeNamespace, string assemblyName, IList<string> generatedTypes = null)
        {
            if (null == generatedTypes)
                generatedTypes = new List<string>();
            var t0 = member.IsComplex && !string.IsNullOrWhiteSpace(member.InferredType)
                  ? member.InferredType
                  : member.Type.ToCSharp();
            if (generatedTypes.Contains(t0)) return string.Empty;


            var script = new StringBuilder();
            var name = t0.Replace("ObjectCollection`1", "");

            script.AppendLinf("bespoke.{0}.domain.{1} = function(optionOrWebid){{", jsNamespace, name);
            script.AppendLine(" var model = {");
            script.AppendLinf("     $type : ko.observable(\"{0}.{1}, {2}\"),", codeNamespace, name,
                assemblyName);
            foreach (var mb in member.MemberCollection)
            {
                if (Member.NativeTypes.Contains(mb.Type))
                    script.AppendLinf("     {0}: ko.observable(),", mb.Name);
                else if (mb.AllowMultiple)
                    script.AppendLinf("     {0}: ko.observableArray([]),", mb.Name);
                else
                    script.AppendLinf("     {0}: ko.observable(new bespoke.{1}.domain.{2}()),", mb.Name, jsNamespace, mb.Type.Name);
            }
            script.AppendFormat(AddRemoveChildItemFunctions);
            script.AppendLine("     WebId: ko.observable()");
            script.AppendLine(" };");
            script.AppendLine(OptionsCode);
            script.AppendFormat(ExtendWithPartial, jsNamespace, member.Name.Replace("Collection", string.Empty));

            script.AppendLine(" return model;");
            script.AppendLine("};");


            foreach (var mb in member.MemberCollection.Where(m => !Member.NativeTypes.Contains(m.Type)))
            {
                var code = this.GenerateJavascriptClass(mb, jsNamespace, codeNamespace, assemblyName, generatedTypes);
                var t = mb.IsComplex && !string.IsNullOrWhiteSpace(mb.InferredType)
                    ? mb.InferredType
                    : mb.Type.ToCSharp();
                generatedTypes.Add(t);
                script.AppendLine(code);
            }
            return script.ToString();
        }
    }

}