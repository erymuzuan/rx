using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "User interface", TypeName = "Screen", Description = "Creates a user interface activity")]
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

            if (!this.Performer.IsPublic && string.IsNullOrWhiteSpace(this.Performer.UserProperty))
                result.Errors.Add(new BuildError(this.WebId,
                             string.Format("[ScreenActivity] : {0} => does not have performer", this.Name)));
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                result.Errors.Add(new BuildError(this.WebId,
                             string.Format("[ScreenActivity] : {0} => does not the next activity defined", this.Name)));

            return result;
        }

        public async override Task CancelAsync(Workflow wf)
        {
            var baseUrl = ConfigurationManager.BaseUrl;
            var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}", baseUrl, wf.WorkflowDefinitionId, wf.Version, this.ActionName, wf.Id);
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
            code.AppendLine("       var url = string.Format(\"{0}/Workflow_{1}_{2}/{3}/{4}?correlation={5}\", baseUrl, this.WorkflowDefinitionId, this.Version, self.ActionName, this.Id, correlation);");
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
                var profile = await context.LoadOneAsync<UserProfile>(p => p.UserName == user1);
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
                case "UserName":
                    users.Add(unwrapValue);
                    break;
                case "Department":
                    var list = await context.GetListAsync<UserProfile, string>(
                        u => u.Department == unwrapValue,
                        u => u.UserName);
                    users.AddRange(list);
                    break;
                case "Designation":
                    var list2 = await context.GetListAsync<UserProfile, string>(
                        u => u.Designation == unwrapValue,
                        u => u.UserName);
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


        public override IEnumerable<Class> GeneratedCustomTypeCode(WorkflowDefinition wd)
        {
            var @classes = new List<Class>();

            // controller
            string name = this.Name.Dehumanize().Replace(" ", string.Empty);
            var controller = new Class
            {
                Name = string.Format("Workflow_{0}_{1}", wd.WorkflowTypeName, wd.Version),
                BaseClass = "Controller",
                FileName = wd.WorkflowTypeName + "Controller." + name + ".cs",
                Namespace = wd.CodeNamespace,
                IsPartial = true
            };
            controller.ImportCollection.Add("System.Web.Mvc");
            controller.ImportCollection.Add("System.Net.Http");
            controller.ImportCollection.Add(typeof(Exception).Namespace);
            controller.ImportCollection.Add(typeof(DomainObject).Namespace);
            controller.ImportCollection.Add(typeof(Task<>).Namespace);
            controller.ImportCollection.Add(typeof(Enumerable).Namespace);


            var getAction = new StringBuilder();
            // GET Action
            getAction.AppendLinf("//exec:{0}", this.WebId);
            getAction.AppendLinf("       public async Task<ActionResult> {0}({1})", this.ActionName, this.IsInitiator ? "" : "int id, string correlation");
            getAction.AppendLine("       {");


            getAction.AppendLine("           var context = new SphDataContext();");
            if (this.IsInitiator)
                getAction.AppendLinf("           var wf =  new  {0}();", wd.WorkflowTypeName);
            else
                getAction.AppendLinf("           var wf =await context.LoadOneAsync<Workflow>(w => w.WorkflowId == id);", wd.WorkflowTypeName);

            getAction.AppendLine("           await wf.LoadWorkflowDefinitionAsync();");
            getAction.AppendLinf("           var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == User.Identity.Name);");
            getAction.AppendLinf("           var screen = wf.GetActivity<ScreenActivity>(\"{0}\");", this.WebId);

            getAction.AppendLinf("           var vm = new {0}(){{", this.ViewModelType);
            getAction.AppendLinf("                   Screen  = screen,");
            getAction.AppendLinf("                   Instance  = wf as {0},", wd.WorkflowTypeName);
            getAction.AppendLinf("                   Controller  = this.GetType().Name,");
            getAction.AppendLinf("                   SaveAction  = \"Save{0}\",", this.ActionName);
            getAction.AppendLinf("                   Namespace  = \"{0}\"", wd.CodeNamespace);
            getAction.AppendLinf("               }};", wd.CodeNamespace);

            if (!this.IsInitiator)
            {
                getAction.AppendLinf("           if(id == 0) throw new ArgumentException(\"id cannot be zero for none initiator\");");
                // tracker
                getAction.AppendLinf("           var tracker = await wf.GetTrackerAsync();");
                getAction.AppendLinf("           if(!tracker.CanExecute(\"{0}\", correlation ))", this.WebId);
                getAction.AppendLine("           {");
                getAction.AppendLine("               return RedirectToAction(\"InvalidState\",\"Workflow\");");
                getAction.AppendLine("           }");
                getAction.AppendLine("           vm.Correlation = correlation;");

            }


            getAction.AppendLine("           var canview = screen.Performer.IsPublic;");
            getAction.AppendLine("           if(!screen.Performer.IsPublic)");
            getAction.AppendLine("           {");
            getAction.AppendLine("               var users = await screen.GetUsersAsync(wf);");
            getAction.AppendLine("               canview = this.User.Identity.IsAuthenticated && users.Contains(this.User.Identity.Name);");
            getAction.AppendLine("           }");

            getAction.AppendLine("           if(canview) return View(vm);");
            getAction.AppendLine("           return new System.Web.Mvc.HttpUnauthorizedResult();");


            getAction.AppendLine("       }");// end GET action
            getAction.AppendLine();
            controller.MethodCollection.Add(new Method { Code = getAction.ToString() });


            var saveAction = new StringBuilder();
            saveAction.AppendLinf("//exec:{0}", this.WebId);
            saveAction.AppendLine("       [System.Web.Mvc.HttpPost]");
            saveAction.AppendLine("       public async Task<System.Web.Mvc.ActionResult> Save" + this.ActionName + "()");
            saveAction.AppendLine("       {");

            saveAction.AppendLinf("           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<{0}>(this);", wd.WorkflowTypeName);// this is extension method
            saveAction.AppendLine(@"          var store = ObjectBuilder.GetObject<IBinaryStore>();
                                        var doc = await store.GetContentAsync(string.Format(""wd.{0}.{1}"", wf.WorkflowDefinitionId, wf.Version));
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }  ");
            saveAction.AppendLinf("           var result = await wf.ExecuteAsync(\"{0}\");", this.WebId);
            // any business rules?            
            saveAction.AppendLine("           this.Response.ContentType = \"application/javascript\";");
            saveAction.AppendLine("           var retVal = new {sucess = true, status = \"OK\", result = result,wf};");
            saveAction.AppendLine("           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));");
            saveAction.AppendLine("       }"); // end SAVE action

            controller.MethodCollection.Add(new Method { Code = saveAction.ToString() });

            var vm = new Class { Name = ViewModelType, Namespace = wd.CodeNamespace, FileName = ViewModelType + ".cs" };
            vm.AddNamespaceImport(typeof(ScreenActivity));

            vm.AddProperty("       public {0} Instance {{get;set;}}", wd.WorkflowTypeName);
            vm.AddProperty("       public WorkflowDefinition WorkflowDefinition {{get;set;}}");
            vm.AddProperty("       public ScreenActivity Screen {{get;set;}}");
            vm.AddProperty("       public string Controller {{get;set;}}");
            vm.AddProperty("       public string Namespace {{get;set;}}");
            vm.AddProperty("       public string SaveAction {{get;set;}}");
            vm.AddProperty("       public string Correlation {{get;set;}}");
            @classes.Add(vm);
            @classes.Add(controller);

            return @classes;
        }

        [JsonIgnore]
        public string ViewModelType
        {
            get
            {
                return String.Format(this.Name.Replace(" ", string.Empty) + "ViewModel");
            }
        }

        public string GetView(WorkflowDefinition wd)
        {

            var controller = string.Format("Workflow_{0}_{1}", wd.Id, wd.Version);
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
                }}", this.ActionName, wd.Id, wd.Version);
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
            var fe1 = fe;
            fe1.Path = fe.Path.ConvertJavascriptObjectToFunction();
            fe1.SetDefaultLayout(Model.Screen.FormDesign);
            @(fe.UseDisplayTemplate ? Html.DisplayFor(f => fe1) : Html.EditorFor(f => fe1))
        }}
   
    </form>

</div>


@section scripts
{{
    <script type=""text/javascript"" src=""/{0}/Schemas""></script>
    <script type=""text/javascript"">
        require(['services/datacontext', 'jquery','services/app', 'services/system', 'services/config'], function(context,jquery,app, system, config) {{

            
           var instance = context.toObservable(@Html.Raw(JsonConvert.SerializeObject(Model.Instance, setting)),/@Model.Namespace.Replace(""."",""\\."")\.(.*?),/),
               screen = context.toObservable(@Html.Raw(JsonConvert.SerializeObject(Model.Screen, setting)),/@Model.Namespace.Replace(""."",""\\."")\.(.*?),/),
               vm = {{
                id : @Model.Instance.WorkflowDefinitionId,
                instance : ko.observable(instance),    
                screen : ko.observable(screen),
                config : config,
                isBusy : ko.observable(){3}
            }};
            
            instance.addChildItem = function(list, type){{
                return function(){{
                     list.push(new type(system.guid()));                  
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
}}", controller, wd.Id, wd.Version, buttonCommandJs);


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