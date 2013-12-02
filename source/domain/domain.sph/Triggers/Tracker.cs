namespace Bespoke.Sph.Domain
{
    public partial class Tracker : Entity
    {
        public override int GetId()
        {
            return this.TrackerId;
        }

        public void AddExecutedActivity(Activity act)
        {
            this.ForbiddenActivities.Add(act.WebId);
        }
    }
}