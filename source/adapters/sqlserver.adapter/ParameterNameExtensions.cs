using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public static class ParameterNameExtensions
    {
        public static string ToSqlParameter(this Member member)
        {
            if (member.Name.StartsWith("@"))
                return member.Name;
            return "@" + member.Name;
        }
        public static string ToSqlParameter(this string member)
        {
            if (member.StartsWith("@"))
                return member;
            return "@" + member;
        }
    }
}