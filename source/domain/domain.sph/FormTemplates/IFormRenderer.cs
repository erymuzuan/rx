using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{

    public interface IElementRenderer<in T>
    {
        Task<BuildValidationResult> RenderAsync(T form);
    }
    public interface IFormRenderer
    {
        Task<BuildValidationResult> RenderAsync(WorkflowForm form);
        Task<BuildValidationResult> RenderAsync(EntityForm form);
        Task<BuildValidationResult> RenderAsync(FormDialog dialog);
        Task<BuildValidationResult> RenderAsync(PartialView view);
    }
}