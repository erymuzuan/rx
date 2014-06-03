using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class Map : DomainObject
    {
        public string Destination { get; set; }
        public virtual Task<string> ConvertAsync(object source)
        {
            throw new System.NotImplementedException();
        }
    }
}