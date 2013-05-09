using System.Diagnostics;
using System.Windows;

namespace Bespoke.Sph.Windows
{
    public static class WpfHelper
    {
        public static bool IsInDesignMode(this object element)
        {
            if (Application.Current == null) return true;
            var process = Process.GetCurrentProcess().ProcessName.ToLower();
            if(process == "devenv") return true;
            if(process == "blend") return true;
            return false;
        }




    }
}