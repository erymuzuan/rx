using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Polly;

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

            var parentName = FindParentAsync(controller);
            if (savedSettings.Any(x => x.Controller == controller && !x.HasAction))
                parentName = savedSettings.Last(x => x.Controller == controller).Parent;

            if (savedSettings.Any(x => x.Action == action && x.Controller == controller))
                parentName = savedSettings.Last(x => x.Action == action && x.Controller == controller).Parent;

            var permission = new EndpointPermissonSetting { Controller = controller, Action = action, Claims = Array.Empty<ClaimSetting>() };

            var actionPermission = savedSettings.LastOrDefault(x => x.Parent == parentName && x.Controller == controller && x.Action == action);
            if (null != actionPermission?.Claims)
                permission.AddParentClaims(actionPermission.Claims);

            var controllerPermission = savedSettings.LastOrDefault(x => x.Parent == parentName && x.Controller == controller && !x.HasAction);
            if (null != controllerPermission?.Claims)
                permission.AddParentClaims(controllerPermission.Claims);

            var parentPermission = savedSettings.LastOrDefault(x => x.Parent == parentName && !x.HasController && !x.HasAction);
            if (null != parentPermission?.Claims)
                permission.AddParentClaims(parentPermission.Claims);

            var rootPermission = savedSettings.LastOrDefault(x => !x.HasAction && !x.HasController && !x.HasParent);
            if (null != rootPermission?.Claims)
                permission.AddParentClaims(rootPermission.Claims);

            return Task.FromResult(permission);
        }

        private static readonly List<EndpointPermissonSetting> m_controllerParentList = new List<EndpointPermissonSetting>();
        private static string FindParentAsync(string controller)
        {
            if (m_controllerParentList.Count == 0)
            {
                var context = new SphDataContext();
                #region "fill in parents"
                var eds = context.LoadFromSources<EntityDefinition>().ToArray();
                var searches = eds.Where(x => x.ServiceContract.FullSearchEndpoint.IsAllowed).Select(EndpointPermissionFactory.CreateSearch);
                var odata = eds.Where(x => x.ServiceContract.OdataEndpoint.IsAllowed).Select(EndpointPermissionFactory.CreateOdata);
                var getOneActions = eds.Where(x => x.ServiceContract.EntityResourceEndpoint.IsAllowed).Select(EndpointPermissionFactory.CreateGetOneById);

                m_controllerParentList.AddRange(searches);
                m_controllerParentList.AddRange(odata);
                m_controllerParentList.AddRange(getOneActions);

                var queries = context.LoadFromSources<QueryEndpoint>().ToArray();
                var getActions = queries.Select(EndpointPermissionFactory.CreateGetAction).ToList();
                var getCounts = queries.Select(EndpointPermissionFactory.CreateGetCount).ToList();
                m_controllerParentList.AddRange(getCounts);
                m_controllerParentList.AddRange(getActions);

                var receiveLocations = context.LoadFromSources<ReceiveLocation>().Select(EndpointPermissionFactory.CreateReceiveLocation)
                    .Where(x => null != x);
                m_controllerParentList.AddRange(receiveLocations);

                var operations = context.LoadFromSources<OperationEndpoint>().ToArray();
                var put = operations.Where(x => x.IsHttpPut).Select(EndpointPermissionFactory.CreatePut);
                var delete = operations.Where(x => x.IsHttpDelete).Select(EndpointPermissionFactory.CreateDelete);
                var post = operations.Where(x => x.IsHttpPost).Select(EndpointPermissionFactory.CreatePost);
                var patch = operations.Where(x => x.IsHttpPatch).Select(EndpointPermissionFactory.CreatePatch);

                m_controllerParentList.AddRange(put);
                m_controllerParentList.AddRange(delete);
                m_controllerParentList.AddRange(post);
                m_controllerParentList.AddRange(patch);

                m_controllerParentList.Add(new EndpointPermissonSetting { Parent = "System", Controller = "RxSystemApi" });
                m_controllerParentList.Add(new EndpointPermissonSetting { Parent = "System", Controller = "List" });
                m_controllerParentList.Add(new EndpointPermissonSetting { Parent = "System", Controller = "Aggregate" });
                m_controllerParentList.Add(new EndpointPermissonSetting { Parent = "System", Controller = "DeveloperService" });

                #endregion
            }

            var pr = Policy.Handle<IndexOutOfRangeException>()
                .WaitAndRetry(5, c => TimeSpan.FromMilliseconds(500))
                .ExecuteAndCapture(() => m_controllerParentList
                    .Where(x => null != x && x.Controller == controller)
                    .ToList()
                    .LastOrDefault());
            if (null != pr.FinalException)
                throw pr.FinalException;

            var me = pr.Result;
            return null == me ? "Custom" : me.Parent;
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