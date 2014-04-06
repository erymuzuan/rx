namespace Bespoke.Sph.Domain
{
    public partial class Watcher : Entity
    {
        public override string ToString()
        {
            return string.Format("Watcher : {0}({1}) by {2}", this.EntityName, this.EntityId, this.User);
        }
    }
}
