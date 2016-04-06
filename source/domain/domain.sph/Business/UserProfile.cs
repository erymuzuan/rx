using System.Collections.Generic;
using System.Security.Claims;

namespace Bespoke.Sph.Domain
{
    public partial class UserProfile : Entity
    {
        public bool IsLockedOut { get; set; }

        public IEnumerable<Claim> GetClaims()
        {
            var list = new List<Claim>();
            if (!string.IsNullOrWhiteSpace(this.Language))
                list.Add(new Claim("language", this.Language, ClaimValueTypes.String));

            if (!string.IsNullOrWhiteSpace(this.Department))
                list.Add(new Claim("department", this.Department, ClaimValueTypes.String));

            if (!string.IsNullOrWhiteSpace(this.Designation))
                list.Add(new Claim("designation", this.Designation, ClaimValueTypes.String));

            if (!string.IsNullOrWhiteSpace(this.Email))
                list.Add(new Claim(ClaimTypes.Email, this.Email));

            if (!string.IsNullOrWhiteSpace(this.FullName))
                list.Add(new Claim(ClaimTypes.Name, this.FullName));

            return list;
        }
    }
}
