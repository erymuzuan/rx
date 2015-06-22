using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class ViewLinkColumnDiagnostics : BuilDiagnostic
    {
        public override async Task<BuildError[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity)
        {

            var routes = (await JsRoute.GetCustomRoutes()).Select(x => x.Route).ToArray();
            var invalidLinks = from f in view.ViewColumnCollection
                where f.IsLinkColumn
                      && !routes.Contains(f.FormRoute)
                select new BuildError(f.WebId, $"[{f.Header}] : Specified link route in column \"{f.Header}\" point to \"{f.FormRoute}\" is invalid");
            var errors = (invalidLinks).ToList();

            // TODO  validate for route parameters

            var emptyLink = from f in view.ViewColumnCollection
                where string.IsNullOrWhiteSpace(f.FormRoute)
                      && f.IsLinkColumn
                select new BuildError
                    (
                    view.WebId,
                    string.Format("[Column] : {1}({0})  does not have for route", f.Path, f.Header)
                    );
            errors.AddRange(emptyLink);

            return errors.ToArray();
        }
    }
}