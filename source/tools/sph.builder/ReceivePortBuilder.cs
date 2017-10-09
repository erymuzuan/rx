using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    public class ReceivePortBuilder : Builder<ReceivePort>
    {
        protected override Task<WorkflowCompilerResult> CompileAssetAsync(ReceivePort item)
        {
            return item.CompileAsync();
        }

     
        

    }
}