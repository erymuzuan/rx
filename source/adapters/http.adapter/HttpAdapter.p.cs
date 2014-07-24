namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class HttpAdapter
    {
        public string Har { get; set; }
        public string BaseAddress { get; set; }
        public AuthenticationMode AuthenticationMode { get; set; }
        public long? Timeout { get; set; }
        public string TimeoutInterval { get; set; }
    }
}
