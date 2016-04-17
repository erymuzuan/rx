using System;
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

        public ClaimSetting(string type, string value, string permission, bool inherited)
        {
            this.Type = type;
            this.Value = value;
            this.Permission = permission;
            this.IsInherited = inherited;
        }

        public ClaimSetting Clone(bool inherit = false)
        {
            var appendInherit = inherit && !this.Permission.StartsWith("i") ? "i" : "";
            return new ClaimSetting(this.Type, this.Value, appendInherit + this.Permission, inherit);
        }

        public ClaimSetting(Claim claim, string permission = "d")
        {
            this.Type = claim.Type;
            this.ValueType = claim.ValueType;
            this.Value = claim.Value;
            this.Permission = permission;
        }
        // private setter used by JSON.Net
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
        public string Type { get; private set; }
        public string Value { get; private set; }
        public string Permission { get; private set; }
        public string ValueType { get; private set; }
        public bool IsInherited { get; private set; }
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Local

        public Claim ToClaim()
        {
            return new Claim(this.Type, this.Value, this.ValueType);
        }


        /// <summary>
        /// match the same claim type and value
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        public bool Match(ClaimSetting cs)
        {
            return this.Type == cs.Type && this.Value == cs.Value;
        }

        public override bool Equals(object obj)
        {
            var cs = obj as ClaimSetting;
            return null != cs && this.Equals(cs);
        }

        protected bool Equals(ClaimSetting other)
        {
            return string.Equals(Type, other.Type, StringComparison.InvariantCultureIgnoreCase) && string.Equals(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Type) : 0)*397) ^ (Value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Value) : 0);
            }
        }
    }
}