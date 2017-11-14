using Newtonsoft.Json.Linq;


// ReSharper disable RedundantCast
// ReSharper disable FunctionRecursiveOnAllPaths

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public abstract class JTokenVisitor<T>
    {
        public T DynamicVisit(JToken token) { return Visit((dynamic)token); }
        protected abstract T Visit(JToken token);
        protected virtual T Visit(JProperty prop) { return Visit((JProperty)prop); }
        protected virtual T Visit(BoolJProperty c) { return Visit((BoolJProperty)c); }
        protected virtual T Visit(TermJProperty c) { return Visit((TermJProperty)c); }
        protected virtual T Visit(MustJProperty c) { return Visit((MustJProperty)c); }
        protected virtual T Visit(MustNotJProperty c) { return Visit((MustNotJProperty)c); }
        protected virtual T Visit(ShouldJProperty c) { return Visit((ShouldJProperty)c); }
        protected virtual T Visit(RangeJProperty c) { return Visit((RangeJProperty)c); }
        protected virtual T Visit(ExistsJProperty c) { return Visit((ExistsJProperty)c); }
        protected virtual T Visit(WildcardJProperty c) { return Visit((WildcardJProperty)c); }
        protected virtual T Visit(PrefixJProperty c) { return Visit((PrefixJProperty)c); }
        protected virtual T Visit(JArray c) { return Visit((JArray)c); }


        // aggregations
        protected virtual T Visit(FieldAggregateJProperty max) { return Visit((FieldAggregateJProperty)max); }
        protected virtual T Visit(MaxJProperty max) { return Visit((MaxJProperty)max); }
        protected virtual T Visit(ValueCountJProperty valueCount) { return Visit((ValueCountJProperty)valueCount); }
        protected virtual T Visit(MinJProperty min) { return Visit((MinJProperty)min); }
        protected virtual T Visit(AvgJProperty avg) { return Visit((AvgJProperty)avg); }
        protected virtual T Visit( SumJProperty sum) { return Visit((SumJProperty)sum); }
        protected virtual T Visit(PercentileRanksJProperty percentileRanks) { return Visit((PercentileRanksJProperty)percentileRanks); }
        protected virtual T Visit(PercentilesJProperty percentiles) { return Visit((PercentilesJProperty)percentiles); }
    }


    /*
     term query
Find documents which contain the exact term specified in the field specified.
terms query
Find documents which contain any of the exact terms specified in the field specified.
range query
Find documents where the field specified contains values (dates, numbers, or strings) in the range specified.
exists query
Find documents where the field specified contains any non-null value.
prefix query
Find documents where the field specified contains terms which begin with the exact prefix specified.
wildcard query
Find documents where the field specified contains terms which match the pattern specified, where the pattern supports single character wildcards (?) and multi-character wildcards (*)
regexp query
Find documents where the field specified contains terms which match the reguxxxxxlar expression specified.
fuzzy query
Find documents where the field specified contains terms which are fuzzily similar to the specified term. Fuzziness is measured as a Levenshtein edit distance of 1 or 2.
type query
Find documents of the specified type.
ids query*/
}