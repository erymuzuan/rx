using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public class FormOperationDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity)
        {
            var errors = new List<BuildError>();
            if (string.IsNullOrWhiteSpace(form.Operation))
                errors.Add(new BuildError(form.Id, "No API operation is selected"));
            if (string.IsNullOrWhiteSpace(form.OperationSuccessMesage) && string.IsNullOrWhiteSpace(form.OperationSuccessNavigateUrl))
                errors.Add(new BuildError(form.Id, "API operation do not specify success message or navigate Uri"));
            return Task.FromResult(errors.ToArray());
        }
    }
}