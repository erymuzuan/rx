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
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Name = {Name}, Formatter={Formatter}")]
    [StoreAsSource(HasDerivedTypes = true)]
    public partial class ReceivePort : Entity
    {
        public async Task<WorkflowCompilerResult> CompileAsync()
        {
            var options = new CompilerOptions { IsDebug = true };
            return await this.CompileAsync(options);
        }

        private async Task<WorkflowCompilerResult> CompileAsync(CompilerOptions options)
        {
            var classes = (await this.GenerateCodeAsync()).ToArray();
            var sources = classes.Select(x => x.Save($"ReceivePort.{TypeName}")).ToArray();

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
        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.ReceivePorts";
        [JsonIgnore]
        public string AssemblyName => $"{ConfigurationManager.ApplicationName}.ReceivePort.{TypeName}.dll";
        [JsonIgnore]
        public string PdbName =>      $"{ConfigurationManager.ApplicationName}.ReceivePort.{TypeName}.pdb";
        [JsonIgnore]
        public string TypeName => Name.ToPascalCase();
        [JsonIgnore]
        public string TypeFullName => $"{CodeNamespace}.{TypeName}, {AssemblyName.Replace(".dll", "")}";

        private void ExtractClasses(ICollection<Class> list, TextFieldMapping field)
        {
            var item = new Class { Name = field.TypeName.ToPascalCase(), Namespace = this.CodeNamespace };
            item.AttributeCollection.Add(TextFormatter.GetRecordAttribute());
            item.AddNamespaceImport<DateTime, FileInfo, FileHelpers.FieldAlignAttribute, JsonIgnoreAttribute>();
            item.AddNamespaceImport<DomainObject, IEnumerable<object>, System.Globalization.CultureInfo>();
            var fieldMembers = field.FieldMappingCollection.Select(x => x.GenerateMember())
            .Select(x => new Property { Code = x.GeneratedCode() });
            item.PropertyCollection.AddRange(fieldMembers);
            list.Add(item);
            foreach (var f in field.FieldMappingCollection.Where(x => x.IsComplex))
            {
                ExtractClasses(list, f);
            }
        }

        public async Task<IEnumerable<Class>> GenerateCodeAsync()
        {
            var classes = new List<Class>();
            var record = new Class { Name = this.Entity.ToPascalCase(), Namespace = this.CodeNamespace };
            record.AddNamespaceImport<DateTime, FileInfo, FileHelpers.FieldAlignAttribute, JsonIgnoreAttribute>();
            record.AddNamespaceImport<DomainObject, IEnumerable<object>, System.Globalization.CultureInfo>();
            record.AddMethod(this.TextFormatter.GetRecordMetadataCode());

            record.AttributeCollection.Add(TextFormatter.GetRecordAttribute());
            classes.Add(record);

            var recordMembers = this.FieldMappingCollection.Select(x => x.GenerateMember())
                .Select(x => new Property { Code = x.GeneratedCode() });
            record.PropertyCollection.ClearAndAddRange(recordMembers);

            foreach (var complexField in this.FieldMappingCollection.Where(x => x.IsComplex))
            {
                ExtractClasses(classes, complexField);
            }

            // port class
            var portClass = await TextFormatter.GetPortClassAsync(this);
            classes.Add(portClass);

            return classes;
        }

        public Task<EntityDefinition> GenerateEntityDefinitionAsync()
        {
            var ed = new EntityDefinition
            {
                Name = Entity,
                Plural = Entity.Pluralize(),
                RecordName = "",
                Transient = true,
                IconClass = "fa fa-database",
                WebId = this.WebId,
                Id = Entity.ToIdFormat()
            };

            var members = this.FieldMappingCollection.Select(SuggestEntityDefinitionMembers).Where(x => null != x);
            ed.MemberCollection.AddRange(members);

            return Task.FromResult(ed);
        }

        private static Member SuggestEntityDefinitionMembers(TextFieldMapping field)
        {
            if (field.Ignore) return null;
            if (!field.IsComplex)
                return new SimpleMember
                {
                    Name = field.Name,
                    TypeName = field.TypeName,
                    IsNullable = field.IsNullable,
                    WebId = field.WebId,
                    AllowMultiple = false,
                    Boost = 1,
                    DefaultValue = null
                };

            var member = new ComplexMember
            {
                Name = field.Name,
                TypeName = field.TypeName,
                WebId = field.WebId,
                AllowMultiple = true,
                DefaultValue = null
            };
            var children = field.FieldMappingCollection.Select(SuggestEntityDefinitionMembers).Where(x => x != null);
            member.MemberCollection.AddRange(children);
            return member;
        }
    }
}