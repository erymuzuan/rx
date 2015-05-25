namespace Bespoke.Sph.Domain
{
    public partial class Watcher : Entity
    {
        public override string ToString()
        {
            return $"Watcher : {this.EntityName}({this.EntityId}) by {this.User}";
        }
    }
}
