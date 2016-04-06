using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebApi
{
    public static class Constants
    {
        public const string ENDPOINT_PERMISSIONS_CACHE_KEY = "endpoint-permissions";
        public static string PermissionsSettingsSource = $"{ConfigurationManager.SphSourceDirectory}\\EndpointPermissionSetting\\default.json";
        
    }
}