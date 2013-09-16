using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(AdhocInvoice))]
    [XmlInclude(typeof(Rent))]
    public partial class Invoice : Entity
    {

    }
}