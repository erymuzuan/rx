using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Linq;
using Humanizer;

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
    [XmlInclude(typeof(ScheduledTriggerActivity))]
    public partial class Activity : DomainObject
    {
        public virtual BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            const string pattern = "^[A-Za-z][A-Za-z0-9_ ]*$";
            var result = new BuildValidationResult();
            var message = string.Format("[{1}] \"{0}\" is not valid identifier", this.Name, this.GetType().Name);
            var validName = new Regex(pattern);
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = message});

            if (string.IsNullOrWhiteSpace(this.WebId))
                result.Errors.Add(new BuildError(this.WebId)
                {
                    Message = string.Format("[{0}] \"{1}\" Missing webid ", this.GetType().Name, this.Name)
                });
            if (wd.ActivityCollection.Count(a => a.WebId == this.WebId) > 1)
                result.Errors.Add(new BuildError(this.WebId)
                {
                    Message = string.Format("[{0}] \"{1}\" Duplicate webid ", this.GetType().Name, this.Name)
                });

            return result;
        }

        public string MethodName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Name)) throw new InvalidOperationException("Name is empty for [" + this.GetType().Name + "]");
                var length = this.WebId.Length > 4 ? 4 : this.WebId.Length;
                var unique = this.WebId.Replace("-", "_").Substring(0, length);
                string name = this.Name.Dehumanize().Replace(" ", string.Empty);
                return string.Format("Exec{0}{1}_{2}Async", this.GetType().Name, name, unique);
            }
        }
        public virtual string GeneratedCustomTypeCode(WorkflowDefinition workflowDefinition)
        {
            return string.Empty;
        }
        public virtual string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            throw new NotImplementedException();
        }

        public virtual Task<ActivityExecutionResult> ExecuteAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task InitiateAsync(Workflow wf)
        {
            throw new NotImplementedException();
        }
        public virtual Task CancelAsync(Workflow wf)
        {
            throw new NotImplementedException();
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

        public virtual void BeforeGenerate(WorkflowDefinition wd)
        {

        }
        public string AfterExcuteCode { get; set; }
        public string BeforeExcuteCode { get; set; }

        public virtual Task TerminateAsync(Workflow wf)
        {
            return Task.FromResult(0);
        }
    }
}