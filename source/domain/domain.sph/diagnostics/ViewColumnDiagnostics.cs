using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class ViewColumnDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity)
        {
            var errors = from f in view.ViewColumnCollection
                where string.IsNullOrWhiteSpace(f.Path)
                select new BuildError
                    (
                    view.WebId,
                    string.Format("[Column] : {1}({0}) does not have path", f.Path, f.Header)
                    );
            return Task.FromResult(errors.ToArray());
        }
        public override Task<BuildError[]> ValidateWarningsAsync(EntityView view, EntityDefinition entity)
        {
            var paths = entity.GetMembersPath();
            var warnings = from f in view.ViewColumnCollection
                where !paths.Contains(f.Path)
                select new BuildError(f.WebId, $"[{f.Header}] : Specified path \"{f.Path}\" may not be valid, ignore this warning if this is intentional");
            return Task.FromResult(warnings.ToArray());
        }
    }
}