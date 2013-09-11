namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class RentalApplication : Entity
    {
        public override string ToString()
        {
            return string.Format("{2} - {1}({3}) \r\n{0}", this.Status, this.CompanyName, this.RegistrationNo, this.CompanyRegistrationNo);
        }
       
    }
}
