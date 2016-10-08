using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
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
        private static readonly ConcurrentDictionary<Type, Type> m_entityTypeLookup = new ConcurrentDictionary<Type, Type>();
        public Type GetEntityType()
        {
            var type = this.GetType();
            if (m_entityTypeLookup.ContainsKey(type))
                return m_entityTypeLookup[type];

            var attr = type.GetCustomAttribute<EntityTypeAttribute>();
            if (null == attr) return type;
            
            m_entityTypeLookup.TryAdd(type, attr.Type);
            return attr.Type;
        }

        public PersistenceOptionAttribute GetPersistenceOption()
        {
            return PersistenceOptionAttribute.GetAttribute(this.GetType());
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

        public Task<ValidationResult> ValidateBusinessRuleAsync(IEnumerable<BusinessRule> businessRules)
        {
            throw new NotImplementedException("Not yet..");
        }
    }
}
