using System;
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
            var type = new Class {Name = port.Name.ToPascalCase(), Namespace = port.CodeNamespace};
            type.AddNamespaceImport<DateTime, JsonIgnoreAttribute, FileInfo, Task>();
            type.AddNamespaceImport<DomainObject>();
            type.AddProperty("Uri", typeof(Uri));
            type.AddProperty(@"public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>(); ");
            type.AddMethod(new Method() {Code = @"public void AddHeader<T>(string name, T value){
        this.Headers.Add(name,$""{value}"");
}"});


            type.AddMethod(new Method {Code = GetProcessHeaderCode(port)});
            return Task.FromResult(type);
        }

        protected virtual string GetProcessHeaderCode(ReceivePort port)
        {
            var code = new StringBuilder();
            code.AppendLine($@"private void ProcessHeader({port.Entity} record)
        {{");
            foreach (var field in port.FieldMappingCollection.OfType<HeaderFieldMapping>().Where(x => !string.IsNullOrWhiteSpace(x.Pattern)))
            {
                var name = field.Name.ToCamelCase();
                code.AppendLine($@"var {name}Raw = Strings.RegexSingleValue(this.Headers[""{field.Header}""], {field.Pattern.ToVerbatim()}, ""value"");");
                if (field.Type == typeof(int))
                {
                    code.AppendLine($@"
                        int {name};
                        if(int.TryParse({name}Raw, out {name}))
                        {{
                            record.{field.Name} = {name};
                        }}");
                }
                if (field.Type == typeof(DateTime))
                {
                    code.AppendLine($@"
                        DateTime {name};
                        if(DateTime.TryParse({name}Raw, out {name}))
                        {{
                            record.{field.Name} = {name};
                        }}");
                }
                if (field.Type == typeof(decimal))
                {
                    code.AppendLine($@"
                        decimal {name};
                        if(decimal.TryParse({name}Raw, out {name}))
                        {{
                            record.{field.Name} = {name};
                        }}");
                }
                if (field.Type == typeof(string))
                {
                    code.AppendLine($@"record.{field.Name} = {name}Raw;");
                }
            }
            code.AppendLine("}");

            return code.ToString();
        }
        
    }

    public partial class HtmlTextFormatter : TextFormatter { }
    public partial class JsonTextFormatter : TextFormatter { }
    public partial class XmlTextFormatter : TextFormatter { }
}