using System;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ServiceContractSetting : DomainObject {}

    public partial class ResourceEndpointSetting : DomainObject { }
    public partial class SearchEndpointSetting : DomainObject { }
    public partial class OdataEndpointSetting : DomainObject { }

    public partial class CachingSetting : DomainObject
    {
        [JsonIgnore]
        public string Etag { get; set; }
        [JsonIgnore]
        public bool IsEtagWeak { get; set; }
    }

}