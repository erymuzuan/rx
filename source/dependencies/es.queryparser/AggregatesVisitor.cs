using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class AggregatesVisitor : JTokenVisitor<IList<Aggregate>>
    {
        protected override IList<Aggregate> Visit(JToken token)
        {
            var aggs = from st in token
                       select DynamicVisit(st);
            return aggs.SelectMany(x => x).ToList();
        }

        protected override IList<Aggregate> Visit(JProperty prop)
        {
            if ( prop.First.First is JProperty agg)
            {
                var name = agg.Name;
                switch (name)
                {
                    case "max":
                        return DynamicVisit(new MaxJProperty(agg, prop.Name));
                    case "min":
                        return DynamicVisit(new MinJProperty(agg, prop.Name));
                    case "avg":
                        return DynamicVisit(new AvgJProperty(agg, prop.Name));
                    case "value_count":
                        return DynamicVisit(new ValueCountJProperty(agg, prop.Name));
                    case "percentiles":
                        return DynamicVisit(new PercentilesJProperty(agg, prop.Name));
                    case "percentile_ranks":
                        return DynamicVisit(new PercentileRanksJProperty(agg, prop.Name));
                    case "sum":
                        return DynamicVisit(new SumJProperty(agg, prop.Name));
                    case "terms":
                        return DynamicVisit(new AggregageTermsJProperty(agg, prop.Name));
                    case "date_histogram":
                        return DynamicVisit(new AggregageDateHistorgramJProperty(agg, prop.Name));
                }
            }

            return new List<Aggregate>();
        }


        protected override IList<Aggregate> Visit(AggregageTermsJProperty terms)
        {
            return new List<Aggregate>
            {
                new GroupByAggregate(terms.AggregateName, terms.FieldName)
            };
        }
        protected override IList<Aggregate> Visit(AggregageDateHistorgramJProperty dhg)
        {
            var interval = dhg.First["interval"].ToEmptyString();
            return new List<Aggregate>
            {
                new DateHistogramAggregate(dhg.AggregateName, dhg.FieldName, interval)
            };
        }

        protected override IList<Aggregate> Visit(PercentilesJProperty agg)
        {
            throw new Exception("shitt...");
        }

        protected override IList<Aggregate> Visit(PercentileRanksJProperty agg)
        {
            throw new Exception("shitt...");
        }


        protected override IList<Aggregate> Visit(SumJProperty agg)
        {
            return new List<Aggregate>
            {
                new SumAggregate(agg.AggregateName, agg.FieldName)
            };
        }

        protected override IList<Aggregate> Visit(MinJProperty agg)
        {
            return new List<Aggregate>
            {
                new MinAggregate(agg.AggregateName, agg.FieldName)
            };
        }

        protected override IList<Aggregate> Visit(AvgJProperty agg)
        {
            return new List<Aggregate>
            {
                new AverageAggregate(agg.AggregateName, agg.FieldName)
            };
        }

        protected override IList<Aggregate> Visit(ValueCountJProperty agg)
        {
            return new List<Aggregate>
            {
                new CountDistinctAggregate(agg.AggregateName, agg.FieldName)
            };
        }
        protected override IList<Aggregate> Visit(MaxJProperty max)
        {
            return new List<Aggregate>
            {
                new MaxAggregate(max.AggregateName, max.FieldName)
            };
        }
    }
}