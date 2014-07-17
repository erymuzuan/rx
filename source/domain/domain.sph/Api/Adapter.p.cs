﻿using System.Xml.Serialization;

namespace Bespoke.Sph.Domain.Api
{
    public abstract partial class Adapter :Entity 
    {
        public AdapterTable[] Tables { get; set; }
        public string Schema { get; set; }

        public virtual string CodeNamespace
        {
            get { return string.Format("{0}.Adapters.{1}", ConfigurationManager.ApplicationName, this.Schema); }
        }

        private readonly ObjectCollection<OperationDefinition> m_operationDefinitionsCollection = new ObjectCollection<OperationDefinition>();

        public ObjectCollection<OperationDefinition> OperationDefinitionCollection
        {
            get { return m_operationDefinitionsCollection; }
        }


        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Description { get; set; }


        public abstract string OdataTranslator { get; }
        [XmlAttribute]
        public int AdapterId { get; set; }

        public override int GetId()
        {
            return AdapterId;
        }

        public override void SetId(int id)
        {
            AdapterId = id;
        }
    }
}
