using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Dynamic;
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
        public override async Task<SphCompilerResult> CompileAsync(Solution solution)
        {
            var result = new SphCompilerResult();
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
            script.AppendLine();
            script.AppendLine();
            script.AppendLinf("bespoke.{0}.domain.{1} = function(optionOrWebid){{", jsNamespace, model.Name);
            script.AppendLine(" var system = require('services/system'),");
            script.AppendLine(" model = {");
            script.AppendLinf("     $type : ko.observable(\"{0}.{1}, {2}\"),", model.DefaultNamespace, model.Name, assemblyName);
            script.AppendLine("     Id : ko.observable(\"0\"),");

            var membersDeclarations = model.Members.Select(m => "     " + m.GetMemberDeclaration(jsNamespace));
            script.AppendLine(string.Join(", \r\n", membersDeclarations) + ",");
            script.AppendFormat(AddRemoveChildItemFunctions);
            script.AppendLine("     WebId : ko.observable()");

            script.AppendLine(" };");

            script.AppendLine(OptionsCode);
            script.AppendFormat(ExtendWithPartial, jsNamespace, model.Name);

            script.AppendLine(" return model;");
            script.AppendLine("};");

            var generated = new List<string>();
            // unknown member types
            foreach (var mb in model.Members.Where(m => m.IsComplex && !string.IsNullOrWhiteSpace(m.InferredType)))
            {
                var code = this.GenerateJavascriptClass(mb, jsNamespace, model.DefaultNamespace, assemblyName, generated);
                generated.Add(mb.InferredType);
                script.AppendLine(code);
            }
            // known member types
            foreach (var mb in model.Members.Where(m => m.IsComplex && !string.IsNullOrWhiteSpace(m.TypeName)))
            {
                var code = this.GenerateJavascriptClass(mb, jsNamespace, model.DefaultNamespace, assemblyName, generated);
                generated.Add(mb.Type.Name);
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
                  : member.Type.Name;
            Console.WriteLine("Generating : {0} \t-> {1}", t0, string.Join(", ", generatedTypes));
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
                    : mb.Type.Name;
                generatedTypes.Add(t);
                script.AppendLine(code);
            }
            return script.ToString();
        }
    }

    internal static class JavascriptCodeHelpers
    {
        internal static string GetMemberDeclaration(this Member mb, string jsNamespace, string operand = ":")
        {
            var array = mb.AllowMultiple;
            var unknowComplex = mb.IsComplex && !mb.AllowMultiple &&
                                !string.IsNullOrWhiteSpace(mb.InferredType);
            var knowComplex = mb.IsComplex && !mb.AllowMultiple
                              && !string.IsNullOrWhiteSpace(mb.TypeName);
            if (array)
            {
                return string.Format("{0} {1} ko.observableArray([])", mb.Name, operand);
            }
            if (unknowComplex)
            {
                return string.Format("{0} :{2} ko.observable(new bespoke.{1}.domain.{0}())", mb.Name, jsNamespace, operand);
            }
            if (knowComplex)
            {
                return string.Format("{0} {3} ko.observable(new bespoke.{1}.domain.{2}())", mb.Name, jsNamespace, mb.Type.Name, operand);
            }
            //simple
            return string.Format("{0} {1} ko.observable()", mb.Name, operand);
        }
    }

}