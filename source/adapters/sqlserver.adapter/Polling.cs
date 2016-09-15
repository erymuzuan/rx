using System;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class Polling
    {
        public long Interval { get; set; }
        public string IntervalPeriod { get; set; }
        public string Query { get; set; }
        public string Option { get; set; }
        public DateTime StartDate { get; set; }
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
                if (this.IntervalPeriod == "days")
                    return Convert.ToInt64(this.Interval * 1000 * 60 * 60 * 24);
                if (this.IntervalPeriod == "weeks")
                    return Convert.ToInt64(this.Interval * 1000 * 60 * 60 * 7);
                throw new InvalidOperationException("Cannot recognize period " + this.IntervalPeriod);

            }
        }
    }
}