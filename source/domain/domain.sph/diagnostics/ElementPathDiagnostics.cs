using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Extensions;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public class ElementPathDiagnostics : BuilDiagnostic
    {
        public override Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity)
        {
            var errors = from f in form.FormDesign.FormElementCollection
                         where f.IsPathIsRequired
                             && string.IsNullOrWhiteSpace(f.Path) && (f.Name != "HTML Section")
                         select new BuildDiagnostic
                         (
                             form.WebId,
                             $"[Input] : {form.Name} => '{f.Label}' does not have path"
                             );
            return Task.FromResult(errors.ToArray());

        }

        public override Task<BuildDiagnostic[]> ValidateWarningsAsync(EntityForm form, EntityDefinition ed)
        {
            var warnings = new List<BuildDiagnostic>();

            if (null == ed)
                return Task.FromResult(new[] {new BuildDiagnostic(form.Id, $"No EntityDefinition defined for form {form.Id} -> {form.EntityDefinitionId}"), });
            var paths = ed.GetMembersPath();
            var invalidPathWarnings = from f in form.FormDesign.FormElementCollection
                                      where f.IsPathIsRequired
                                            && !paths.Contains(f.Path)
                                      select new BuildDiagnostic(f.WebId, $"[{f.Label}] : Specified path \"{f.Path}\" may not be valid, ignore this warning if this is intentional");
            warnings.AddRange(invalidPathWarnings);


            return Task.FromResult(warnings.ToArray());
        }

    }
}