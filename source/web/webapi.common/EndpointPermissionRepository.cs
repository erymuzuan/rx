using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.WebApi
{
    public class EndpointPermissionRepository : IEndpointPermissionRepository
    {

        public Task<EndpointPermissonSetting> FindSettingsAsync(string controller, string action)
        {

            var cache = ObjectBuilder.GetObject<ICacheManager>();
            var savedSettings = cache.Get<EndpointPermissonSetting[]>(Constants.ENDPOINT_PERMISSIONS_CACHE_KEY);
            if (null == savedSettings)
            {
                if (File.Exists(Constants.PermissionsSettingsSource))
                {
                    var settings = new JsonSerializerSettings { ContractResolver = new ClaimSettingResolver() };
                    savedSettings = JsonConvert.DeserializeObject<EndpointPermissonSetting[]>(File.ReadAllText(Constants.PermissionsSettingsSource), settings);
                    cache.Insert(Constants.ENDPOINT_PERMISSIONS_CACHE_KEY, savedSettings, TimeSpan.FromSeconds(300), Constants.PermissionsSettingsSource);
                }
            }
            if (null == savedSettings) savedSettings = Array.Empty<EndpointPermissonSetting>();

            var root = savedSettings.Single(x => !x.HasController && !x.HasAction && !x.HasParent);
            var parentName = "Custom";
            if (savedSettings.Any(x => x.Controller == controller))
            {
                parentName = savedSettings.Single(x => x.Controller == controller).Parent;
            }
            if (savedSettings.Any(x => x.Action == action && x.Controller == controller))
            {
                parentName = savedSettings.Single(x => x.Action == action && x.Controller == controller).Parent;
            }

            var permission = new EndpointPermissonSetting { Controller = controller, Action = action, Claims = root.Claims };

            // get the action permission
            var actionPermission = savedSettings.SingleOrDefault(x => x.Parent == parentName && x.Controller == controller && x.Action == action);
            if (null != actionPermission)
            {
                permission.AddParentClaims(actionPermission.Claims);
            }
            // get the controller
            var controllerPermission = savedSettings.SingleOrDefault(x => x.Parent == parentName && x.Controller == controller && !x.HasAction);
            if (null != controllerPermission)
            {
                permission.AddParentClaims(controllerPermission.Claims);
            }
            // get the parent claims settings
            var parentPermission = savedSettings.SingleOrDefault(x => x.Parent == parentName && !x.HasController && !x.HasAction);
            if (null != parentPermission)
            {
                permission.AddParentClaims(parentPermission.Claims);
            }

            // get the root claims
            var rootPermission = savedSettings.Single(x => !x.HasAction && !x.HasController && !x.HasParent);
            permission.AddParentClaims(rootPermission.Claims);

            return Task.FromResult(permission);
        }

        public Task<IEnumerable<EndpointPermissonSetting>> LoadAsync()
        {
            var list = new List<EndpointPermissonSetting> {new EndpointPermissonSetting
            {
                Claims = new[]
                {
                    new ClaimSetting(new Claim(ClaimTypes.Role, "administators", "a")) ,
                    new ClaimSetting(new Claim(ClaimTypes.Role, "developers"), "a"),
                }
            } };

            if (File.Exists(Constants.PermissionsSettingsSource))
            {
                var jst = new JsonSerializerSettings { ContractResolver = new ClaimSettingResolver() };
                var settings = JsonConvert.DeserializeObject<EndpointPermissonSetting[]>(File.ReadAllText(Constants.PermissionsSettingsSource), jst);
                list.Clear();
                list.AddRange(settings);
            }

            return Task.FromResult(list.AsEnumerable());
        }

        public Task SaveAsync(IEnumerable<EndpointPermissonSetting> settings)
        {
            var json = settings.ToJson();
            File.WriteAllText(Constants.PermissionsSettingsSource, json);
            return Task.FromResult(0);
        }


    }
}