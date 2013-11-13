using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.CSharp;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition : Entity
    {
        public Task<Workflow> InitiateAsync(IEnumerable<VariableValue> values = null, ScreenActivity screen = null)
        {
            var wf = new Workflow
            {
                Name = this.Name,
                WorkflowDefinitionId = this.WorkflowDefinitionId,
                State = "Active"

            };
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
            // TODO , #1050
            foreach (var variable in this.VariableDefinitionCollection)
            {
                var result1 = variable.ValidateBuild();
            }
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
                var parameters = new CompilerParameters
                {
                    OutputAssembly = string.Format("workflows.{0}.{1}.dll", this.WorkflowDefinitionId, this.Version),
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
                foreach (var er in result.Errors)
                {
                    cr.Errors.Add(new CompilerMessage
                    {
                        IsError = true,
                        Error = er
                    });
                }


                return cr;
            }
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

    public class BuildValidationResult
    {
        public bool Result { get; set; }
        private readonly ObjectCollection<BuildError> m_errors = new ObjectCollection<BuildError>();

        public ObjectCollection<BuildError> Errors
        {
            get { return m_errors; }
        } 
       // prop
    }

    public class BuildError
    {
        public string Message { get; set; }
    }
}