using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class AggregateExtension
    {

        public static (string sql, bool rewrite) GenerateQuery<T>(this QueryDsl query) where T : DomainObject, new()
        {
            var entity = new T();
            if (query.Aggregates.Count == 0)
                return (string.Empty, false);
            //
            var rw = query.Aggregates.OfType<GroupByAggregate>().Any() || query.Aggregates.OfType<DateHistogramAggregate>().Any();
            var projection = "TODO";
            if (!rw && query.Aggregates.Count == 1)
            {
                projection = GenerateQuery(query.Aggregates.Single());
            }



            var elements = new Dictionary<string, object>();
            var hasFilters = query.Filters.Any();
            if (hasFilters)
            {
                var arrayEsFilters = query.Filters.ToSqlPredicate();
                elements.Add("WHERE", arrayEsFilters.CompileToBoolQuery(entity));
            }
            if (rw)
            {
                var gb = query.Aggregates.OfType<GroupByAggregate>().Single();
                projection = $"JSON_VALUE([Json], '$.{gb.Path}') as '{gb.Name}', COUNT(Id) as 'Count'";
                elements.Add("GROUP BY", $"JSON_VALUE([Json], '$.{gb.Path}')");
            }


            var sql = $@"SELECT {projection} FROM [{ConfigurationManager.ApplicationName}].[{typeof(T).Name}]
    {elements.ToString("\r\n", x => $@"{x.Key} {x.Value}")}";
            return (sql, true);
        }

        public static Task ReadAsync<T>(this Aggregate agg, SqlCommand cmd) where T : Entity, new()
        {
            switch (agg)
            {
                case MaxAggregate max:
                    return ReadAsync<T>(max, cmd);
                case MinAggregate min:
                    return ReadAsync<T>(min, cmd);
                case AverageAggregate avg:
                    return ReadAsync<T>(avg, cmd);
                case SumAggregate sum:
                    return ReadAsync<T>(sum, cmd);
                case CountDistinctAggregate count:
                    return ReadAsync<T>(count, cmd);
                case DateHistogramAggregate dhg:
                    return ReadAsync<T>(dhg, cmd);
                case GroupByAggregate grp:
                    return ReadAsync<T>(grp, cmd);
            }
            return null;
        }

        private static string GenerateQuery(this Aggregate agg)
        {
            switch (agg)
            {
                case MaxAggregate max:
                    return GenerateSqlAggs(max);
                case MinAggregate min:
                    return GenerateSqlAggs(min);
                case AverageAggregate avg:
                    return GenerateSqlAggs(avg);
                case SumAggregate sum:
                    return GenerateSqlAggs(sum);
                case CountDistinctAggregate count:
                    return GenerateSqlAggs(count);
                case DateHistogramAggregate dhg:
                    return GenerateSqlAggs(dhg);
                case GroupByAggregate grp:
                    return GenerateSqlAggs(grp);
            }
            return null;
        }

        private static async Task ReadAsync<T>(MinAggregate min, SqlCommand cmd) where T : Entity, new()
        {
            var result = await cmd.ExecuteScalarAsync();
            min.SetValue(result);
        }

        private static async Task ReadAsync<T>(MaxAggregate max, SqlCommand cmd) where T : Entity, new()
        {
            var result = await cmd.ExecuteScalarAsync();
            max.SetValue(result);
        }
        private static async Task ReadAsync<T>(CountDistinctAggregate count, SqlCommand cmd) where T : Entity, new()
        {
            var result = await cmd.ExecuteScalarAsync();
            count.SetValue(result);
        }
        private static async Task ReadAsync<T>(SumAggregate sum, SqlCommand cmd) where T : Entity, new()
        {
            var result = await cmd.ExecuteScalarAsync();
            sum.SetValue(result);
        }
        private static async Task ReadAsync<T>(AverageAggregate avg, SqlCommand cmd) where T : Entity, new()
        {
            var result = await cmd.ExecuteScalarAsync();
            avg.SetValue(result);
        }

        private static async Task ReadAsync<T>(GroupByAggregate gb, SqlCommand cmd) where T : Entity, new()
        {
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                var result = new Dictionary<string, int>();
                while (await reader.ReadAsync())
                {
                    result.Add(reader.GetString(0), reader.GetInt32(1));
                }
                gb.SetValue(result);
            }
        }

        private static async Task ReadAsync<T>(DateHistogramAggregate dateHistogram, SqlCommand cmd) where T : Entity, new()
        {
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                var temp = new Dictionary<DateTime, int>();
                var result = new Dictionary<DateTime, int>();
                while (await reader.ReadAsync())
                {
                    temp.Add(DateTime.Parse(reader.GetString(0)), reader.GetInt32(1));
                }
                // TODO : group by SQL date function like, YEAR,MONTH etc.. so less resultset, let the SQL server handles the aggregations
                if (dateHistogram.Interval == "year")
                {
                    foreach (var date in temp.Keys)
                    {
                        var year = new DateTime(date.Year, 1, 1);
                        if (result.ContainsKey(year))
                            result[year] += temp[date];
                        else
                            result.Add(year, temp[date]);
                    }
                }

                if (dateHistogram.Interval == "month")
                {
                    foreach (var date in temp.Keys)
                    {
                        var month = new DateTime(date.Year, date.Month, 1);
                        if (result.ContainsKey(month))
                            result[month] += temp[date];
                        else
                            result.Add(month, temp[date]);
                    }
                }

                if (dateHistogram.Interval == "day")
                {
                    foreach (var date in temp.Keys)
                    {
                        var day = new DateTime(date.Year, date.Month, date.Day);
                        if (result.ContainsKey(day))
                            result[day] += temp[date];
                        else
                            result.Add(day, temp[date]);
                    }
                }
                if (dateHistogram.Interval == "hour")
                {
                    foreach (var date in temp.Keys)
                    {
                        var hour = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
                        if (result.ContainsKey(hour))
                            result[hour] += temp[date];
                        else
                            result.Add(hour, temp[date]);
                    }
                }

                dateHistogram.SetValue(result);
            }
        }

        private static string GenerateSqlAggs(AverageAggregate avg)
        {
            return $@"AVG([{avg.Path}]) as '{avg.Name}'";
        }
        private static string GenerateSqlAggs(CountDistinctAggregate count)
        {
            return $@"COUNT(DISTINCT [{count.Path}]) as '{count.Name}'";
        }
        private static string GenerateSqlAggs(SumAggregate sum)
        {
            return $@"SUM([{sum.Path}]) as '{sum.Name}'";
        }
        private static string GenerateSqlAggs(MinAggregate min)
        {
            return $@"MIN([{min.Path}]) as '{min.Name}'";
        }

        private static string GenerateSqlAggs(MaxAggregate max)
        {
            return $@"MAX([{max.Path}]) as '{max.Name}'";
        }
        private static string GenerateSqlAggs(GroupByAggregate grp)
        {
            return $@"""{grp.Name}"" : {{ ""terms"" : {{ ""field"":  ""{grp.Path}""}}}}";
        }

        private static string GenerateSqlAggs(DateHistogramAggregate grp)
        {
            return $@"""{grp.Name}"" : {{ ""date_histogram"" : {{ 
                    ""field"":  ""{grp.Path}"",
                    ""interval"": ""{grp.Interval}"",
                    ""offset"": ""+8h"",
                    ""format"": ""yyyy-MM-dd HH:mm:ss""

}}}}";
        }

        public static bool ExtractValueFromSearchResult(this Aggregate agg, JToken json)
        {
            switch (agg)
            {
                case MaxAggregate max:
                    return max.ExtractValueFromSearchResult(json);
                case DateHistogramAggregate dhg:
                    return dhg.ExtractValueFromSearchResult(json);
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
        private static bool ExtractValueFromSearchResult(this DateHistogramAggregate agg, JToken json)
        {
            var path = $"$.aggregations.{agg.Name}.buckets";
            if (json.SelectToken(path) is JArray buckets)
            {
                var result = new Dictionary<string, int>();
                var result2 = new Dictionary<DateTime, int>();
                foreach (var bucket in buckets)
                {
                    var key = (string)bucket["key"];
                    var keyAsString = bucket["key_as_string"];
                    var count = (int)bucket["doc_count"];
                    if (DateTime.TryParse($"{keyAsString}", out var date))
                    {
                        key = date.ToString("s");
                        result2.Add(date, count);
                    }
                    result.Add(key, count);
                }
                if (result2.Count > 0)
                {

                    agg.SetValue(result2);
                    agg.SetStringValue(result2.ToJson());
                    return true;
                }
                agg.SetValue(result);
                agg.SetStringValue(result.ToJson());
            }
            return true;
        }
    }
}