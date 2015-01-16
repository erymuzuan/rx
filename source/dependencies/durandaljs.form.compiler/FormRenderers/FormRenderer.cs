using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormRenderers
{
    public abstract class FormRenderer
    {
        public abstract Task<string> GenerateCodeAsync(IForm form, IProjectProvider project);
    }
}