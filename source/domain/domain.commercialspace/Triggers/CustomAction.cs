using System;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    [XmlInclude(typeof(EmailAction))]
    [XmlInclude(typeof(SetterAction))]
    public partial class CustomAction : DomainObject
    {
        public virtual void Execute(Entity item)
        {
            throw new Exception("NotImplemented");
        }
        public virtual Task ExecuteAsync(Entity item)
        {
            throw new Exception("NotImplemented");
        }
        public virtual bool UseAsync
        {
            get
            {
                throw new Exception("NotImplemented");
            }
        }
    }
}
