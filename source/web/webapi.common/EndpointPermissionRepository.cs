using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.WebApi
{
    public class EndpointPermissionRepository : IEndpointPermissionRepository
    {
        public EndpointPermissionRepository()
        {
            this.Settings = new List<EndpointPermissonSetting>();
        }
        public IEnumerable<EndpointPermissonSetting> Settings { get; }
        public Task<EndpointPermissonSetting> FindSettingsAsync(string controller, string action)
        {
            var setting = this.Settings.SingleOrDefault(x => x.Controller == controller && x.Action == action) ??
                          this.Settings.SingleOrDefault(x => x.Controller == controller);
            if (null == setting)
                setting = new EndpointPermissonSetting { Controller = controller, Action = action, Claims = Array.Empty<ClaimSetting>()};
            return Task.FromResult(setting);
        }

        public Task<IEnumerable<EndpointPermissonSetting>> LoadAsync()
        {
            var source = $"{ConfigurationManager.SphSourceDirectory}\\EndpointPermissionSetting\\default.json";
            var settings = Array.Empty<EndpointPermissonSetting>();
            if (File.Exists(source))
            {
                settings = JsonConvert.DeserializeObject<EndpointPermissonSetting[]>(File.ReadAllText(source));
            }

            var list = new List<EndpointPermissonSetting>();
            var context = new SphDataContext();
            var eds = context.LoadFromSources<EntityDefinition>().ToArray();
            var searches = eds.Where(x => x.ServiceContract.FullSearchEndpoint.IsAllowed).Select(EndpointPermissionFactory.CreateSearch);
            var odata = eds.Where(x => x.ServiceContract.OdataEndpoint.IsAllowed).Select(EndpointPermissionFactory.CreateSearch);
            var getOneActions = eds.Where(x => x.ServiceContract.EntityResourceEndpoint.IsAllowed).Select(EndpointPermissionFactory.CreateSearch);
            
            list.AddRange(searches);
            list.AddRange(odata);
            list.AddRange(getOneActions);

            var queries = context.LoadFromSources<QueryEndpoint>().ToArray();
            var getActions = queries.Select(EndpointPermissionFactory.CreateGetAction).ToList();
            var getCounts = queries.Select(EndpointPermissionFactory.CreateGetCount).ToList();
            list.AddRange(getCounts);
            list.AddRange(getActions);

            var operations = context.LoadFromSources<OperationEndpoint>().ToArray();
            var put = operations.Where(x => x.IsHttpPut).Select(EndpointPermissionFactory.CreatePut);
            var delete = operations.Where(x => x.IsHttpDelete).Select(EndpointPermissionFactory.CreateDelete);
            var post = operations.Where(x => x.IsHttpPost).Select(EndpointPermissionFactory.CreatePost);
            var patch = operations.Where(x => x.IsHttpPatch).Select(EndpointPermissionFactory.CreatePatch);

            list.AddRange(put);
            list.AddRange(delete);
            list.AddRange(post);
            list.AddRange(patch);

            foreach (var st in list)
            {
                var e = settings.FirstOrDefault(x =>
                        x.Parent == st.Parent &&
                        x.Action == st.Action &&
                        x.Controller == st.Controller);
                if (null != e)
                {
                    st.Claims = e.Claims;
                }
            }

            return Task.FromResult(list.AsEnumerable());
        }

        public Task SaveAsync(IEnumerable<EndpointPermissonSetting> settings)
        {
            var source = $"{ConfigurationManager.SphSourceDirectory}\\EndpointPermissionSetting\\default.json";
            var json = settings.ToJson();
            File.WriteAllText(source, json);
            return Task.FromResult(0);
        }
    }
}