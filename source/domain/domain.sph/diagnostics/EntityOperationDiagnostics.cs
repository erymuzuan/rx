using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    sealed class EntityOperationDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(OperationEndpoint endpoint, EntityDefinition entity)
        {
            return Task.FromResult(Array.Empty<BuildError>());
        }
    }
}