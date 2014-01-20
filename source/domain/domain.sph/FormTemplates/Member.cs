using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Member : DomainObject
    {
        [XmlAttribute]
        public string FullName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}->{1}:{2}", this.Name, this.FullName, this.TypeName);
        }
        [XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }

        public string GeneratedCode()
        {
            if (null == this.Type)
                throw new InvalidOperationException(this + " doesn't have a type");
            var code = new StringBuilder();
            if (typeof(object) == this.Type)
            {
                code.AppendLinf("   private {0} m_{1} = new {0}();", this.Name, this.Name.ToCamelCase());
                code.AppendLinf("   public {0} {1}", this.Name, this.Name);
                code.AppendLine("   {");
                code.AppendLinf("       get{{ return m_{0};}}", this.Name.ToCamelCase());
                code.AppendLinf("       set{{ m_{0} = value;}}", this.Name.ToCamelCase());
                code.AppendLine("   }");
                return code.ToString();
            }
            if (typeof(Array) == this.Type)
            {
                code.AppendLinf("   private readonly ObjectCollection<{0}> m_{1} = new ObjectCollection<{0}>();", this.Name.Replace("Collection", ""), this.Name.ToCamelCase());
                code.AppendLinf("   public ObjectCollection<{0}> {1}", this.Name.Replace("Collection", ""), this.Name);
                code.AppendLine("   {");
                code.AppendLinf("       get{{ return m_{0};}}", this.Name.ToCamelCase());
                code.AppendLine("   }");
                return code.ToString();
            }
            code.AppendLinf("   private {0} m_{1};", this.Type.FullName, this.Name.ToCamelCase());
            code.AppendLinf("   public {0} {1}", this.Type.FullName, this.Name);
            code.AppendLine("   {");
            code.AppendLinf("       get{{ return m_{0};}}", this.Name.ToCamelCase());
            code.AppendLinf("       set{{ m_{0} = value;}}", this.Name.ToCamelCase());
            code.AppendLine("   }");
            return code.ToString();
        }

        public string GeneratedCustomClass()
        {
            var code = new StringBuilder();
            if (typeof(object) == this.Type)
            {
                code.AppendLinf("   public class {0}: DomainObject", this.Name);
            }
            if (typeof(Array) == this.Type)
            {
                code.AppendLinf("   public class {0}: DomainObject", this.Name.Replace("Collection", ""));
            }

            if (typeof(object) == this.Type || typeof(Array) == this.Type)
            {

                code.AppendLine("   {");
                foreach (var member in this.MemberCollection)
                {
                    code.AppendLine(member.GeneratedCode());

                }
                code.AppendLine("   }");
                foreach (var member in this.MemberCollection)
                {
                    code.AppendLine(member.GeneratedCustomClass());
                }
            }
            return code.ToString();
        }


        public void Add(Dictionary<string, Type> members)
        {
            foreach (var k in members.Keys)
            {
                this.MemberCollection.Add(new Member { Name = k, Type = members[k] });
            }
        }

        public new Member this[string index]
        {
            get { return this.MemberCollection.Single(m => m.Name == index); }
        }

        public IEnumerable<string> GetMembersPath(string root)
        {
            var list = new List<string>();
            list.AddRange(this.MemberCollection.Select(a => root + this.Name.Replace("Collection", "") + "." + a.Name));
            foreach (var member in this.MemberCollection)
            {
                list.AddRange(member.GetMembersPath(root + this.Name.Replace("Collection", "") + "."));
            }
            return list.ToArray();
        }

        public string GenerateJavascriptClass(string jsNamespace, string codeNamespace, string assemblyName)
        {
            var script = new StringBuilder();
            var name = this.Name.Replace("Collection", "");

            script.AppendLinf("bespoke.{0}.domain.{1} = function(optionOrWebid){{", jsNamespace, name);
            script.AppendLine(" var model = {");
            script.AppendLinf("     $type = \"{0}.{1}, {2}\"", codeNamespace, name,
                assemblyName);
            foreach (var item in this.MemberCollection)
            {
                if (item.Type == typeof(Array))
                    script.AppendLinf("     {0}: ko.observableArray([])", item.Name);
                else if (item.Type == typeof(object))
                    script.AppendLinf("     {0}: ko.observable(new bespoke.{1}.domain.{0}())", item.Name, jsNamespace);
                else
                    script.AppendLinf("     {0}: ko.observable()", item.Name);
            }

            script.AppendLine(" }");
            script.AppendLine(" return model;");
            script.AppendLine("};");

            foreach (var item in this.MemberCollection.Where(m => m.Type == typeof(object) || m.Type == typeof(Array)))
            {
                var code = item.GenerateJavascriptClass(jsNamespace, codeNamespace, assemblyName);
                script.AppendLine(code);
            }
            return script.ToString();
        }
    }
}