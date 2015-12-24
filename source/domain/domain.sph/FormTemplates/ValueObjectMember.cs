using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ValueObjectMember : Member
    {
        public override string GetDefaultValueCode(int count)
        {
            return this.AllowMultiple ? null : $"this.{Name} = new {ValueObjectName}();";
        }


        public override string GeneratedCode(string padding = "      ")
        {
            if (this.AllowMultiple)
                return padding +
                       $"public ObjectCollection<{ValueObjectName}> {Name} {{get;}} = new ObjectCollection<{ValueObjectName}>();";
            return padding + $"public {ValueObjectName} {Name} {{ get; set;}}";
        }


        public override string GeneratedCustomClass(string codeNamespace, string[] usingNamespaces, out string fileName)
        {
            fileName = $"{this.ValueObjectName}.cs";

            var code = new StringBuilder(this.GetCodeHeader(codeNamespace, usingNamespaces));
            code.AppendLine($"   public class {ValueObjectName}: DomainObject");

            code.AppendLine("   {");
            // ctor
            code.AppendLine($"       public {ValueObjectName}()");
            code.AppendLine("       {");
            code.AppendLinf("           var rc = new RuleContext(this);");



            var count = 0;
            foreach (var member in this.MemberCollection)
            {
                count++;
                var defaultValueCode = member.GetDefaultValueCode(count);
                if (!string.IsNullOrWhiteSpace(defaultValueCode))
                    code.AppendLine(defaultValueCode);
            }
            code.AppendLine("       }");
            foreach (var member in this.MemberCollection)
            {
                code.AppendLine(member.GeneratedCode());

            }
            code.AppendLine("   }");
            foreach (var member in this.MemberCollection.Where(x => x.GetType() != typeof(ValueObjectMember)))
            {
                string fileName2;
                code.AppendLine(member.GeneratedCustomClass(codeNamespace, usingNamespaces, out fileName2));
            }
            code.AppendLine("}");
            return code.FormatCode();
        }


        public ValueObjectMember()
        {
            this.TypeName = typeof(object).GetShortAssemblyQualifiedName();
        }

        [JsonIgnore]
        public override Type Type
        {
            get { return null; }
            set
            {
                Console.WriteLine(value);
            }
        }

        public override string GenerateJavascriptMember(string ns)
        {
            return this.AllowMultiple
                    ? $"     {Name}: ko.observableArray([]),"
                    : $"     {Name}: ko.observable(new bespoke.{ns}.domain.{ValueObjectName}()),";
        }

        public override string GenerateJavascriptClass(string jns, string csNs, string assemblyName)
        {
            var script = new StringBuilder();

            script.AppendLine($"bespoke.{jns}.domain.{ValueObjectName} = function(optionOrWebid){{");
            script.AppendLine(" var model = {");
            script.AppendLine($"     $type : ko.observable(\"{csNs}.{ValueObjectName}, {assemblyName}\"),");

            var members = from mb in this.MemberCollection
                          let m = mb.GenerateJavascriptMember(jns)
                          where !string.IsNullOrWhiteSpace(m)
                          select m;
            members.ToList().ForEach(m => script.AppendLine(m));

            script.AppendFormat(@"
    addChildItem : function(list, type){{
                        return function(){{
                            list.push(new childType(system.guid()));
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
    }}
", jns, this.Name.Replace("Collection", string.Empty));

            script.AppendLine(" return model;");
            script.AppendLine("};");
            var classes = from m in this.MemberCollection
                          let c = m.GenerateJavascriptClass(jns, csNs, assemblyName)
                          where !string.IsNullOrWhiteSpace(c)
                          select c;
            classes.ToList().ForEach(x => script.AppendLine(x));
            return script.ToString();
        }

        public new ObjectCollection<Member> MemberCollection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.ValueObjectName))
                    return new ObjectCollection<Member>();

                var context = new SphDataContext();
                var vod = context.LoadOne<ValueObjectDefinition>(d => d.Name == this.ValueObjectName);
                return null == vod ? new ObjectCollection<Member>() : vod.MemberCollection;
            }
        }

        public new string TypeName
        {
            get { return typeof(ValueObjectDefinition).GetShortAssemblyQualifiedName(); }
            set { Debug.Write(value); }
        }
    }
}