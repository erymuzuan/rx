using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class Map : DomainObject
    {
        public virtual Task<string> ConvertAsync(object source)
        {
            throw new System.NotImplementedException();
        }
    }
}