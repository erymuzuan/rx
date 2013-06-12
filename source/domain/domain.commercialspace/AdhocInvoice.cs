namespace Bespoke.SphCommercialSpaces.Domain
{
    [EntityType(typeof(Invoice))]
    public partial class AdhocInvoice : Invoice
    {
        public AdhocInvoice()
        {
            this.Type = InvoiceType.AdhocInvoice;
        }
    }
}
