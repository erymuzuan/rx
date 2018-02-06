using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Bespoke.Sph.Domain;

namespace odata.queryparser
{
    public static class MemberExtension
    {
        public static XElement ToEdmxEntityTypeProperty(this SimpleMember m)
        {
            XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

            var attributes = new List<XAttribute>
            {
                new XAttribute("Name", m.Name),
                new XAttribute("Type", GetEdmType(m.Type.Name)),
                new XAttribute("Nullable", m.IsNullable)
            };

            var maxLength = new XAttribute("MaxLength", "255");
            if (typeof(string) == m.Type)
                attributes.Add(maxLength);

            return new XElement(edm + "Property", attributes);
        }

        public static XElement ToEdmxEntityTypeProperty(this ComplexMember m, string entityNamespace)
        {
            XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

            var attributes = new List<XAttribute>
            {
                new XAttribute("Name", m.Name),
                new XAttribute("Type", entityNamespace + "." + m.TypeName)
            };

            return new XElement(edm + "Property", attributes);
        }

        public static XElement ToEdmxComplexTypeProperty(this ComplexMember m)
        {
            XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

            var nodes = m.MemberCollection.Select(de => new List<XAttribute>
                {
                    new XAttribute("Name", ((dynamic) de).Name),
                    new XAttribute("Type", GetEdmType(((dynamic) de).Type.Name))
                })
                .Select(attributes => new XElement(edm + "Property", attributes));

            return new XElement(edm + "ComplexType", new XAttribute("Name", m.TypeName), nodes);
        }

        private static string GetEdmType(string name)
        {
            if (name == "DateTime")
                return "Edm.Date";
            return "Edm." + name;
        }
    }
}