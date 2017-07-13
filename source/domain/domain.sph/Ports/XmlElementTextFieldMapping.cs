using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class XmlElementTextFieldMapping : TextFieldMapping
    {
        public XElement Element { get; set; }

        public XmlElementTextFieldMapping(XElement element)
        {
            Element = element;
        }

        public XmlElementTextFieldMapping()
        {

        }
        public override Member GenerateMember()
        {
            if (Element.HasElements || Element.HasAttributes)
            {
                var complex = new ComplexMember
                {
                    Name = this.Name,
                    TypeName = this.Name,
                    AllowMultiple = this.AllowMultiple
                };
                var children = from f in this.FieldMappingCollection
                               select f.GenerateMember();
                complex.MemberCollection.AddRange(children);
                return complex;
            }
            return base.GenerateMember();
        }

        public string GenerateReadValueCode(string path, string elementName, XmlElementTextFieldMapping[] children = null)
        {
            var complex = this.IsComplex && !this.AllowMultiple;
            var array = this.IsComplex && this.AllowMultiple;
            if (array)
            {
                return "//array";
            }
            if (complex)
            {
                var attributeAssignments = this.FieldMappingCollection.OfType<XmlAttributeTextFieldMapping>()
                    .Where(x => !x.AllowMultiple)
                    .Select(x => $"{path}.{this.Name}.{x.Name} = " + x.GenerateReadValueCode("ce", elementName) + ";")
                    .ToString("\r\n");

                var elementAssignments = this.FieldMappingCollection.OfType<XmlElementTextFieldMapping>()
                    .Where(x => !x.AllowMultiple && !x.IsComplex)
                    .Select(x => $"{path}.{this.Name}.{x.Name} = " + x.GenerateReadValueCode(path, $"{elementName}.Element(xn + \"{this.Name}\")?") + ";")
                    .ToString("\r\n");

                var complexElementAssignments = this.FieldMappingCollection.OfType<XmlElementTextFieldMapping>()
                    .Where(x => !x.AllowMultiple && x.IsComplex & !x.IsNullable)
                    .Select(x => $"{path}.{this.Name}.{x.Name} = " + x.GenerateReadValueCode($"{path}.{Name}", $"{elementName}.Element(xn + \"{this.Name}\")?") + ";")
                    .ToString("\r\n");

                var arrayElements = this.FieldMappingCollection.OfType<XmlElementTextFieldMapping>().Where(x => x.IsComplex && x.AllowMultiple);
                var arrayAssignments = new StringBuilder();
                foreach (var xe in arrayElements)
                {

                    var elements = xe.FieldMappingCollection.OfType<XmlElementTextFieldMapping>();
                    var attributes = xe.FieldMappingCollection.OfType<XmlAttributeTextFieldMapping>().ToArray();

                    var elementsCode = elements.ToString("\r\n", x => $"item.{x.Name} = " + x.GenerateReadValueCode($"{path}.{x.Name}", "ce") + ";");
                    var attributesCode = attributes.ToString("\r\n", x => $"item.{x.Name} = " + x.GenerateReadValueCode($"{path}.{x.Name}", "ce") + ";");
                    arrayAssignments.AppendLine($@"
                           foreach(var ce in {elementName}.Element(""{Element.Name}"").Elements(xn + ""{xe.Name}""))
                           {{
                                var item = new {xe.Name}();
{elementsCode}
{attributesCode}

                                {path}.{Name}.{xe.Name}.Add(item);

                           }}");
                }

          

                return this.IsNullable ? $@"{elementName}.Element(xn + ""{Name}"") == null ? default({TypeName}) : new {Name}();
                    if(null != {path}.{Name})
                    {{
                        {attributeAssignments}
                        {elementAssignments}
                        {arrayAssignments}
                    }}"
                    : $@" new {Name}();
                    {attributeAssignments}
                    {elementAssignments}
                    {arrayAssignments}
                    {complexElementAssignments}
                ";

            }
            //TODO: create a subclass for each Type and Nullability
            if (this.Type == typeof(string))
                return $@"{elementName}.Element(xn + ""{Name}"")?.Value";

            if (this.Type == typeof(int) && !this.IsNullable)
                return $@"int.Parse({elementName}.Element(xn + ""{Name}"")?.Value?? ""{DefaultValueString}"")";
            if (this.Type == typeof(int) && this.IsNullable)
                return $@"{elementName}.Element(xn + ""{Name}"")?.Value.ParseNullableInt32()";

            if (this.Type == typeof(decimal) && !this.IsNullable)
                return $@"decimal.Parse({elementName}.Element(xn + ""{Name}"")?.Value ?? ""{DefaultValueString}"")";
            if (this.Type == typeof(decimal) && this.IsNullable)
                return $@"{elementName}.Element(xn + ""{Name}"")?.Value.ParseNullableDecimal()";

            if (this.Type == typeof(DateTime) && !this.IsNullable)
                return $@"DateTime.Parse({elementName}.Element(xn + ""{Name}"")?.Value ?? ""{DefaultValueString}"")";
            if (this.Type == typeof(DateTime) && this.IsNullable)
                return $@"{elementName}.Element(xn + ""{Name}"")?.Value.ParseNullableDateTime()";

            if (this.Type == typeof(bool) && !this.IsNullable)
                return $@"bool.Parse({elementName}.Element(xn + ""{Name}"")?.Value ?? ""{DefaultValueString}"")";
            if (this.Type == typeof(bool) && this.IsNullable)
                return $@"{elementName}.Element(xn + ""{Name}"")?.Value.ParseNullableBoolean()";

            return $@"{elementName}.Element(xn + ""{Name}"")?.Value";
        }

    }
}