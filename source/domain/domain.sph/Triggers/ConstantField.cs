using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ConstantField : Field
    {
        public override async Task<IEnumerable<BuildError>> ValidateErrorsAsync(Filter filter)
        {
            var errors = (await base.ValidateErrorsAsync(filter)).ToList();
            var value = $"{Value}";
            if (typeof(DateTime) == this.Type && !string.IsNullOrWhiteSpace(value))
            {
                if (!DateTime.TryParse(value, out _))
                    errors.Add(new BuildError(this.WebId, $@"""{value}"" is not a valid DateTime value for {Name} in {filter.Term} filter"));
            }
            if (typeof(int) == this.Type && !string.IsNullOrWhiteSpace(value))
            {
                if (!int.TryParse(value, out _))
                    errors.Add(new BuildError(this.WebId, $@"""{value}"" is not a valid Int32 value for {Name} in {filter.Term} filter"));
            }
            if (typeof(decimal) == this.Type && !string.IsNullOrWhiteSpace(value))
            {
                if (!decimal.TryParse(value, out _))
                    errors.Add(new BuildError(this.WebId, $@"""{value}"" is not a valid decimal value for {Name} in {filter.Term} filter"));
            }
            if (typeof(bool) == this.Type && !string.IsNullOrWhiteSpace(value))
            {
                if (!bool.TryParse(value, out _))
                    errors.Add(new BuildError(this.WebId, $@"""{value}"" is not a valid boolean value for {Name} in {filter.Term} filter, the only valid value is ""true"" or  ""false"""));
            }
            return errors;
        }

        private object m_value;

        [XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get => Strings.GetType(this.TypeName);
            set => this.TypeName = value.GetShortAssemblyQualifiedName();
        }


        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == "TypeName" && !string.IsNullOrWhiteSpace(this.TypeName) && null != m_tempVal)
            {

                var val = $"{m_tempVal}";
                this.Value = this.ParseValue(val);
                m_tempVal = null;
                RaisePropertyChanged("Value");

            }
            base.OnPropertyChanged(propertyName);
        }

        private object ParseValue(string val)
        {
            if (this.Type == typeof(int))
                return int.Parse(val);
            if (this.Type == typeof(DateTime))
                return DateTime.Parse(val);
            if (this.Type == typeof(decimal))
                return decimal.Parse(val);
            if (this.Type == typeof(bool))
                return bool.Parse(val);

            return val;


        }

        private object m_tempVal;
        public object Value
        {
            get => m_value;
            set
            {
                if (string.IsNullOrWhiteSpace(this.TypeName))
                {
                    m_tempVal = value;
                    return;
                }
                m_value = this.ParseValue($"{value}");
                RaisePropertyChanged();
            }
        }


        public override object GetValue(RuleContext context)
        {
            if (string.IsNullOrWhiteSpace(this.TypeName) && null != m_tempVal)
            {
                this.Type = m_tempVal.GetType();
            }
            var val = $"{this.Value}";
            if (this.Type == typeof(int))
            {
                if (int.TryParse(val, out var f))
                    return f;
            }

            if (this.Type == typeof(DateTime))
            {
                if (DateTime.TryParse(val, out var f))
                    return f;
            }
            if (this.Type == typeof(bool))
            {
                if (bool.TryParse(val, out var f))
                    return f;
            }
            if (this.Type == typeof(decimal))
            {
                if (decimal.TryParse(val, out var f))
                    return f;
            }

            return this.Value;
        }

        public override string GenerateCode()
        {
            if (this.Type == typeof(string))
                return $"\"{this.Value}\"";
            if (this.Type == typeof(DateTime))
                return $"DateTime.Parse(\"{this.Value}\")";
            if (Type == typeof(bool) && this.Value is bool)
                return (bool)this.Value ? "true" : "false";

            return $"{this.Value}";
        }
    }
}