using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Path = {Name}({SampleValue}), TypeName= {TypeName}")]
    public partial class TextFieldMapping : DomainObject
    {
        [JsonIgnore]
        public virtual Type Type
        {
            get
            {
                return Strings.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }
        public virtual Member GenerateMember()
        {
            var simple = new SimpleMember
            {
                Name = this.Name,
                TypeName = this.TypeName,
                AllowMultiple = false,
                IsNullable = this.IsNullable,
                IsNotIndexed = true

            };
            if (!this.IsComplex) return simple;

            var complex = new ComplexMember
            {
                Name = this.Name,
                TypeName = this.TypeName,
                AllowMultiple = this.AllowMultiple
            };
            var children = from f in this.FieldMappingCollection
                           select f.GenerateMember();
            complex.MemberCollection.AddRange(children);
            return complex;
        }

        protected virtual string GenerateReadFieldCode(string objectName, string rawName = "")
        {
            if (typeof(int) == this.Type)
                return GenerateReadIntField(objectName, rawName);
            var code = new StringBuilder();
            code.AppendLine($@"        if({Name}Raw == {this.NullPlaceholder.ToVerbatim()})");
            code.AppendLine($@"             return null;");
            if (this.Type == typeof(string))
                code.AppendLine($@"        return {Name}Raw;");
            if (this.Type == typeof(DateTime))
                code.AppendLine($@"        return DateTime.ParseExact({Name}Raw,{this.Converter.ToVerbatim()}, System.Globalization.CultureInfo.InvariantCulture);");

            // TODO : parse according to format
            if (this.Type == typeof(int))
                code.AppendLine($@"        
                                        var n = 0;
                                        if(int.TryParse({Name}Raw, NumberStyles.Any, CultureInfo.InvariantCulture, out n)) return n;
                                        return null;");
            //TODO : parse according to format
            if (this.Type == typeof(decimal))
                code.AppendLine($@"        
                                        var n = 0m;
                                        if(decimal.TryParse({Name}Raw, NumberStyles.Any, CultureInfo.InvariantCulture, out n)) return n;
                                        return null;");
            // TODO : assuming Converter is the True string
            if (this.Type == typeof(bool))
                code.AppendLine($@"  return {Name}Raw == {this.Converter.ToVerbatim()};");
            return string.Empty;
        }

        protected virtual string GenerateReadIntField(string objectName, string rawName)
        {
            var code = new StringBuilder();
            if (string.IsNullOrWhiteSpace(rawName))
                rawName = $"{Name.ToCamelCase()}Raw";
            var name = Name.ToCamelCase();
            code.AppendLine($@"
                        int {name};
                        if(int.TryParse({rawName}, out {name}))
                        {{
                            {objectName}.{Name} = {name};
                        }}");

            return code.ToString();
        }
    }
}