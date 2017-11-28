using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    public class OperationEndpointBuilder : Builder<OperationEndpoint>
    {
        protected override Task<RxCompilerResult> CompileAssetAsync(OperationEndpoint item)
        {
            var context = new SphDataContext();
            var ed = context.LoadOneFromSources<EntityDefinition>(x => x.Name == item.Entity);

            return item.CompileAsync(ed);
        }

     


    }
}