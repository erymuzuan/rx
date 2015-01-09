using System;
using System.Text;

namespace Bespoke.Sph.Domain.Codes
{
    public class Property
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        private readonly ObjectCollection<string> m_attributeCollection = new ObjectCollection<string>();

        public ObjectCollection<string> AttributeCollection
        {
            get { return m_attributeCollection; }
        }

        private string m_code;
        public string Code
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(m_code)) return m_code;

                var code = new StringBuilder();
                code.AppendFormat("public {0} {1}",
                    string.IsNullOrWhiteSpace(this.TypeName) ? this.Type.ToCSharp() : this.TypeName, this.Name);
                code.Append("{");
                if (this.IsReadOnly && this.Initialized)
                {
                    code.Append("get { return m_" + this.Name.ToCamelCase() + ";}}");
                    code.Insert(0, string.Format("private readonly {0} m_{1} = new {0}();\r\n", this.TypeName, this.Name.ToCamelCase()));
                }
                else
                {
                    code.Append("get; set;}");
                }
                return code.ToString();
            }
            set { m_code = value; }
        }

        public string TypeName { get; set; }
        public bool Initialized { get; set; }
        public bool IsReadOnly { get; set; }
        public string IsNullable { get; set; }
    }
}