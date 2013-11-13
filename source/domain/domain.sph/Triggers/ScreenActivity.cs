using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ScreenActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var errors = from f in this.FormDesign.FormElementCollection
                where string.IsNullOrWhiteSpace(f.Path)
                select new BuildError
                {
                    Message = string.Format("{0} does not have path", f.Label)
                };
            var result = new BuildValidationResult();
            result.Errors.AddRange(errors);

            return base.ValidateBuild(wd);
        }

        public async override Task InitiateAsync(Workflow wf)
        {
            var baseUrl = ConfigurationManager.AppSettings["sph:BaseUrl"] ?? "http://localhost:4436";
            var imb = this.InvitationMessageBody ?? "= @Model.Screen.Name task is assigned to you go here @string.Format(\"" +
                baseUrl+
                      "/Workflow_{0}_{1}/{2}/{3}\",@Model.Item.WorkflowDefinitionId, @Model.Item.Version,\"" + this.ActionName + "\", @Model.Item.WorkflowId)";
            var ims = this.InvitationMessageSubject ?? "= [Sph] @Model.Screen.Name  task is assigned to you";

            var users = new List<string>();
            var context = new SphDataContext();
            var ad = ObjectBuilder.GetObject<IDirectoryService>();


            var model = new { Screen = this, Item = wf };

            switch (this.Performer.UserProperty)
            {
                case "Username":
                    users.Add(this.Performer.Value);
                    break;
                case "Department":
                    var list = await context.GetListAsync<UserProfile, string>(
                        u => u.Department == this.Performer.Value,
                        u => u.Username);
                    users.AddRange(list);
                    break;
                case "Designation":
                    var list2 = await context.GetListAsync<UserProfile, string>(
                        u => u.Designation == this.Performer.Value,
                        u => u.Username);
                    users.AddRange(list2);
                    break;
                case "Roles":
                    var list3 = await ad.GetUserInRolesAsync(this.Performer.Value);
                    users.AddRange(list3);
                    break;
                default:
                    throw new Exception("Whoaaa we cannot send invitation to " + this.Performer.UserProperty + " for " + this.Name);

            }

            foreach (var user in users)
            {
                string user1 = user;
                var profile = await context.LoadOneAsync<UserProfile>(p => p.Username == user1);
                var subject = await this.TransformTemplateAsync(ims, model);
                var body = await this.TransformTemplateAsync(imb, model);


                var message = new Message { Subject = subject, UserName = user, Body = body };
                using (var session = context.OpenSession())
                {
                    session.Attach(message);
                    await session.SubmitChanges("Initiate " + this.Name);
                }

                var ns = ObjectBuilder.GetObject<INotificationService>();
                await ns.SendMessageAsync(message, profile.Email);
            }

        }

        public override bool IsAsync
        {
            get { return true; }
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);

            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       this.State = \"Ready\";");
            // set the next activity
            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", this.NextActivityWebId);/* webid*/
            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            //code.AppendLine("   result.NextActivity = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLine("       return result;");
            code.AppendLine("   }");

            return code.ToString();
        }

        public Task<string> GenerateCustomXsdJavascriptClassAsync(WorkflowDefinition wd)
        {
            var script = new StringBuilder();
            //var store = ObjectBuilder.GetObject<IBinaryStore>();
            //var doc = await store.GetContentAsync(wd.SchemaStoreId);
            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var xsd = wd.GetCustomSchema();

            var elements = xsd.Elements(x + "element").ToList();
            var customSchemaCode = elements.Select(wd.GenerateXsdElementJavascript).ToList();
            customSchemaCode.ForEach(c => script.AppendLine(c));


            return Task.FromResult(script.ToString());
        }

        public override string GeneratedCustomTypeCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            var controller = string.Format("Workflow_{0}_{1}", wd.WorkflowDefinitionId, wd.Version);
            code.AppendLinf("public partial class {0}Controller : System.Web.Mvc.Controller", controller);
            code.AppendLine("{");

            // custom schema
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Schemas{0}()", this.ActionName);
            code.AppendLine("       {");
            code.AppendLine("           var store = ObjectBuilder.GetObject<IBinaryStore>();");
            code.AppendLinf("           var doc = await store.GetContentAsync(\"wd.{0}.{1}\");", wd.WorkflowDefinitionId, wd.Version);
            code.AppendLine(@"          WorkflowDefinition wd;
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wd = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }

                                        ");
            code.AppendLinf("           var screen = wd.ActivityCollection.Single(w =>w.WebId ==\"{0}\") as ScreenActivity;", this.WebId);
            code.AppendLinf("           var script =await  screen.GenerateCustomXsdJavascriptClassAsync(wd);", this.WebId);
            code.AppendLine("           this.Response.ContentType = \"application/javascript\";");

            code.AppendLine("           return Content(script);");
            code.AppendLine("       }");

            // GET Action
            code.AppendLine("       public async Task<System.Web.Mvc.ActionResult> " + this.ActionName + "(int id = 0)");
            code.AppendLine("       {");

            code.AppendLine("           try{");
            code.AppendLinf("               var vm = new {0}();", this.ViewModelType);
            code.AppendLine("               var context = new SphDataContext();");


            code.AppendLinf("               var wf = id == 0 ? new  {0}() :( await context.LoadOneAsync<Workflow>(w => w.WorkflowId == id));", wd.WorkflowTypeName);
            code.AppendLinf("               var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.WorkflowDefinitionId == {0});", wd.WorkflowDefinitionId);
            code.AppendLinf("               var profile = await context.LoadOneAsync<UserProfile>(u => u.Username == User.Identity.Name);");
            code.AppendLinf("               var screen = wd.ActivityCollection.OfType<ScreenActivity>().SingleOrDefault(s => s.WebId == \"{0}\");", this.WebId);
            code.AppendLinf("               if(!screen.IsInitiator && id == 0) throw new ArgumentException(\"id cannot be zero for none initiator\");");


            code.AppendLinf("               vm.Screen  = screen;");
            code.AppendLinf("               vm.Instance  = wf as {0};", wd.WorkflowTypeName);
            code.AppendLinf("               vm.WorkflowDefinition  = wd;");
            code.AppendLinf("               vm.Controller  = this.GetType().Name;");
            code.AppendLinf("               vm.SaveAction  = \"Save{0}\";", this.ActionName);
            code.AppendLinf("               vm.Namespace  = \"{0}\";", wd.CodeNamespace);
            code.AppendLine("               var canview = screen.Performer.IsPublic;");
            code.AppendLine("               if(!screen.Performer.IsPublic)");
            code.AppendLine("               {");
            code.AppendLine("                   switch (screen.Performer.UserProperty)");
            code.AppendLine("                   { ");
            code.AppendLine("                       case \"Username\":");
            code.AppendLine("                           canview = screen.Performer.Value == profile.Username;");
            code.AppendLine("                           break;");
            code.AppendLine("                       case \"Department\":");
            code.AppendLine("                           canview = screen.Performer.Value == profile.Department;");
            code.AppendLine("                           break;");
            code.AppendLine("                       case \"Designation\":");
            code.AppendLine("                           canview = screen.Performer.Value == profile.Designation;");
            code.AppendLine("                           break;");
            code.AppendLine("                       case \"Roles\":");
            code.AppendLine("                           canview = profile.RoleTypes.Contains(screen.Performer.Value);");
            code.AppendLine("                           break;");
            code.AppendLine("                       default:");
            code.AppendLine("                           canview = false;");
            code.AppendLine("                           break;");
            code.AppendLine("                   } ");
            code.AppendLine("               }");

            code.AppendLine("               if(canview) return View(vm);");
            code.AppendLine("               return new System.Web.Mvc.HttpUnauthorizedResult();");

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
            code.AppendLine("           return Json(new {sucess = true, status = \"OK\", result = result,wf});");
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
                return String.Format(this.Name.Replace(" ", string.Empty) + "ViewModel");
            }
        }

        public string GetView(WorkflowDefinition wd)
        {

            var controller = string.Format("Workflow_{0}_{1}", wd.WorkflowDefinitionId, wd.Version);
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
    var confirmationText = Model.Screen.ConfirmationOptions.Value;
    
}}

