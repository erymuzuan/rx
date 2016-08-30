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
        private EntityDefinition m_testEntityDefinition;

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
        private Task<IEnumerable<Class>> GenerateCodeAsync()
        {
            var classes = new List<Class>();
            var record = new Class { Name = this.Entity.ToPascalCase(), Namespace = this.CodeNamespace };
            record.AddNamespaceImport<DateTime, FileInfo, FileHelpers.FieldAlignAttribute>();
            classes.Add(record);

            var recordMembers = this.FieldMappingCollection.Select(x => x.GenerateMember())
                .Select(x => new Property { Code = x.GeneratedCode() });
            record.PropertyCollection.ClearAndAddRange(recordMembers);

            var ed = m_testEntityDefinition;
            if (null == ed)
            {
                var context = new SphDataContext();
                ed = context.LoadOneFromSources<EntityDefinition>(x => x.Name == this.Entity);
            }

            var edType = new Class { Name = ed.TypeName, Namespace = this.CodeNamespace };
            foreach (var member in ed.MemberCollection)
            {
                edType.AddProperty(member.GeneratedCode());
            }

            return Task.FromResult(classes.AsEnumerable());
        }

        public void InitTest(EntityDefinition entityDefinition)
        {
            this.m_testEntityDefinition = entityDefinition;
        }

        private static BuildError GetSourceError(CompilerError er, IList<string> sources)
        {
            var member = string.Empty;
            for (var i = 0; i < er.Line; i++)
            {
                if (sources[i].StartsWith("//exec:"))
                    member = sources[i].Replace("//exec:", string.Empty);
            }
            var message = er.ErrorText;

            try
            {
                return new BuildError(member, message)
                {
                    Code = sources[er.Line - 1],
                    Line = er.Line
                };
            }
            catch (Exception)
            {
                return new BuildError(member, message)
                {
                    Code = "",
                    Line = er.Line
                };
            }

        }
    }
}