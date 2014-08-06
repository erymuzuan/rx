﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "C# code", FontAwesomeIcon = "code", Category = FunctoidCategory.Common)]
    public partial class ScriptFunctoid : Functoid
    {
        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = (await base.ValidateAsync()).ToList();
            if(string.IsNullOrWhiteSpace(this.Name))
                errors.Add("Name","Script's name cannot be empty", this.WebId);
            return errors;
        }

        public override string GeneratePreCode(FunctoidMap map)
        {
            var block = this.Expression;
            if (!block.Contains("return")) return string.Empty;
            
            var code = new StringBuilder();
            code.AppendLine();
            code.AppendLine();
            code.AppendLinf("               Func<{{SOURCE_TYPE}}, {1}> {0} = d =>", this.Name, map.DestinationType.FullName);
            code.AppendLine("                                           {");
            code.AppendLine("                                               " + this.Expression);
            code.AppendLine("                                           };");
            return code.ToString();
        }

        public override string GenerateCode()
        {
            if(string.IsNullOrWhiteSpace(this.Name))throw new InvalidOperationException("Name cannot be empty");
            var block = this.Expression;
            if (!block.EndsWith("return")) return this.Expression;

            return string.Format("{0}(item)", this.Name);
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
            },
            edit = function(){
               var w = window.open(""/sph/editor/ace"", '_blank', 'height=600px,width=600px,toolbar=0,location=0');
                        if (typeof w.window === ""object"") {

                            w.window.code = functoid().Expression();
                            w.window.saved = function (script) {
                                functoid().Expression(script);
                                w.close();
                            };
                        }
                        w.code = functoid().Expression();
                        w.saved = function (script) {
                            functoid().Expression(script);
                            w.close();
                        };
                    
            };
            var vm = {
                functoid: functoid,
                edit: edit,
                okClick: okClick,
                cancelClick: cancelClick
                };
            return vm;
});";
        }

        public override string GetEditorView()
        {
            return @"
<section class=""view-model-modal"" id=""script-functoid-editor-dialog"">
    <div class=""modal-dialog"">
        <div class=""modal-content"">

            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal""
                        data-bind=""click : cancelClick"">&times;</button>
                <h3>Functoid Properties Editor</h3>
            </div>
            <div class=""modal-body"" data-bind=""with:functoid"">

                <div class=""form-group"">
                        <label for=""script-functoid-name"" class=""col-lg-2 control-label"">Name</label>
                        <div class=""col-lg-9"">
                            <input required class=""form-control"" data-bind=""value: Name"" id=""script-functoid-name"" type=""text"" name=""script-functoid-name"" />
                        </div>
                 </div>
                <form class=""form-horizontal"">
                          <div class=""form-group"">
                        <label for=""function-field-script"" class=""col-sm-2 control-label"">Script</label>
                        <div class=""col-sm-8"">
                            <pre id=""function-field-script"" data-bind=""text:Expression""></pre>
                            <a class=""btn btn-default"" id=""script-help-buttton"">
                                <span class=""glyphicon glyphicon-question-sign""></span>
                            </a>
                            <a href=""#"" data-bind=""click : $root.edit"">Edit</a>
                            <div class=""hidden"" id=""script-help-content"">
                                Must be a strict C# code, you can use """"item"" member to access the current item, e.g. to return a Rental application status use <pre>return item.Status;</pre>

                                any C# construct is valid
                                <pre>
// return date = 1 Jan 2015
return new DateTime(2015,1,1);
</pre>
                            </div>
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