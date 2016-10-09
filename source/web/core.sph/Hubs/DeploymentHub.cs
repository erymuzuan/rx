using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bespoke.Sph.Domain;
using Microsoft.AspNet.SignalR;

namespace Bespoke.Sph.Web.Hubs
{
    [Authorize(Roles = "developers,administrators")]
    public class DeploymentHub : Hub
    {

        public IEnumerable<string> GetDeploymentEnvironments()
        {
            var configs = Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}", "*.json")
                    .Select(f => f.DeserializeFromJsonFile<WorkersConfig>());

            return configs.Select(x => x.Environment).Distinct();
        }
        public IEnumerable<WorkersConfig> GetConfigs(string env)
        {
            var configs = Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}", "*.json")
                    .Select(f => f.DeserializeFromJsonFile<WorkersConfig>());

            return configs.Where(x => x.Environment == env);
        }
        public void AddWorkersConfig(WorkersConfig config)
        {
            config.Id = $"{config.Environment}.{config.Name.ToIdFormat()}";
            config.CreatedBy = base.Context.User.Identity.Name;
            config.ChangedBy = base.Context.User.Identity.Name;
            config.CreatedDate = DateTime.Now;
            config.ChangedDate = DateTime.Now;
            config.WebId = config.Id;

            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}\\{config.Id}.json", config.ToJson());
        }
        public void SaveWorkersConfig(WorkersConfig config)
        {
            config.Id = $"{config.Environment}.{config.Name.ToIdFormat()}";
            config.ChangedBy = base.Context.User.Identity.Name;
            config.ChangedDate = DateTime.Now;

            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}\\{config.Id}.json", config.ToJson());
        }

        public void DeleteWorkersConfig(WorkersConfig config)
        {
            File.Delete($"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}\\{config.Id}.json");
        }
    }
}