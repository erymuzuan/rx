using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    internal class WorkflowDefinitionBuilder : Builder<WorkflowDefinition>
    {
        protected override Task<RxCompilerResult> CompileAssetAsync(WorkflowDefinition item)
        {
            return item.CompileAsync();
        }
    }
}