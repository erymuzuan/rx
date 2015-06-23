using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    sealed class EntityDuplicateFieldDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(EntityDefinition entity)
        {
            var errors = new List<BuildError>();

            var names = entity.MemberCollection.Select(a => a.Name);
            var duplicates = names.GroupBy(a => a).Any(a => a.Count() > 1);
            if (duplicates)
                errors.Add(new BuildError(entity.WebId, "There are duplicates field names"));

            return Task.FromResult(errors.ToArray());
        }
    }
}