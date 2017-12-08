using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Domain.diagnostics
{
    public class BuilDiagnostic : IBuildDiagnostics
    {
        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(WorkflowForm form, WorkflowDefinition wd)
        {
            return Task.FromResult(new BuildDiagnostic[] {});
        }
        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(OperationEndpoint endpoint, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] {});
        }
        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(QueryEndpoint endpoint, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] {});
        }
        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(PartialView view, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] {});
        }
        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(FormDialog form, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] {});
        }
        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] {});
        }

        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(WorkflowDefinition workflow)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(Trigger trigger)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(TransformDefinition map)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(Adapter adapter)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }
        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(ReceiveLocation location)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(ReceivePort port)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateErrorsAsync(ReportColumn port)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }



        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(WorkflowForm form, WorkflowDefinition wd)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }
        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(OperationEndpoint endpoint, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(QueryEndpoint endpoint, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }
        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(PartialView view, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }
        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(FormDialog form, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }
        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(EntityForm form, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(EntityView view, EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(EntityDefinition entity)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(WorkflowDefinition workflow)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(Trigger trigger)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(TransformDefinition map)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }

        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(Adapter adapter)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }
        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(ReceiveLocation location)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }
        public virtual Task<BuildDiagnostic[]> ValidateWarningsAsync(ReceivePort port)
        {
            return Task.FromResult(new BuildDiagnostic[] { });
        }
    }
}