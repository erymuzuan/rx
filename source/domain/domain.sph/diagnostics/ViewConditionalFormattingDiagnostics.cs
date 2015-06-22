using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class ViewConditionalFormattingDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity)
        {
            var errors = from f in view.ConditionalFormattingCollection
                where string.IsNullOrWhiteSpace(f.Condition) || f.Condition.Contains("\"")
                select new BuildError
                    (
                    view.WebId,
                    "[ConditionalFormatting] : Condition cannot contains \" or empty"
                    );
            return Task.FromResult(errors.ToArray());
        }
    }
}