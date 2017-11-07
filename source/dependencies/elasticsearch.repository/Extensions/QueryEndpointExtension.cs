using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class QueryEndpointExtension
    {

        public static Task<string> GenerateEsQueryAsync(this QueryEndpoint qe)
        {
            var filter = qe.GenerateElasticSearchFilterDsl(qe.FilterCollection);
            var sort = "\"sort\": [" + qe.SortCollection.ToString(",", x => $@"{{""{x.Path}"": {{""order"":""{x.Direction.ToString().ToLowerInvariant()}""}}}}") + "]";
            var max = @"
    ""aggs"" : {
        ""filtered_max_date"" : { 
              ""filter"" : " + filter + @",
              ""aggs"": {
                        ""last_changed_date"": {
                           ""max"": {
                              ""field"": ""ChangedDate""
                            }
                        }
               }
        }
    }
";
            var query =
                $@"{{
                    ""filter"":{filter} 
                    {qe.GetFields()},
                    {sort},
                    {max} 

    }}";

            return Task.FromResult(query);

        }

        public static string GetFields(this QueryEndpoint qe)
        {
            if (!qe.MemberCollection.Any()) return string.Empty;
            var fields = $@"""fields"" :[ { string.Join(",", qe.MemberCollection.Select(x => $"\"{x}\""))}]";
            return "," + fields;
        }

    }
}
