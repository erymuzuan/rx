using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class TermJProperty : JProperty
    {
        public TermJProperty(JProperty prop) : base("term", prop.Value)
        {
        }
    }
    public class ValueCountJProperty : FieldAggregateJProperty
    {
        public ValueCountJProperty(JProperty prop, string aggregateName) : base(prop.Value, aggregateName, "value_count")
        {
        }
    }
    public class FieldAggregateJProperty : JProperty
    {
        public string AggregateName { get; }
        public string AggregateType { get; }
        public string FieldName => this.First.SelectToken("field").ToString();

        public FieldAggregateJProperty(JToken token, string aggregateName, string aggregateType) : base(aggregateName, token)
        {
            AggregateName = aggregateName;
            AggregateType = aggregateType;
        }
    }

    public class MaxJProperty : FieldAggregateJProperty
    {
        public MaxJProperty(JProperty prop, string aggregateName) : base(prop.Value, aggregateName, "max")
        {
        }
    }
    public class MinJProperty : FieldAggregateJProperty
    {
        public MinJProperty(JProperty prop, string aggregateName) : base(prop.Value, aggregateName, "min")
        {
        }
    }
    public class SumJProperty : FieldAggregateJProperty
    {
        public SumJProperty(JProperty prop, string aggregateName) : base(prop.Value, aggregateName, "sum")
        {
        }
    }
    public class AggregageTermsJProperty : FieldAggregateJProperty
    {
        public AggregageTermsJProperty(JProperty prop, string aggregateName) : base(prop.Value, aggregateName, "terms")
        {
        }
    }
    public class AggregageDateHistorgramJProperty : FieldAggregateJProperty
    {
        public AggregageDateHistorgramJProperty(JProperty prop, string aggregateName) : base(prop.Value, aggregateName, "date_histogram")
        {
        }
    }
    public class AvgJProperty : FieldAggregateJProperty
    {
        public AvgJProperty(JProperty prop, string aggregateName) : base(prop.Value, aggregateName, "avg")
        {
        }
    }
    public class PercentilesJProperty : FieldAggregateJProperty
    {
        public PercentilesJProperty(JProperty prop, string aggregateName) : base(prop.Value, aggregateName, "percentiles")
        {
        }
    }
    public class PercentileRanksJProperty : FieldAggregateJProperty
    {
        public PercentileRanksJProperty(JProperty prop, string aggregateName) : base(prop.Value, aggregateName, "percentile_ranks")
        {
        }
    }
}