using System;
using System.Linq;
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

                var nullability = GetNullability();

                var code = new StringBuilder();
                code.AppendFormat("public {0}{2} {1}",
                    string.IsNullOrWhiteSpace(this.TypeName) ? this.Type.ToCSharp() : this.TypeName, this.Name, nullability);
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

        private string GetNullability()
        {
            if (null == this.Type) return string.Empty;

            var nullability = this.IsNullable ? "?" : string.Empty;
            if (this.Type == typeof(string)) return string.Empty;

            var primitiveTypes = new[]
            {
                typeof (int),typeof (long),typeof (short),
                typeof (double),typeof (float), typeof (decimal), 
                typeof (DateTime), typeof (bool), typeof (char),typeof (byte)
            };
            if (!primitiveTypes.Contains(this.Type))
                nullability = string.Empty;
            return nullability;
        }

        public string TypeName { get; set; }
        public bool Initialized { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsNullable { get; set; }
    }
}