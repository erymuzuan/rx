using Bespoke.Sph.Domain;

namespace Bespoke.Sph.CosmosDbRepository.Extensions
{
    public static class SortExtension
    {

        public static string GenerateQuery(this Sort sort)
        {
            var direction = sort.Direction == SortDirection.Asc ? "ASC" : "DESC";
            return $@"ORDER BY [{sort.Path}] {direction}";
        }


    }
}