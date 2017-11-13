using Newtonsoft.Json.Linq;


// ReSharper disable RedundantCast
// ReSharper disable FunctionRecursiveOnAllPaths

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public abstract class JTokenVisitor<T>
    {
        public T DynamicVisit(JToken p) { return Visit((dynamic)p); }
        protected abstract T Visit(JToken p);
        protected virtual T Visit(JProperty c) { return Visit((JProperty)c); }
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
    }

    public class ShouldJProperty : JProperty
    {
        public ShouldJProperty(JProperty prop) : base("should", prop.Value)
        {
        }
        public ShouldJProperty(JToken prop) : base("should", prop.Children())
        {
        }
    }


    public class MustNotJProperty : JProperty
    {
        public MustNotJProperty(JProperty prop) : base("must_not", prop.Value)
        {
        }
    }
    public class MustJProperty : JProperty
    {
        public MustJProperty(JProperty prop) : base("must", prop.Value)
        {
        }
    }
    public class BoolJProperty : JProperty
    {
        public BoolJProperty(JProperty prop) : base("bool", prop.Value)
        {
        }
    }
    public class TermJProperty : JProperty
    {
        public TermJProperty(JProperty prop) : base("term", prop.Value)
        {
        }
    }
    public class RangeJProperty : JProperty
    {
        public RangeJProperty(JProperty prop) : base("range", prop.Value)
        {
        }
    }
    public class ExistsJProperty : JProperty
    {
        public ExistsJProperty(JProperty prop) : base("exists", prop.Value)
        {
        }
    }
    public class WildcardJProperty : JProperty
    {
        public WildcardJProperty(JProperty prop) : base("wildcard", prop.Value)
        {
        }
    }
    public class PrefixJProperty : JProperty
    {
        public PrefixJProperty(JProperty prop) : base("prefix", prop.Value)
        {
        }
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
Find documents where the field specified contains terms which match the regular expression specified.
fuzzy query
Find documents where the field specified contains terms which are fuzzily similar to the specified term. Fuzziness is measured as a Levenshtein edit distance of 1 or 2.
type query
Find documents of the specified type.
ids query*/
}