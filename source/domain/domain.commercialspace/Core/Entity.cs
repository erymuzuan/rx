using System;
using System.Linq.Expressions;
using System.Xml.Serialization;

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
    [XmlInclude(typeof(Message))]
    [XmlInclude(typeof(ReportDefinition))]
    [XmlInclude(typeof(ReportDelivery))]
    [XmlInclude(typeof(ReportContent))]
    public abstract class Entity : DomainObject
    {
        [XmlAttribute]
        public string CreatedBy { get; set; }
       
        [XmlAttribute]
        public DateTime CreatedDate { get; set; }

        [XmlAttribute]
        public string ChangedBy { get; set; }

        [XmlAttribute]
        public DateTime ChangedDate { get; set; }


    }
}
