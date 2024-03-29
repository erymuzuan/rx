﻿using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ChartSeries : DomainObject
    {
        [XmlIgnore]
        [JsonIgnore]
        public decimal[] Values { get; set; }
    }
}