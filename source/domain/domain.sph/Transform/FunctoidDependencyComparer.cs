using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public class FunctoidDependencyComparer : IComparer<Functoid>
    {
        private readonly TransformDefinition m_map;

        public FunctoidDependencyComparer(TransformDefinition map)
        {
            m_map = map;
        }

        private IList<string> GetDependentFunctoids(string id)
        {
            var list = new List<string>();
            var functoid = m_map.FunctoidCollection.SingleOrDefault(f => f.WebId == id);
            if (null == functoid) return list;
            foreach (var xid in functoid.ArgumentCollection.Select(a => a.Functoid))
            {
                list.Add(xid);
                var child = GetDependentFunctoids(xid);
                list.AddRange(child);
            }

            return list;
        }
        public int Compare(Functoid x, Functoid y)
        {
            const int ONE = 1;
            const int MINUS_ONE = -1;
            const int ZERO = 0;

            if (x.WebId == y.WebId) return ZERO;

            var xargs = this.GetDependentFunctoids(x.WebId);
            var yargs = this.GetDependentFunctoids(y.WebId);

            if (xargs.Contains(y.WebId)) return ONE;
            if (yargs.Contains(x.WebId)) return MINUS_ONE;
            return ZERO;
        }
    }
}