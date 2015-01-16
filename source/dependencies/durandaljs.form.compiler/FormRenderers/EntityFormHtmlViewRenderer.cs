using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
namespace Bespoke.Sph.FormCompilers.DurandalJs.FormRenderers
{
    [Export("ViewRenderer", typeof (FormRenderer))]
    [FormRendererMetadata(FormType = typeof (EntityForm))]
    public class EntityFormHtmlViewRenderer : FormRenderer
    {
        public override Task<string> GenerateCodeAsync(IForm form, IProjectProvider project)
        {
            return Task.FromResult("html");
        }
    }
}