<div class=""row"">
    <h1>@Model.Screen.Title</h1>
</div>
<div class=""row"">
    <form class=""form-horizontal"" id=""workflow-start-form"" data-bind=""with: instance"">
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
    <script type=""text/javascript"" src=""/{0}/Schemas{1}"">
    <script type=""text/javascript"">
        require(['services/datacontext', 'jquery','durandal/app'], function(context,jquery,app) {{

            
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
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(vm.instance),
                    button = $(this);

                button.prop('disabled', true);
                context.post(data, ""/@Model.Controller.Replace(controllerString, string.Empty)/@Model.SaveAction"")
                    .then(function(result) {{
                        tcs.resolve(result);
                        @if(Model.Screen.ConfirmationOptions.Type == ""Message"")
                        {{
                            <text>
                            var msg = _.template('@Html.Raw(confirmationText)')(result.wf);
                            
                            console.log(msg);
                            alert(msg);
                            </text>
                        }}else
                        {{
                            <text>
                            window.location = ""@confirmationText"";
                            </text>
                        }}
                       

                    }});
                return tcs.promise();
            }});

        }});

    </script>
}}",controller,this.ActionName);


            return code.ToString();
        }

        [JsonIgnore]
        [XmlIgnore]
        public string ActionName
        {
            get
            {
                return this.Name.Replace(" ", string.Empty);
            }
        }

        public override Task<ActivityExecutionResult> ExecuteAsync()
        {
            return null;
        }


        private async Task<string> TransformTemplateAsync(string template, object model)
        {
            if (string.IsNullOrWhiteSpace(template)) return string.Empty;
            if (template.StartsWith("="))
            {
                var engine = ObjectBuilder.GetObject<ITemplateEngine>();
                var razor = template.Substring(1, template.Length - 1);
                return await engine.GenerateAsync(razor, model);
            }
            return template;
        }

    }
}