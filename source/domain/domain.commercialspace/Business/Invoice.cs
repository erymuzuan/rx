using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    [XmlInclude(typeof(AdhocInvoice))]
    [XmlInclude(typeof(Rent))]
    public partial class Invoice : Entity
    {

    }
}