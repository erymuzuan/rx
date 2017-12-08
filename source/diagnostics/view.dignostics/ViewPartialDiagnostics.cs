using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.diagnostics;

namespace Bespoke.Sph.Diagnostics
{
    /*[Export(typeof(IBuildDiagnostics))]*/
    public sealed class ViewPartialDiagnostics : BuilDiagnostic
    {

        public override Task<BuildDiagnostic[]> ValidateWarningsAsync(EntityView view, EntityDefinition ed)
        {
            var warnings = new List<BuildDiagnostic>();
            if (string.IsNullOrWhiteSpace(view.Partial)) return Task.FromResult(warnings.ToArray());


            var partial = $"{ConfigurationManager.WebPath}\\SphApp\\partial\\{view.Route}.js";
            if (File.Exists(partial))
            {
                var js = File.ReadAllText(partial);
                if (!js.Contains("define("))
                {
                    warnings.Add(new BuildDiagnostic(view.WebId, $"Your view {view.Name} defined a partial but the content might not be valid "));
                }
            }
            else
            {
                warnings.Add(new BuildDiagnostic(view.WebId, $"Your view {view.Name} defined a partial but the partial file cannot be found at {partial} "));
            }

            return Task.FromResult(warnings.ToArray());
        }
    }
}