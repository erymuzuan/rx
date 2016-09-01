using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class TextFormatter : DomainObject
    {
        public virtual Task<TextFieldMapping[]> GetFieldMappingsAsync()
        {
            throw new System.NotImplementedException();
        }

        public virtual string GetRecordAttribute()
        {
            return string.Empty;
        }

        public virtual Task<Class> GetPortClassAsync(ReceivePort port)
        {
            var type = new Class {Name = port.Name.ToPascalCase(), Namespace = port.CodeNamespace};
            type.AddNamespaceImport<System.DateTime, JsonIgnoreAttribute, FileInfo, Task>();

            return Task.FromResult(type);
        }
        
    }

    public partial class HtmlTextFormatter : TextFormatter { }
    public partial class JsonTextFormatter : TextFormatter { }
    public partial class XmlTextFormatter : TextFormatter { }
}