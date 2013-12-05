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

       
    }
}