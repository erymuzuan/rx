using System.Collections.Generic;
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
                case GroupByAggregate grp:
                    return GenerateEsAggs(grp);
            }
            return null;
        }

        private static string GenerateEsAggs(MaxAggregate max)
        {
            return $@"""{max.Name}"" : {{ ""max"" : {{ ""field"":  ""{max.Path}""}}}}";
        }
        private static string GenerateEsAggs(GroupByAggregate grp)
        {
            return $@"""{grp.Name}"" : {{ ""terms"" : {{ ""field"":  ""{grp.Path}""}}}}";
        }

        public static bool ExtractValueFromSearchResult(this Aggregate agg, JToken json)
        {
            switch (agg)
            {
                case MaxAggregate max:
                    return max.ExtractValueFromSearchResult(json);
                case GroupByAggregate grp:
                    return grp.ExtractValueFromSearchResult(json);
            }
            return false;
        }
        private static bool ExtractValueFromSearchResult(this MaxAggregate agg, JToken json)
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
            return true;
        }
        private static bool ExtractValueFromSearchResult(this GroupByAggregate agg, JToken json)
        {
            var path = $"$.aggregations.{agg.Name}.buckets";
            if (json.SelectToken(path) is JArray buckets)
            {
                var result = new Dictionary<string, int>();
                foreach (var bucket in buckets)
                {
                    var key = (string)bucket["key"];
                    var count = (int)bucket["doc_count"];
                    result.Add(key, count);
                }
                agg.SetValue(result);
                agg.SetStringValue(result.ToJson());
            }
            return true;
        }
    }
}