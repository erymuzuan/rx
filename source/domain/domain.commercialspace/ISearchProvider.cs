using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface ISearchProvider
    {
        Task<IEnumerable<SearchResult>> SearchAsync(string term);
    }

    public class SearchResult
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string Summary { get; set; }
        public float Score { get; set; }
        public string Status { get; set; }
        public string OwnerCode { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
