using System;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json;

namespace Bespoke.Sph.Messaging
{
    public partial  class MessagingAction
    {

        public string OutboundMap { get; set; }
        public string Adapter { get; set; }
        public string Operation { get; set; }
        public string Table { get; set; }
        public string Schema { get; set; }
        public string Crud { get; set; }
        public int? Retry { get; set; }
        /// <summary>
        /// Interval in miliseconds
        /// </summary>
        public long RetryInterval { get; set; }
        /// <summary>
        /// the multiplies of miliseconds, second = 1000, minute = 60 000 , hour 3 600 000, day =3600000*24
        /// </summary>
        public long RetryIntervalTimeSpan { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public Type OutboundMapType
        {
            get
            {
                return Strings.GetType(this.OutboundMap);
            }
            set
            {
                this.OutboundMap = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }
        [XmlIgnore]
        [JsonIgnore]
        public Type AdapterType
        {
            get
            {
                var type = Strings.GetType(this.Adapter);
                if (null == type)
                {
                    var context = new SphDataContext();
                    var adapter = context.LoadOneFromSources<Adapter>(x => x.Name == this.Adapter || x.Id == this.Adapter);
                    type = Strings.GetType($"{adapter.CodeNamespace}.{adapter.Name}, {adapter.AssemblyName}");
                }
                return type;
            }
            set
            {
                this.Adapter = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

        public override bool UseAsync => true;

        public override bool UseCode => true;
    }
}
