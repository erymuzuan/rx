using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Domain
{
    public interface IBuildDiagnostics
    {
        Task<BuildError[]> ValidateErrorsAsync(FormDialog form, EntityDefinition entity);
        Task<BuildError[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity);
        Task<BuildError[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity);
        Task<BuildError[]> ValidateErrorsAsync(EntityDefinition entity);
        Task<BuildError[]> ValidateErrorsAsync(WorkflowDefinition workflow);
        Task<BuildError[]> ValidateErrorsAsync(Trigger trigger);
        Task<BuildError[]> ValidateErrorsAsync(TransformDefinition map);
        Task<BuildError[]> ValidateErrorsAsync(Adapter adapter);
        Task<BuildError[]> ValidateWarningsAsync(FormDialog form, EntityDefinition entity);
        Task<BuildError[]> ValidateWarningsAsync(EntityForm form, EntityDefinition entity);
        Task<BuildError[]> ValidateWarningsAsync(EntityView view, EntityDefinition entity);
        Task<BuildError[]> ValidateWarningsAsync(EntityDefinition entity);
        Task<BuildError[]> ValidateWarningsAsync(WorkflowDefinition workflow);
        Task<BuildError[]> ValidateWarningsAsync(Trigger trigger);
        Task<BuildError[]> ValidateWarningsAsync(TransformDefinition map);
        Task<BuildError[]> ValidateWarningsAsync(Adapter adapter);
    }
}
