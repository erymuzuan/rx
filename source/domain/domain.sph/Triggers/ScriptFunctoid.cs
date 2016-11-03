using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "C# code", Description = "C# custom code, should provide return type and return statement for the output targer", FontAwesomeIcon = "stumbleupon", Category = FunctoidCategory.COMMON)]
    public partial class ScriptFunctoid : Functoid
    {
        public override bool Initialize()
        {
            return true;
        }

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

            // make sure all args are valid C# identifier
            foreach (var arg in this.ArgumentCollection)
            {
                var kind = SyntaxFacts.GetKeywordKind(arg.Name);
                if (kind.ToString().EndsWith("Keyword"))
                    errors.Add($"arg.{arg.Name}", $"{arg.Name} is a C# reserved keyword and cannot be used as argument name", this.WebId);
                var valid = SyntaxFacts.IsValidIdentifier(arg.Name);
                if (!valid)
                    errors.Add($"arg.{arg.Name}", $"{arg.Name} is not a valid C# identifier for argument name", this.WebId);
            }
            return errors;
        }

        public override string GenerateStatementCode()
        {
            var block = this.Expression;
            if (!block.Contains("return")) return string.Empty;

            var code = new StringBuilder();
            code.AppendLine();
            code.AppendLine();

            var asyncLambda = CodeExpression.Load(this.Expression).HasAsyncAwait ? "async " : "";
            var funcGenericArgs = this.ArgumentCollection.Select(x => x.Type.ToCSharp()).ToList();
            funcGenericArgs.Add(this.OutputType.ToCSharp());

            code.Append("Func<");
            code.JoinAndAppend(funcGenericArgs, ", ");
            code.Append($"> {Name} = {asyncLambda}(");
            code.JoinAndAppend(this.ArgumentCollection, ",", x => x.Name);
            code.Append(") =>");
            code.AppendLine("{");
            code.AppendLine(this.Expression);
            code.AppendLine("};");

            return code.ToString();



        }

        public override string GenerateAssignmentCode()
        {
            if (string.IsNullOrWhiteSpace(this.Name)) throw new InvalidOperationException("Name cannot be empty");
            var block = this.Expression;
            if (!block.Contains("return")) return this.Expression;

            var asyncLambda = CodeExpression.Load(this.Expression).HasAsyncAwait;
            var code = new StringBuilder();
            if (asyncLambda)
                code.Append("await ");
            code.Append($"{Name}(");
            code.JoinAndAppend(this.ArgumentCollection, ", ", x => this[x.Name].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode());

            code.AppendLine(")");

            return code.ToString();

        }

        public override string GetEditorViewModel()
        {
            //language=javascript
            return @"
define(['services/datacontext', 'services/logger', 'plugins/dialog', objectbuilders.system],
    function (context, logger, dialog, system) {
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
                    
            },
            addArg = function(){
                var arg = new bespoke.sph.domain.FunctoidArg(system.guid());
                functoid().ArgumentCollection.push(arg);
            },
            removeArg = function(arg){
                return function(){
                    functoid().ArgumentCollection.remove(arg);
                };
            };
            var vm = {
                functoid: functoid,
                edit: edit,
                okClick: okClick,
                cancelClick: cancelClick,
                addArg : addArg,
                removeArg : removeArg
                };
            return vm;
});";
        }

        public override string GetEditorView()
        {
            //language=html
            var html = @"
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
                                <option value=""System.Double, mscorlib"">Double</option>
                                <option value=""System.Single, mscorlib"">Single</option>
                                <option value=""System.Boolean, mscorlib"">Boolean</option>
                                <option value=""System.Nullable`1[[System.DateTime, mscorlib]], mscorlib"">Nullable DateTime</option>
                                <option value=""System.Nullable`1[[System.Int32, mscorlib]], mscorlib"">Nullable Integer</option>
                                <option value=""System.Nullable`1[[System.Double, mscorlib]], mscorlib"">Nullable Double</option>
                                <option value=""System.Nullable`1[[System.Single, mscorlib]], mscorlib"">Nullable Single</option>
                                <option value=""System.Nullable`1[[System.Decimal, mscorlib]], mscorlib"">Nullable Decimal</option>
                                <option value=""System.Nullable`1[[System.Boolean, mscorlib]], mscorlib"">Nullable Boolean</option>
                            </select>
                        </div>
                    </div>
                    <div class=""form-group"">
                        <label for=""args"" class=""col-lg-2 control-label"">Args</label>
                        <div class=""col-lg-9"">
                            <table class=""table table-striped"">
                                <thead>
                                    <tr>
                                        <th>Name</th>
                                        <th>Type</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody data-bind=""foreach : ArgumentCollection"">
                                    <tr>
                                        <td>
                                	        <input class=""form-control"" data-bind=""value: Name"" pattern=""^[a-z_@][A-Za-z0-9_]*$"" title=""valid C# identifier, with this pattern ^[a-z_@][A-Za-z0-9_]*$"" required />
                                        </td>
                                        <td>
                                            <select required class=""form-control"" data-bind=""value: TypeName"">
                                                <option value=""System.String, mscorlib"">String</option>
                                                <option value=""System.DateTime, mscorlib"">DateTime</option>
                                                <option value=""System.Int32, mscorlib"">Integer</option>
                                                <option value=""System.Decimal, mscorlib"">Decimal</option>
                                                <option value=""System.Double, mscorlib"">Double</option>
                                                <option value=""System.Single, mscorlib"">Single</option>
                                                <option value=""System.Boolean, mscorlib"">Boolean</option>
                                                <option value=""System.Nullable`1[[System.DateTime, mscorlib]], mscorlib"">Nullable DateTime</option>
                                                <option value=""System.Nullable`1[[System.Int32, mscorlib]], mscorlib"">Nullable Integer</option>
                                                <option value=""System.Nullable`1[[System.Double, mscorlib]], mscorlib"">Nullable Double</option>
                                                <option value=""System.Nullable`1[[System.Single, mscorlib]], mscorlib"">Nullable Single</option>
                                                <option value=""System.Nullable`1[[System.Decimal, mscorlib]], mscorlib"">Nullable Decimal</option>
                                                <option value=""System.Nullable`1[[System.Boolean, mscorlib]], mscorlib"">Nullable Boolean</option> 
                                            </select>
                                        </td>
                                        <td>
                                            <a href=""javascript:;"" data-bind=""click: $root.removeArg.call($parent, $data)"" title=""Remove the argument"">
                                                <i class=""fa fa-trash-o""></i>
                                            </a>
                                        </td>
                               
                                    </tr>
                                </tbody>
                            </table>
                            <a class=""btn btn-link"" href=""javascript:;"" data-bind=""click : $root.addArg"">
                                <i class=""fa fa-plus-circle""></i>
                                Add an argument
                            </a>
                        
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

            return html;
        }


    }
}