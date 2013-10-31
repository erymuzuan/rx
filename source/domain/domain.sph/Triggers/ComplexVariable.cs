using System.IO;
using System.Xml.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class ComplexVariable : Variable
    {
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
    }
}