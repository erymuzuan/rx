using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebApi
{
    public interface ITokenService
    {
        Task<ClaimsPrincipal> ValidateAsync(string token, bool checkExpiration);

        /// <summary>
        /// Create a Jwt with user information
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles">The users roles</param>
        /// <param name="expiry">The timespan for the token to live</param>
        /// <returns></returns>
        Task<string> CreateTokenAsync(UserProfile user, string[] roles, TimeSpan expiry);
    }
}