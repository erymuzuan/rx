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




        public IEnumerable<Class> GenerateCode()
        {
            var @class = new Class { Name = this.Name, FileName = $"{Name}.cs", Namespace = CodeNamespace, BaseClass = nameof(Entity) };
            @class.ImportCollection.AddRange(m_importDirectives);
            var list = new ObjectCollection<Class> { @class };

            if (this.TreatDataAsSource)
            {
                var es = this.StoreInElasticsearch ?? true ? "true" : "false";
                var db = this.StoreInDatabase ?? true ? "true" : "false";
                @class.AttributeCollection.Add($"  [StoreAsSource(IsElasticsearch={es}, IsSqlDatabase={db})]");
            }

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
            @class.PropertyCollection.ClearAndAddRange(properties);

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
            return sources
                    .Select(f => $"{ConfigurationManager.GeneratedSourceDirectory}\\{this.Name}\\{f.FileName}")
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


        public Task<string> GenerateCustomXsdJavascriptClassAsync()
        {
            var jsNamespace = ConfigurationManager.ApplicationName + "_" + this.Id;
            var assemblyName = ConfigurationManager.ApplicationName + "." + this.Name;
            var script = new StringBuilder();
            script.AppendLine("var bespoke = bespoke ||{};");
            script.AppendLinf("bespoke.{0} = bespoke.{0} ||{{}};", jsNamespace);
            script.AppendLinf("bespoke.{0}.domain = bespoke.{0}.domain ||{{}};", jsNamespace);

            script.AppendLinf("bespoke.{0}.domain.{1} = function(optionOrWebid){{", jsNamespace, this.Name);
            script.AppendLine(" var system = require('services/system'),");
            script.AppendLine(" model = {");
            script.AppendLine($"     $type : ko.observable(\"{CodeNamespace}.{Name}, {assemblyName}\"),");
            script.AppendLine("     Id : ko.observable(\"0\"),");

            var members = from item in this.MemberCollection
                          let m = item.GenerateJavascriptMember(jsNamespace)
                          where !string.IsNullOrWhiteSpace(m)
                          select m;
            members.ToList().ForEach(m => script.AppendLine(m));

            script.AppendFormat(@"
    addChildItem : function(list, type){{
                        return function(){{                          
                            list.push(new type(system.guid()));
                        }}
                    }},
            
   removeChildItem : function(list, obj){{
                        return function(){{
                            list.remove(obj);
                        }}
                    }},
");
            script.AppendLine("     WebId: ko.observable()");

            script.AppendLine(" };");

            script.AppendLine(@" 
             if (optionOrWebid && typeof optionOrWebid === ""object"") {
                for (var n in optionOrWebid) {
                    if (typeof model[n] === ""function"") {
                        model[n](optionOrWebid[n]);
                    }
                }
            }
            if (optionOrWebid && typeof optionOrWebid === ""string"") {
                model.WebId(optionOrWebid);
            }");

            script.AppendFormat(@"

                if (bespoke.{0}.domain.{1}Partial) {{
                    return _(model).extend(new bespoke.{0}.domain.{1}Partial(model));
                }}", jsNamespace, this.Name);

            script.AppendLine(" return model;");
            script.AppendLine("};");

            var classes = from m in this.MemberCollection
                          let c = m.GenerateJavascriptClass(jsNamespace, CodeNamespace, assemblyName)
                          where !string.IsNullOrWhiteSpace(c)
                          select c;
            classes.ToList().ForEach(x => script.AppendLine(x));


            return Task.FromResult(script.ToString());
        }


        public string GetElasticsearchMapping()
        {
            var map = new StringBuilder();
            map.AppendLine("{");
            map.AppendLine($"    \"{Name.ToLowerInvariant()}\":{{");
            map.AppendLine("        \"properties\":{");
            // add entity default properties
            map.AppendLine("            \"CreatedBy\": {\"type\": \"string\", \"index\":\"not_analyzed\"},");
            map.AppendLine("            \"ChangedBy\": {\"type\": \"string\", \"index\":\"not_analyzed\"},");
            map.AppendLine("            \"WebId\": {\"type\": \"string\", \"index\":\"not_analyzed\"},");
            map.AppendLine("            \"CreatedDate\": {\"type\": \"date\"},");
            map.AppendLine("            \"ChangedDate\": {\"type\": \"date\"},");

            var memberMappings = string.Join(",\r\n", this.MemberCollection.Select(d => d.GetEsMapping()));
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            map.AppendLine("}");
            return map.ToString();
        }
    }
}
