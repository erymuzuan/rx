using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    public class ReceivePortBuilder : Builder<ReceivePort>
    {
        protected override Task<RxCompilerResult> CompileAssetAsync(ReceivePort item)
        {
            return item.CompileAsync();
        }

     
        

    }
}