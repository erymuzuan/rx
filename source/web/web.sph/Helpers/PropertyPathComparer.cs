using System;
using System.Collections.Generic;

namespace Bespoke.Sph.Web.Helpers
{
    public class PropertyPathComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (y.Contains(".") && !x.Contains(".")) return -1;
            if (!y.Contains(".") && x.Contains(".")) return 1 ;
            return String.Compare(x, y, StringComparison.Ordinal);
        }
    }
}