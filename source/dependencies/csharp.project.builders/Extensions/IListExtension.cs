using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Csharp.CompilersServices.Extensions
{
    public static class ListExtension
    {
        public static void AddRange<T>(this List<T> list, params T[] items)
        {
            list.AddRange(items.AsEnumerable());
        }
    }
}
