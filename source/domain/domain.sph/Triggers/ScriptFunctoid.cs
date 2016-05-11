using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "C# code", Description = "C# custom code, should provide return type and return statement for the output targer", FontAwesomeIcon = "stumbleupon", Category = FunctoidCategory.COMMON)]
    public partial class ScriptFunctoid : Functoid
    {

        [XmlIgnore]
        [JsonIgnore]
        public Type OutputType
        {
            get
            {
                return Strings.GetType(this.OutputTypeName);
            }
            set
            {
                this.OutputTypeName = value.GetShortAssemblyQualifiedName();
            }
        }
        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = (await base.ValidateAsync()).ToList();
            if (string.IsNullOrWhiteSpace(this.Name))
                errors.Add("Name", "Script's name cannot be empty", this.WebId);
            return errors;
        }

        public override string GenerateStatementCode()
        {
            var block = this.Expression;
            if (!block.Contains("return")) return string.Empty;

            var code = new StringBuilder();
            code.AppendLine();
            code.AppendLine();

            var asyncLambda = CodeExpression.Load(this.Expression).HasAsyncAwait;
            code.AppendLine(asyncLambda
                ? $"               Func<{{SOURCE_TYPE}}, Task<{this.OutputType.ToCSharp()}>> {Name} = async (d) =>"
                : $"               Func<{{SOURCE_TYPE}}, {this.OutputType.ToCSharp()}> {Name} = d =>");

            code.AppendLine("                                           {");
            code.AppendLine("                                               " + this.Expression);
            code.AppendLine("                                           };");
            return code.ToString();
        }





        public override string GenerateAssignmentCode()
        {
            if (string.IsNullOrWhiteSpace(this.Name)) throw new InvalidOperationException("Name cannot be empty");
            var block = this.Expression;
            if (!block.Contains("return")) return this.Expression;

            var asyncLambda = CodeExpression.Load(this.Expression).HasAsyncAwait;
            return asyncLambda ? $"await {this.Name}(item)" : $"{this.Name}(item)";
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

                <form class=""form-horizontal"">
                    <div class=""form-group"">
                        <label for=""script-functoid-name"" class=""col-lg-2 control-label"">Name</label>
                        <div class=""col-lg-9"">
                            <input required class=""form-control"" data-bind=""value: Name"" pattern=""^[A-Za-z_][A-Za-z0-9_]*$"" id=""script-functoid-name"" type=""text"" name=""script-functoid-name"" />
                        </div>
                    </div>
                    <div class=""form-group"">
                        <label for=""constant-field-typename"" class=""col-lg-2 control-label"">Type</label>
                        <div class=""col-lg-9"">
                            <select required class=""form-control"" id=""constant-field-typename"" name=""constant-field-typename"" data-bind=""value: OutputTypeName"">
                                <option value=""System.String, mscorlib"">String</option>
                                <option value=""System.DateTime, mscorlib"">DateTime</option>
                                <option value=""System.Int32, mscorlib"">Integer</option>
                                <option value=""System.Decimal, mscorlib"">Decimal</option>
                                <option value=""System.Boolean, mscorlib"">Boolean</option>
                                <option value=""System.Nullable`1[[System.DateTime, mscorlib]], mscorlib"">DateTime Nullable</option>
                                <option value=""System.Nullable`1[[System.Int32, mscorlib]], mscorlib"">Integer Nullable</option>
                                <option value=""System.Nullable`1[[System.Decimal, mscorlib]], mscorlib"">Decimal Nullable</option>
                                <option value=""System.Nullable`1[[System.Boolean, mscorlib]], mscorlib"">Boolean Nullable</option>
                            </select>
                        </div>
                    </div>
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