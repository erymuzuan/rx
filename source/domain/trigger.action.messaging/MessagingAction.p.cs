using System;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Messaging
{
    public partial  class MessagingAction
    {

        public string OutboundMap { get; set; }
        public string Adapter { get; set; }
        public string Operation { get; set; }
        public string Table { get; set; }
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
                return Type.GetType(this.OutboundMap);
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
                return Type.GetType(this.Adapter);
            }
            set
            {
                this.Adapter = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

        public override bool UseAsync
        {
            get { return true; }
        }

        public override bool UseCode
        {
            get { return true; }
        }
    }
}
