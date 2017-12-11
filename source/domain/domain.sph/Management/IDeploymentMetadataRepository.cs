using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Management
{
    public interface IDeploymentMetadataRepository
    {
        Task InitializeAsync();
    }
}