using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;
using Polly;

namespace Bespoke.Sph.Csharp.CompilersServices.Extensions
{
    public static class EntityDefinitionExtension
    {

        private static readonly string[] ImportDirectives =
        {
            typeof(Entity).Namespace,
            typeof(Int32).Namespace ,
            typeof(Task<>).Namespace,
            typeof(Enumerable).Namespace
        };


        public static async Task<BuildValidationResult> ValidateBuildAsync(this EntityDefinition ed, IBuildDiagnostics[] buildDiagnosticses)
        {
            var result = ed.CanSave();
            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(3, c => TimeSpan.FromMilliseconds(500),
                    (ex, ts) =>
                    {
                        ObjectBuilder.GetObject<ILogger>()
                            .Log(new LogEntry(ex));
                    });

            var errorTasks = buildDiagnosticses
                .Select(d => policy.ExecuteAndCapture(() => d.ValidateErrorsAsync(ed)))
                .Where(x => null != x)
                .Where(x => x.FinalException == null)
                .Select(x => x.Result)
                .ToArray();
            var errors = (await Task.WhenAll(errorTasks)).Where(x => null != x).SelectMany(x => x);

            var warningTasks = buildDiagnosticses
                .Select(d => policy.ExecuteAndCapture(() => d.ValidateWarningsAsync(ed)))
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

        public static RxCompilerResult Compile(this EntityDefinition ed, CompilerOptions options, params string[] files)
        {
            if (files.Length == 0)
                throw new ArgumentException(@"Resources.Adapter_Compile_No_source_files_supplied_for_compilation",
                    nameof(files));


            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.CompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly =
                        Path.Combine(outputPath, $"{ConfigurationManager.ApplicationName}.{ed.Name}.dll"),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true
                };

                parameters.AddReference(typeof(Entity),
                    typeof(int),
                    typeof(INotifyPropertyChanged),
                    typeof(Expression<>),
                    typeof(XmlAttributeAttribute),
                    typeof(SmtpClient),
                    typeof(HttpClient),
                    typeof(XElement),
                    typeof(ConfigurationManager));

                foreach (var es in options.EmbeddedResourceCollection)
                {
                    parameters.EmbeddedResources.Add(es);
                }
                foreach (var ass in options.ReferencedAssembliesLocation)
                {
                    parameters.ReferencedAssemblies.Add(ass);
                }
                var result = provider.CompileAssemblyFromFile(parameters, files);
                var cr = new RxCompilerResult
                {
                    Result = true,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;
                var errors = from CompilerError x in result.Errors
                             select new BuildError(ed.WebId, x.ErrorText)
                             {
                                 Line = x.Line,
                                 FileName = x.FileName
                             };
                cr.Errors.AddRange(errors);
                return cr;
            }
        }



        public static async Task<IEnumerable<Class>> GenerateCodeAsync(this EntityDefinition ed)
        {
            var cvs = ObjectBuilder.GetObject<ICvsProvider>();
            var src = $@"{ConfigurationManager.SphSourceDirectory}\{nameof(EntityDefinition)}\{ed.Id}.json";
            var commitId = await cvs.GetCommitIdAsync(src);

            var @class = new Class { Name = ed.Name, FileName = $"{ed.Name}.cs", Namespace = ed.CodeNamespace, BaseClass = $"{nameof(Entity)}, {nameof(IVersionInfo)}" };
            @class.ImportCollection.AddRange(ImportDirectives);
            @class.PropertyCollection.Add(new Property { Code = $@"string IVersionInfo.Version => ""{commitId}"";" });


            var list = new ObjectCollection<Class> { @class };

            if (ed.Transient)
            {
                ed.StoreInDatabase = false;
                // for elasticsearch, use the value from user
            }
            else
            {
                ed.StoreInElasticsearch = true;
                ed.StoreInDatabase = true;
            }
            var es = ed.StoreInElasticsearch ?? true ? "true" : "false";
            var db = ed.StoreInDatabase ?? true ? "true" : "false";
            var source = ed.TreatDataAsSource ? "true" : "false";
            var audit = ed.EnableAuditing ? "true" : "false";
            @class.AttributeCollection.Add($"  [PersistenceOption(IsElasticsearch={es}, IsSqlDatabase={db}, IsSource={source}, EnableAuditing={audit})]");


            var ctor = new StringBuilder();
            // ctor
            ctor.AppendLine($"       public {ed.Name}()");
            ctor.AppendLine("       {");
            ctor.AppendLinf("           var rc = new RuleContext(this);");
            var count = 0;
            foreach (var member in ed.MemberCollection)
            {
                count++;
                var defaultValueCode = member.GetDefaultValueCode(count);
                if (!string.IsNullOrWhiteSpace(defaultValueCode))
                    ctor.AppendLine(defaultValueCode);
            }
            ctor.AppendLine("       }");
            @class.CtorCollection.Add(ctor.ToString());

            var toString = $@"     
        public override string ToString()
        {{
            return ""{ed.Name}:"" + {ed.RecordName};
        }}";
            @class.MethodCollection.Add(new Method { Code = toString });

            var properties = from m in ed.MemberCollection
                             let prop = m.GeneratedCode("   ")
                             select new Property { Code = prop };
            @class.PropertyCollection.AddRange(properties);

            // classes for members
            foreach (var member in ed.MemberCollection)
            {
                var mc = member.GeneratedCustomClass(ed.CodeNamespace, ImportDirectives);
                list.AddRange(mc);
            }
            return list;
        }


        public static string[] SaveSources(this EntityDefinition ed, IEnumerable<Class> classes)
        {
            var sources = classes.ToArray();
            var folder = Path.Combine(ConfigurationManager.GeneratedSourceDirectory, ed.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (var cs in sources)
            {
                var file = Path.Combine(folder, cs.FileName);
                File.WriteAllText(file, cs.GetCode());
            }

            // add versioning information
            var assemblyInfoCs = AssemblyInfoClass.GenerateAssemblyInfoAsync(ed, true).Result;

            return sources
                    .Select(f => $"{ConfigurationManager.GeneratedSourceDirectory}\\{ed.Name}\\{f.FileName}")
                    .Concat(new[] { assemblyInfoCs.FileName })
                    .ToArray();
        }
    }
}
