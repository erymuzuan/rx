using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class ComplexVariable : Variable
    {
        public override string GeneratedCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendFormat("public {0} {1} {{get;set;}}", this.TypeName, this.Name);


            return code.ToString();
        }

        public override string GetEmptyJson(WorkflowDefinition wd)
        {
            var fileName = wd.SchemaStoreId;
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = store.GetContent(fileName);
            using (var stream = new MemoryStream(content.Content))
            {
                var xsd = XElement.Load(stream);
                return wd.GenerateJson(this.TypeName, xsd);
            }
        }


        public List<string> GetXsdElementName(string id)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = store.GetContent(id);
            using (var stream = new MemoryStream(content.Content))
            {
                var xsd = XElement.Load(stream);

                XNamespace x = "http://www.w3.org/2001/XMLSchema";
                var elements = xsd.Elements(x + "element").Select(e => e.Attribute("name").Value).ToList();
                return elements;

            }
        }
    }
}