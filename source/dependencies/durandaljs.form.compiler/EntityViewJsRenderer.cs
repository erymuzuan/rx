using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export]
    public class EntityViewJsRenderer
    {
        public async Task<string> GenerateCodeAsync(EntityView view, IProjectProvider project)
        {
            var vm = new EntityViewHtmlViewModel
            {
                Definition = (EntityDefinition)project,
                View = view,
                FilterDsl = view.GenerateElasticSearchFilterDsl(),
                SortDsl = view.GenerateEsSortDsl(),
                Routes = string.Join(",", view.RouteParameterCollection.Select(r => r.Name)),
                PartialArg = string.IsNullOrWhiteSpace(view.Partial) ? "" : ", partial",
                PartialPath = string.IsNullOrWhiteSpace(view.Partial) ? "" : ", \"" + view.Partial + "\""
            };
            var raw = Properties.Resources.EntityViewJs;
            var js = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, vm);
            return js;
        }
    }
}