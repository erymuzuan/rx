﻿using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Linq;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(ScreenActivity))]
    [XmlInclude(typeof(DecisionActivity))]
    [XmlInclude(typeof(NotificationActivity))]
    [XmlInclude(typeof(EndActivity))]
    [XmlInclude(typeof(MappingActivity))]
    [XmlInclude(typeof(CreateEntityActivity))]
    [XmlInclude(typeof(UpdateEntityActivity))]
    [XmlInclude(typeof(DeleteEntityActivity))]
    [XmlInclude(typeof(ExpressionActivity))]
    [XmlInclude(typeof(ParallelActivity))]
    [XmlInclude(typeof(JoinActivity))]
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
        public virtual string GeneratedCustomTypeCode(WorkflowDefinition workflowDefinition)
        {
            return string.Empty;
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



        public virtual string GetEditorViewModel()
        {

            return @"
define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {
        var functoid = ko.observable(),
            okClick = function (data, ev) {
                dialog.close(this, 'OK');

            },
            cancelClick = function () {
                dialog.close(this, 'Cancel');
            };
            var vm = {
                functoid: functoid,
                okClick: okClick,
                cancelClick: cancelClick
                };
            return vm;
});";
        }

        public virtual Bitmap GetPngIcon()
        {
            return null;
        }
        public virtual string GetEditorView()
        {
            return @"
<section class=""view-model-modal"" id=""functoid-editor-dialog"">
    <div class=""modal-dialog"">
        <div class=""modal-content"">

            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal""
                        data-bind=""click : cancelClick"">&times;</button>
                <h3>Functoid Properties Editor</h3>
            </div>
            <div class=""modal-body"" data-bind=""with:functoid"">

             <h4>No editor is provided for 
                <!-- ko text:Name -->
                <!--/ko -->
              </h4>

            </div>
            <div class=""modal-footer"">
                <a href=""#"" class=""btn btn-default"" data-dismiss=""modal"" data-bind=""click : cancelClick"">Cancel</a>
            </div>
        </div>
    </div>
</section>
";
        }
    }
}