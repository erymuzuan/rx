using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export]
    public class EntityViewHtmlRenderer
    {
        public async Task<string> GenerateCodeAsync(EntityView view, IProjectProvider project)
        {
            var vm = new EntityViewHtmlViewModel {Definition = (EntityDefinition) project, View = view};
            var raw = Properties.Resources.entity_view;
            var markup = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, vm);
            return markup;
        }
    }
}