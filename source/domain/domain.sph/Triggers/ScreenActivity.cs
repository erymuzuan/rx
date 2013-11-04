using System;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ScreenActivity : Activity
    {
        public override string GeneratedCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendFormat("public partial class Workflow_{0}_{1}Controller : System.Web.Mvc.Controller", wd.WorkflowDefinitionId, wd.Version);
            code.AppendLine("   {");
            code.AppendLine("       public async Task<System.Web.Mvc.ActionResult> " + this.ActionName + "(int id = 0)");
            code.AppendLine("       {");

            //
            /* code.AppendLine("               return Content(\"test test\");");
                * */
            code.AppendLine("try{");
            code.AppendFormatLine("              var vm = new {0}();", this.ViewModelType);
            code.AppendLine("           var context = new SphDataContext();");


            code.AppendFormatLine("           var wf = id == 0 ? new  {0}() :( await context.LoadOneAsync<Workflow>(w => w.WorkflowId == id));", wd.WorkflowTypeName);
            code.AppendFormatLine("           var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.WorkflowDefinitionId == {0});", wd.WorkflowDefinitionId);
            code.AppendFormatLine("           var screen = wd.ActivityCollection.OfType<ScreenActivity>().SingleOrDefault(s => s.WebId == \"{0}\");", this.WebId);

            code.AppendFormatLine("           if(!screen.IsInitiator && id == 0) throw new ArgumentException(\"id cannot be zero for none initiator\");");



            code.AppendFormatLine("         vm.Screen  = screen;");
            code.AppendFormatLine("         vm.Instance  = wf as {0};", wd.WorkflowTypeName);
            code.AppendFormatLine("         vm.WorkflowDefinition  = wd;");



           // code.AppendLine("               return View(vm);");
            code.AppendLine("               return Content(wf.ToXmlString());");
            //this.ToXmlString()
            code.AppendLine("}");// end try
            code.AppendLine("catch(Exception exc){return Content(exc.ToString());}");

            code.AppendLine("       }");// end action

            code.AppendLine("   }");// end controller

            // viewmodel
            code.AppendLine("   public class " + ViewModelType);
            code.AppendLine("   {");
            code.AppendFormatLine("     public {0} Instance {{get;set;}}", wd.WorkflowTypeName);
            code.AppendFormatLine("     public WorkflowDefinition WorkflowDefinition {{get;set;}}");
            code.AppendFormatLine("     public ScreenActivity Screen {{get;set;}}");
            code.AppendLine("   }");


            return code.ToString();
        }

        public string ViewModelType
        {
            get
            {
                return String.Format(this.Title.Replace(" ", string.Empty) + "ViewModel");
            }
        }

        public string GetView(WorkflowDefinition wd)
        {

            var code = new StringBuilder();
            code.AppendFormatLine("@inherits System.Web.Mvc.WebViewPage<{0}>", this.ViewModelType);
            code.AppendLine("@using System.Web.Mvc.Html");
            code.AppendLine("@using Bespoke.Sph.Domain");
            code.AppendLine("@model " + wd.CodeNamespace + "." + this.ViewModelType);

            code.AppendFormat(@"
@{{
    ViewBag.Title = Model.WorkflowDefinition.Name;
    Layout = ""~/Views/Shared/_Layout.cshtml"";
}}

<div class=""row"">
    <h1>@Model.Screen.Title</h1>
</div>
<div class=""row"">
    <form class=""form-horizontal"" id=""workflow-start-form"">
        @foreach (var fe in Model.Screen.FormDesign.FormElementCollection)
        {{
            fe.Path = fe.Path.ConvertJavascriptObjectToFunction();

            @Html.EditorFor(f => fe)
        }}
        <div class=""form-group"" >
            <label class=""control-label col-lg-2""></label>
            <div class=""col-lg-2 col-lg-offset-8"">
                <button id=""save-button"" type=""submit"" class=""btn btn-default"">Save</button>
            </div>
        </div>
    </form>

</div>


@section scripts
{{
    <script type=""text/javascript"">
        require(['services/datacontext', 'jquery'], function(context) {{
           var vm = {{
                $type : ""Bespoke.Sph.Domain.ScreenActivityViewModel,custom.workflow"",
                id : @Model.WorkflowDefinition.WorkflowDefinitionId,
                @foreach (var v in Model.WorkflowDefinition.VariableDefinitionCollection)
                {{
                    <text>
                @v.Name : @Html.Raw(v.GetEmptyJson(Model.WorkflowDefinition)),

                </text>
                }}

                isBusy : ko.observable()
            }};
            var ovm = context.toObservable(vm);
            ko.applyBindings(ovm);

            $('#save-button').click(function(e) {{
                e.preventDefault();
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(ovm);
                context.post(data, ""/Workflow/StartWorkflow"")
                    .then(function(result) {{
                        tcs.resolve(result);
                    }});
                return tcs.promise();
            }});

        }});

    </script>
}}");


            return code.ToString();
        }

        [JsonIgnore]
        [XmlIgnore]
        public string ActionName
        {
            get
            {
                return this.Title.Replace(" ", string.Empty);
            }
        }
    }
}