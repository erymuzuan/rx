using System;

namespace Bespoke.Sph.Domain
{
    public partial class ExceptionFilter : DomainObject
    {
        public long IntervalInMilisecods
        {
            get
            {
                if (this.IntervalPeriod == "miliseconds")
                    return Convert.ToInt64(this.Interval);
                if (this.IntervalPeriod == "seconds")
                    return Convert.ToInt64(this.Interval * 1000);
                if (this.IntervalPeriod == "minutes")
                    return Convert.ToInt64(this.Interval * 1000 * 60);
                if (this.IntervalPeriod == "hours")
                    return Convert.ToInt64(this.Interval * 1000 * 60 * 60);
                throw new InvalidOperationException("Cannot recognize period " + this.IntervalPeriod);

            }
        }
    }
}