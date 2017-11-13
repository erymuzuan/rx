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
        protected virtual T Visit(JArray c) { return Visit((JArray)c); }
    }

    public class ShouldJProperty : JProperty
    {
        public ShouldJProperty(JProperty prop) : base("should", prop.Value)
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
}