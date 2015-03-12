using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public class FunctoidDependencyComparer : IComparer<Functoid>
    {
        public int Compare(Functoid x, Functoid y)
        {
            const int ONE = 1;
            const int MINUS_ONE = -1;
            const int ZERO = 0;

            if (x.WebId == y.WebId) return ZERO;
            //if (x.ArgumentCollection.Count == 0) return ONE;
            //if (y.ArgumentCollection.Count == 0) return MINUS_ONE;

            var xargs = x.ArgumentCollection.Select(a => a.Functoid).ToArray();
            var yargs = y.ArgumentCollection.Select(a => a.Functoid).ToArray();
            if (xargs.Contains(y.WebId)) return ONE;
            if (yargs.Contains(x.WebId)) return MINUS_ONE;
            return ZERO;
        }
    }
}