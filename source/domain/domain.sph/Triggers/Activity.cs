using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(ScreenActivity))]
    public partial class Activity : DomainObject
    {
        public virtual string GeneratedCode(WorkflowDefinition workflowDefinition)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<ActivityExecutionResult> ExecuteAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}