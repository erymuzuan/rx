using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public class FunctoidDependencyComparer : IComparer<Functoid>
    {
        public int Compare(Functoid x, Functoid y)
        {
            if (x.WebId == y.WebId) return 0;
            if (x.ArgumentCollection.Count == 0) return -1;
            if (y.ArgumentCollection.Count == 0) return 1;

            var args = x.ArgumentCollection.Select(a => a.Functoid).ToArray();
            if (args.Contains(y.WebId)) return 1;
            return -1;
        }
    }
}