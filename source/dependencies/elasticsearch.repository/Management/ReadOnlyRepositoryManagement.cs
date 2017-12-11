using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Management;

namespace Bespoke.Sph.ElasticsearchRepository.Management
{
    public class ReadOnlyRepositoryManagement : IReadOnlyRepositoryManagement
    {
        public async Task<bool> GetAccesibleStatusAsync()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(EsConfigurationManager.Host) })
            {
                try
                {
                    var cat = await client.GetStringAsync("_cat/indices");
                    return cat.Contains(ConfigurationManager.ApplicationName.ToLowerInvariant());
                }
                catch
                {
                    return false;
                }
            }
        }

        public void RegisterConnectionChanged(Func<bool, int> connectionStateChanged)
        {
            throw new NotImplementedException();
        }

        public void OpenManagementConsole()
        {
            throw new NotImplementedException();
        }
    }
}
