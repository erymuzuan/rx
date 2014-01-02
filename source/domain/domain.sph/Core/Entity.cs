﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(Setting))]
    [XmlInclude(typeof(UserProfile))]
    [XmlInclude(typeof(BinaryStore))]
    [XmlInclude(typeof(SpatialEntity))]
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



        public Type GetEntityType()
        {
            var item = this;
            var type = item.GetType();
            var attr = type.GetCustomAttribute<EntityTypeAttribute>();
            if (null != attr) return attr.Type;
            return type;
        }

        public virtual int GetId()
        {
            var type = this.GetEntityType();
            var id = type.GetProperty(type.Name + "Id");
            return (int)id.GetValue(this);
        }


        public void SetId(int id)
        {
            var type = this.GetEntityType();
            var idp = type.GetProperties().AsQueryable().Single(p => p.PropertyType == typeof(int)
                                                                    && p.Name == type.Name + "Id");
            idp.SetValue(this, id);
        }

        public ValidationResult ValidateBusinessRule(IEnumerable<BusinessRule> businessRules)
        {
            var result = new ValidationResult();
            var context = new RuleContext(this);
            var valids = businessRules.Select(r => r.Evaluate(this)).ToList();
            var errors = valids.SelectMany(v => v.ValidationErrors);

            result.Success = valids.All(v => v.Success);
            result.ValidationErrors.AddRange(errors);

            return result;
        }
    }
}
