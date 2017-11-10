using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

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

        public static void ExtractValueFromSearchResult(this Aggregate agg, JObject json)
        {
            var vp = $"$.aggregations.{agg.Name}.value";
            var vps = $"$.aggregations.{agg.Name}.value_as_string";
            if (json.SelectToken(vp) is JValue valueToken)
            {
                agg.SetValue(valueToken.Value);
            }
            if (json.SelectToken(vps) is JValue vs)
            {
                agg.SetValue(vs.Value);
                agg.SetStringValue($"{vs.Value}");
            }
        }
    }
}