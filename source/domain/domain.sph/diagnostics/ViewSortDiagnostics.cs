using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class ViewSortDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(EntityQuery view, EntityDefinition entity)
        {
            var errors = from f in view.SortCollection
                where string.IsNullOrWhiteSpace(f.Path)
                select new BuildError
                    (
                    view.WebId,
                    $"[Sort] : {f.Path} does not have path"
                    );
            return Task.FromResult(errors.ToArray());
        }
    }
}