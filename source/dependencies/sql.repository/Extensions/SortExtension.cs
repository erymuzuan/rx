using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class SortExtension
    {

        public static string GenerateQuery(this Sort sort)
        {
            var direction = sort.Direction == SortDirection.Desc ? " DESC" : "";
            return $@"[{sort.Path}]{direction}";
        }

        
    }
}