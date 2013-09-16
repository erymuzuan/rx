using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Trigger : Entity
    {
        public static Trigger ParseJson(string json)
        {
            var trigger = JsonConvert.DeserializeObject<Trigger>(json);
            return trigger;
        }


    }
}
