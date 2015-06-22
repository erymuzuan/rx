using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class ViewFilterDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity)
        {
            var paths = entity.GetMembersPath();
            var invalidFilters = from f in view.FilterCollection
                                 where !paths.Contains(f.Term)
                                 select new BuildError(f.WebId, $"[{f.Term}] : Specified filter term is \"{f.Term}\" may not be valid");
            var errors = (invalidFilters).ToList();


            var emptyFieldErrors = from f in view.FilterCollection
                                   where string.IsNullOrWhiteSpace(f.Term) || null == f.Field
                                   select new BuildError
                                   (
                                       view.WebId,
                                       $"[Filter] : {f.Term} => '{f.Field}' does not have term or field"
                                       );
            errors.AddRange(emptyFieldErrors);
            return Task.FromResult(errors.ToArray());
        }
    }
}
