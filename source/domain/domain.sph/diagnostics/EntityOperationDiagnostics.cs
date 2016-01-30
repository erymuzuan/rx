using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    sealed class EntityOperationDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(OperationEndpoint endpoint, EntityDefinition entity)
        {
            var errors = new List<BuildError>();
            return Task.FromResult((errors.ToArray()));
        }
    }
}