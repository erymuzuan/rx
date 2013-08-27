namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ReportColumn : DomainObject
    {
        public override string ToString()
        {
            return string.Format("{0}", this.Value);
        }
    }
}