using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.SphCommercialSpaces.Domain
{
    [XmlInclude(typeof(Setting))]
    [XmlInclude(typeof(Building))]
    [XmlInclude(typeof(RentalApplication))]
    [XmlInclude(typeof(Maintenance))]
    [XmlInclude(typeof(CommercialSpace))]
    [XmlInclude(typeof(ComplaintTemplate))]
    [XmlInclude(typeof(Complaint))]
    [XmlInclude(typeof(UserProfile))]
    [XmlInclude(typeof(Tenant))]
    [XmlInclude(typeof(Contract))]
    [XmlInclude(typeof(ContractTemplate))]
    [XmlInclude(typeof(BinaryStore))]
    [XmlInclude(typeof(SpatialEntity))]
    [XmlInclude(typeof(Rebate))]
    [XmlInclude(typeof(Designation))]
    [XmlInclude(typeof(Inventory))]
    public abstract class Entity : DomainObject
    {

        [JsonIgnore]
        [XmlIgnore]
        public string CreatedBy { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public string ChangedBy { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public DateTime ChangedDate { get; set; }


    }
}
