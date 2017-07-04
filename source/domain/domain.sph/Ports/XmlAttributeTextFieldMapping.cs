using System;

namespace Bespoke.Sph.Domain
{
    public partial class XmlAttributeTextFieldMapping : TextFieldMapping
    {
        public string GenerateReadValueCode(string elementName)
        {
            if (this.Path.Contains("$Parent$"))
                elementName += "." + this.Path.Replace("$Parent$", "Parent").Replace("." + Name, "");
            if (this.Path.Contains("$Root$"))
                elementName = "doc";
            //TODO: create a subclass for each Type and Nullability
            if (this.Type == typeof(string))
                return $@"{elementName}.Attribute(""{Name}"")?.Value";

            if (this.Type == typeof(int) && !this.IsNullable)
                return $@"int.Parse({elementName}.Attribute(""{Name}"")?.Value?? ""{DefaultValueString}"")";
            if (this.Type == typeof(int) && this.IsNullable)
                return $@"{elementName}.Attribute(""{Name}"")?.Value.ParseNullableInt32()";

            if (this.Type == typeof(decimal) && !this.IsNullable)
                return $@"decimal.Parse({elementName}.Attribute(""{Name}"")?.Value ?? ""{DefaultValueString}"")";
            if (this.Type == typeof(decimal) && this.IsNullable)
                return $@"{elementName}.Attribute(""{Name}"")?.Value.ParseNullableDecimal()";

            if (this.Type == typeof(DateTime) && !this.IsNullable)
                return $@"DateTime.Parse({elementName}.Attribute(""{Name}"")?.Value ?? ""{DefaultValueString}"")";
            if (this.Type == typeof(DateTime) && this.IsNullable)
                return $@"{elementName}.Attribute(""{Name}"")?.Value.ParseNullableDateTime()";

            if (this.Type == typeof(bool) && !this.IsNullable)
                return $@"bool.Parse({elementName}.Attribute(""{Name}"")?.Value ?? ""{DefaultValueString}"")";
            if (this.Type == typeof(bool) && this.IsNullable)
                return $@"{elementName}.Attribute(""{Name}"")?.Value.ParseNullableBoolean()";

            return $@"{elementName}.Attribute(""{Name}"")?.Value";
        }
        
    }
}