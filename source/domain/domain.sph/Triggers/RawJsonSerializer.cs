using System;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class RawJsonSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var name = value as WorkflowPresentation;
            if (name != null) writer.WriteRawValue(name.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(WorkflowPresentation).IsAssignableFrom(objectType);
        }
    }
}