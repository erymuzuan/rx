using System;

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
                if(string.IsNullOrWhiteSpace(m_code))
                return string.Format("public {0} {1} {{get;set}}", this.Type.ToCSharp(), this.Name);
                return m_code;
            }
            set { m_code = value; }
        }

        public string TypeName { get; set; }
        public bool Initialized { get; set; }
        public bool IsReadOnly { get; set; }
        public string IsNullable { get; set; }
    }
}