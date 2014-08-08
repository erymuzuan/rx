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
using Microsoft.CSharp;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Trigger : Entity
    {
        public static Trigger ParseJson(string json)
        {
            var trigger = JsonConvert.DeserializeObject<Trigger>(json);
            return trigger;
        }


        public async Task<WorkflowCompilerResult> CompileAsync(CompilerOptions options)
        {
            var code = await this.GenerateCodeAsync();
            Debug.WriteLineIf(options.IsVerbose, code);

            var sourceFile = string.Empty;
            if (!string.IsNullOrWhiteSpace(options.SourceCodeDirectory))
            {
                sourceFile = Path.Combine(options.SourceCodeDirectory,
                    string.Format("{0}.cs", this.Name));
                File.WriteAllText(sourceFile, code);
            }

            using (var provider = new CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.WorkflowCompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, string.Format("subscriber.trigger.{0}.dll", this.TriggerId)),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true

                };
                var edDll = string.Format("{0}.{1}.dll", ConfigurationManager.ApplicationName, this.Entity);
                options.ReferencedAssembliesLocation.Add(Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, edDll));

                var subscriberInfraDll = Path.Combine(ConfigurationManager.SubscriberPath, "subscriber.infrastructure.dll");
                options.ReferencedAssembliesLocation.Add(subscriberInfraDll);
          
                parameters.ReferencedAssemblies.Add(typeof(Entity).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Int32).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Expression<>).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Trigger).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(INotifyPropertyChanged).Assembly.Location);

                foreach (var ass in options.ReferencedAssembliesLocation)
                {
                    parameters.ReferencedAssemblies.Add(ass);
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

        public async Task<string> GenerateCodeAsync()
        {
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Name == this.Entity);


            var routingKeys = new List<string>();
            if(this.IsFiredOnAdded)
                routingKeys.Add(string.Format("{0}.added.#", this.Entity));
            if(this.IsFiredOnChanged)
                routingKeys.Add(string.Format("{0}.changed.#", this.Entity));
            if(this.IsFiredOnDeleted)
                routingKeys.Add(string.Format("{0}.deleted.#", this.Entity));
            var ops = this.FiredOnOperations.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => string.Format("{0}.#.{1}", this.Entity, s));
            routingKeys.AddRange(ops);

            var keys =string.Join(",\r\n",routingKeys.Select(s => string.Format("\"{0}\"", s)).ToArray());

            var code = new StringBuilder();
            var edTypeFullName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName,
                ed.EntityDefinitionId, ed.Name);
            code.AppendLine("using " + typeof(Trigger).Namespace + ";");
            code.AppendLine("using " + typeof(Int32).Namespace + ";");
            code.AppendLine("using " + typeof(Task<>).Namespace + ";");
            code.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            code.AppendLine("using Bespoke.Sph.SubscribersInfrastructure;");
            code.AppendLine();

            code.AppendLine("namespace " + this.CodeNamespace);
            code.AppendLine("{");

            code.AppendLinf("   public class Trigger{0}Subscriber: Subscriber<{1}>", 
                this.TriggerId, edTypeFullName);
            code.AppendLine("   {");

            code.AppendFormat(@"  
        public override string QueueName
        {{
            get {{ return ""trigger_subs_{1}""; }}
        }}

        public override string[] RoutingKeys
        {{
            get {{ return new[] {{ {0} }}; }}
        }}

        protected override async Task ProcessMessage({2} item, MessageHeaders header)
        {{
            var context = new SphDataContext();
            var trigger = await context.LoadOneAsync<Trigger>(t => t.TriggerId == {1});

            this.WriteMessage(""Running triggers({{0}}) with {{1}} actions and {{2}} rules"", trigger.Name,
                trigger.ActionCollection.Count(x => x.IsActive),
                trigger.RuleCollection.Count);

            foreach (var rule in trigger.RuleCollection)
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


            foreach (var customAction in trigger.ActionCollection.Where(a => a.IsActive))
            {{
                this.WriteMessage("" ==== Executing {{0}} ======"", customAction.Title);
                if (customAction.UseAsync)
                    await customAction.ExecuteAsync(new RuleContext(item)).ConfigureAwait(false);
                else
                    customAction.Execute(new RuleContext(item));

                this.WriteMessage(""done..."");
            }}
        }}", keys, this.TriggerId, edTypeFullName);

            code.AppendLine();
            code.AppendLine("   }");// end class


            code.AppendLine("}");// end namespace
            return code.ToString();
        }

        public string CodeNamespace { get { return string.Format("Bespoke.Sph.TriggerSubscribers"); } }

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
            var message = er.ErrorText;

            return new BuildError(member, message)
            {
                Code = sources[er.Line - 1],
                Line = er.Line
            };

        }
    }
}
