using System.Xml.Serialization;

namespace Bespoke.Sph.Domain.Api
{
    public class AdapterTable
    {
        public string Name { get; set; }
        public string[] Parents { get; set; }
        public string[] Children { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public abstract partial class Adapter
    {
        public AdapterTable[] Tables { get; set; }
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
