using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{

    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Constant", BootstrapIcon = "font", Category = FunctoidCategory.COMMON)]
    public partial class ConstantFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            if (typeof (string) == this.Type)
                return "\"" + Value + "\"";
            if (typeof(double) == this.Type)
                return string.Format("{0}d", Value);
            if (typeof(decimal) == this.Type)
                return string.Format("{0}m", Value);

            return string.Format("{0}", Value) ;
        }

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = (await base.ValidateAsync()).ToList();
            if(string.IsNullOrWhiteSpace(this.TypeName))
                errors.Add(this.GetType().Name, "TypeName is not specified", this.WebId);
            if(!string.IsNullOrWhiteSpace(this.TypeName) && null == this.Type)
                errors.Add(this.GetType().Name, "TypeName is not recognized : " + this.TypeName, this.WebId);

            return errors;
        }

        [XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }

        
        public override string GetEditorViewModel()
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

        public override  string GetEditorView()
        {
            return @"
<section class=""view-model-modal"" id=""constant-functoid-editor-dialog"">
    <div class=""modal-dialog"">
        <div class=""modal-content"">

            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal""
                        data-bind=""click : cancelClick"">&times;</button>
                <h3>Functoid Properties Editor</h3>
            </div>
            <div class=""modal-body"" data-bind=""with:functoid"">

                <form class=""form-horizontal"">
                    <div class=""form-group"">
                        <label for=""constant-field-value"" class=""col-lg-2 control-label"">Value</label>
                        <div class=""col-lg-9"">
                            <input required="""" class=""form-control"" data-bind=""value: Value"" id=""constant-field-value"" type=""text"" name=""constant-field-value"" />
                        </div>
                    </div>
                  
                    <div class=""form-group"">
                        <label for=""constant-field-typename"" class=""col-lg-2 control-label"">Type</label>
                        <div class=""col-lg-9"">
                            <select required class=""form-control"" id=""constant-field-typename"" name=""constant-field-typename"" data-bind=""value: TypeName"">
                                <option value=""System.String, mscorlib"">String</option>
                                <option value=""System.DateTime"">DateTime</option>
                                <option value=""System.Int32, mscorlib"">Integer</option>
                                <option value=""System.Decimal"">Decimal</option>
                                <option value=""System.Boolean, mscorlib"">Boolean</option>
                            </select>
                        </div>
                    </div>
                    

                </form>

            </div>
            <div class=""modal-footer"">
                <input  data-dismiss=""modal"" type=""submit"" class=""btn btn-default""
                       value=""OK"" data-bind=""click: okClick""/>
                <a href=""#"" class=""btn btn-default"" data-dismiss=""modal"" data-bind=""click : cancelClick"">Cancel</a>
            </div>
        </div>
    </div>
</section>
";
        }
    

    
    }
}