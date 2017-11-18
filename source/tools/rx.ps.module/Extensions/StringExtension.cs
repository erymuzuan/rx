using System.Text.RegularExpressions;

namespace Bespoke.Sph.RxPs.Extensions
{
    public static class StringExtension
    {
        public static string RegexSingleValue(this string input, string pattern, string group)
        {
            const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var matches = Regex.Matches(input, pattern, OPTIONS);
            return matches.Count == 1 ? matches[0].Groups[@group].Value.Trim() : null;
        }
    }
}