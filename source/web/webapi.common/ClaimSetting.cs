using System.Diagnostics;
using System.Security.Claims;

namespace Bespoke.Sph.WebApi
{
    [DebuggerDisplay("{Type} = {Value}")]
    public class ClaimSetting
    {
        public ClaimSetting()
        {

        }
        public ClaimSetting(string type, string value, string permission)
        {
            this.Type = type;
            this.Value = value;
            this.Permission = permission;
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