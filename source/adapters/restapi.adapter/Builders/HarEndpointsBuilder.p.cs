namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class HarEndpointsBuilder
    {
        public string RequestHeaderSample { get; set; }
        public string RequestBodySample { get; set; }
        public string HttpVersion { get; set; }
        public string RequestContentType { get; set; }
        public double Order => 1;
        public string StoreId { get; set; }
        public string ResponseBodySample { get; set; }
    }
}