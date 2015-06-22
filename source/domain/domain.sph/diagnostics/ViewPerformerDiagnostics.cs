using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Web.Security;
using Humanizer;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class ViewPerformerDiagnostics : BuilDiagnostic
    {
        public override async Task<BuildError[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity)
        {
            var context = new SphDataContext();
            if (!view.Performer.Validate())
                return new[] { new BuildError(view.WebId, "You have not set the permission correctly") };

            if (view.Performer.IsPublic)
                return new BuildError[] { };

            var userProperty = view.Performer.UserProperty;
            var value = view.Performer.Value;
            if (userProperty == "Roles" && !Roles.RoleExists(value))
            {
                return (new[] { new BuildError(view.WebId, $"Role '{value}' does not exists") });
            }
            if (userProperty == "Designation")
            {
                var designation = await context.GetCountAsync<Designation>(d => d.Name == value);
                if (designation != 1)
                    return (new[] { new BuildError(view.WebId, $"There are {"desgination".ToQuantity(designation)} found with the name '{value}'") });
            }

            //TODO : department

            return new BuildError[] { };

        }
    }
}