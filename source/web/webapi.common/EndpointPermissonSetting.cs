using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
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
        public bool IsInherited { get; set; }
        public void MarkInherited()
        {
            this.IsInherited = true;
            if (null == this.Claims)
            {
                this.Claims = Array.Empty<ClaimSetting>();
            }
            foreach (var c in this.Claims)
            {
                c.Permission = "i" + c.Permission;
                c.IsInherited = true;
            }
        }

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

        public void AddInheritedClaims(List<EndpointPermissonSetting> settings)
        {
            var list = new List<ClaimSetting>();
            var defaultClaims = settings.Single(x => string.IsNullOrWhiteSpace(x.Parent)
                                                     && string.IsNullOrWhiteSpace(x.Controller)
                                                     && string.IsNullOrWhiteSpace(x.Action));
            list.AddRange(defaultClaims.Claims ?? Array.Empty<ClaimSetting>());

            if (this.HasAction && this.HasController && this.HasParent)
            {
                var controller = settings.SingleOrDefault(
                        x => !x.HasAction && x.Controller == this.Controller && x.Parent == this.Parent);
                if (null != controller)
                    list.AddRange(controller.Claims ?? Array.Empty<ClaimSetting>());



            }
            if (this.HasController && this.HasParent)
            {
                var parent = settings.SingleOrDefault(x => !x.HasAction && !x.HasController && x.Parent == this.Parent);
                if (null != parent)
                    list.AddRange(parent.Claims ?? Array.Empty<ClaimSetting>());
            }
            list.ForEach(x => x.IsInherited = true);
            list.ForEach(x => x.Permission = "i" + x.Permission);
            list.AddRange(this.Claims ?? Array.Empty<ClaimSetting>());
            this.Claims = list.ToArray();
        }

        public bool HasParent => !string.IsNullOrWhiteSpace(this.Parent);
        public bool HasAction => !string.IsNullOrWhiteSpace(this.Action);
        public bool HasController => !string.IsNullOrWhiteSpace(this.Controller);

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
            return new Claim(this.Type, this.Value, this.ValueType);
        }

        public string ValueType { get; set; }
        public bool IsInherited { get; set; }
    }
}