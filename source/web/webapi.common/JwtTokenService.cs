using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using JWT;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;

namespace Bespoke.Sph.WebApi
{
    public class JwtTokenService : ITokenService
    {
        public ITokenRepository Repository { get; }
        public JwtTokenService(ITokenRepository repository)
        {
            this.Repository = repository;
        }

        public async Task<ClaimsPrincipal> ValidateAsync(string token, bool checkExpiration)
        {
            var secret = ConfigurationManager.TokenSecret;
            var result = Policy.Handle<InvalidOperationException>()
                .Or<SignatureVerificationException>()
                .Retry()
                .ExecuteAndCapture(() => JsonWebToken.Decode(token, secret));
            if (result.ExceptionType != null) return null;
            var json = result.Result;
            

            var settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var jo = JObject.Parse(json);
            var accessToken = JsonConvert.DeserializeObject<AccessToken>(json, settings);
            accessToken.WebId = jo.SelectToken("$.sub").Value<string>();

            if (null != this.Repository)
            {
                var ok = await this.Repository.LoadOneAsync(accessToken.WebId);
                if (null == ok)
                    return default(ClaimsPrincipal);
            }

            var validTo = FromUnixTime(accessToken.Expiry);
            if (DateTime.Compare(validTo, DateTime.UtcNow) <= 0)
            {
                return null;
            }


            var subject = new ClaimsIdentity("Federation", ClaimTypes.Name, ClaimTypes.Role);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, accessToken.Username, ClaimValueTypes.String),
                new Claim(ClaimTypes.Email, accessToken.Email, ClaimValueTypes.Email)
            };
            var roles = from r in accessToken.Roles
                        select new Claim(ClaimTypes.Role, r, ClaimValueTypes.String);
            claims.AddRange(roles);

            subject.AddClaims(claims);
            var principal = new ClaimsPrincipal(subject);
            return principal;
        }

        private static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
        /// <summary>
        /// Create a Jwt with user information
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles">The users roles</param>
        /// <param name="expiry">The timespan for the token to live</param>
        /// <returns></returns>
        public async Task<AccessToken> CreateTokenAsync(UserProfile user, string[] roles, DateTime expiry)
        {
            var payload = new AccessToken(user, roles, expiry);

            if (null != Repository)
                await Repository.SaveAsync(payload);
            return payload;
        }
    }
}
