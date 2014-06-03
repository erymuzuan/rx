using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class FunctoidMap : Map
    {
        public Functoid Functoid { get; set; }

        public async override Task<string> ConvertAsync(object source)
        {
            var val =await  this.Functoid.ConvertAsync(source);
            var json = string.Format("\"{0}\":\"{1}\"", this.Destination, val);

            return json;
        }
    }
}