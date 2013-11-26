using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.CSharp;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition : Entity
    {
        public T GetActivity<T>(string webId) where T : Activity
        {
            return this.ActivityCollection.OfType<T>().Single(w => w.WebId == webId);
        }

        public Task<Workflow> InitiateAsync(IEnumerable<VariableValue> values = null, ScreenActivity screen = null)
        {
            var typeName = string.Format("{0},workflows.{1}.{2}", this.WorkflowTypeName, this.WorkflowDefinitionId, this.Version);
            // TODO : load the type and instantiate it
            var type = Type.GetType(typeName);
            if (null == type) throw new InvalidOperationException("Cannot instantiate  " + typeName);

            dynamic wf = Activator.CreateInstance(type);
            wf.Name = this.Name;
            wf.WorkflowDefinitionId = this.WorkflowDefinitionId;
            wf.State = "Active";


            if (null != screen)
            {
                wf.VariableValueCollection.ClearAndAddRange(values);
            }

            return Task.FromResult(wf);
        }

        public ScreenActivity GetInititorScreen()
        {
            return this.ActivityCollection.Single(p => p.IsInitiator) as ScreenActivity;
        }

        public BuildValidationResult ValidateBuild()
        {
            var result = new BuildValidationResult();

            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9 -]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must be started with letter.You cannot use symbol or number as first character" });

            foreach (var variable in this.VariableDefinitionCollection)
            {
                var v = variable.ValidateBuild(this);
                result.Errors.AddRange(v.Errors);
            }

            foreach (var activity in this.ActivityCollection)
            {
                var a = activity.ValidateBuild(this);
                if (null == a) continue;
                result.Errors.AddRange(a.Errors);
            }
            result.Result = !result.Errors.Any();
            return result;
        }


        public WorkflowCompilerResult Compile(CompilerOptions options)
        {
            var code = this.GenerateCode();
            Debug.WriteLineIf(options.IsVerbose, code);

            var sourceFile = string.Empty;
            if (!string.IsNullOrWhiteSpace(options.SourceCodeDirectory))
            {
                sourceFile = Path.Combine(options.SourceCodeDirectory,
                    string.Format("Workflow_{0}_{1}.cs", this.WorkflowDefinitionId, this.Version));
                File.WriteAllText(sourceFile, code);
            }

            using (var provider = new CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.AppSettings["sph:OutputPath"] ?? AppDomain.CurrentDomain.BaseDirectory;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, string.Format("workflows.{0}.{1}.dll", this.WorkflowDefinitionId, this.Version)),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true

                };

                parameters.ReferencedAssemblies.Add(typeof(Entity).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Int32).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(INotifyPropertyChanged).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Expression<>).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(XmlAttributeAttribute).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Net.Mail.SmtpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(XElement).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Web.HttpResponseBase).Assembly.Location);
                foreach (var ass in options.ReferencedAssemblies)
                {
                    parameters.ReferencedAssemblies.Add(ass.Location);
                }
                var result = !string.IsNullOrWhiteSpace(sourceFile) ? provider.CompileAssemblyFromFile(parameters, sourceFile)
                    : provider.CompileAssemblyFromSource(parameters, code);
                var cr = new WorkflowCompilerResult
                {
                    Result = true,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;
                cr.Errors.AddRange(this.GetCompileErrors(result, code));

                return cr;
            }
        }

        private IEnumerable<BuildError> GetCompileErrors(CompilerResults result, string code)
        {
            var temp = Path.GetTempFileName() + ".cs";
            File.WriteAllText(temp, code);
            var sources = File.ReadAllLines(temp);
            var list = (from object er in result.Errors.OfType<CompilerError>()
                        select this.GetSourceError(er as CompilerError, sources));
            File.Delete(temp);

            return list;
        }

        private BuildError GetSourceError(CompilerError er, string[] sources)
        {
            var member = string.Empty;
            for (var i = 0; i < er.Line; i++)
            {
                if (sources[i].StartsWith("//exec:"))
                    member = sources[i].Replace("//exec:", string.Empty);
            }
            var act = this.GetActivity<Activity>(member);
            var message = er.ErrorText;
            if (null != act)
                message = string.Format("[{0}] -< {1} : {2}", act.GetType().Name, act.Name, er.ErrorText);
            return new BuildError(member, message)
            {
                Code = sources[er.Line - 1],
                Line = er.Line
            };

        }

        [XmlIgnore]
        [JsonIgnore]
        public string WorkflowTypeName
        {
            get
            {
                return string.Format("{0}_{1}_{2}", this.Name.Replace(" ", string.Empty), this.WorkflowDefinitionId, this.Version);
            }
        }
        [XmlIgnore]
        [JsonIgnore]
        public string CodeNamespace
        {
            get
            {
                return "Bespoke.Sph.Workflows_" + this.WorkflowDefinitionId + "_" + this.Version;
            }
        }

    }
}