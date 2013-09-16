using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class Contract : Entity
    {
        public string GetQuarters(DateTime currentDate)
        {
            if (this.RentType != "3 bulan") throw new InvalidOperationException("Only for 3 months");
            var quarter = "Q1";
            var sofar = (currentDate - this.StartDate).TotalDays;
            var balanceDays = sofar % 365;
            if (balanceDays >= 90) quarter = "Q2";
            if (balanceDays > 180) quarter = "Q3";
            if (balanceDays > 270) quarter = "Q4";

            return string.Format("{0}", quarter);
        }

        public string GetHalf(DateTime currentDate)
        {
            if (this.RentType != "6 bulan") throw new InvalidOperationException("Only for 6 months");
            var half = "H1";
            var sofar = (currentDate - this.StartDate).TotalDays;
            var balanceDays = sofar % 365;
            if (balanceDays > 180) half = "H2";

            return string.Format("{0}", half);
        }

       
    }
}