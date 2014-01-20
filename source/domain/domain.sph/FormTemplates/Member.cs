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
            if(null == this.Type)
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
    }
}