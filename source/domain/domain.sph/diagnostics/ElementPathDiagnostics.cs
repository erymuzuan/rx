using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public class ElementPathDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity)
        {
            var errors = from f in form.FormDesign.FormElementCollection
                         where f.IsPathIsRequired
                             && string.IsNullOrWhiteSpace(f.Path) && (f.Name != "HTML Section")
                         select new BuildError
                         (
                             form.WebId,
                             $"[Input] : {form.Name} => '{f.Label}' does not have path"
                             );
            return Task.FromResult(errors.ToArray());

        }

        public override Task<BuildError[]> ValidateWarningsAsync(EntityForm form, EntityDefinition ed)
        {

            var warnings = new List<BuildError>();

            var paths = ed.GetMembersPath();
            var invalidPathWarnings = from f in form.FormDesign.FormElementCollection
                                      where f.IsPathIsRequired
                                            && !paths.Contains(f.Path)
                                      select new BuildError(f.WebId, $"[{f.Label}] : Specified path \"{f.Path}\" may not be valid, ignore this warning if this is intentional");
            warnings.AddRange(invalidPathWarnings);


            return Task.FromResult(warnings.ToArray());
        }

    }
}