using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class DocumentField : Field
    {

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get => Strings.GetType(this.TypeName);
            set => this.TypeName = value.GetShortAssemblyQualifiedName();
        }

        public override object GetValue(RuleContext context)
        {
            var item = context.Item;
            if (string.IsNullOrWhiteSpace(this.Path))
                throw new InvalidOperationException("The Path property for " + this.Name + " cannot be null or empty");

            var json = JObject.Parse(item.ToJson());
            var token = json.SelectToken($"$.{Path}");
            if (null == token) return null;

            if (this.Type == typeof(int))
                return token.Value<int>();
            if (this.Type == typeof(short))
                return token.Value<short>();
            if (this.Type == typeof(long))
                return token.Value<long>();
            if (this.Type == typeof(byte))
                return token.Value<byte>();


            if (this.Type == typeof(int?))
                return token.Value<int?>();
            if (this.Type == typeof(short?))
                return token.Value<short?>();
            if (this.Type == typeof(long?))
                return token.Value<long?>();
            if (this.Type == typeof(byte?))
                return token.Value<byte?>();

            if (this.Type == typeof(string))
                return token.Value<string>();

            if (this.Type == typeof(DateTime))
                return token.Value<DateTime>();

            if (this.Type == typeof(DateTime?))
                return token.Value<DateTime?>();

            if (this.Type == typeof(bool))
                return token.Value<bool>();
            if (this.Type == typeof(bool?))
                return token.Value<bool?>();

            if (this.Type == typeof(decimal))
                return token.Value<decimal>();
            if (this.Type == typeof(decimal?))
                return token.Value<decimal?>();

            if (this.Type == typeof(double))
                return token.Value<double>();
            if (this.Type == typeof(double?))
                return token.Value<double?>();

            if (this.Type == typeof(float))
                return token.Value<float>();
            if (this.Type == typeof(float?))
                return token.Value<float?>();

            if (token.Value<object>() is JValue jv)
                return jv.Value;

            return token.Value<object>();
        }

        public override string GenerateCode()
        {
            return $"item.{this.Path}";
        }
    }
}