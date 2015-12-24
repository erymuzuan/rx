using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Bespoke.Sph.ControlCenter
{
    public static class Strings
    {
        public static string RegexSingleValue(this string input, string pattern, string group)
        {
            const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var matches = Regex.Matches(input, pattern, OPTIONS);
            return matches.Count == 1 ? matches[0].Groups[@group].Value.Trim() : null;
        }

        public static string TranslatePath(this string path)
        {
            if (Path.IsPathRooted(path))
                return Path.GetFullPath(path);
            var abs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\" + path);
            try
            {
                return Path.GetFullPath(abs);
            }
            catch
            {
                return abs;
            }
        }
        public const string DEFAULT_NAMESPACE = "http://www.bespoke.com.my/";

        public static string Title => "Rx Developer";
    }
}
