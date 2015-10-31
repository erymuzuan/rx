using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ReportColumn : DomainObject
    {
        [XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Strings.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }

        public override string ToString()
        {
            return $"{this.Value}";
        }

        public void SetValue(string value)
        {
            this.Value = null;
            if (this.Type == typeof (string))
                this.Value = value;

            if (this.Type == typeof (DateTime))
            {
                DateTime date;
                if (DateTime.TryParse(value, out date))
                    this.Value = date;
            }

            if (this.Type == typeof (int))
            {
                int date;
                if (int.TryParse(value, out date))
                    this.Value = date;
            }
            if (this.Type == typeof (decimal))
            {
                decimal date;
                if (decimal.TryParse(value, out date))
                    this.Value = date;
            }
            if (this.Type == typeof (double))
            {
                double date;
                if (double.TryParse(value, out date))
                    this.Value = date;
            }
            if (this.Type == typeof (bool))
            {
                bool date;
                if (bool.TryParse(value, out date))
                    this.Value = date;
            }
            if (this.Type == typeof (float))
            {
                float date;
                if (float.TryParse(value, out date))
                    this.Value = date;
            }
        }
    }
}