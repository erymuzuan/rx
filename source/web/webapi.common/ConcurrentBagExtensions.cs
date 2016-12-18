using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Bespoke.Sph.WebApi
{
    public static class ConcurrentBagExtensions
    {
        public static void AddRange<T>(this ConcurrentBag<T> bags, IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                bags.Add(item);
            }
        }
    }
}