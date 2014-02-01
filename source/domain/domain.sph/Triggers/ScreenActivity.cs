using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var elements = from f in this.FormDesign.FormElementCollection
                           let err = f.ValidateBuild(wd, this)
                           where null != err
                           select err;

            var result = base.ValidateBuild(wd);
            result.Errors.AddRange(errors);
            result.Errors.AddRange(elements.SelectMany(v => v));

            if(!this.Performer.IsPublic && string.IsNullOrWhiteSpace(this.Performer.UserProperty))
                result.Errors.Add(new BuildError(this.WebId,
                             string.Format("[ScreenActivity] : {0} => does not have performer", this.Name)));

            return result;
        }

        public async override Task CancelAsync(Workflow wf)
        {
            var baseUrl = ConfigurationManager.BaseUrl;
            var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}", baseUrl, wf.WorkflowDefinitionId, wf.Version, this.ActionName, wf.WorkflowId);
            var cmb = this.CancelMessageBody ?? "@Model.Screen.Name task assigned to has been cancelled";
            var cms = this.CancelMessageSubject ?? "[Sph] @Model.Screen.Name  task is cancelled";
            
            await SendNotificationToPerformers(wf, baseUrl, url, cms, cmb);
            var tracker = await wf.GetTrackerAsync();

            tracker.CancelAsyncList(this.WebId);
            await tracker.SaveAsync();
        }

        public override string GeneratedInitiateAsyncCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendLinf("   public async Task<InitiateActivityResult> InitiateAsync{0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLinf("       var correlation = Guid.NewGuid().ToString();", this.WebId);
            code.AppendLinf("       var self = this.GetActivity<ScreenActivity>(\"{0}\");", this.WebId);
            code.AppendLine("       var baseUrl = ConfigurationManager.BaseUrl;");
            code.AppendLine("       var url = string.Format(\"{0}/Workflow_{1}_{2}/{3}/{4}?correlation={5}\", baseUrl, this.WorkflowDefinitionId, this.Version, self.ActionName, this.WorkflowId, correlation);");
            code.AppendLine("       var imb = self.InvitationMessageBody ?? \"@Model.Screen.Name task is assigned to you go here @Model.Url\";");
            code.AppendLine("       var ims = self.InvitationMessageSubject ?? \"[Sph] @Model.Screen.Name task is assigned to you\";");

            code.AppendLine("       await self.SendNotificationToPerformers(this, baseUrl, url, ims, imb);");
            code.AppendLine("       return new InitiateActivityResult{ Correlation = correlation };");
            code.AppendLine("   }");

            return code.ToString();
        }

        public async Task SendNotificationToPerformers(Workflow wf, string baseUrl, string url, string subjectTemplate, string bodyTemplate)
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
            code.AppendLine(this.ExecutingCode);
            code.AppendLine("       await Task.Delay(40);");
            code.AppendLine("       this.State = \"Ready\";");
            // set the next activity
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLinf("       result.NextActivities = new[]{{ \"{0}\"}};", this.NextActivityWebId);

            code.AppendLine(this.ExecutedCode);
            code.AppendLine("       return result;");
            code.AppendLine("   }");

            return code.ToString();
        }


        public override string GeneratedCustomTypeCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            var controller = string.Format("Workflow_{0}_{1}", wd.WorkflowDefinitionId, wd.Version);
            code.AppendLinf("public partial class {0}Controller : System.Web.Mvc.Controller", controller);
            code.AppendLine("{");


            // GET Action
            code.AppendLinf("//exec:{0}", this.WebId);
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> {0}({1})", this.ActionName, this.IsInitiator ? "" : "int id, string correlation");
            code.AppendLine("       {");


            code.AppendLine("           var context = new SphDataContext();");
            if (this.IsInitiator)
                code.AppendLinf("           var wf =  new  {0}();", wd.WorkflowTypeName);
            else
                code.AppendLinf("           var wf =await context.LoadOneAsync<Workflow>(w => w.WorkflowId == id);", wd.WorkflowTypeName);

            code.AppendLine("           await wf.LoadWorkflowDefinitionAsync();");
            code.AppendLinf("           var profile = await context.LoadOneAsync<UserProfile>(u => u.Username == User.Identity.Name);");
            code.AppendLinf("           var screen = wf.GetActivity<ScreenActivity>(\"{0}\");", this.WebId);

            code.AppendLinf("           var vm = new {0}(){{", this.ViewModelType);
            code.AppendLinf("                   Screen  = screen,");
            code.AppendLinf("                   Instance  = wf as {0},", wd.WorkflowTypeName);
            code.AppendLinf("                   Controller  = this.GetType().Name,");
            code.AppendLinf("                   SaveAction  = \"Save{0}\",", this.ActionName);
            code.AppendLinf("                   Namespace  = \"{0}\"", wd.CodeNamespace);
            code.AppendLinf("               }};", wd.CodeNamespace);

            if (!this.IsInitiator)
            {
                code.AppendLinf("           if(id == 0) throw new ArgumentException(\"id cannot be zero for none initiator\");");
                // tracker
                code.AppendLinf("           var tracker = await wf.GetTrackerAsync();");
                code.AppendLinf("           if(!tracker.CanExecute(\"{0}\", correlation ))", this.WebId);
                code.AppendLine("           {");
                code.AppendLine("               return RedirectToAction(\"InvalidState\",\"Workflow\");");
                code.AppendLine("           }");
                code.AppendLine("           vm.Correlation = correlation;");

            }


            code.AppendLine("           var canview = screen.Performer.IsPublic;");
            code.AppendLine("           if(!screen.Performer.IsPublic)");
            code.AppendLine("           {");
            code.AppendLine("               var users = await screen.GetUsersAsync(wf);");
            code.AppendLine("               canview = this.User.Identity.IsAuthenticated && users.Contains(this.User.Identity.Name);");
            code.AppendLine("           }");

            code.AppendLine("           if(canview) return View(vm);");
            code.AppendLine("           return new System.Web.Mvc.HttpUnauthorizedResult();");


            code.AppendLine("       }");// end GET action
            code.AppendLine();


            code.AppendLinf("//exec:{0}", this.WebId);
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
            code.AppendLinf("           var result = await wf.ExecuteAsync(\"{0}\");", this.WebId);
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
            code.AppendLine("       public WorkflowDefinition WorkflowDefinition {get;set;}");
            code.AppendLine("       public ScreenActivity Screen {get;set;}");
            code.AppendLine("       public string Controller {get;set;}");
            code.AppendLine("       public string Namespace {get;set;}");
            code.AppendLine("       public string SaveAction {get;set;}");
            code.AppendLine("       public string Correlation {get;set;}");
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

            // buttons
            var buttonCommands = this.FormDesign.FormElementCollection.OfType<Button>()
                .Where(b => b.CommandName != "save")
                .Select(b => string.Format("{0} : function(){{" +
                                           "{1}" +
                                           "}}", b.CommandName, b.Command))
                                           .ToArray();
            var buttonCommandJs = string.Join(",\r\n", buttonCommands);
            var saveCommand = string.Format(@",
                save : function(){{
                      var tcs = new $.Deferred(),
                            data = ko.mapping.toJSON(vm.instance),
                            button = $(this);

                        button.prop('disabled', true);
                        context.post(data, ""/workflow_{1}_{2}/Save{0}"")
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
                }}", this.ActionName, wd.WorkflowDefinitionId, wd.Version);
            if (buttonCommandJs.Length > 0)
                buttonCommandJs = saveCommand + "," + buttonCommandJs;
            else
                buttonCommandJs = saveCommand;

            code.AppendLine("@using System.Web.Mvc.Html");
            code.AppendLine("@using Bespoke.Sph.Domain");
            code.AppendLine("@using Newtonsoft.Json");
            code.AppendLine("@model " + wd.CodeNamespace + "." + this.ViewModelType);

            code.AppendFormat(@"
@{{
    ViewBag.Title = Model.Instance.Name;
    Layout = ""~/Views/Shared/_Layout.cshtml"";
    const string controllerString = ""Controller"";
    var setting = new JsonSerializerSettings {{TypeNameHandling = TypeNameHandling.All}};
    var confirmationText = Model.Screen.ConfirmationOptions.Value;
    
}}

<div class=""row"">
    <h1>@Model.Screen.FormDesign.Name</h1>
    <span>@Model.Screen.FormDesign.Description</span>
</div>
<div class=""row"">
    <form class=""form-horizontal"" id=""workflow-start-form"" data-bind=""with: instance"">
        @foreach (var fe in Model.Screen.FormDesign.FormElementCollection)
        {{
            fe.Path = fe.Path.ConvertJavascriptObjectToFunction();
            fe.SetDefaultLayout(Model.Screen.FormDesign);
            @Html.EditorFor(f => fe)
        }}
   
    </form>

</div>


@section scripts
{{
    <script type=""text/javascript"" src=""/{0}/Schemas""></script>
    <script type=""text/javascript"">
        require(['services/datacontext', 'jquery','services/app', 'services/system'], function(context,jquery,app, system) {{

            
           var instance = context.toObservable(@Html.Raw(JsonConvert.SerializeObject(Model.Instance, setting)),/@Model.Namespace.Replace(""."",""\\."")\.(.*?),/),
               screen = context.toObservable(@Html.Raw(JsonConvert.SerializeObject(Model.Screen, setting)),/@Model.Namespace.Replace(""."",""\\."")\.(.*?),/),
               vm = {{
                id : @Model.Instance.WorkflowDefinitionId,
                instance : ko.observable(instance),    
                screen : ko.observable(screen),
                isBusy : ko.observable(){3}
            }};
            
            instance.addChildItem = function(list, type){{
                        return function(){{
                            var item = bespoke.sph.w_{1}_{2}[type](system.guid());
                            list.push(item);
                        }}
                    }};
            
            instance.removeChildItem = function(list, obj){{
                        return function(){{
                            list.remove(obj);
                        }}
                    }};
            ko.applyBindings(vm, document.getElementById('body'));
            @*  the div#body is defined in _Layout.cshtml, if you use different Layout then this has got to changed accordingly *@

      
        }});

    </script>
}}", controller, wd.WorkflowDefinitionId, wd.Version, buttonCommandJs);


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