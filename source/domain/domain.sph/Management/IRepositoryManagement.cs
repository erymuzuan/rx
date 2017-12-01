using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Management
{
    public interface IRepositoryManagement
    {
        Task<bool> GetAccesibleStatusAsync();
        void RegisterConnectionChanged(Func<bool, int> connectionStateChanged);
        void OpenManagementConsole();
        Task TruncateDataAsync(EntityDefinition ed);
    }
}