using System;

namespace Bespoke.Sph.Domain
{
    [StoreAsSource(IsElasticsearch = true)]
    public partial class QuotaPolicy : Entity { }
    public partial class RateLimit : DomainObject { }
    public partial class QuotaLimit : DomainObject { }
    public partial class BandwidthLimit : DomainObject { }
    public partial class EndpointLimit : DomainObject { }

    public partial class TimePeriod : DomainObject
    {
        public int ToSeconds()
        {
            switch (this.Unit)
            {
                case "Miliseconds": return this.Count / 1000;
                case "Seconds": return this.Count;
                case "Minutes": return this.Count * 60;
                case "Days": return this.Count * 60 * 24;
                case "Weeks": return this.Count * 60 * 24 * 7;
            }

            return 0;
        }
        public TimeSpan ToTimeSpan()
        {
            var seconds = this.ToSeconds();
            return TimeSpan.FromSeconds(seconds);
        }

    }
}
