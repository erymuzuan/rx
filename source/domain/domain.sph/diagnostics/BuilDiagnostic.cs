using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Domain.diagnostics
{
    public class BuilDiagnostic : IBuildDiagnostics
    {
        public virtual Task<BuildError[]> ValidateErrorsAsync(FormDialog form, EntityDefinition entity)
        {
            return Task.FromResult(new BuildError[] {});
        }
        public virtual Task<BuildError[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity)
        {
            return Task.FromResult(new BuildError[] {});
        }

        public virtual Task<BuildError[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateErrorsAsync(EntityDefinition entity)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateErrorsAsync(WorkflowDefinition workflow)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateErrorsAsync(Trigger trigger)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateErrorsAsync(TransformDefinition map)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateErrorsAsync(Adapter adapter)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateWarningsAsync(FormDialog form, EntityDefinition entity)
        {
            return Task.FromResult(new BuildError[] { });
        }
        public virtual Task<BuildError[]> ValidateWarningsAsync(EntityForm form, EntityDefinition entity)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateWarningsAsync(EntityView view, EntityDefinition entity)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateWarningsAsync(EntityDefinition entity)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateWarningsAsync(WorkflowDefinition workflow)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateWarningsAsync(Trigger trigger)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateWarningsAsync(TransformDefinition map)
        {
            return Task.FromResult(new BuildError[] { });
        }

        public virtual Task<BuildError[]> ValidateWarningsAsync(Adapter adapter)
        {
            return Task.FromResult(new BuildError[] { });
        }
    }
}