using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;
using Polly;

namespace Bespoke.Sph.Csharp.CompilersServices.Extensions
{
    public static class EntityDefinitionExtension
    {

        private static readonly string[] ImportDirectives =
        {
            typeof(Entity).Namespace
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
    }
}
