using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(SimpleMapping))]
    [XmlInclude(typeof(FunctoidMapping))]
    partial class PropertyMapping : DomainObject
    {
    }
}
