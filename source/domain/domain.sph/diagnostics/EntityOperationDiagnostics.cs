using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    sealed class EntityOperationDiagnostics : BuilDiagnostic
    {
        public override async Task<BuildError[]> ValidateErrorsAsync(EntityDefinition entity)
        {
            var errors = new List<BuildError>();
            foreach (var operation in entity.EntityOperationCollection)
            {
                var opErrors = (await operation.ValidateBuildAsync(entity)).ToList();
                errors.AddRange(opErrors);
            }

            return (errors.ToArray());
        }
    }
}