using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("{ProviderName}/{AttachedTo}/{Name}/{ValueAsString}")]
    public class AttachedProperty : DomainObject
    {
        public class EqualityComparer : IEqualityComparer<AttachedProperty>
        {
            public bool Equals(AttachedProperty x, AttachedProperty y)
            {
                if (null == x) return false;
                if (null == y) return false;
                return
                    x.ProviderName == y.ProviderName
                    && x.Name == y.Name
                    && x.AttachedTo == y.AttachedTo;
            }

            public int GetHashCode(AttachedProperty obj)
            {
                return $"{obj.ProviderName}/{obj.Name}/{obj.AttachedTo}".GetHashCode();
            }
        }
        public AttachedProperty()
        {

        }

        public static Task<IEnumerable<AttachedProperty>> EmtptyListTask = Task.FromResult(Array.Empty<AttachedProperty>().AsEnumerable());

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
        public object[] ValidOptions { get; set; }
        public string EnabledExpression { get; set; } = "true";

        public string ValueAsString { get; set; }
        private object m_value;

        public object Value
        {
            get
            {
                if (null == m_value && null == ValueAsString)
                {
                    return null;
                }

                if (null != m_value && (m_value.GetType() == Type || this.Type.IsInstanceOfType(m_value)))
                    return m_value;


                if (Type == typeof(int) && int.TryParse(ValueAsString, out var intValue))
                    return intValue;
                if (Type == typeof(decimal) && decimal.TryParse(ValueAsString, out var decimalValue))
                    return decimalValue;
                if (Type == typeof(DateTime) && DateTime.TryParse(ValueAsString, out var dateTimeValue))
                    return dateTimeValue;
                if (Type == typeof(bool) && bool.TryParse(ValueAsString, out var bv))
                    return bv;
                return Type == typeof(string) ? ValueAsString : null;
            }
            set
            {
                this.ValueAsString = $"{value}";
                m_value = value;
            }
        }

        public T GetValue<T>(T defaultValue)
        {
            var value = this.Value;
            if (null == value)
                return default;

            return (T)value;
        }


        [JsonIgnore]
        public Type Type
        {
            get => Strings.GetType(this.TypeName);
            set => this.TypeName = value.GetShortAssemblyQualifiedName();
        }

        public object[] AllowedValue { get; set; }

        public AttachedProperty WithValue(AttachedProperty value)
        {
            if (null == value)
                return this.Clone();

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
        public AttachedProperty WithValue(AttachedProperty value, Member member)
        {
            if (null == value)
            {
                var clone = this.Clone();
                clone.AttachedTo = member.WebId;
                return clone;
            }

            return new AttachedProperty
            {
                Name = Name,
                Type = Type,
                Value = value.Value,
                AttachedTo = member.WebId,
                Description = Description,
                Help = Help,
                ProviderName = ProviderName,
                Required = Required,
                ValueAsString = value.ValueAsString,
                WebId = value.WebId,
                EnabledExpression = this.EnabledExpression
            };
        }
    }
}