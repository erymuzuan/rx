using System;
using Bespoke.Sph.Domain;
using JWT;
using Newtonsoft.Json;

namespace Bespoke.Sph.WebApi
{
    public class AccessToken : DomainObject
    {
        public AccessToken()
        {
            this.WebId = Guid.NewGuid().ToString();
        }

        public AccessToken(UserProfile user, string[] roles, TimeSpan expiry)
        {
            this.Email = user.Email;
            this.Username = user.UserName;
            this.Subject = user.Id;
            this.Roles = roles;


            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.Expiry = Math.Round((DateTime.UtcNow.Add(expiry) - unixEpoch).TotalSeconds);
            this.IssueAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            this.NotBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


            this.WebId = Guid.NewGuid().ToString();
        }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }


        [JsonProperty(PropertyName = "user")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "roles")]
        public string[] Roles { get; set; }

        [JsonProperty(PropertyName = "sub")]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "nbf")]
        public double NotBefore { get; set; }

        [JsonProperty(PropertyName = "iat")]
        public double IssueAt { get; set; }

        [JsonProperty(PropertyName = "exp")]
        public double Expiry { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GenerateToken()
        {
            return JsonWebToken.Encode(this, ConfigurationManager.TokenSecret, JwtHashAlgorithm.HS256);
        }
    }
}