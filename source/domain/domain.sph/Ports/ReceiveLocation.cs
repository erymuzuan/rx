using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;
using Polly;

namespace Bespoke.Sph.Domain
{
    public interface IReceiveLocation
    {
        bool Start();
        bool Stop();
        void Pause();
        void Resume();
    }

    [DebuggerDisplay("Name = {Name}")]
    [StoreAsSource(HasDerivedTypes = true)]
    public partial class ReceiveLocation : Entity
    {
        [ImportMany(typeof(IBuildDiagnostics))]
        [JsonIgnore]
        [XmlIgnore]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }

        public virtual async Task<BuildValidationResult> ValidateBuildAsync()
        {
            if (null == this.BuildDiagnostics)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.BuildDiagnostics)
                throw new InvalidOperationException($"Fail to initialize MEF for {nameof(ReceiveLocation)}.{nameof(BuildDiagnostics)}");

            var result = this.CanSave();
            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(3, c => TimeSpan.FromMilliseconds(500),
                    (ex, ts) =>
                    {
                        ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(ex));
                    });

            var errorTasks = this.BuildDiagnostics
                .Select(d => policy.ExecuteAndCapture(() => d.ValidateErrorsAsync(this)))
                .Where(x => null != x)
                .Where(x => x.FinalException == null)
                .Select(x => x.Result)
                .ToArray();
            var errors = (await Task.WhenAll(errorTasks)).Where(x => null != x).SelectMany(x => x);

            var warningTasks = this.BuildDiagnostics
                .Select(d => policy.ExecuteAndCapture(() => d.ValidateWarningsAsync(this)))
                .Where(x => null != x)
                .Where(x => x.FinalException == null)
                .Select(x => x.Result)
                .ToArray();
            var warnings = (await Task.WhenAll(warningTasks)).SelectMany(x => x);

            result.Errors.AddRange(errors);
            result.Warnings.AddRange(warnings);

            result.Result = result.Errors.Count == 0;

            return result;
        }
        public BuildValidationResult CanSave()
        {
            var result = new BuildValidationResult();
            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must start with letter.You cannot use symbol or number as first character" });
            if (string.IsNullOrWhiteSpace(this.Name))
                result.Errors.Add(new BuildError(this.WebId, "Name is missing"));

            result.Result = !result.Errors.Any();
            return result;
        }

        public async Task<WorkflowCompilerResult> CompileAsync(ReceivePort port)
        {
            var options = new CompilerOptions { IsDebug = true };
            if (this.GenerateExecutable())
            {
                var config = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""sph:ApplicationName"" value=""{ConfigurationManager.ApplicationName}"" />
  </appSettings>
</configuration>";
                File.WriteAllText($"{ConfigurationManager.CompilerOutputPath}\\{AssemblyName}.config", config);
            }
            return await this.CompileAsync(port, options);
        }


        private async Task<WorkflowCompilerResult> CompileAsync(ReceivePort port, CompilerOptions options)
        {
            var classes = (await this.GenerateClassesAsync(port)).ToArray();
            var sources = classes.Select(x => x.Save($"ReceiveLocation.{TypeName}")).ToArray();

            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.CompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, this.AssemblyName),
                    GenerateExecutable = this.GenerateExecutable(),
                    IncludeDebugInformation = true

                };
                parameters.ReferencedAssemblies.Add(typeof(Entity).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(int).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Expression<>).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Trigger).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(INotifyPropertyChanged).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(FileHelpers.DelimitedField).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(JsonIgnoreAttribute).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(DomainObject).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Net.Http.HttpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add($"{ConfigurationManager.CompilerOutputPath}\\{port.AssemblyName}");

                foreach (var ass in options.ReferencedAssembliesLocation)
                {
                    parameters.ReferencedAssemblies.Add(ass);
                }
                foreach (var ra in this.ReferencedAssemblyCollection)
                {
                    parameters.ReferencedAssemblies.Add(ra.Location);
                }

                var result = provider.CompileAssemblyFromFile(parameters, sources);
                var cr = new WorkflowCompilerResult
                {
                    Result = result.Errors.Count == 0,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                var errors = from CompilerError x in result.Errors
                             select new BuildError(this.WebId, x.ErrorText)
                             {
                                 Line = x.Line,
                                 FileName = x.FileName
                             };
                cr.Errors.AddRange(errors);
                return cr;
            }
        }

        [JsonIgnore]
        public string AssemblyExtension => this.GenerateExecutable() ? ".exe" : ".dll";
        [JsonIgnore]
        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.ReceiveLocations";
        [JsonIgnore]
        public string AssemblyName => $"{ConfigurationManager.ApplicationName}.ReceiveLocation.{TypeName}{AssemblyExtension}";
        [JsonIgnore]
        public string PdbName =>      $"{ConfigurationManager.ApplicationName}.ReceiveLocation.{TypeName}.pdb";
        [JsonIgnore]
        public string TypeName => Name.ToPascalCase();
        [JsonIgnore]
        public string TypeFullName => $"{CodeNamespace}.{TypeName}, {AssemblyName.Replace(AssemblyExtension, "")}";


        protected virtual bool GenerateExecutable()
        {
            return false;
        }
        public virtual async Task<IEnumerable<Class>> GenerateClassesAsync(ReceivePort port)
        {
            var list = new List<Class>();
            var watcher = new Class { Name = Name.ToPascalCase(), Namespace = CodeNamespace, BaseClass = "IReceiveLocation, IDisposable" };
            list.Add(watcher);
            await InitializeServiceClassAsync(watcher, port);
            watcher.AddMethod(GenerateStartMethod(port).Result);
            watcher.AddMethod(GenerateStopMethod(port).Result);
            watcher.AddMethod(GeneratePauseMethod(port).Result);
            watcher.AddMethod(GenerateResumeMethod(port).Result);

            return list;
        }

        protected virtual Task InitializeServiceClassAsync(Class watcher, ReceivePort port)
        {
            return Task.FromResult(new Method { Code = "public bool Stop(){ return true;}" });
        }
        protected virtual Task<Method> GenerateStopMethod(ReceivePort port)
        {
            return Task.FromResult(new Method { Code = "public bool Stop(){ return true;}" });
        }
        protected virtual Task<Method> GenerateStartMethod(ReceivePort port)
        {
            return Task.FromResult(new Method { Code = "public bool Start(){ return true;}" });
        }
        protected virtual Task<Method> GeneratePauseMethod(ReceivePort port)
        {
            return Task.FromResult(new Method { Code = "public void Pause(){}" });
        }
        protected virtual Task<Method> GenerateResumeMethod(ReceivePort port)
        {
            return Task.FromResult(new Method { Code = "public void Resume(){}" });
        }

        public virtual Task<string> PackageAsync()
        {
            throw new NotImplementedException();
        }
    }
}