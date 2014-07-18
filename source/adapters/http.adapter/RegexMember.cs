using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class RegexMember : Member
    {
        public string Pattern { get; set; }
        public string Group { get; set; }
    }
}