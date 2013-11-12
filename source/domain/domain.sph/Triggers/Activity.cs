using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(ScreenActivity))]
    [XmlInclude(typeof(DecisionActivity))]
    [XmlInclude(typeof(NotificationActivity))]
    [XmlInclude(typeof(EndActivity))]
    [XmlInclude(typeof(CreateEntityActivity))]
    public partial class Activity : DomainObject
    {
        public string MethodName
        {
            get
            {
                return string.Format("Exec{0}{1}Async", this.GetType().Name, this.WebId.Replace("-", "_"));
            }
        }
        public virtual string GeneratedCustomTypeCode(WorkflowDefinition workflowDefinition)
        {
            return string.Empty;
        }
        public virtual string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<ActivityExecutionResult> ExecuteAsync()
        {
            throw new System.NotImplementedException();
        }
        public virtual Task InitiateAsync(Workflow wf)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Flags to say that this activity listen to event
        /// </summary>
        public virtual bool IsAsync
        {
            get
            {
                return false;
            }
        }
    }
}