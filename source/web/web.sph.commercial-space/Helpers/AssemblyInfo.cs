using System.Reflection;

namespace Bespoke.Station.Web.Helpers
{
    public static class AssemblyInfo
    {
        private static string m_version;

        public static string Version
        {
            get
            {
                if(string.IsNullOrWhiteSpace(m_version))
                {
                    var atts = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                    if (atts.Length == 1)
                    {
                        m_version = ((AssemblyFileVersionAttribute)atts[0]).Version;
                    }
                    
                }
                return m_version;
            }
        }
    }
}