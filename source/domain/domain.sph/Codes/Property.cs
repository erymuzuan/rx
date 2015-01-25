using System;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Codes
{
    public class Property
    {
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
        public string Name { get; set; }
        public string FileName { get; set; }
        private readonly ObjectCollection<string> m_attributeCollection = new ObjectCollection<string>();

        public ObjectCollection<string> AttributeCollection
        {
            get { return m_attributeCollection; }
        }

        private string m_code;
        public override string ToString()
        {
            return this.Code;
        }

        public string Code
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(m_code)) return m_code;

                var nullability = GetNullability();
                var type = this.Type == null ? this.TypeName : this.Type.ToCSharp();
                var code = new StringBuilder();
                code.AppendFormat("public {0}{2} {1}", type, this.Name, nullability);
                code.Append(" {");
                if (this.IsReadOnly && this.Initialized)
                {
                    code.Append(" get { return m_" + this.Name.ToCamelCase() + "; } }");
                    code.Insert(0, string.Format("private readonly {0} m_{1} = new {0}();\r\n", type, this.Name.ToCamelCase()));
                }
                else
                {
                    code.Append(" get; set; }");
                }
                return code.ToString();
            }
            set { m_code = value; }
        }

        private string GetNullability()
        {
            if (null == this.Type) return string.Empty;
            if (this.Type == typeof(string)) return string.Empty;
            return this.IsNullable && Member.NativeTypes.Contains(this.Type) ? "?" : string.Empty;
        }

        public string TypeName { get; set; }
        public bool Initialized { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsNullable { get; set; }
    }
}