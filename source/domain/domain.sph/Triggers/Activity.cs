using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Linq;
using Bespoke.Sph.Domain.Codes;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(CreateEntityActivity))]
    [XmlInclude(typeof(DecisionActivity))]
    [XmlInclude(typeof(DelayActivity))]
    [XmlInclude(typeof(DeleteEntityActivity))]
    [XmlInclude(typeof(EndActivity))]
    [XmlInclude(typeof(ExpressionActivity))]
    [XmlInclude(typeof(JoinActivity))]
    [XmlInclude(typeof(ListenActivity))]
    [XmlInclude(typeof(MappingActivity))]
    [XmlInclude(typeof(NotificationActivity))]
    [XmlInclude(typeof(ParallelActivity))]
    [XmlInclude(typeof(ReceiveActivity))]
    [XmlInclude(typeof(UpdateEntityActivity))]
    [XmlInclude(typeof(ScheduledTriggerActivity))]
    [XmlInclude(typeof(ScreenActivity))]
    [XmlInclude(typeof(SendActivity))]
    public partial class Activity : DomainObject
    {
        public virtual BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            const string PATTERN = "^[A-Za-z][A-Za-z0-9_ ]*$";
            var result = new BuildValidationResult();
            var message = string.Format("[{1}] \"{0}\" is not valid identifier", this.Name, this.GetType().Name);
            var validName = new Regex(PATTERN);
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = message });

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

        [JsonIgnore]
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
        public virtual IEnumerable<Class> GeneratedCustomTypeCode(WorkflowDefinition workflowDefinition)
        {
            return new Class[]{};
        }
        public virtual string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            throw new NotImplementedException();
        }
        public virtual string GeneratedInitiateAsyncCode(WorkflowDefinition wd)
        {
            throw new NotImplementedException();
        }

        public virtual Task<ActivityExecutionResult> ExecuteAsync()
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
        public string ExecutedCode { get; set; }
        public string ExecutingCode { get; set; }

        public virtual Task TerminateAsync(Workflow wf)
        {
            return Task.FromResult(0);
        }

        public virtual Bitmap GetPngIcon()
        {
            return null;
        }

        /// <summary>
        /// Default implementation just read from the resource
        /// </summary>
        /// <returns></returns>
        public virtual string GetEditorViewModel()
        {
            var name = this.GetType().Name.Replace("Activity", "").ToLowerInvariant();
            var manager = Properties.ActivityJsResources.ResourceManager;
            var resourceCulture = Properties.ActivityJsResources.Culture;
            return manager.GetString("activity_" + name, resourceCulture);
        }
        /// <summary>
        /// Default implementation just read from the resource
        /// </summary>
        /// <returns></returns>
        public virtual string GetEditorView()
        {
            var name = this.GetType().Name.Replace("Activity", "").ToLowerInvariant();
            var manager = Properties.ActivityHtmlResources.ResourceManager;
            var resourceCulture = Properties.ActivityHtmlResources.Culture;
            return manager.GetString("activity_" + name, resourceCulture);
        }
        /// <summary>
        /// The unique typename for each activity, should be overriden if you wish to have different name to avoid conflict
        /// </summary>
        public virtual string TypeName
        {
            get { return this.GetType().Name.Replace("Activity",""); }
        }
    }
}