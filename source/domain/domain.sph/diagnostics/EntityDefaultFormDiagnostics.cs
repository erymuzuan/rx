using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    sealed class EntityDefaultFormDiagnostics : BuilDiagnostic
    {
        public override async Task<BuildError[]> ValidateErrorsAsync(EntityDefinition entity)
        {
            var context = new SphDataContext();
            var errors = new List<BuildError>();

            // ReSharper disable RedundantBoolCompare
            var defaultForm = await context.LoadOneAsync<EntityForm>(f => f.IsDefault == true && f.EntityDefinitionId == entity.Id);
            // ReSharper restore RedundantBoolCompare
            if (null == defaultForm)
                errors.Add(new BuildError(entity.WebId, "Please set a default form"));

            return (errors.ToArray());
        }
    }
}