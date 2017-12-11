using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;

namespace Bespoke.Sph.SourceBuilders
{
    public class ReceiveLocationBuilder : Builder<ReceiveLocation>
    {
        protected override async Task<RxCompilerResult> CompileAssetAsync(ReceiveLocation item)
        {
            var context = ObjectBuilder.GetObject<ISourceRepository>();
            var port = await context.LoadOneAsync<ReceivePort>(x => x.Id == item.ReceivePort.ToIdFormat());
            return await item.CompileAsync(port);
        }

    }
}