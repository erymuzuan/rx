using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Newtonsoft.Json;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [StoreAsSource(HasDerivedTypes = true)]
    public partial class Trigger : Entity
    {
        [ImportMany(typeof(IBuildDiagnostics))]
        [JsonIgnore]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }

        public async Task<BuildValidationResult> ValidateBuildAsync()
        {
            if (null == this.BuildDiagnostics)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.BuildDiagnostics)
                throw new InvalidOperationException($"Fail to initialize MEF for {nameof(EntityForm)}.{nameof(BuildDiagnostics)}");

            var result = new BuildValidationResult();
            var errorTasks = this.BuildDiagnostics.Select(d => d.ValidateErrorsAsync(this));
            var errors = (await Task.WhenAll(errorTasks)).SelectMany(x => x);

            var warningTasks = this.BuildDiagnostics.Select(d => d.ValidateWarningsAsync(this));
            var warnings = (await Task.WhenAll(warningTasks)).SelectMany(x => x);

            result.Errors.AddRange(errors);
            result.Warnings.AddRange(warnings);
            
            result.Result = result.Errors.Count == 0;

            return result;
        }

        public static Trigger ParseJson(string json)
        {
            var trigger = JsonConvert.DeserializeObject<Trigger>(json);
            return trigger;
        }


        public async Task<WorkflowCompilerResult> CompileAsync()
        {
            var options = new CompilerOptions
            {
                IsDebug = true,
                SourceCodeDirectory = $"{ConfigurationManager.GeneratedSourceDirectory}\\Trigger.{this.Name}"
            };
            if (!Directory.Exists(options.SourceCodeDirectory))
                Directory.CreateDirectory(options.SourceCodeDirectory);
            return await this.CompileAsync(options);
        }
        public async Task<WorkflowCompilerResult> CompileAsync(CompilerOptions options)
        {
            var code = await this.GenerateCodeAsync();

            Debug.WriteLineIf(options.IsVerbose, code);

            var sourceFile = string.Empty;
            if (!string.IsNullOrWhiteSpace(options.SourceCodeDirectory))
            {
                sourceFile = Path.Combine(options.SourceCodeDirectory,
                    $"{this.Id}.cs");
                File.WriteAllText(sourceFile, code);
            }

            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.CompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, $"subscriber.trigger.{this.Id}.dll"),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true

                };
                var edDll = $"{ConfigurationManager.ApplicationName}.{this.Entity}.dll";
                options.ReferencedAssembliesLocation.Add(Path.Combine(ConfigurationManager.CompilerOutputPath, edDll));

                var subscriberInfraDll = Path.Combine(ConfigurationManager.SubscriberPath, "subscriber.infrastructure.dll");
                options.ReferencedAssembliesLocation.Add(subscriberInfraDll);

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

        public string ClassName => (this.Id.Humanize(LetterCasing.Title).Dehumanize() + "TriggerSubscriber").Replace("TriggerTrigger", "Trigger");

        public async Task<string> GenerateCodeAsync()
        {
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Name == this.Entity);
            if(null == ed)
                throw new FileNotFoundException($"Cannot load {this.Entity}.json file from {ConfigurationManager.SphSourceDirectory}\\EntityDefinition");


            var routingKeys = new List<string>();
            if (this.IsFiredOnAdded)
                routingKeys.Add($"{this.Entity}.added.#");
            if (this.IsFiredOnChanged)
                routingKeys.Add($"{this.Entity}.changed.#");
            if (this.IsFiredOnDeleted)
                routingKeys.Add($"{this.Entity}.deleted.#");
            var ops = this.FiredOnOperations.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => $"{this.Entity}.#.{s}");
            routingKeys.AddRange(ops);

            var keys = string.Join(",\r\n", routingKeys.Select(s => $"\"{s}\"").ToArray());

            var code = new StringBuilder();
            var edTypeFullName = $"{ed.CodeNamespace}.{ed.Name}";
            code.AppendLine("using " + typeof(Trigger).Namespace + ";");
            code.AppendLine("using " + typeof(int).Namespace + ";");
            code.AppendLine("using " + typeof(Task<>).Namespace + ";");
            code.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            code.AppendLine("using Bespoke.Sph.SubscribersInfrastructure;");
            code.AppendLine();

            code.AppendLine("namespace " + this.CodeNamespace);
            code.AppendLine("{");

            code.AppendLinf("   public class {0}: Subscriber<{1}>",
                this.ClassName, edTypeFullName);
            code.AppendLine("   {");

            code.AppendLine($@"  
        public override string QueueName
        {{
            get {{ return ""trigger_subs_{this.Id}""; }}
        }}

        public override string[] RoutingKeys
        {{
            get {{ return new[] {{ {keys} }}; }}
        }}
        private Trigger m_trigger;
        protected override void OnStart()
        {{
            var context = new SphDataContext();
            m_trigger = context.LoadOneFromSources<Trigger>(x => x.Id == ""{Id}"");
        }}


        protected override async Task ProcessMessage({edTypeFullName} item, MessageHeaders header)
        {{
            if(null == m_trigger){{
                this.WriteMessage(""{Id} trigger cannot be loaded"");
                return;
            }}
            this.WriteMessage(""Running triggers({{0}}) with {{1}} actions and {{2}} rules"", m_trigger.Name,
                m_trigger.ActionCollection.Count(x => x.IsActive),
                m_trigger.RuleCollection.Count);

            foreach (var rule in m_trigger.RuleCollection)
            {{
                try
                {{
                    var result = rule.Execute(new RuleContext(item) {{ Log = header.Log }});
                    if (!result)
                    {{
                        this.WriteMessage(""Rule {{0}} evaluated to FALSE"", rule);
                        return;
                    }}
                    this.WriteMessage(""Rule {{0}} evaluated to TRUE"", rule);
                }}
                catch (Exception e)
                {{
                    this.WriteError(e);
                }}
            }}


            foreach (var customAction in m_trigger.ActionCollection.Where(a => a.IsActive && !a.UseCode))
            {{
                this.WriteMessage("" ==== Executing {{0}} ======"", customAction.Title);
                if (customAction.UseAsync)
                    await customAction.ExecuteAsync(new RuleContext(item)).ConfigureAwait(false);
                else
                    customAction.Execute(new RuleContext(item));

                this.WriteMessage(""done..."");
            }}
        ");


            var count = 1;
            foreach (var ca in this.ActionCollection.Where(x => x.UseCode))
            {
                var method = ca.Title.ToCsharpIdentitfier();
                code.AppendLine($"           var ca{count} = m_trigger.ActionCollection.Single(x => x.Title == \"{method}\");");
                code.AppendLine($"           if(ca{count}.IsActive)");
                code.AppendLine(ca.UseAsync
                    ? $"               await this.{method}(item);"
                    : $"               this.{method}(item);");
                code.AppendLine();
                count++;
            }
            code.AppendLine("}");
            code.AppendLine();
            foreach (var ca in this.ActionCollection.Where(x => x.UseCode))
            {
                var method = ca.Title.ToCsharpIdentitfier();
                code.AppendLine(ca.UseAsync
                    ? $"       public async Task<object> {method}({edTypeFullName} item)"
                    : $"       public object {method}({edTypeFullName} item)");
                code.AppendLine("       {");
                ca.GeneratorCode().Split(new[] { "\r\n" }, StringSplitOptions.None).ToList().ForEach(x => code.AppendLine("            " + x));
                code.AppendLine("       }");
            }
            code.AppendLine("   }");// end class


            code.AppendLine("}");// end namespace
            return code.ToString();
        }

        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.TriggerSubscribers";

        private IEnumerable<BuildError> GetCompileErrors(CompilerResults result, string code)
        {
            var temp = Path.GetTempFileName() + ".cs";
            File.WriteAllText(temp, code);
            var sources = File.ReadAllLines(temp);
            var list = (from object er in result.Errors.OfType<CompilerError>()
                        select GetSourceError(er as CompilerError, sources));
            File.Delete(temp);

            return list;
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
