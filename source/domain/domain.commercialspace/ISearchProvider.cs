using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface ISearchProvider
    {
        Task<IEnumerable<SearchResult>> SearchAsync(dynamic term);
    }

    public class SearchResult
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string Summary { get; set; }
    }
}
