using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

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
            var inheritedClaims = this.Claims.Select(x => x.Clone(true));
            this.Claims = inheritedClaims.ToArray();
        }

        public Task<bool> CheckAccessAsync(ClaimsPrincipal subject)
        {
            if (this.Claims == null) return Task.FromResult(true);
            if (this.Claims.Length == 0) return Task.FromResult(true);

            var deniedClaims = this.Claims.Where(x => x?.Permission?.EndsWith("d") ?? false).Select(x => x.ToClaim()).ToArray();
            if (deniedClaims.Any(clm => subject.HasClaims2(x => x.Type == clm.Type && x.Value == clm.Value)))
            {
                return Task.FromResult(false);
            }
            var allowedClaims = this.Claims.Where(x => x?.Permission?.EndsWith("a") ?? false).Select(x => x.ToClaim()).ToArray();
            if (allowedClaims.Any(clm => subject.HasClaims2(x => x.Type == clm.Type && x.Value == clm.Value)))
            {
                return Task.FromResult(true);
            }


            return Task.FromResult(false);
        }

        public void AddInheritedClaims(params EndpointPermissonSetting[] settings)
        {
            var originalList = new List<ClaimSetting>();
            var defaultClaims = settings.Single(x => string.IsNullOrWhiteSpace(x.Parent)
                                                     && string.IsNullOrWhiteSpace(x.Controller)
                                                     && string.IsNullOrWhiteSpace(x.Action));
            originalList.AddRange(defaultClaims.Claims ?? Array.Empty<ClaimSetting>());

            if (this.HasAction && this.HasController && this.HasParent)
            {
                var controller = settings.SingleOrDefault(
                        x => !x.HasAction && x.Controller == this.Controller && x.Parent == this.Parent);
                var claims = controller?.Claims ?? Array.Empty<ClaimSetting>();
                var diffs = originalList.Except(claims).ToArray();
                originalList.AddRange(diffs);

            }
            if (this.HasController && this.HasParent)
            {
                var parent = settings.SingleOrDefault(x => !x.HasAction && !x.HasController && x.Parent == this.Parent);
                var claims = parent?.Claims ?? Array.Empty<ClaimSetting>();
                var diffs = originalList.Except(claims).ToArray();
                originalList.AddRange(diffs);
            }

            var list = originalList.Select(x => x.Clone(true)).ToList();
            var temps = (this.Claims ?? Array.Empty<ClaimSetting>()).Where(x => !x.IsInherited).Except(list).ToArray();
            list.AddRange(temps);
            this.Claims = list.Distinct().ToArray();
        }

        public bool HasParent => !string.IsNullOrWhiteSpace(this.Parent);
        public bool HasAction => !string.IsNullOrWhiteSpace(this.Action);
        public bool HasController => !string.IsNullOrWhiteSpace(this.Controller);

        /// <summary>
        /// Add parent claims to a specified resource
        /// </summary>
        /// <param name="claims"></param>
        public void AddParentClaims(params ClaimSetting[] claims)
        {
            var list = new List<ClaimSetting>();
            foreach (var c in claims)
            {
                var cs = c;
                var item = list.SingleOrDefault(x => x.Match(cs));
                if (null == item)
                {
                    list.Add(cs);
                }
            }
            this.Claims = list.ToArray();

        }

        public static EndpointPermissonSetting Parse(string json)
        {
            var jst = new JsonSerializerSettings { ContractResolver = new ClaimSettingResolver() };
            return JsonConvert.DeserializeObject<EndpointPermissonSetting>(json, jst);
        }
    }
}