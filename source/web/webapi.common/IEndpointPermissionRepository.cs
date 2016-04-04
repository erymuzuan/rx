using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.WebApi
{
    public interface IEndpointPermissionRepository
    {
        Task<EndpointPermissonSetting> FindSettingsAsync(string controller, string action);
        Task<IEnumerable<EndpointPermissonSetting>> LoadAsync();
        Task SaveAsync(IEnumerable<EndpointPermissonSetting> settings);
    }
}