using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Management
{
    public interface IReadOnlyRepositoryManagement
    {
        Task<bool> GetAccesibleStatusAsync();
        void RegisterConnectionChanged(Func<bool, int> connectionStateChanged);
    }
    public interface IRepositoryManagement
    {
        Task<bool> GetAccesibleStatusAsync();
        void RegisterConnectionChanged(Func<bool, int> connectionStateChanged);

    }
}
