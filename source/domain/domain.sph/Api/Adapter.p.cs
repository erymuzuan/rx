using System.Xml.Serialization;

namespace Bespoke.Sph.Domain.Api
{
    public abstract partial class Adapter
    {
        public string[] Tables { get; set; }
        public string Schema { get; set; }

        public virtual string CodeNamespace
        {
            get { return string.Format("{0}.Adapters.{1}", ConfigurationManager.ApplicationName, this.Schema); }
        }


        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Description { get; set; }

        [XmlAttribute]
        public string WebId { get; set; }

    }
}
