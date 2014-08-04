using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class FunctoidMap : Map
    {

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(this.Destination))
                errors.Add("Destination", "Destination is null or empty for " + this.GetType().Name);
            var f = await this.Functoid.ValidateAsync();
            errors.AddRange(f);

            return errors;
        }


        public override string GenerateCode()
        {
            return this.Functoid.GeneratePreCode(this)
                + "\r\n" +
                string.Format("               dest.{1} = {0};", this.Functoid.GenerateCode(), this.Destination);

        }
    }
}