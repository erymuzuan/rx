using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("{ProviderName}/{AttachedTo}/{Name}/{ValueAsString}")]
    public class AttachedProperty : DomainObject
    {
        public AttachedProperty()
        {
            
        }

        public AttachedProperty(string name, object value, DomainObject attachedTo, string providerName)
        {
            this.Name = name;
            this.Type = value.GetType();
            this.Value = value;
            ProviderName = providerName;
            AttachedTo = attachedTo.WebId;
        }

        public string ProviderName { get; set; }
        public string AttachedTo { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool Required { get; set; }
        public string Help { get; set; }
        public string Description { get; set; }

        public string ValueAsString { get; set; }
        private object m_value;
        public object Value
        {
            get
            {
                if (null != m_value)
                    return m_value;

                if (Type == typeof(int) && int.TryParse(ValueAsString, out var intValue))
                    return intValue;
                if (Type == typeof(decimal) && decimal.TryParse(ValueAsString, out var decimalValue))
                    return decimalValue;
                if (Type == typeof(DateTime) && DateTime.TryParse(ValueAsString, out var dateTimeValue))
                    return dateTimeValue;
                if (Type == typeof(bool) && bool.TryParse(ValueAsString, out var bv))
                    return bv;

                return ValueAsString;
            }
            set
            {
                this.ValueAsString = $"{value}";
                m_value = value;
            }
        }

        public T GetValue<T>(T defaultValue)
        {
            return (T)this.Value;
        }


        [JsonIgnore]
        public Type Type
        {
            get => Strings.GetType(this.TypeName);
            set => this.TypeName = value.GetShortAssemblyQualifiedName();
        }

        public AttachedProperty WithValue(AttachedProperty value)
        {
            return new AttachedProperty
            {
                Name = Name,
                Type = Type,
                Value = value.Value,
                AttachedTo = value.AttachedTo,
                Description = Description,
                Help = Help,
                ProviderName = ProviderName,
                Required = Required,
                ValueAsString = value.ValueAsString,
                WebId = value.WebId
            };
        }
    }
}