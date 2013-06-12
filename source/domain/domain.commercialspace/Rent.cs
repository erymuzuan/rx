namespace Bespoke.SphCommercialSpaces.Domain
{
    [EntityType(typeof(Invoice))]
    public partial class Rent : Invoice
    {
        public Rent()
        {
            this.Type = InvoiceType.Rental;
        }
    }
}
