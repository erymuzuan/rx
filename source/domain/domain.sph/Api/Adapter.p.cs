using System.Xml.Serialization;

namespace Bespoke.Sph.Domain.Api
{
    public abstract partial class Adapter 
    {
        public AdapterTable[] Tables { get; set; }
        public string Schema { get; set; }
        public override string DefaultNamespace
        {
            get { return string.Format("{0}.Adapters.{1}.{2}", ConfigurationManager.ApplicationName, this.Schema, this.Name); }
        }


        private readonly ObjectCollection<OperationDefinition> m_operationDefinitionsCollection = new ObjectCollection<OperationDefinition>();
        private readonly ObjectCollection<TableDefinition> m_tableDefinitionCollection = new ObjectCollection<TableDefinition>();

        public ObjectCollection<TableDefinition> TableDefinitionCollection
        {
            get { return m_tableDefinitionCollection; }
        }
        public ObjectCollection<OperationDefinition> OperationDefinitionCollection
        {
            get { return m_operationDefinitionsCollection; }
        }
        
        [XmlAttribute]
        public string Description { get; set; }


        public abstract string OdataTranslator { get; }


    }
}
