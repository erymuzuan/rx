using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    public class OperationEndpointBuilder : Builder<OperationEndpoint>
    {
        protected override async Task<RxCompilerResult> CompileAssetAsync(OperationEndpoint item)
        {
            return await base.CompileAsync(item);
        }
    }
}