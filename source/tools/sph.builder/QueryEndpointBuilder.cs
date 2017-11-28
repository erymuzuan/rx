using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    public class QueryEndpointBuilder : Builder<QueryEndpoint>
    {
        protected override Task<RxCompilerResult> CompileAssetAsync(QueryEndpoint item)
        {
            var context = new SphDataContext();
            var ed = context.LoadOneFromSources<EntityDefinition>(x => x.Id == item.Entity.ToIdFormat());
            return item.CompileAsync(ed);
        }
    }
}