using System;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class ReportRow : DomainObject
    {
       
        public new ReportColumn this[string name]
        {
            get
            {
                var col = this.ReportColumnCollection.SingleOrDefault(c => c.Name == name);
                if(null == col) Console.WriteLine("Cannot find column " + name);
                return col;
            }
        }
    }
}