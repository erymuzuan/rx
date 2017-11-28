using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    public class ReceiveLocationBuilder : Builder<ReceiveLocation>
    {
        protected override Task<RxCompilerResult> CompileAssetAsync(ReceiveLocation item)
        {
            var context = new SphDataContext();
            var port = context.LoadOneFromSources<ReceivePort>(x => x.Id == item.ReceivePort.ToIdFormat());
            return item.CompileAsync(port);
        }
        
    }
}