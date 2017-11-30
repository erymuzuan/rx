using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    public class QueryEndpointBuilder : Builder<QueryEndpoint>
    {
        protected override Task<RxCompilerResult> CompileAssetAsync(QueryEndpoint item)
        {
            return  base.CompileAsync(item);
        }
    }
}