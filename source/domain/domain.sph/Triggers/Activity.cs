using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(ScreenActivity))]
    public partial class Activity : DomainObject
    {
        
    }
}