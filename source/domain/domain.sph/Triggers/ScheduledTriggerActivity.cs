namespace Bespoke.Sph.Domain
{
    public partial class ScheduledTriggerActivity : Activity
    {
        public override bool IsAsync
        {
            get { return true; }
        }
    }
}