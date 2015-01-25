using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition
    {
        private XElement m_customSchema;
        // ReSharper disable InconsistentNaming
        static readonly XNamespace x = "http://www.w3.org/2001/XMLSchema";
        // ReSharper restore InconsistentNaming
        
        public IEnumerable<Class> GenerateXsdCsharpClasses()
        {
            var gen = new CsharpCodeGenerator(this.GetCustomSchema());
            return gen.Generate();
        }



        public XElement GetCustomSchema(string id = null)
        {
            if (null != m_customSchema) return m_customSchema;
            if (string.IsNullOrWhiteSpace(this.SchemaStoreId))
                return null;

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = store.GetContent(id ?? this.SchemaStoreId);
            if (null == content) return null;
            using (var stream = new MemoryStream(content.Content))
            {
                m_customSchema = XElement.Load(stream);
                return m_customSchema;
            }
        }

        public List<string> GetCustomSchemaElementNames(string name)
        {
            var xsd = this.GetCustomSchema();
            var elements = xsd.Elements(x + "element").Select(e => e.Attribute("name").Value).ToList();
            return elements;
        }

    }
}