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
    public static class Constants
    {
        public const string ENDPOINT_PERMISSIONS_CACHE_KEY = "endpoint-permissions";
        public static string PermissionsSettingsSource = $"{ConfigurationManager.SphSourceDirectory}\\EndpointPermissionSetting\\default.json";
        
    }
    public class EndpointPermissionRepository : IEndpointPermissionRepository
    {
        private List<EndpointPermissonSetting> m_permissionTree;

        public EndpointPermissionRepository()
        {
            this.BuildPermissionTree();
        }
        public Task<EndpointPermissonSetting> FindSettingsAsync(string controller, string action)
        {
            var cache = ObjectBuilder.GetObject<ICacheManager>();
            var savedSettings = cache.Get<EndpointPermissonSetting[]>(Constants.ENDPOINT_PERMISSIONS_CACHE_KEY);
            if (null == savedSettings)
            {
                if (File.Exists(Constants.PermissionsSettingsSource))
                {
                    savedSettings = JsonConvert.DeserializeObject<EndpointPermissonSetting[]>(File.ReadAllText(Constants.PermissionsSettingsSource));
                    cache.Insert(Constants.ENDPOINT_PERMISSIONS_CACHE_KEY, savedSettings, TimeSpan.FromSeconds(300), Constants.PermissionsSettingsSource);
                }
            }
            if (null == savedSettings) savedSettings = Array.Empty<EndpointPermissonSetting>();

            var parentName = m_permissionTree.FirstOrDefault(x => x.Controller == controller && x.Action == action)?.Parent;
            var root = savedSettings.Single(x => !x.HasController && !x.HasAction && !x.HasParent);

            var permission = new EndpointPermissonSetting { Controller = controller, Action = action, Claims = root.Claims };
            if (!string.IsNullOrWhiteSpace(parentName))
            {
                // get the parent claims settings
                var parentPermission = savedSettings.SingleOrDefault(x => x.Parent == parentName && !x.HasController && !x.HasAction);
                if (null != parentPermission)
                {
                    permission.OverrideClaims(parentPermission.Claims);
                }

                // get the controller
                var controllerPermission = savedSettings.SingleOrDefault(
                        x => x.Parent == parentName && x.Controller == controller && !x.HasAction);
                if (null != controllerPermission)
                {
                    permission.OverrideClaims(controllerPermission.Claims);
                }

                // get the action permission
                var actionPermission =
                    savedSettings.SingleOrDefault(
                        x => x.Parent == parentName && x.Controller == controller && x.Action == action);
                if (null != actionPermission)
                {
                    permission.OverrideClaims(actionPermission.Claims);
                }
            }
            return Task.FromResult(permission);
        }

        public Task<IEnumerable<EndpointPermissonSetting>> LoadAsync()
        {
            var list = new List<EndpointPermissonSetting> {new EndpointPermissonSetting
            {
                Claims = new[]
                {
                    new ClaimSetting(new Claim(ClaimTypes.Role, "administators")) {Permission = "a"},
                    new ClaimSetting(new Claim(ClaimTypes.Role, "developers")) {Permission = "a"},
                }
            } };

            if (File.Exists(Constants.PermissionsSettingsSource))
            {
                var settings = JsonConvert.DeserializeObject<EndpointPermissonSetting[]>(File.ReadAllText(Constants.PermissionsSettingsSource));
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

        private void BuildPermissionTree()
        {
            m_permissionTree = new List<EndpointPermissonSetting>();
            var context = new SphDataContext();
            var eds = context.LoadFromSources<EntityDefinition>().ToArray();
            var searches = eds.Where(x => x.ServiceContract.FullSearchEndpoint.IsAllowed).Select(EndpointPermissionFactory.CreateSearch);
            var odata = eds.Where(x => x.ServiceContract.OdataEndpoint.IsAllowed).Select(EndpointPermissionFactory.CreateSearch);
            var getOneActions = eds.Where(x => x.ServiceContract.EntityResourceEndpoint.IsAllowed).Select(EndpointPermissionFactory.CreateSearch);

            m_permissionTree.AddRange(searches);
            m_permissionTree.AddRange(odata);
            m_permissionTree.AddRange(getOneActions);

            var queries = context.LoadFromSources<QueryEndpoint>().ToArray();
            var getActions = queries.Select(EndpointPermissionFactory.CreateGetAction).ToList();
            var getCounts = queries.Select(EndpointPermissionFactory.CreateGetCount).ToList();
            m_permissionTree.AddRange(getCounts);
            m_permissionTree.AddRange(getActions);

            var operations = context.LoadFromSources<OperationEndpoint>().ToArray();
            var put = operations.Where(x => x.IsHttpPut).Select(EndpointPermissionFactory.CreatePut);
            var delete = operations.Where(x => x.IsHttpDelete).Select(EndpointPermissionFactory.CreateDelete);
            var post = operations.Where(x => x.IsHttpPost).Select(EndpointPermissionFactory.CreatePost);
            var patch = operations.Where(x => x.IsHttpPatch).Select(EndpointPermissionFactory.CreatePatch);

            m_permissionTree.AddRange(put);
            m_permissionTree.AddRange(delete);
            m_permissionTree.AddRange(post);
            m_permissionTree.AddRange(patch);


        }
    }
}