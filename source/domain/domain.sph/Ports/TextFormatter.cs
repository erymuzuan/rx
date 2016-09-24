using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class TextFormatter : DomainObject
    {
        public virtual Task<TextFieldMapping[]> GetFieldMappingsAsync()
        {
            throw new NotImplementedException();
        }

        public virtual string GetRecordAttribute()
        {
            return string.Empty;
        }

        public virtual Task<Class> GetPortClassAsync(ReceivePort port)
        {
            var type = new Class { Name = port.Name.ToPascalCase(), Namespace = port.CodeNamespace };
            type.AddNamespaceImport<DateTime, JsonIgnoreAttribute, FileInfo, Task>();
            type.AddNamespaceImport<DomainObject, IDictionary<string, string>, System.Text.RegularExpressions.Match>();
            type.AddProperty("Uri", typeof(Uri));
            type.AddProperty(@"public ILogger Logger {get;} ");
            type.AddProperty(@"public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>(); ");
            type.AddMethod(new Method() { Code = @"public void AddHeader<T>(string name, T value){
        this.Headers.Add(name,$""{value}"");
}" });

            type.CtorCollection.Add($"public {type.Name}(ILogger logger){{ this.Logger = logger; }}");


            type.AddMethod(new Method { Code = GetProcessHeaderCode(port) });
            return Task.FromResult(type);
        }

        protected virtual string GetProcessHeaderCode(ReceivePort port)
        {
            var headers = port.FieldMappingCollection.OfType<HeaderFieldMapping>().Where(f => !string.IsNullOrWhiteSpace(f.Pattern));
            var uriFields = port.FieldMappingCollection.OfType<UriFieldMapping>().Where(x => !string.IsNullOrWhiteSpace(x.Pattern));


            var code = new StringBuilder();
            code.AppendLine($@"private void ProcessHeader({port.Entity} record)");
            code.AppendLine("{");

            code.JoinAndAppendLine(headers, "\r\n", f => f.GenerateProcessRecordCode());
            code.JoinAndAppendLine(uriFields, "\r\n", f => f.GenerateProcessRecordCode());

            code.AppendLine("}");
            return code.ToString();
        }

    }

    public partial class HtmlTextFormatter : TextFormatter { }
    public partial class JsonTextFormatter : TextFormatter { }
    public partial class XmlTextFormatter : TextFormatter { }
}