using System;
using Humanizer;

namespace Bespoke.Sph.Domain
{
    public partial class ExecutedActivity : DomainObject
    {

        public string Elapse
        {
            get
            {
                if (null == this.Run) return string.Empty;
                if (null == this.Initiated) return string.Empty;
                var span = this.Run.Value - this.Initiated.Value;
                return span.Humanize();
            }
        }

        public long ElapseSeconds
        {
            get
            {
                if (null == this.Run) return 0;
                if (null == this.Initiated) return 0;
                var span = this.Run.Value - this.Initiated.Value;
                return Convert.ToInt64(span.TotalSeconds);
            }
        }


    }
}