using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    [XmlInclude(typeof(EmailAction))]
    [XmlInclude(typeof(SetterAction))]
    public abstract partial class CustomAction : DomainObject
    {
        public abstract void Execute(Entity item);
        public abstract Task ExecuteAsync(Entity item);
        public abstract bool UseAsync { get; }
    }
}
