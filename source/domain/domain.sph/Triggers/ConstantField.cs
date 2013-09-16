using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ConstantField : Field
    {
        private object m_value;

        [XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.AssemblyQualifiedName;
            }
        }


        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == "TypeName" && !string.IsNullOrWhiteSpace(this.TypeName) && null != m_tempVal)
            {

                var val = string.Format("{0}", m_tempVal);
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
            get { return m_value; }
            set
            {
                if (string.IsNullOrWhiteSpace(this.TypeName))
                {
                    m_tempVal = value;
                    return;
                }
                m_value = this.ParseValue(string.Format("{0}",value));
                RaisePropertyChanged();
            }
        }


        public override object GetValue(RuleContext context)
        {
            if (string.IsNullOrWhiteSpace(this.TypeName) && null != m_tempVal)
            {
                this.Type = m_tempVal.GetType();
            }
            var val = string.Format("{0}", this.Value);
            if (this.Type == typeof(int))
            {
                int f;
                if (int.TryParse(val, out f))
                    return f;
            }

            if (this.Type == typeof(DateTime))
            {
                DateTime f;
                if (DateTime.TryParse(val, out f))
                    return f;
            }
            if (this.Type == typeof(bool))
            {
                bool f;
                if (bool.TryParse(val, out f))
                    return f;
            }
            if (this.Type == typeof(decimal))
            {
                decimal f;
                if (decimal.TryParse(val, out f))
                    return f;
            }

            return this.Value;
        }
    }
}