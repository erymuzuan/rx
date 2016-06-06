﻿using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    public abstract partial class Adapter : Entity
    {
        [JsonIgnore]
        public string RoutePrefix => $"api/{Id}/{Schema.ToIdFormat()}";
        public AdapterTable[] Tables { get; set; }
        public string Schema { get; set; }
        [JsonIgnore]
        public virtual string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.Adapters.{this.Name.ToPascalCase()}.{this.Schema.ToPascalCase()}";
        [JsonIgnore]
        public virtual string AssemblyName => $"{ConfigurationManager.ApplicationName}.{Name}";
        public ObjectCollection<TableDefinition> TableDefinitionCollection { get; } = new ObjectCollection<TableDefinition>();
        public virtual ObjectCollection<OperationDefinition> OperationDefinitionCollection { get; } = new ObjectCollection<OperationDefinition>();

        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Description { get; set; }


        public abstract string OdataTranslator { get; }


    }
}
