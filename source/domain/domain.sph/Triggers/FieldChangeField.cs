using System;
using System.Linq;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class FieldChangeField : Field
    {
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }

        public override object GetValue(RuleContext context)
        {
            var log = context.Log;
            if(null == log)throw new ArgumentException("Context did not provide a log");

            var change = log.ChangeCollection.SingleOrDefault(c => c.PropertyName == this.Path);
            var val = null != change ? change.OldValue : null;
            if (null == val) return null;

            if (this.Type == typeof (int))
                return int.Parse(val);
            if (this.Type == typeof (DateTime))
                return DateTime.Parse(val);
            if (this.Type == typeof (decimal))
                return decimal.Parse(val);
            if (this.Type == typeof (bool))
                return bool.Parse(val);

            return val;
        }
    }
}