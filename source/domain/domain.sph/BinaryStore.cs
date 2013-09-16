namespace Bespoke.Sph.Domain
{
    public class BinaryStore : Entity
    {
        public string StoreId { get; set; }
        public byte[] Content { get; set; }
        public string Extension { get; set; }

        public string FileName { get; set; }
    }
}
