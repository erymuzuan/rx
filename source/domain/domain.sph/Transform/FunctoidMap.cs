using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class FunctoidMap : Map
    {
        public Functoid GetFunctoid(TransformDefinition map)
        {
            if (string.IsNullOrWhiteSpace(this.Functoid))
                return null;
            return map.FunctoidCollection.Single(x => x.WebId == this.Functoid);
        }

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(this.Destination))
                errors.Add("Destination", "Destination is null or empty for " + this.GetType().Name);
            var f = await this.GetFunctoid(this.TransformDefinition).ValidateAsync();
            errors.AddRange(f);

            return errors;
        }


        public override string GenerateCode()
        {
            var fnt = this.GetFunctoid(this.TransformDefinition);
            return fnt.GeneratePreCode()
                + "\r\n" +
                string.Format("               dest.{1} = {0};", fnt.GenerateCode(), this.Destination);

        }
    }
}