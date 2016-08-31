using System;
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
    [DebuggerDisplay("Name = {Name}, Formatter={Formatter}")]
    [StoreAsSource(HasDerivedTypes = true)]
    public partial class ReceivePort : Entity
    {
        public async Task<WorkflowCompilerResult> CompileAsync()
        {
            var options = new CompilerOptions
            {
                IsDebug = true,
                SourceCodeDirectory = $"{ConfigurationManager.GeneratedSourceDirectory}\\ReceivePort.{this.Name}"
            };
            if (!Directory.Exists(options.SourceCodeDirectory))
                Directory.CreateDirectory(options.SourceCodeDirectory);
            return await this.CompileAsync(options);
        }
        private async Task<WorkflowCompilerResult> CompileAsync(CompilerOptions options)
        {
            var classes = (await this.GenerateCodeAsync()).ToArray();
            var sources = classes.Select(x => x.Save($"ReceivePort{TypeName}")).ToArray();

            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.CompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, $"receive.port.{this.Id}.dll"),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true

                };
                var edDll = $"{ConfigurationManager.ApplicationName}.{this.Entity}.dll";
                options.ReferencedAssembliesLocation.Add(Path.Combine(ConfigurationManager.CompilerOutputPath, edDll));

                parameters.ReferencedAssemblies.Add(typeof(Entity).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(int).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Expression<>).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Trigger).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(INotifyPropertyChanged).Assembly.Location);

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
                    Result = true,
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
        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.ReceivePorts";
        [JsonIgnore]
        public string AssemblyName => $"{ConfigurationManager.ApplicationName}.ReceivePort.{Entity}.{Id}.dll";
        [JsonIgnore]
        public string PdbName => $"{ConfigurationManager.ApplicationName}.ReceivePort.{Entity}.{Id}.pdb";
        [JsonIgnore]
        public string TypeName => Name.ToPascalCase();
        [JsonIgnore]
        public string TypeFullName => $"{CodeNamespace}.{TypeName}, {AssemblyName.Replace(".dll", "")}";

        private void ExtractClasses(ICollection<Class> list, TextFieldMapping field)
        {
            var item = new Class { Name = field.TypeName.ToPascalCase(),Namespace = this.CodeNamespace};
            item.AddNamespaceImport<DateTime, FileInfo, FileHelpers.FieldAlignAttribute, JsonIgnoreAttribute>();
            var fieldMembers = field.FieldMappingCollection.Select(x => x.GenerateMember())
            .Select(x => new Property { Code = x.GeneratedCode() });
            item.PropertyCollection.AddRange(fieldMembers);
            list.Add(item);
            foreach (var f in field.FieldMappingCollection.Where(x => x.IsComplex))
            {
                ExtractClasses(list, f);
            }
        }

        public Task<IEnumerable<Class>> GenerateCodeAsync()
        {
            var classes = new List<Class>();
            var record = new Class { Name = this.Entity.ToPascalCase(), Namespace = this.CodeNamespace };
            record.AddNamespaceImport<DateTime, FileInfo, FileHelpers.FieldAlignAttribute, JsonIgnoreAttribute>();
            classes.Add(record);

            var recordMembers = this.FieldMappingCollection.Select(x => x.GenerateMember())
                .Select(x => new Property { Code = x.GeneratedCode() });
            record.PropertyCollection.ClearAndAddRange(recordMembers);

            foreach (var complexField in this.FieldMappingCollection.Where(x => x.IsComplex))
            {
                ExtractClasses(classes, complexField);
            }


            return Task.FromResult(classes.AsEnumerable());
        }
    }
}