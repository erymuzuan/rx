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
            var list = new List<EndpointPermissonSetting> {new EndpointPermissonSetting
            {
                Claims = new[]
                {
                    new ClaimSetting(new Claim(ClaimTypes.Role, "administators")) {Permission = "a"}, 
                    new ClaimSetting(new Claim(ClaimTypes.Role, "developers")) {Permission = "a"}, 
                }
            } };

            if (File.Exists(source))
            {
                var settings = JsonConvert.DeserializeObject<EndpointPermissonSetting[]>(File.ReadAllText(source));
                list.Clear();
                list.AddRange(settings);
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