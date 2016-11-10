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
    public partial class Functoid : DomainObject, IComparable<Functoid>
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
            return $"// NOT IMPLEMENTED => {this.GetType().Name}";
        }

        public virtual Mono.Cecil.TypeReference GetOutputType()
        {
            return null;
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
                         Message = $"[{a.Name}] Functoid is null -{this.GetType().Name}"
                     };
            errors.AddRange(nf);
            var vfTasks = from a in this.ArgumentCollection
                          where !string.IsNullOrWhiteSpace(a.Functoid)
                          let fnt = a.GetFunctoid(this.TransformDefinition)
                          where null != fnt
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

        protected IList<string> GetDependentFunctoids(string id)
        {
            var list = new List<string>();
            var functoid = this.TransformDefinition.FunctoidCollection.SingleOrDefault(f => f.WebId == id);
            if (null == functoid) return list;
            foreach (var xid in functoid.ArgumentCollection.Select(a => a.Functoid))
            {
                list.Add(xid);
                var child = GetDependentFunctoids(xid);
                list.AddRange(child);
            }

            return list;
        }
        public virtual bool? DependsOn(Functoid y)
        {
            if (this.WebId == y.WebId) return null;

            var type = y.GetType();
            var dependsOnMethodInfo = type.GetMethod(nameof(DependsOn));
            if (dependsOnMethodInfo.DeclaringType != typeof(Functoid))
            {
                return !y.DependsOn(this);
            }

            var args = this.GetDependentFunctoids(this.WebId);
            var yargs = this.GetDependentFunctoids(y.WebId);

            if (args.Contains(y.WebId)) return true;
            if (yargs.Contains(this.WebId)) return false;

            return null;

        }

        public int CompareTo(Functoid other)
        {
            var depends = this.DependsOn(other);
            if (null == depends) return 0;
            return depends.Value ? 1 : -1;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} - {this.WebId}";
        }
    }
}