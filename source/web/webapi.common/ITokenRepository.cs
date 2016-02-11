using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebApi
{
    public interface ITokenRepository
    {
        Task SaveAsync(AccessToken token);
        Task<LoadOperation<AccessToken>> LoadAsync(string user, DateTime expiry, int page = 1, int size = 20);
        Task<LoadOperation<AccessToken>> LoadAsync(DateTime expiry, int page = 1, int size = 20);
        Task<AccessToken> LoadOneAsync(string id);
        Task RemoveAsync(string id);
    }
}