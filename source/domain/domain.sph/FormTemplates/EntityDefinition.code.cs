using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class EntityDefinition
    {
        private readonly string[] m_importDirectives =
        {
            typeof(Entity).Namespace,
            typeof(Int32).Namespace ,
            typeof(Task<>).Namespace,
            typeof(Enumerable).Namespace ,
            typeof(XmlAttributeAttribute).Namespace,
            "System.Web.Mvc",
            "Bespoke.Sph.Web.Helpers"
        };




        public async Task<IEnumerable<Class>> GenerateCodeAsync()
        {
            var cvs = ObjectBuilder.GetObject<ICvsProvider>();
            var src = $@"{ConfigurationManager.SphSourceDirectory}\{nameof(EntityDefinition)}\{Id}.json";
            var commitId = await cvs.GetCommitIdAsync(src);

            var @class = new Class { Name = this.Name, FileName = $"{Name}.cs", Namespace = CodeNamespace, BaseClass = $"{nameof(Entity)}, {nameof(IVersionInfo)}" };
            @class.ImportCollection.AddRange(m_importDirectives);
            @class.PropertyCollection.Add(new Property { Code = $@"string IVersionInfo.Version => ""{commitId}"";" });


            var list = new ObjectCollection<Class> { @class };

            if (this.Transient)
            {
                this.StoreInDatabase = false;
                // for elasticsearch, use the value from user
            }
            else
            {
                this.StoreInElasticsearch = true;
                this.StoreInDatabase = true;
            }
            var es = this.StoreInElasticsearch ?? true ? "true" : "false";
            var db = this.StoreInDatabase ?? true ? "true" : "false";
            var source = this.TreatDataAsSource ? "true" : "false";
            var audit = this.EnableAuditing ? "true" : "false";
            @class.AttributeCollection.Add($"  [PersistenceOption(IsElasticsearch={es}, IsSqlDatabase={db}, IsSource={source}, EnableAuditing={audit})]");


            var ctor = new StringBuilder();
            // ctor
            ctor.AppendLine($"       public {Name}()");
            ctor.AppendLine("       {");
            ctor.AppendLinf("           var rc = new RuleContext(this);");
            var count = 0;
            foreach (var member in this.MemberCollection)
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
            return ""{Name}:"" + {RecordName};
        }}";
            @class.MethodCollection.Add(new Method { Code = toString });

            var properties = from m in this.MemberCollection
                             let prop = m.GeneratedCode("   ")
                             select new Property { Code = prop };
            @class.PropertyCollection.AddRange(properties);

            // classes for members
            foreach (var member in this.MemberCollection)
            {
                var mc = member.GeneratedCustomClass(this.CodeNamespace, m_importDirectives);
                list.AddRange(mc);
            }
            return list;
        }


        public string[] SaveSources(IEnumerable<Class> classes)
        {
            var sources = classes.ToArray();
            var folder = Path.Combine(ConfigurationManager.GeneratedSourceDirectory, this.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (var cs in sources)
            {
                var file = Path.Combine(folder, cs.FileName);
                File.WriteAllText(file, cs.GetCode());
            }

            // add versioning information
            var assemblyInfoCs = AssemblyInfoClass.GenerateAssemblyInfoAsync(this, true).Result;

            return sources
                    .Select(f => $"{ConfigurationManager.GeneratedSourceDirectory}\\{this.Name}\\{f.FileName}")
                    .Concat(new[] { assemblyInfoCs.FileName })
                    .ToArray();
        }
        [JsonIgnore]
        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.{this.Plural}.Domain";
        [JsonIgnore]
        public string SourceFile => $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\{Id}.json";
        [JsonIgnore]
        public string TypeName => $"{CodeNamespace}.{Name}";

        /// <summary>
        /// return CodeNamespace.Name, assemblyName
        /// </summary>
        [JsonIgnore]
        public string FullTypeName => $"{CodeNamespace}.{Name}, {ConfigurationManager.ApplicationName}.{Name}";




    }
}
