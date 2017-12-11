using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Web.Security;
using Humanizer;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class ViewPerformerDiagnostics : BuilDiagnostic
    {
        public override async Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity)
        {
            var performer = view.Performer;
            return await ValidateAsync(view, performer);
        }
     
        private static async Task<BuildDiagnostic[]> ValidateAsync(Entity item, Performer performer)
        {
            var context = new SphDataContext();
            if (!performer.Validate())
                return new[] { new BuildDiagnostic(item.WebId, "You have not set the permission correctly") };

            if (performer.IsPublic)
                return new BuildDiagnostic[] { };

            var userProperty = performer.UserProperty;
            var value = performer.Value;
            if (userProperty == "Roles" && !Roles.RoleExists(value))
            {
                return (new[] { new BuildDiagnostic(item.WebId, $"Role '{value}' does not exists") });
            }
            if (userProperty == "Designation")
            {
                var designation = await context.GetCountAsync<Designation>(d => d.Name == value);
                if (designation != 1)
                    return
                        (new[]
                        {
                            new BuildDiagnostic(item.WebId,
                                $"There are {"desgination".ToQuantity(designation)} found with the name '{value}'")
                        });
            }
            //TODO : department

            return new BuildDiagnostic[] { };
        }

    }
}