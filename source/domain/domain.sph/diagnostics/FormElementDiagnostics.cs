using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public class FormElementDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity)
        {

            var elements = from f in form.FormDesign.FormElementCollection
                let err = f.ValidateBuild(entity)
                where null != err
                select err;
            var errors = elements.SelectMany(v => v).ToArray();
            return Task.FromResult(errors);
        }
    }
}