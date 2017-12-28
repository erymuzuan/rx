using System.Linq;
using System.Xml.Linq;
using Bespoke.Sph.Domain;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;

namespace odata.queryparser
{
    public static class EntityDefinitionExtension
    {
        public static IEdmModel GenerateEdmModel(this EntityDefinition ed)
        {
            XNamespace edmx = "http://docs.oasis-open.org/odata/ns/edmx";
            XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

            var entityNamespace = $"{ed.Name}Service";

            var simpleMemberProperties = from m in ed.MemberCollection.OfType<SimpleMember>()
                select m.ToEdmxEntityTypeProperty();
            var complexMemberProperties = from m in ed.MemberCollection.OfType<ComplexMember>()
                select m.ToEdmxEntityTypeProperty(entityNamespace);
            var properties = simpleMemberProperties.Concat(complexMemberProperties);
            var key = new XElement(edm + "Key", new XElement(edm + "PropertyRef", new XAttribute("Name", "Id")));
            var id = new SimpleMember {Name = "Id", Type = typeof(string)}.ToEdmxEntityTypeProperty();
            var entityType = new XElement(edm + "EntityType", new XAttribute("Name", ed.Name), key, id, properties);
            var complexType = from m in ed.MemberCollection.OfType<ComplexMember>()
                select m.ToEdmxComplexTypeProperty();
            var entitySet = new XElement(edm + "EntitySet", new XAttribute("Name", ed.Plural),
                new XAttribute("EntityType", entityNamespace + "." + ed.Name));
            var entityContainer = new XElement(edm + "EntityContainer", new XAttribute("Name", ed.Name + "Entity"),
                entitySet);
            var schema = new XElement(edm + "Schema", new XAttribute("Namespace", entityNamespace), entityType,
                complexType, entityContainer);

            var xml = new XDocument(new XElement(edmx + "Edmx", new XAttribute("Version", "4.0"),
                new XElement(edmx + "DataServices", schema)
            ));

            var reader = xml.CreateReader();
            return CsdlReader.Parse(reader);
        }
    }
}