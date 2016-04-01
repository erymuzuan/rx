using System.Threading.Tasks;

namespace Bespoke.Sph.WebApi
{
    public interface IEndpointPermissionRepository
    {
        Task<EndpointPermissonSetting> FindSettingsAsync(string controller, string action);
    }
}