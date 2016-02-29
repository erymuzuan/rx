using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    
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
                    Message = $"[{this.GetType().Name}] \"{this.Name}\" Missing webid "
                });
            if (wd.ActivityCollection.Count(a => a.WebId == this.WebId) > 1)
                result.Errors.Add(new BuildError(this.WebId)
                {
                    Message = $"[{this.GetType().Name}] \"{this.Name}\" Duplicate webid "
                });

            return result;
        }

        [JsonIgnore]
        public string MethodName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Name)) throw new InvalidOperationException("Name is empty for [" + this.GetType().Name + "]");
                var name = this.Name.Dehumanize().Replace(" ", string.Empty);
                return $"{name}Async";
            }
        }
        public virtual IEnumerable<Class> GeneratedCustomTypeCode(WorkflowDefinition workflowDefinition)
        {
            return new Class[] { };
        }
        public virtual string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            throw new NotImplementedException();
        }
        public virtual string GenerateInitAsyncMethod(WorkflowDefinition wd)
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

        public Class GenerateWorkflowPartial(WorkflowDefinition wd)
        {
            if (!this.OtherMethodCollection.Any()) return null;


            var actPartial = new Class
            {
                IsPartial = true,
                Name = wd.WorkflowTypeName,
                Namespace = wd.CodeNamespace,
                FileName = wd.WorkflowTypeName + "." + this.MethodName.Replace("Async", "") + ".cs"
            };
            actPartial.ImportCollection.Add(typeof(Entity).Namespace);
            actPartial.ImportCollection.Add(typeof(int).Namespace);
            actPartial.ImportCollection.Add(typeof(Task<>).Namespace);
            actPartial.ImportCollection.Add(typeof(Enumerable).Namespace);
            actPartial.ImportCollection.Add(typeof(XmlAttributeAttribute).Namespace);
            actPartial.MethodCollection.AddRange(this.OtherMethodCollection);

            return actPartial;
        }

        /// <summary>
        /// Flags to say that this activity listen to event
        /// </summary>
        public virtual bool IsAsync => false;

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
        public virtual string TypeName => this.GetType().Name.Replace("Activity", "");

        public ObjectCollection<Method> OtherMethodCollection { get; } = new ObjectCollection<Method>();

        protected Method AddMethod(StringBuilder code)
        {
            var method = new Method {Code = code.ToString()};
            this.OtherMethodCollection.Add(method);
            return method;
        }
    }
}