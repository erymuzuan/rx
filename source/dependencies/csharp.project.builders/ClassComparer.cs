using System.Collections.Generic;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Csharp.CompilersServices
{
    public class ClassComparer : IEqualityComparer<Class>
    {
        public bool Equals(Class x, Class y)
        {
            if (null == x) return false;
            if (null == y) return false;
            return $"{x.Namespace}.{x.Name}" == $"{y.Namespace}.{y.Name}";
        }

        public int GetHashCode(Class obj)
        {
            return $"{obj.Namespace}.{obj.Name}".GetHashCode();
        }
    }
}