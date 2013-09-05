using System.Linq;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ReportRow : DomainObject
    {
       
        public new ReportColumn this[string name]
        {
            get
            {
                var col = this.ReportColumnCollection.SingleOrDefault(c => c.Name == name);
                return col;
            }
        }
    }
}