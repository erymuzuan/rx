using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class TriggerActionDiagnostics : BuilDiagnostic
    {
        public override Task<BuildDiagnostic[]> ValidateErrorsAsync(Trigger trigger)
        {
            var errors = new List<BuildDiagnostic>();
            if(trigger.ActionCollection.Count == 0)
                errors.Add(new BuildDiagnostic(trigger.WebId, "This trigger does not contains any action"));
            return Task.FromResult(errors.ToArray());
        }
    }
}