using Newtonsoft.Json.Linq;

namespace es.diagnostics
{
    public static class Helper
    {
        public static bool MapEquals<T>(this JToken source, JToken es)
        {
            if (null == source && null == es) return true;
            if (null == source) return false;
            if (null == es) return false;


            if (!source.HasValues && !es.HasValues) return true;
            if (!source.HasValues) return false;
            if (!es.HasValues) return false;

            var sourceValue = source.Value<T>();
            var esValue = es.Value<T>();
            return esValue.Equals(sourceValue);
        }
    }
}