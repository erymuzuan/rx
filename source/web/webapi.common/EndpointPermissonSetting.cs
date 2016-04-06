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

            var deniedClaims = this.Claims.Where(x => x.Permission == "d" || x.Permission == "id").Select(x => x.ToClaim()).ToArray();
            if (deniedClaims.Any(clm => subject.HasClaims2(x => x.Type == clm.Type && x.Value == clm.Value)))
            {
                return Task.FromResult(false);
            }
            var allowedClaims = this.Claims.Where(x => x.Permission == "a" || x.Permission == "ia").Select(x => x.ToClaim()).ToArray();
            if (allowedClaims.Any(clm => subject.HasClaims2(x => x.Type == clm.Type && x.Value == clm.Value)))
            {
                return Task.FromResult(true);
            }


            return Task.FromResult(false);
        }

        public void AddInheritedClaims(params EndpointPermissonSetting[] settings)
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

        public void OverrideClaims(params ClaimSetting[] claims)
        {
            var list = new List<ClaimSetting>();
            foreach (var c in claims)
            {
                var cs = c;
                var item = list.SingleOrDefault(x => x.Value == cs.Value && x.Type == cs.Type);
                if (null != item)
                {
                    item.Permission = cs.Permission;
                }
                else
                {
                    list.Add(cs);
                }
            }
            this.Claims = list.ToArray();

        }
    }
}