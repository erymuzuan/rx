using System;
using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class Sort : DomainObject
    {
        public static IEnumerable<Sort> Parse(string odata)
        {
            var queries = odata.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var sorts = from q in queries
                          let words = q.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                          let path = words[0]
                          let direction = ( words.Length == 2 && words[1] == "desc") ? SortDirection.Desc : SortDirection.Asc
                          select new Sort{Path = path, Direction =  direction};

            return sorts.ToArray();
        }
    }
}