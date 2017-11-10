using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class AggregateExtension
    {
        public static string GenerateQuery(this Aggregate agg)
        {
            switch (agg)
            {
                case MaxAggregate max:
                    return GenerateEsAggs(max);
            }
            return null;
        }

        private static string GenerateEsAggs(MaxAggregate max)
        {
            return $@"""{max.Name}"" : {{ ""max"" : {{ ""field"":  ""{max.Path}""}}}}";
        }
    }
}