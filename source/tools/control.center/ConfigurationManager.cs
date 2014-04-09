namespace Bespoke.Sph.ControlCenter
{
    public static class ConfigurationManager
    {
        public static System.Collections.Specialized.NameValueCollection AppSettings
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings;
            }
        }

        public static string UpdateBaseUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sph:UpdateBaseUrl"] ?? "http://www.bespoke.com.my/download";
            }
        }
    }
}
