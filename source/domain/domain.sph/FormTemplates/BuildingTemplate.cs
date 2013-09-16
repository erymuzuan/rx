using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class BuildingTemplate : Entity
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = new List<ValidationError>();

            if(string.IsNullOrWhiteSpace(this.Name))
                errors.Add(new ValidationError{PropertyName = "Name", Message = "Missing template name"});

            return Task.FromResult(errors.AsEnumerable());
        }
    }
}
