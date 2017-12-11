using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public class FormDeleteOperationDiagnostics : BuilDiagnostic
    {
        public override Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity)
        {
            if (!form.IsRemoveAvailable) return Task.FromResult(Array.Empty<BuildDiagnostic>());
            var errors = new List<BuildDiagnostic>();
            if (string.IsNullOrWhiteSpace(form.DeleteOperation))
                errors.Add(new BuildDiagnostic(form.Id, "Removed is available but no DELETE operation is selected"));
            if (string.IsNullOrWhiteSpace(form.DeleteOperationSuccessMesage) && string.IsNullOrWhiteSpace(form.DeleteOperationSuccessNavigateUrl))
                errors.Add(new BuildDiagnostic(form.Id,"Removed is available but DELETE operation do not specify success message or navigate Uri"));
            return Task.FromResult(errors.ToArray());
        }
    }
}