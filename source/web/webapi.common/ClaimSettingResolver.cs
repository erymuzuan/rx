using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.WebApi
{
    public class ClaimSettingResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (member.DeclaringType == typeof(ClaimSetting) && prop.PropertyName == nameof(ClaimSetting.Type))
            {
                prop.Writable = true;
                prop.Readable = true;
            }
            if (member.DeclaringType == typeof(ClaimSetting) && prop.PropertyName == nameof(ClaimSetting.ValueType))
            {
                prop.Writable = true;
                prop.Readable = true;
            }

            if (member.DeclaringType == typeof(ClaimSetting) && prop.PropertyName == nameof(ClaimSetting.Value))
            {
                prop.Writable = true;
                prop.Readable = true;
            }

            if (member.DeclaringType == typeof(ClaimSetting) && prop.PropertyName == nameof(ClaimSetting.Permission))
            {
                prop.Writable = true;
                prop.Readable = true;
            }

            return prop;
        }
    }
}