using System;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(EmailAction))]
    [XmlInclude(typeof(SetterAction))]
    [XmlInclude(typeof(AssemblyAction))]
    [XmlInclude(typeof(StartWorkflowAction))]
    public partial class CustomAction : DomainObject
    {
        public virtual void Execute(RuleContext context)
        {
            throw new Exception("NotImplemented");
        }
        public virtual Task ExecuteAsync(RuleContext context)
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
