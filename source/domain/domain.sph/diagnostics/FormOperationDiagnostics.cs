using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public class FormOperationDiagnostics : BuilDiagnostic
    {
        public override Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity)
        {
            var errors = new List<BuildDiagnostic>();

            if (!string.IsNullOrWhiteSpace(form.DeleteOperation))
                return Task.FromResult(errors.ToArray());

            if (!string.IsNullOrWhiteSpace(form.Operation))
            {
                if (string.IsNullOrWhiteSpace(form.OperationSuccessMesage) && string.IsNullOrWhiteSpace(form.OperationSuccessNavigateUrl))
                    errors.Add(new BuildDiagnostic(form.Id, "API operation do not specify success message or navigate Uri"));
            }
            return Task.FromResult(errors.ToArray());
        }
    }
}