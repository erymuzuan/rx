using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class BinaryStore : Entity
    {
        [JsonIgnore]
        public byte[] Content { get; set; }
        public string Extension { get; set; }

        public string FileName { get; set; }
    }
}
