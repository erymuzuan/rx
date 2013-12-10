namespace Bespoke.Sph.Domain
{
    public partial class JoinActivity : Activity
    {
        public override bool IsAsync
        {
            get { return true; }
        }
    }
}