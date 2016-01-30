using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class QueryEndpointSortDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(QueryEndpoint endpoint, EntityDefinition entity)
        {
            var errors = from f in endpoint.SortCollection
                where string.IsNullOrWhiteSpace(f.Path)
                select new BuildError
                    (
                    endpoint.WebId,
                    $"[Sort] : {f.Path} does not have path"
                    );
            return Task.FromResult(errors.ToArray());
        }
    }
}