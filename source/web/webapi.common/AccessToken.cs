using System;
using System.Collections.Generic;
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

        public AccessToken(UserProfile user, string[] roles, DateTime expiry)
        {
            this.Email = user.Email;
            this.Username = user.UserName;
            this.Roles = roles;

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.Expiry = Convert.ToInt32((expiry - unixEpoch).TotalSeconds);
            this.IssueAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            this.NotBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);

            this.WebId = DateTime.Now.Ticks + Guid.NewGuid().ToString().Substring(0, 8);
            this.Subject = this.WebId;
            this.ExpiryDate = expiry;
        }

        [JsonIgnore]
        public DateTime ExpiryDate { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("user")]
        public string Username { get; set; }

        [JsonProperty("roles")]
        public string[] Roles { get; set; }

        [JsonProperty("sub")]
        public string Subject { get; set; }

        [JsonProperty("nbf")]
        public double NotBefore { get; set; }

        [JsonProperty("iat")]
        public double IssueAt { get; set; }

        [JsonProperty("exp")]
        public int Expiry { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GenerateToken()
        {
            var payLoad = new Dictionary<string, object>
            {
                {"user", this.Username},
                {"roles", this.Roles},
                {"email", this.Email},
                {"sub", this.Subject},
                {"nbf", this.NotBefore},
                {"iat", this.IssueAt},
                {"exp", this.Expiry},
                {"aud", ConfigurationManager.ApplicationName},
            };
            return JsonWebToken.Encode(payLoad, ConfigurationManager.TokenSecret, JwtHashAlgorithm.HS256);
        }
    }
}