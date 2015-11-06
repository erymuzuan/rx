namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class FlatFileAdapter
    {
        public string SampleFile { get; set; }
        public bool IsPositional { get; set; }
        public bool IsTagIdentifier { get; set; }
        public string Delimiter { get; set; }
        public string EscapeCharacted { get; set; }
        public string Tag { get; set; }
    }
}