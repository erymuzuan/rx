using System.Reflection;

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
