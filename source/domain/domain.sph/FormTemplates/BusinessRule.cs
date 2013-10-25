using System.Linq;

namespace Bespoke.Sph.Domain
{
    partial class BusinessRule : DomainObject
    {
        public ValidationResult Evaluate(Entity item)
        {
            var result = new ValidationResult { Success = true };
            var context = new RuleContext(item);

            var valid = this.RuleCollection.All(r => r.Execute(context));
            if (!valid)
            {
                result.Success = false;
                result.ValidationErrors.Add(new ValidationError
                {
                    Message = this.ErrorMessage,
                    ErrorLocation = this.ErrorLocation,

                });
            }
            return result;
        }
    }
}
