using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class FunctoidMap : Map
    {
        public Functoid Functoid { get; set; }

        public async override Task<string> ConvertAsync(object source)
        {
            var val = await this.Functoid.ConvertAsync(source);
            var json = string.Format("\"{0}\":\"{1}\"", this.Destination, val);

            return json;
        }

        public override string GenerateCode()
        {
            return this.Functoid.GeneratePreCode(this)
                + "\r\n" +
                string.Format("                dest.{1} = {0};", this.Functoid.GenerateCode(), this.Destination);

        }
    }
}