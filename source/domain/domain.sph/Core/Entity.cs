using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(Setting))]
    [XmlInclude(typeof(Building))]
    [XmlInclude(typeof(RentalApplication))]
    [XmlInclude(typeof(Maintenance))]
    [XmlInclude(typeof(Space))]
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



        public virtual Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            return Task.FromResult(new List<ValidationError>().AsEnumerable());
        }



        private Type GetEntityType(Entity item)
        {
            var type = item.GetType();
            var attr = type.GetCustomAttribute<EntityTypeAttribute>();
            if (null != attr) return attr.Type;
            return type;
        }
        public int GetId()
        {
            var type = this.GetEntityType(this);
            var id = type.GetProperties().AsQueryable().Single(p => p.PropertyType == typeof(int)
                                                                    && p.Name == type.Name + "Id");
            return (int)id.GetValue(this);
        }
    }

    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
    }
}
