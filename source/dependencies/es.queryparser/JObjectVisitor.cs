using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public abstract class JObjectVisitor<T>
    {
        public T DynamicVisit(JObject p) { return Visit((dynamic)p); }
        protected abstract T Visit(JObject p);
        protected virtual T Visit(JProperty c) { return Visit((JProperty)c); }
        protected virtual T Visit(JArray c) { return Visit((JArray)c); }
    }
}