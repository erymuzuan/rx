using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class Field : DomainObject
    {
        public virtual object GetValue(RuleContext context)
        {
            throw new NotImplementedException("whoaaa");
        }
        public virtual Task<object> GetValueAsync(RuleContext context)
        {
            throw new NotImplementedException("whoaaa");
        }

        public virtual  string GenerateCode()
        {
            return $"// NOT IMPLEMENTED => {this.GetType().Name}";
        }
        public virtual  string GenerateAsyncCode()
        {
            return $"// NOT IMPLEMENTED => {this.GetType().Name}";
        }
    }
}