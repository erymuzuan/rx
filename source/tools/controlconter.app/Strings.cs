using System;
using System.IO;

namespace Bespoke.Sph.ControlCenter
{
    public static class Strings
    {
        public static string TranslatePath(this string path)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\" + path);
        }
        public const string DefaultNamespace = "http://www.bespoke.com.my/";
   
    }
}
