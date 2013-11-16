using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(ScreenActivity))]
    [XmlInclude(typeof(DecisionActivity))]
    [XmlInclude(typeof(NotificationActivity))]
    [XmlInclude(typeof(EndActivity))]
    [XmlInclude(typeof(CreateEntityActivity))]
    [XmlInclude(typeof(UpdateEntityActivity))]
    [XmlInclude(typeof(DeleteEntityActivity))]
    [XmlInclude(typeof(ExpressionActivity))]
    [XmlInclude(typeof(ParallelActivity))]
    [XmlInclude(typeof(ListenActivity))]
    [XmlInclude(typeof(DelayActivity))]
    public partial class Activity : DomainObject
    {
        public virtual BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            const string pattern = "^[A-Za-z][A-Za-z0-9_ ]*$";
            var result = new BuildValidationResult();
            var message = string.Format("[Variable] \"{0}\" is not valid identifier", this.Name);
            var validName = new Regex(pattern);
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError { Message = message });


            return result;
        }

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