using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Humanizer;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Trigger : CustomProject, IProjectProvider
    {
        public static Trigger ParseJson(string json)
        {
            var trigger = JsonConvert.DeserializeObject<Trigger>(json);
            return trigger;
        }



        public string ClassName
        {
            get { return (this.Id.Humanize(LetterCasing.Title).Dehumanize() + "TriggerSubscriber").Replace("TriggerTrigger", "Trigger"); }
        }

        public override async Task<IEnumerable<Class>> GenerateCodeAsync()
        {

            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Name == this.Entity);


            var routingKeys = new List<string>();
            if (this.IsFiredOnAdded)
                routingKeys.Add(string.Format("{0}.added.#", this.Entity));
            if (this.IsFiredOnChanged)
                routingKeys.Add(string.Format("{0}.changed.#", this.Entity));
            if (this.IsFiredOnDeleted)
                routingKeys.Add(string.Format("{0}.deleted.#", this.Entity));
            var ops = this.FiredOnOperations.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => string.Format("{0}.#.{1}", this.Entity, s));
            routingKeys.AddRange(ops);

            var keys = string.Join(",\r\n", routingKeys.Select(s => string.Format("\"{0}\"", s)).ToArray());

            var code = new StringBuilder();
            var edTypeFullName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName,
                ed.Id, ed.Name);
            code.AppendLine("using " + typeof(Trigger).Namespace + ";");
            code.AppendLine("using " + typeof(Int32).Namespace + ";");
            code.AppendLine("using " + typeof(Task<>).Namespace + ";");
            code.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            code.AppendLine("using Bespoke.Sph.SubscribersInfrastructure;");
            code.AppendLine();

            code.AppendLine("namespace " + this.DefaultNamespace);
            code.AppendLine("{");

            code.AppendLinf("   public class {0}: Subscriber<{1}>",
                this.ClassName, edTypeFullName);
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
            var trigger = await context.LoadOneAsync<Trigger>(t => t.Id == ""{1}"");

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


            foreach (var customAction in trigger.ActionCollection.Where(a => a.IsActive && !a.UseCode))
            {{
                this.WriteMessage("" ==== Executing {{0}} ======"", customAction.Title);
                if (customAction.UseAsync)
                    await customAction.ExecuteAsync(new RuleContext(item)).ConfigureAwait(false);
                else
                    customAction.Execute(new RuleContext(item));

                this.WriteMessage(""done..."");
            }}
        ", keys, this.Id, edTypeFullName);


            int count = 1;
            foreach (var ca in this.ActionCollection.Where(x => x.UseCode))
            {
                var method = ca.Title.ToCsharpIdentitfier();
                code.AppendLinf("   var ca{0} = trigger.ActionCollection.Single(x => x.Title == \"{1}\");", count, method);
                code.AppendLinf("           if(ca{0}.IsActive)", count, method);
                code.AppendLinf("               await this.{0}(item);", method, edTypeFullName);
                code.AppendLine();
                count++;
            }
            code.AppendLine("}");
            code.AppendLine();
            foreach (var ca in this.ActionCollection.Where(x => x.UseCode))
            {
                var method = ca.Title.ToCsharpIdentitfier();
                code.AppendLinf("       public async Task<object> {0}({1} item)", method, edTypeFullName);
                code.AppendLine("       {");
                ca.GeneratorCode().Split(new[] { "\r\n" }, StringSplitOptions.None).ToList().ForEach(x => code.AppendLine("            " + x));
                code.AppendLine("       }");
            }
            code.AppendLine("   }");// end class


            code.AppendLine("}");// end namespace

            var @classes = new List<Class>();
            return @classes;
        }

        public override string DefaultNamespace
        {
            get { return "Bespoke.Sph.TriggerSubscribers"; }
        }

        public override MetadataReference[] GetMetadataReferences()
        {

            return new[]
            {
                this.CreateMetadataReference<Entity>()
            };

        }

    }
}
