﻿using System.Xml.Serialization;

namespace Bespoke.Sph.Domain.Api
{
    public abstract partial class Adapter : Entity
    {
        public AdapterTable[] Tables { get; set; }
        public string Schema { get; set; }
        public virtual string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.Adapters.{this.Name.ToPascalCase()}.{this.Schema.ToPascalCase()}";
        public ObjectCollection<TableDefinition> TableDefinitionCollection { get; } = new ObjectCollection<TableDefinition>();
        public ObjectCollection<OperationDefinition> OperationDefinitionCollection { get; } = new ObjectCollection<OperationDefinition>();

        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Description { get; set; }


        public abstract string OdataTranslator { get; }


    }
}
