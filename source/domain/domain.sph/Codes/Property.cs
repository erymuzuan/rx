using System;

namespace Bespoke.Sph.Domain.Codes
{
    public class Property
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }

        public ObjectCollection<string> AttributeCollection { get; } = new ObjectCollection<string>();

        private string m_code;
        public string Code
        {
            get
            {
                var attributes = string.Join("\r\n", this.AttributeCollection);
                return string.IsNullOrWhiteSpace(m_code) ?
                    $@"
            {attributes}
            public {this.Type.ToCSharp()} {this.Name} {{get;set;}}" : 
                    m_code;
            }
            set { m_code = value; }
        }
    }
}