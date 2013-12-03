﻿using System;
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
                         where f.IsPathIsRequired
                             && string.IsNullOrWhiteSpace(f.Path) && (f.Name != "HTML Section")

                         select new BuildError
                         (
                             this.WebId,
                             string.Format("[ScreenActivity] : {0} => '{1}' does not have path", this.Name, f.Label)
                         );
            var result = base.ValidateBuild(wd);
            result.Errors.AddRange(errors);

            return result;
        }

        public async override Task CancelAsync(Workflow wf)
        {
            var baseUrl = ConfigurationManager.AppSettings["sph:BaseUrl"] ?? "http://localhost:4436";
            var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}", baseUrl, wf.WorkflowDefinitionId, wf.Version, this.ActionName, wf.WorkflowId);
            var cmb = this.CancelMessageBody ?? "@Model.Screen.Name task assigned to has been cancelled";
            var cms = this.CancelMessageSubject ?? "[Sph] @Model.Screen.Name  task is cancelled";

            // TODO : Activity is now cancelled, should not be made available anymore

            await SendNotificationToPerformers(wf, baseUrl, url, cms, cmb);
        }

        public async override Task InitiateAsync(Workflow wf)
        {
            var baseUrl = ConfigurationManager.AppSettings["sph:BaseUrl"] ?? "http://localhost:4436";
            var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}", baseUrl, wf.WorkflowDefinitionId, wf.Version, this.ActionName, wf.WorkflowId);
            var imb = this.InvitationMessageBody ?? "@Model.Screen.Name task is assigned to you go here @Model.Url";
            var ims = this.InvitationMessageSubject ?? "[Sph] @Model.Screen.Name task is assigned to you";

            await SendNotificationToPerformers(wf, baseUrl, url, ims, imb);
        }

        private async Task SendNotificationToPerformers(Workflow wf, string baseUrl, string url, string subjectTemplate, string bodyTemplate)
        {
            var context = new SphDataContext();


            var model = new { Screen = this, Item = wf, BaseUrl = baseUrl, Url = url };
            var users = await GetUsersAsync(wf);

            foreach (var user in users)
            {
                string user1 = user;
                var profile = await context.LoadOneAsync<UserProfile>(p => p.Username == user1);
                var subject = await this.TransformTemplateAsync(subjectTemplate, model);
                var body = await this.TransformTemplateAsync(bodyTemplate, model);

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

        public async Task<string[]> GetUsersAsync(Workflow wf)
        {
            var script = ObjectBuilder.GetObject<IScriptEngine>();
            var context = new SphDataContext();
            var ad = ObjectBuilder.GetObject<IDirectoryService>();

            var unwrapValue = this.Performer.Value;
            if (!string.IsNullOrWhiteSpace(unwrapValue) && unwrapValue.StartsWith("="))
                unwrapValue = script.Evaluate<string, Workflow>(unwrapValue.Remove(0, 1), wf);

            var users = new List<string>();
            switch (this.Performer.UserProperty)
            {
                case "Username":
                    users.Add(unwrapValue);
                    break;
                case "Department":
                    var list = await context.GetListAsync<UserProfile, string>(
                        u => u.Department == unwrapValue,
                        u => u.Username);
                    users.AddRange(list);
                    break;
                case "Designation":
                    var list2 = await context.GetListAsync<UserProfile, string>(
                        u => u.Designation == unwrapValue,
                        u => u.Username);
                    users.AddRange(list2);
                    break;
                case "Roles":
                    var list3 = await ad.GetUserInRolesAsync(unwrapValue);
                    users.AddRange(list3);
                    break;
                default:
                    throw new Exception("Whoaaa we cannot send invitation to " + this.Performer.UserProperty + " for " +
                                        this.Name);
            }
            return users.ToArray();
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
            code.AppendLine(this.BeforeExcuteCode);
            code.AppendLine("       this.State = \"Ready\";");
            // set the next activity
            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", this.NextActivityWebId);
            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");

            code.AppendLine(this.AfterExcuteCode);
            code.AppendLine("       return result;");
            code.AppendLine("   }");

            return code.ToString();
        }

        public Task<string> GenerateCustomXsdJavascriptClassAsync(WorkflowDefinition wd)
        {
            var script = new StringBuilder();
            script.AppendLine("var bespoke = bespoke ||{};");
            script.AppendLine("bespoke.sph = bespoke.sph ||{};");
            script.AppendLinf("bespoke.sph.w_{0}_{1} = bespoke.sph.w_{0}_{1} ||{{}};", wd.WorkflowDefinitionId, wd.Version);

            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var xsd = wd.GetCustomSchema();

            var complexTypesElement = xsd.Elements(x + "complexType").ToList();
            var complexTypeClasses = complexTypesElement.Select(wd.GenerateXsdComplexTypeJavascript).ToList();
            complexTypeClasses.ForEach(c => script.AppendLine(c));

            var elements = xsd.Elements(x + "element").ToList();
            var elementClasses = elements.Select(e => wd.GenerateXsdElementJavascript(e, 0, s => complexTypesElement.Single(f => f.Attribute("name").Value == s))).ToList();
            elementClasses.ForEach(c => script.AppendLine(c));



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
            code.AppendLinf("           var script = await screen.GenerateCustomXsdJavascriptClassAsync(wd);", this.WebId);
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
            code.AppendLinf("               var screen = wd.GetActivity<ScreenActivity>(\"{0}\");", this.WebId);
            code.AppendLinf("               if(!screen.IsInitiator && id == 0) throw new ArgumentException(\"id cannot be zero for none initiator\");");

            // tracker
            code.AppendLinf("               var tracker = await wf.GetTrackerAsync();");
            code.AppendLinf("               if(!tracker.CanExecute(\"{0}\"))", this.WebId);
            code.AppendLine("               {");
            code.AppendLine("                   return RedirectToAction(\"InvalidState\",\"Workflow\");");
            code.AppendLine("               }");



            code.AppendLinf("               vm.Screen  = screen;");
            code.AppendLinf("               vm.Instance  = wf as {0};", wd.WorkflowTypeName);
            code.AppendLinf("               vm.WorkflowDefinition  = wd;");
            code.AppendLinf("               vm.Controller  = this.GetType().Name;");
            code.AppendLinf("               vm.SaveAction  = \"Save{0}\";", this.ActionName);
            code.AppendLinf("               vm.Namespace  = \"{0}\";", wd.CodeNamespace);
            code.AppendLine("               var canview = screen.Performer.IsPublic;");
            code.AppendLine("               if(!screen.Performer.IsPublic)");
            code.AppendLine("               {");
            code.AppendLine("                   var users = await screen.GetUsersAsync(wf);");
            code.AppendLine("                   canView = this.User.Identity.IsAuthenticated && users.Contains(this.User.Identity.Name);");
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
            code.AppendLine(@"          var store = ObjectBuilder.GetObject<IBinaryStore>();
                                        var doc = await store.GetContentAsync(string.Format(""wd.{0}.{1}"", wf.WorkflowDefinitionId, wf.Version));
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }  ");
            code.AppendLinf("           var result = await wf.{0}();", this.MethodName);
            // any business rules?            
            code.AppendLine("           this.Response.ContentType = \"application/javascript\";");
            code.AppendLine("           var retVal = new {sucess = true, status = \"OK\", result = result,wf};");
            code.AppendLine("           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));");
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
    <script type=""text/javascript"" src=""/{0}/Schemas{1}""></script>
    <script type=""text/javascript"">
        require(['services/datacontext', 'jquery','services/app', 'services/system'], function(context,jquery,app, system) {{

            
           var instance =context.toObservable(@Html.Raw(JsonConvert.SerializeObject(Model.Instance, setting)),/@Model.Namespace.Replace(""."",""\\."")\.(.*?),/),
               screen = context.toObservable(@Html.Raw(JsonConvert.SerializeObject(Model.Screen, setting)),/@Model.Namespace.Replace(""."",""\\."")\.(.*?),/),
               vm = {{
                id : @Model.WorkflowDefinition.WorkflowDefinitionId,
                instance : ko.observable(instance),    
                screen : ko.observable(screen),
                isBusy : ko.observable()
            }};
            ko.applyBindings(vm, document.getElementById('body'));
            @*  the div#body is defined in _Layout.cshtml, if you use different Layout then this has got to changed accordingly *@

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
                            app.showMessage(msg, '@Model.Screen.Name', ['OK'])
                                .done(function(dr){{
                                    console.log(dr);
                                }});
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
}}", controller, this.ActionName);


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
                var script = ObjectBuilder.GetObject<IScriptEngine>();
                return script.Evaluate<string, object>(template.Remove(0, 1), model);
            }
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, model);
        }

    }
}