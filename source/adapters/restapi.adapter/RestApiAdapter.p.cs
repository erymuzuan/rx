namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class RestApiAdapter
    {
        public string BaseAddress { get; set; }

        public override string OdataTranslator { get; }
    }
}