using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    [XmlInclude(typeof(EmailAction))]
    [XmlInclude(typeof(SetterAction))]
    public abstract class CustomAction : DomainObject
    {
        public abstract void Execute(Entity item);
        public abstract Task ExecuteAsync(Entity item);
        public abstract bool UseAsync { get; }

        [XmlAttribute]
        public int CustomActionId { get; set; }
        [XmlAttribute]
        public int TriggerId { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Note { get; set; }
        [XmlAttribute]
        public bool IsActive{ get; set; }
    }
}
