using System;
using System.Linq;
using System.Xml.Linq;

namespace Bespoke.Sph.Domain
{
    public class XmlElementTextFieldMapping : TextFieldMapping
    {
        private readonly XElement m_element;

        public XmlElementTextFieldMapping(XElement element)
        {
            m_element = element;
        }

        public XmlElementTextFieldMapping()
        {

        }
        public override Member GenerateMember()
        {
            if (m_element.HasElements || m_element.HasAttributes)
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

        public string GenerateReadValueCode(string elementName, XmlElementTextFieldMapping[] children = null)
        {
            var elemetAssignments = "";
            var attributeAssignments = "";
            if (this.TypeName == this.Name)
            {
                attributeAssignments = this.FieldMappingCollection.OfType<XmlAttributeTextFieldMapping>()
                    .Select(x => x.Name + " = " + x.GenerateReadValueCode(elementName))
                    .ToString(",\r\n");
                // TODO : when we have more ComplextElement , should recurse
                elemetAssignments = this.FieldMappingCollection.OfType<XmlElementTextFieldMapping>()
                    .Select(x => x.Name + " = " + x.GenerateReadValueCode($"{elementName}.Element(\"{this.Name}\")"))
                    .ToString(",\r\n");
            }
            if (this.TypeName == this.Name)
                return $@" new {Name}{{
                    {attributeAssignments}
                    {elemetAssignments}
                }}";

            //TODO: create a subclass for each Type and Nullability
            if (this.Type == typeof(string))
                return $@"{elementName}.Element(""{Name}"")?.Value";

            if (this.Type == typeof(int) && !this.IsNullable)
                return $@"int.Parse({elementName}.Element(""{Name}"")?.Value?? ""{this.SampleValue}"")";
            if (this.Type == typeof(int) && this.IsNullable)
                return $@"{elementName}.Element(""{Name}"")?.Value.ParseNullableInt32()";

            if (this.Type == typeof(decimal) && !this.IsNullable)
                return $@"decimal.Parse({elementName}.Element(""{Name}"")?.Value ?? ""{SampleValue}"")";
            if (this.Type == typeof(decimal) && this.IsNullable)
                return $@"{elementName}.Element(""{Name}"")?.Value.ParseNullableDecimal()";

            if (this.Type == typeof(DateTime) && !this.IsNullable)
                return $@"DateTime.Parse({elementName}.Element(""{Name}"")?.Value ?? ""{SampleValue}"")";
            if (this.Type == typeof(DateTime) && this.IsNullable)
                return $@"{elementName}.Element(""{Name}"")?.Value.ParseNullableDateTime()";

            if (this.Type == typeof(bool) && !this.IsNullable)
                return $@"bool.Parse({elementName}.Element(""{Name}"")?.Value ?? ""{SampleValue}"")";
            if (this.Type == typeof(bool) && this.IsNullable)
                return $@"{elementName}.Element(""{Name}"")?.Value.ParseNullableBoolean()";

            return $@"{elementName}.Element(""{Name}"")?.Value";
        }

    }
}