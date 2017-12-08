using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Domain
{
    public interface IBuildDiagnostics
    {
        Task<BuildDiagnostic[]> ValidateErrorsAsync(WorkflowForm form, WorkflowDefinition wd);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(OperationEndpoint endpoint, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(QueryEndpoint endpoint, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(PartialView view, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(FormDialog form, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(WorkflowDefinition workflow);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(Trigger trigger);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(TransformDefinition map);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(Adapter adapter);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(ReceivePort port);
        Task<BuildDiagnostic[]> ValidateErrorsAsync(ReceiveLocation location);


        Task<BuildDiagnostic[]> ValidateWarningsAsync(WorkflowForm form, WorkflowDefinition wd);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(OperationEndpoint endpoint, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(QueryEndpoint endpoint, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(PartialView view, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(FormDialog form, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(EntityForm form, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(EntityView view, EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(EntityDefinition entity);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(WorkflowDefinition workflow);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(Trigger trigger);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(TransformDefinition map);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(Adapter adapter);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(ReceivePort port);
        Task<BuildDiagnostic[]> ValidateWarningsAsync(ReceiveLocation adapter);
    }
}
