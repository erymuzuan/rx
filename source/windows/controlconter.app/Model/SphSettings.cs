using System.Xml.Serialization;
using Bespoke.Sph.ControlCenter.Helpers;

namespace Bespoke.Sph.ControlCenter.Model
{
    [XmlType("SphSettings", Namespace = Strings.DefaultNamespace)]
    public class SphSettings : DomainObject
    {
        public string ApplicationName { get; set; }
        public string SqlLocalDbName { get; set; }
        public string IisExpressDirectory { get; set; }
        public string SphDirectory { get; set; }
        public string RabbitMqDirectory { get; set; }
        public string JavaHome { get; set; }
        public string ElasticSearchHome { get; set; }
    }
}
