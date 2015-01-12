using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{

    public interface IElementRenderer<T>
    {
        
    }
    public interface IFormRenderer
    {
        Task<BuildValidationResult> RenderAsync(EntityForm form);
    }
}