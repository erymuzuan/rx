using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public class FormDeleteOperationDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity)
        {
            if (!form.IsRemoveAvailable) return Task.FromResult(Array.Empty<BuildError>());
            var errors = new List<BuildError>();
            if (string.IsNullOrWhiteSpace(form.DeleteOperation))
                errors.Add(new BuildError(form.Id, "Removed is available but no DELETE operation is selected"));
            if (string.IsNullOrWhiteSpace(form.DeleteOperationSuccessMesage) && string.IsNullOrWhiteSpace(form.DeleteOperationSuccessNavigateUrl))
                errors.Add(new BuildError(form.Id,"Removed is available but DELETE operation do not specify success message or navigate Uri"));
            return Task.FromResult(errors.ToArray());
        }
    }
}