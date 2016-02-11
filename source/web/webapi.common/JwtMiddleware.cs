using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using JWT;
using Microsoft.Owin.Security.Jwt;
using Owin;

namespace Bespoke.Sph.WebApi
{
    public class JwtMiddleware : ITokenService
    {
        private readonly ITokenRepository m_repository;

        public void Configure(IAppBuilder app)
        {
            var options = new JwtBearerAuthenticationOptions { AuthenticationType = ConfigurationManager.ApplicationName };
            app.UseJwtBearerAuthentication(options);
        }

        public JwtMiddleware(ITokenRepository repository)
        {
            m_repository = repository;
        }

        public Task<ClaimsPrincipal> ValidateAsync(string token, bool checkExpiration)
        {
            var secret = ConfigurationManager.TokenSecret;
            var payloadJson = JsonWebToken.Decode(token, secret);
            var payloadData = payloadJson.DeserializeFromJson<Dictionary<string, object>>();


            object exp;
            if (payloadData != null && (checkExpiration && payloadData.TryGetValue("exp", out exp)))
            {
                var validTo = FromUnixTime(long.Parse(exp.ToString()));
                if (DateTime.Compare(validTo, DateTime.UtcNow) <= 0)
                {
                    throw new Exception(
                        $"Token is expired. Expiration: '{validTo}'. Current: '{DateTime.UtcNow}'");
                }
            }

            var subject = new ClaimsIdentity("Federation", ClaimTypes.Name, ClaimTypes.Role);

            var claims = new List<Claim>();

            if (payloadData != null)
                foreach (var pair in payloadData)
                {
                    var claimType = pair.Key;

                    var source = pair.Value as ArrayList;

                    if (source != null)
                    {
                        claims.AddRange(from object item in source
                                        select new Claim(claimType, item.ToString(), ClaimValueTypes.String));

                        continue;
                    }

                    switch (pair.Key)
                    {
                        case "name":
                            claims.Add(new Claim(ClaimTypes.Name, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        case "surname":
                            claims.Add(new Claim(ClaimTypes.Surname, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        case "email":
                            claims.Add(new Claim(ClaimTypes.Email, pair.Value.ToString(), ClaimValueTypes.Email));
                            break;
                        case "roles":
                            claims.Add(new Claim(ClaimTypes.Role, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        case "userId":
                            claims.Add(new Claim(ClaimTypes.UserData, pair.Value.ToString(), ClaimValueTypes.Integer));
                            break;
                        default:
                            claims.Add(new Claim(claimType, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                    }
                }

            subject.AddClaims(claims);
            var principal = new ClaimsPrincipal(subject);
            return Task.FromResult(principal);
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
        public async Task<string> CreateTokenAsync(UserProfile user, string[] roles, DateTime expiry)
        {
            var payload = new AccessToken(user, roles, expiry);
            var token = payload.GenerateToken();

            if (null != m_repository)
                await m_repository.SaveAsync(payload);
            return token;
        }
    }
}
