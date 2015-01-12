namespace Bespoke.Sph.Domain
{
    public class BinaryStore : Entity
    {
        public byte[] Content { get; set; }
        public string Extension { get; set; }

        public string FileName { get; set; }
    }
}
