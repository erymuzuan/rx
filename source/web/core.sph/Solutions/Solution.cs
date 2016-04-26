using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    public class Solution
    {
        public ObjectCollection<SolutionItem> itemCollection { get; } = new ObjectCollection<SolutionItem>();
    }
}