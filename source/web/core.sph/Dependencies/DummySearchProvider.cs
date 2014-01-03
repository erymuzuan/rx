using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Dependencies
{
    public class DummySearchProvider : ISearchProvider
    {
        public Task<IEnumerable<SearchResult>> SearchAsync(string term)
        {
            var results = from i in Enumerable.Range(1, 4)
                          select new SearchResult
                              {
                                  Id = i,
                                  Title = "sample " + i + " :" + term,
                                  Summary = "Sample summary for " + i
                              };
            return Task.FromResult(results);
        }
    }
}