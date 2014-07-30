using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class StringConcateFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            var codes = this.ArgumentCollection.Select(a => a.GenerateCode());
            return string.Join(" + ", codes);
        }
    }
}