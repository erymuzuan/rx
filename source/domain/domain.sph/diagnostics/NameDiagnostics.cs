using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public class NameDiagnostics : BuilDiagnostic
    {
        private const string NamePattern = @"^[A-Za-z][A-Za-z0-9 -]*$";
        private const string ErrorMessage = "Name must start with letter.You cannot use symbol or number as first character";

        public override Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity)
        {
            var errors = new List<BuildDiagnostic>();
            var validName = new Regex(NamePattern);
            if (!validName.Match(form.Name).Success)
                errors.Add(new BuildDiagnostic(form.WebId) { Message = ErrorMessage });

            return Task.FromResult(errors.ToArray());

        }
        public override Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity)
        {
            var errors = new List<BuildDiagnostic>();
            var validName = new Regex(NamePattern);
            if (!validName.Match(view.Name).Success)
                errors.Add(new BuildDiagnostic(view.WebId) { Message = ErrorMessage });

            return Task.FromResult(errors.ToArray());

        }

    }
}