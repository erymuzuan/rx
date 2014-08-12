using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("WebId = {WebId}, Type={GetType().Name}")]
    public partial class Functoid : DomainObject
    {
        public const string DESIGNER_CONTRACT = "FunctoidDesigner";
        [JsonIgnore]
        [XmlIgnore]
        public TransformDefinition TransformDefinition { get; set; }
        public virtual bool Initialize()
        {
            return true;
        }
        [JsonIgnore]
        [XmlIgnore]
        public int Index { get; set; }

        public virtual string GenerateStatementCode()
        {
            return string.Empty;
        }
        public virtual string GenerateAssignmentCode()
        {
            return string.Format("// NOT IMPLEMENTED => {0}", this.GetType().Name);
        }


        public new FunctoidArg this[string index]
        {
            get { return this.ArgumentCollection.SingleOrDefault(x => x.Name == index); }

        }

        public override bool Validate()
        {
            throw new Exception("Not implemented, use ValidateAsync");
        }

        public virtual void RemoveInvalidArgument()
        {
            this.ArgumentCollection.RemoveAll(x => x.Functoid == null);
        }
        public virtual async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = new List<ValidationError>();
            var nf = from a in this.ArgumentCollection
                     where string.IsNullOrWhiteSpace(a.Functoid)
                     && !a.IsOptional
                     select new ValidationError
                     {
                         PropertyName = a.Name,
                         ErrorLocation = this.WebId,
                         Message = string.Format("[{0}] Functoid is null -{1}", a.Name, this.GetType().Name)
                     };
            errors.AddRange(nf);
            var vfTasks = from a in this.ArgumentCollection
                          where !string.IsNullOrWhiteSpace(a.Functoid)
                          let fnt = a.GetFunctoid(this.TransformDefinition)
                          select fnt.ValidateAsync();

            var vf = (await Task.WhenAll(vfTasks)).SelectMany(x => x.ToArray());
            errors.AddRange(vf);

            return errors;
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