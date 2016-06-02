using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SprocParameter : SimpleMember
    {
        public int? MaxLength { get; set; }
        public ParameterMode Mode { get; set; }
        public string SqlType { get; set; }
        public int Position { get; set; }
    }
}