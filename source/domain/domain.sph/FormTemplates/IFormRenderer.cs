using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{

    public interface IElementRenderer<T>
    {
        
    }
    public interface IFormRenderer
    {
        Task<BuildValidationResult> RenderAsync(WorkflowForm form);
        Task<BuildValidationResult> RenderAsync(EntityForm form);
        Task<BuildValidationResult> RenderAsync(FormDialog dialog);
        Task<BuildValidationResult> RenderAsync(PartialView view);
    }
}