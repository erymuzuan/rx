using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Name = {Name}")]
    [StoreAsSource(HasDerivedTypes = true)]
    public partial class ReceiveLocation : Entity
    {

        public async Task<WorkflowCompilerResult> CompileAsync(ReceivePort port)
        {
            var options = new CompilerOptions { IsDebug = true };
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
                    GenerateExecutable = false,
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
        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.ReceiveLocations";
        [JsonIgnore]
        public string AssemblyName => $"{ConfigurationManager.ApplicationName}.ReceiveLocation.{TypeName}.dll";
        [JsonIgnore]
        public string PdbName => $"{ConfigurationManager.ApplicationName}.ReceiveLocation.{TypeName}.{Id}.pdb";
        [JsonIgnore]
        public string TypeName => Name.ToPascalCase();
        [JsonIgnore]
        public string TypeFullName => $"{CodeNamespace}.{TypeName}, {AssemblyName.Replace(".dll", "")}";


        public virtual Task<IEnumerable<Class>> GenerateClassesAsync(ReceivePort port)
        {
            throw new System.NotImplementedException();
        }
    }
}