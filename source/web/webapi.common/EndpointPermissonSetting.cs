using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebApi
{
    [DebuggerDisplay("{Parent}.{Controller}.{Action}")]
    public class EndpointPermissonSetting
    {
        public EndpointPermissonSetting()
        {
            
        }
        
        public EndpointPermissonSetting(OperationEndpoint operation, string method)
        {
            Parent = operation.Entity;
            Controller = operation.TypeName.Replace("Controller", "");
            Action = $"{method}{operation.Name}";
            Claims = Array.Empty<ClaimSetting>();
        }

        public string Parent { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Id { get; set; }
        public ClaimSetting[] Claims { get; set; }

        public Task<bool> CheckAccessAsync(ClaimsPrincipal subject)
        {
            if (this.Claims == null) return Task.FromResult(true);
            if (this.Claims.Length == 0) return Task.FromResult(true);

            var claims = this.Claims.Select(x => x.ToClaim()).ToArray();
            if (claims.Any(clm => subject.HasClaim(x => x.Type == clm.Type && x.Value == clm.Value)))
            {
                return Task.FromResult(true);
            }


            return Task.FromResult(false);
        }
    }

    public class ClaimSetting
    {
        public ClaimSetting()
        {
            
        }

        public ClaimSetting(Claim claim)
        {
            this.Type = claim.Type;
            this.ValueType = claim.ValueType;
            this.Value = claim.Value;
        }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Permission { get; set; }

        public Claim ToClaim()
        {
            return new Claim(this.Type,this.Value, this.ValueType);
        }

        public string ValueType { get; set; }
    }
}