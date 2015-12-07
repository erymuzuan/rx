namespace Bespoke.Sph.ControlCenter
{
    public static class ConfigurationManager
    {
        public static System.Collections.Specialized.NameValueCollection AppSettings => System.Configuration.ConfigurationManager.AppSettings;

        public static string UpdateBaseUrl => System.Configuration.ConfigurationManager.AppSettings["sph:UpdateBaseUrl"] ?? "http://www.bespoke.com.my/download";
    }
}
