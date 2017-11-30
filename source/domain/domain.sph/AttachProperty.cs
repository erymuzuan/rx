using System;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class AttachProperty : DomainObject
    {
        public AttachProperty()
        {
            
        }

        public AttachProperty(string name, object value)
        {
            this.Name = name;
            this.Type = value.GetType();
            this.Value = value;
        }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool Required { get; set; }
        public string Help { get; set; }

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
    }
}