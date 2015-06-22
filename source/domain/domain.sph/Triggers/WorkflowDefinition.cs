using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml.Serialization;
using Humanizer;
using Microsoft.CSharp;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition : Entity
    {
        public T GetActivity<T>(string webId) where T : Activity
        {
            return this.ActivityCollection.OfType<T>().Single(w => w.WebId == webId);
        }

        public async Task<Workflow> InitiateAsync(VariableValue[] values = null, ScreenActivity screen = null)
        {
            var typeName = string.Format("{3}.{0},workflows.{1}.{2}", this.WorkflowTypeName, this.Id, this.Version, this.CodeNamespace);
            var type = Type.GetType(typeName);
            if (null == type) throw new InvalidOperationException("Cannot instantiate  " + typeName);

            var initiator = this.GetInitiatorActivity();

            dynamic wf = Activator.CreateInstance(type);
            wf.Name = this.Name;
            wf.WorkflowDefinitionId = this.Id;
            wf.State = "Active";
            wf.WorkflowDefinition = this;

            // set the initial variable's value
            if (null != values)
                foreach (var vv in values)
                {
                    this.SetVariableValue(vv, wf, type);
                }


            if (null != screen)
            {
                wf.VariableValueCollection.ClearAndAddRange(values);
            }
            await wf.ExecuteAsync(initiator.WebId);
            return wf as Workflow;
        }

        private void SetVariableValue(VariableValue vv, Workflow wf, Type type)
        {
            var path = vv.Name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            var prop = type.GetProperty(path[0]);
            if (path.Length == 1)
            {
                prop.SetValue(wf, vv.Value);
                return;
            }

            object dd = wf;
            for (int i = 0; i < path.Length - 1; i++)
            {
                var pname = path[i];
                prop = dd.GetType().GetProperty(pname);
                dd = prop.GetValue(dd);
            }
            prop = dd.GetType().GetProperty(path.Last());
            prop.SetValue(dd, vv.Value);

        }

        public Activity GetInitiatorActivity()
        {
            return this.ActivityCollection.Single(p => p.IsInitiator);
        }

        public BuildValidationResult ValidateBuild()
        {
            var result = new BuildValidationResult();

            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9 -]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must be started with letter.You cannot use symbol or number as first character" });

            if (this.ActivityCollection.Count(a => a.IsInitiator) != 1)
                result.Errors.Add(new BuildError(this.WebId) { Message = "You must have exactly one initiator activity" });

            if (string.IsNullOrWhiteSpace(this.SchemaStoreId))
                result.Errors.Add(new BuildError(this.WebId) { Message = "You must have exactly one schema defined" });


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
            var codes = this.GenerateCode();
            Debug.WriteLineIf(options.IsVerbose, codes);

            var sourceFiles = new List<string>();
            if (string.IsNullOrWhiteSpace(options.SourceCodeDirectory))
            {
                options.SourceCodeDirectory = Path.Combine(ConfigurationManager.UserSourceDirectory, this.Id);
            }
            if (!Directory.Exists(options.SourceCodeDirectory))
                Directory.CreateDirectory(options.SourceCodeDirectory);
            foreach (var @class in codes)
            {
                var cs = Path.Combine(options.SourceCodeDirectory, @class.FileName);
                File.WriteAllText(cs, @class.GetCode());
                sourceFiles.Add(cs);
            }

            using (var provider = new CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.WorkflowCompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, $"workflows.{this.Id}.{this.Version}.dll"),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true
                };

                parameters.ReferencedAssemblies.Add(typeof(Entity).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(int).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(INotifyPropertyChanged).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Expression<>).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(XmlAttributeAttribute).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Net.Mail.SmtpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Net.Http.HttpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(XElement).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Web.HttpResponseBase).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(ConfigurationManager).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Binder).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(ApiController).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(RoutePrefixAttribute).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Net.Http.Formatting.JsonMediaTypeFormatter).Assembly.Location);

                this.ReferencedAssemblyCollection
                    .Select(u => $"{ConfigurationManager.WorkflowCompilerOutputPath}\\{Path.GetFileName(u.Location)}")
                    .Where(File.Exists)
                    .ToList()
                    .ForEach(u => parameters.ReferencedAssemblies.Add(u));

                this.ReferencedAssemblyCollection
                    .Where(u => !File.Exists($"{ConfigurationManager.WorkflowCompilerOutputPath}\\{Path.GetFileName(u.Location)}"))
                    .Select(u => $"{ConfigurationManager.WebPath}\\bin\\{Path.GetFileName(u.Location)}")
                    .Where(File.Exists)
                    .ToList()
                    .ForEach(u => parameters.ReferencedAssemblies.Add(u));
                this.ReferencedAssemblyCollection
                    .Where(u => !File.Exists($"{ConfigurationManager.WebPath}\\bin\\{Path.GetFileName(u.Location)}"))
                    .Where(u => !File.Exists($"{ConfigurationManager.WorkflowCompilerOutputPath}\\{Path.GetFileName(u.Location)}"))
                    .ToList()
                    .ForEach(u => parameters.ReferencedAssemblies.Add(u.Location));
            

                // custom entities
                foreach (var ass in options.ReferencedAssembliesLocation)
                {
                    if (!parameters.ReferencedAssemblies.Contains(ass))
                        parameters.ReferencedAssemblies.Add(ass);
                }
                var result = provider.CompileAssemblyFromFile(parameters, sourceFiles.ToArray());
                var cr = new WorkflowCompilerResult
                {
                    Result = true,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;
                cr.Errors.AddRange(this.GetCompileErrors(result));

                return cr;
            }
        }

        private IEnumerable<BuildError> GetCompileErrors(CompilerResults result)
        {

            var list = from CompilerError er in result.Errors.OfType<CompilerError>()
                       select this.GetSourceError(er, er.FileName);
            return list;
        }

        private BuildError GetSourceError(CompilerError er, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
                return new BuildError(this.WebId, er.ErrorText);

            var sources = File.ReadAllLines(fileName);
            var member = string.Empty;
            for (var i = 0; i < er.Line; i++)
            {
                if (sources[i].Trim().StartsWith("//exec:"))
                    member = sources[i].Trim().Replace("//exec:", string.Empty);
            }
            if (this.ActivityCollection.All(a => a.WebId != member))
            {
                return new BuildError(null, er.ToString())
                {
                    Line = er.Line,
                    Code = er.Line > 1 ? sources[er.Line - 1] : string.Empty
                };

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

            get { return (this.Id.Humanize(LetterCasing.Title).Dehumanize() + "Workflow").Replace("WorkflowWorkflow", "Workflow"); }
        }
        [XmlIgnore]
        [JsonIgnore]
        public string CodeNamespace
        {
            get
            {
                var id = (this.Id.Humanize(LetterCasing.Title).Dehumanize());
                return string.Format("Bespoke.Sph.Workflows_{0}_{1}", id, this.Version);
            }
        }


    }
}