using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class StoreAsSourceAttribute : Attribute
    {
        public bool IsElasticsearch { get; set; }
        public bool IsSqlDatabase { get; set; }
    }
    public abstract class Entity : DomainObject
    {
        [XmlAttribute]
        public string CreatedBy { get; set; }
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public DateTime CreatedDate { get; set; }

        [XmlAttribute]
        public string ChangedBy { get; set; }

        [XmlAttribute]
        public DateTime ChangedDate { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public bool IsNewItem => string.IsNullOrWhiteSpace(this.Id) || this.Id == "0";

        public virtual Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            return Task.FromResult(new List<ValidationError>().AsEnumerable());
        }

        public Type GetEntityType()
        {
            var item = this;
            var type = item.GetType();
            var attr = type.GetCustomAttribute<EntityTypeAttribute>();
            if (null != attr) return attr.Type;
            return type;
        }

        

        public ValidationResult ValidateBusinessRule(IEnumerable<BusinessRule> businessRules)
        {
            var result = new ValidationResult();
            var valids = businessRules.Select(r => r.Evaluate(this)).ToList();
            var errors = valids.SelectMany(v => v.ValidationErrors);

            result.Success = valids.All(v => v.Success);
            result.ValidationErrors.AddRange(errors);

            return result;
        }
    }
}
