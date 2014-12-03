using System;

namespace Bespoke.Sph.Domain.Codes
{
    public class Method
    {
        public string Comment { get; set; }
        public Modifier Modifier { get; set; }
        public Type ReturnType { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Code { get; set; }

        private readonly ObjectCollection<string> m_attributeCollection = new ObjectCollection<string>();

        public ObjectCollection<string> AttributeCollection
        {
            get { return m_attributeCollection; }
        }
    }

    public enum Modifier
    {
        Public,
        Private,
        Protected
    }
}