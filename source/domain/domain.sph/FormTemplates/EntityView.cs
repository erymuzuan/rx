using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class EntityView : Entity
    {
        public string GenerateEsSortDsl()
        {
            var f = from s in this.SortCollection
                select string.Format("{{\"{0}\":\"{1}\"}}", s.Path, s.Direction.ToString().ToLowerInvariant());
            return "[" + string.Join(",", f.ToArray()) + "]";
        }
    }
}