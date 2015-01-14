using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
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

            var result = base.ValidateBuild(wd);

            if (!this.Performer.IsPublic && string.IsNullOrWhiteSpace(this.Performer.UserProperty))
                result.Errors.Add(new BuildError(this.WebId,
                             string.Format("[ScreenActivity] : {0} => does not have performer", this.Name)));
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                result.Errors.Add(new BuildError(this.WebId,
                             string.Format("[ScreenActivity] : {0} => does not have the next activity defined", this.Name)));

            if (string.IsNullOrWhiteSpace(this.FormId))
                result.Errors.Add(new BuildError(this.WebId,
                             string.Format("[ScreenActivity] : {0} => does not have a Form defined", this.Name)));

            return result;
        }

        public async override Task CancelAsync(Workflow wf)
        {
            var baseUrl = ConfigurationManager.BaseUrl;
            var url = string.Format("{0}/wf/{1}/v{2}/{3}/{4}", baseUrl, wf.WorkflowDefinitionId.ToIdFormat(), wf.Version, this.Name.ToIdFormat(), wf.Id);
            var cmb = this.CancelMessageBody ?? "@Model.Screen.Name task assigned to has been cancelled";
            var cms = this.CancelMessageSubject ?? "[Sph] @Model.Screen.Name  task is cancelled";

            await SendNotificationToPerformers(wf, baseUrl, url, cms, cmb);
            var tracker = await wf.GetTrackerAsync();

            tracker.CancelAsyncList(this.WebId);
            await tracker.SaveAsync();
        }

        public override string GenerateInitAsyncMethod(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendLinf("   public async Task<InitiateActivityResult> InitiateAsync{0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLinf("       var correlation = Guid.NewGuid().ToString();", this.WebId);
            code.AppendLinf("       var self = this.GetActivity<ScreenActivity>(\"{0}\");", this.WebId);
            code.AppendLine("       var baseUrl = ConfigurationManager.BaseUrl;");
            code.AppendLine("       var url = string.Format(\"{0}/wf/{1}/v{2}/{3}/{4}/{5}\", baseUrl, this.WorkflowDefinitionId, this.Version, self.Name.ToIdFormat(), this.Id, correlation);");
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

                var message = new Message { Subject = subject, UserName = user, Body = body, Id = Strings.GenerateId() };
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

        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);

            var code = new StringBuilder();
            
            code.AppendLine(this.ExecutingCode);
            code.AppendLine("       this.State = \"Ready\";");
            // set the next activity
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLinf("       result.NextActivities = new[]{{ \"{0}\"}};", this.NextActivityWebId);
            code.AppendLine(this.ExecutedCode);
            

            return code.ToString();
        }


        public override IEnumerable<Class> GeneratedCustomTypeCode(WorkflowDefinition wd)
        {
            var @classes = new List<Class>();

            // controller
            string name = this.Name.Dehumanize().Replace(" ", string.Empty);
            var controller = new Class
            {
                Name = string.Format("{0}Controller", wd.WorkflowTypeName),
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
            controller.ImportCollection.Add(typeof(JsonConvert).Namespace);
            controller.ImportCollection.Add(typeof(MemoryStream).Namespace);


            var getAction = new StringBuilder();
            // GET Action
            getAction.AppendLinf("//exec:{0}", this.WebId);
            getAction.AppendLinf("       [HttpGet]");
            getAction.AppendLinf("       [Route(\"{0}/{{id}}/{{correlation?}}\")]", this.Name.ToIdFormat());
            getAction.AppendLinf("       [Route(\"{0}{1}\")]", this.ActionName, this.IsInitiator ? "" : "{id}/{correlation}");
            getAction.AppendLinf("       public async Task<ActionResult> {0}({1})", this.ActionName, this.IsInitiator ? "" : "string id, string correlation=\"\"");
            getAction.AppendLine("       {");


            getAction.AppendLine("           var context = new SphDataContext();");
            getAction.AppendLinf(
                this.IsInitiator
                    ? "                      var wf =  new  {0}();"
                    : "                      var wf = await context.LoadOneAsync<Workflow>(w => w.Id == id);", wd.WorkflowTypeName);

            getAction.AppendLine("           await wf.LoadWorkflowDefinitionAsync();");
            getAction.AppendLinf("           var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == User.Identity.Name);");
            getAction.AppendLinf("           var screen = wf.GetActivity<ScreenActivity>(\"{0}\");", this.WebId);

            getAction.AppendLinf("           var vm = new {0}(){{", this.ViewModelType);
            getAction.AppendLinf("                   Screen  = screen,");
            getAction.AppendLinf("                   Instance  = wf as {0},", wd.WorkflowTypeName);
            getAction.AppendLinf("                   Controller  = this.GetType().Name,");
            getAction.AppendLinf("                   SaveAction  = \"{0}\",", this.ActionName);
            getAction.AppendLinf("                   Namespace  = \"{0}\"", wd.CodeNamespace);
            getAction.AppendLinf("               }};", wd.CodeNamespace);


            if (!this.IsInitiator)
            {
                getAction.AppendLinf("           if(id == \"0\" || string.IsNullOrWhiteSpace(id)) throw new ArgumentException(\"id cannot be zero for none initiator\");");
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

            getAction.AppendLinf("           return Json(new {{ canView, vm,version = \"{0}\" }}, , JsonRequestBehavior.AllowGet);", this.ActionName, wd.Version);

            getAction.AppendLine("       }");// end GET action
            getAction.AppendLine();
            controller.MethodCollection.Add(new Method { Code = getAction.ToString() });


            var saveAction = new StringBuilder();
            saveAction.AppendLinf("//exec:{0}", this.WebId);
            saveAction.AppendLine("       [HttpPost]");
            saveAction.AppendLinf("       [Route(\"{0}\")]", this.ActionName.ToIdFormat());
            saveAction.AppendLine("       public async Task<ActionResult> " + this.ActionName + "()");
            saveAction.AppendLine("       {");

            saveAction.AppendLinf("           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<{0}>(this);", wd.WorkflowTypeName);// this is extension method
            saveAction.AppendLine(@"           var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(string.Format(""wd.{0}.{1}"", wf.WorkflowDefinitionId, wf.Version));
            using (var stream = new MemoryStream(doc.Content))
            {
                wf.WorkflowDefinition = stream.DeserializeFromJson<WorkflowDefinition>();
            }  
");
            saveAction.AppendLinf("           var result = await wf.ExecuteAsync(\"{0}\");", this.WebId);
            // any business rules?            
            saveAction.AppendLine("           this.Response.ContentType = \"application/javascript\";");
            saveAction.AppendLine("           var retVal = new {sucess = true, status = \"OK\", result = result,wf};");
            saveAction.AppendLine("           return Content(JsonConvert.SerializeObject(retVal));");
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