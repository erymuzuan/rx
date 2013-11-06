using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ScreenActivity : Activity
    {
        public override bool IsAsync
        {
            get { return true; }
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       this.State = \"Ready\";");
            // set the next activity
            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", wd.GetNextActivity(this.WebId));/* webid*/
            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            //code.AppendLine("   result.NextActivity = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLine("       return result;");
            code.AppendLine("   }");

            return code.ToString();
        }

        public override string GeneratedCustomTypeCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendLinf("public partial class Workflow_{0}_{1}Controller : System.Web.Mvc.Controller", wd.WorkflowDefinitionId, wd.Version);
            code.AppendLine("{");
            code.AppendLine("       public async Task<System.Web.Mvc.ActionResult> " + this.ActionName + "(int id = 0)");
            code.AppendLine("       {");

            code.AppendLine("           try{");
            code.AppendLinf("               var vm = new {0}();", this.ViewModelType);
            code.AppendLine("               var context = new SphDataContext();");


            code.AppendLinf("               var wf = id == 0 ? new  {0}() :( await context.LoadOneAsync<Workflow>(w => w.WorkflowId == id));", wd.WorkflowTypeName);
            code.AppendLinf("               var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.WorkflowDefinitionId == {0});", wd.WorkflowDefinitionId);
            code.AppendLinf("               var screen = wd.ActivityCollection.OfType<ScreenActivity>().SingleOrDefault(s => s.WebId == \"{0}\");", this.WebId);

            code.AppendLinf("               if(!screen.IsInitiator && id == 0) throw new ArgumentException(\"id cannot be zero for none initiator\");");



            code.AppendLinf("               vm.Screen  = screen;");
            code.AppendLinf("               vm.Instance  = wf as {0};", wd.WorkflowTypeName);
            code.AppendLinf("               vm.WorkflowDefinition  = wd;");
            code.AppendLinf("               vm.Controller  = this.GetType().Name;");
            code.AppendLinf("               vm.SaveAction  = \"Save{0}\";",this.ActionName);
            code.AppendLinf("               vm.Namespace  = \"{0}\";",wd.CodeNamespace);
            code.AppendLinf("               ");
            code.AppendLine("               return View(vm);");

            code.AppendLine("           }");// end try
            code.AppendLine("           catch(Exception exc){return Content(exc.ToString());}");

            code.AppendLine("       }");// end GET action
            code.AppendLine();


            code.AppendLine("       [System.Web.Mvc.HttpPost]");
            code.AppendLine("       public async Task<System.Web.Mvc.ActionResult> Save" + this.ActionName + "()");
            code.AppendLine("       {");

            code.AppendLinf("           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<{0}>(this);", wd.WorkflowTypeName);// this is extension method

            code.AppendLinf("           var result = await wf.{0}();", this.MethodName);

            // any business rules?


            /*   
            code.AppendLine("           var context = new SphDataContext();");
            code.AppendLine("           using(var session = context.OpenSession())");
            code.AppendLine("           {");
            code.AppendLine("               session.Attach(wf);");
            code.AppendLinf("               await session.SubmitChanges(\"{0}\");",this.WebId);
            code.AppendLine("           }");
            */
            code.AppendLine("           return Json(new {sucess = true, status = \"OK\", result = result});");
            code.AppendLine("       }"); // end SAVE action


            code.AppendLine("   }");// end controller

            // viewmodel
            code.AppendLine("   public class " + ViewModelType);
            code.AppendLine("   {");
            code.AppendLinf("       public {0} Instance {{get;set;}}", wd.WorkflowTypeName);
            code.AppendLinf("       public WorkflowDefinition WorkflowDefinition {{get;set;}}");
            code.AppendLinf("       public ScreenActivity Screen {{get;set;}}");
            code.AppendLinf("       public string Controller {{get;set;}}");
            code.AppendLinf("       public string Namespace {{get;set;}}");
            code.AppendLinf("       public string SaveAction {{get;set;}}");
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
           // code.AppendLinf("@inherits System.Web.Mvc.WebViewPage<{0}>", this.ViewModelType);
            code.AppendLine("@using System.Web.Mvc.Html");
            code.AppendLine("@using Bespoke.Sph.Domain");
            code.AppendLine("@using Newtonsoft.Json");
            code.AppendLine("@model " + wd.CodeNamespace + "." + this.ViewModelType);

            code.AppendFormat(@"
@{{
    ViewBag.Title = Model.WorkflowDefinition.Name;
    Layout = ""~/Views/Shared/_Layout.cshtml"";
    const string controllerString = ""Controller"";
    var setting = new JsonSerializerSettings {{TypeNameHandling = TypeNameHandling.All}};
    
}}

<div class=""row"">
    <h1>@Model.Screen.Title</h1>
</div>
<div class=""row"">
    <form class=""form-horizontal"" id=""workflow-start-form"">
        <!-- ko with :instance -->
        @foreach (var fe in Model.Screen.FormDesign.FormElementCollection)
        {{
            fe.Path = fe.Path.ConvertJavascriptObjectToFunction();

            @Html.EditorFor(f => fe)
        }}
        <!-- /ko -->
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

            
           var instance =context.toObservable(@Html.Raw(JsonConvert.SerializeObject(Model.Instance, setting)),/@Model.Namespace.Replace(""."",""\\."")\.(.*?),/),
               screen = context.toObservable(@Html.Raw(JsonConvert.SerializeObject(Model.Screen, setting)),/@Model.Namespace.Replace(""."",""\\."")\.(.*?),/),
               vm = {{
                id : @Model.WorkflowDefinition.WorkflowDefinitionId,
                instance : ko.observable(instance),    
                screen : ko.observable(screen),
                isBusy : ko.observable()
            }};
            ko.applyBindings(vm);

            $('#save-button').click(function(e) {{
                e.preventDefault();
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.instance);
                context.post(data, ""/@Model.Controller.Replace(controllerString, string.Empty)/@Model.SaveAction"")
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

        public override Task<ActivityExecutionResult> ExecuteAsync()
        {
            return null;
        }

       
    }
}