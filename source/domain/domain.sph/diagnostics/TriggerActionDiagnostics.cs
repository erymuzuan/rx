using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class TriggerActionDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(Trigger trigger)
        {
            var errors = new List<BuildError>();
            if(trigger.ActionCollection.Count == 0)
                errors.Add(new BuildError(trigger.WebId, "This trigger does not contains any action"));
            return Task.FromResult(errors.ToArray());
        }
    }
}