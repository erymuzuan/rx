using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IDirectoryService
    {
        string CurrentUserName { get; }
        Task<string[]> GetUserInRolesAsync(string role);
        Task<string[]> GetUserRolesAsync(string userName);
        Task<bool> AuthenticateAsync(string userName, string password);
    }
}