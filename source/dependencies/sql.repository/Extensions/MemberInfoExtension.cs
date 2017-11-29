using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.SqlRepository.Extensions
{
    public static class MemberInfoExtension
    {
        public static bool IsFieldPath(this MemberInfo mi)
        {
            return mi.DeclaringType?.Namespace?.StartsWith("System") ?? true;
        }
    }
}
