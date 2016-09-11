namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class RestApiServerAdapter
    {
        public string BaseAddress { get; set; }

        public override string OdataTranslator { get; }
    }
}