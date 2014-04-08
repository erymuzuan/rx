using System;
using System.IO;

namespace Bespoke.Sph.ControlCenter
{
    public static class Strings
    {
        public static string TranslatePath(this string path)
        {
            var abs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\" + path);
            return Path.GetFullPath(abs);
        }
        public const string DefaultNamespace = "http://www.bespoke.com.my/";

        public static string Title { get { return "Rx Developer"; } }
    }
}
