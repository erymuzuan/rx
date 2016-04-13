using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Receive", TypeName = "Receive", Description = "Wait for a message to be delivered")]
    public partial class ReceiveActivity : Activity
    {
        public override bool IsAsync => true;

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {

            var result = base.ValidateBuild(wd);

            if (this.FollowingCorrelationSetCollection.Count > 0 && this.IsInitiator)
                result.Errors.Add(new BuildError(this.WebId, $"[ReceiveActivity] : {this.Name} => Receive must follow a correlation or set to be a start activity but not both"));

            if (this.FollowingCorrelationSetCollection.Count == 0 && !this.IsInitiator)
                result.Errors.Add(new BuildError(this.WebId, $"[ReceiveActivity] : {this.Name} => Receive must follow a correlation or set to be a start activity"));

            if (string.IsNullOrWhiteSpace(this.Operation))
                result.Errors.Add(new BuildError(this.WebId, $"[ReceiveActivity] : {this.Name} => does not have Operation"));
            if (string.IsNullOrWhiteSpace(this.MessagePath))
                result.Errors.Add(new BuildError(this.WebId, $"[ReceiveActivity] : {this.Name} => does not have the MessagePath"));

            var variable = wd.VariableDefinitionCollection.SingleOrDefault(x => x.Name == this.MessagePath);
            if (null == variable)
            {
                result.Errors.Add(new BuildError(this.WebId, $"[ReceiveActivity] : {this.Name} => Cannot find variable {this.MessagePath} in the VariableCollection"));
            }

            return result;
        }


        public override string GenerateInitAsyncMethod(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendLinf("   public Task<InitiateActivityResult> InitiateAsync{0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLinf("       var result =  new InitiateActivityResult{{ Correlation = \"{0}\" }};", this.WebId);
            code.AppendLine("       return Task.FromResult(result);");
            code.AppendLine();
            code.AppendLine("   }");


            var variable = wd.VariableDefinitionCollection.Single(x => x.Name == this.MessagePath);
            var vt = Strings.GetType(variable.TypeName);
            var vt2 = vt?.FullName;
            if (null == vt)
            {
                vt2 = variable.TypeName;
            }

            code.AppendLine($"   public async Task<ActivityExecutionResult> {Name}Async({vt2} message)");
            code.AppendLine("   {");

            code.AppendLine($"       this.{MessagePath} = message;");

            foreach (var cs in this.InitializingCorrelationSetCollection)
            {
                var cors = wd.CorrelationSetCollection.Single(x => x.Name == cs);
                var cort = wd.CorrelationTypeCollection.Single(x => x.Name == cors.Type);
                var valExpression = cort.CorrelationPropertyCollection.Select(x => "string.Format(\"{0}\",this." + x.Path + ")");

                code.AppendLinf("       await this.InitializeCorrelationSetAsync(\"{0}\", string.Join(\";\",new []{{{1}}})).ConfigureAwait(false);", cors.Name, string.Join(",", valExpression));
            }

            code.AppendLinf("       return await this.{0}();", this.MethodName);
            code.AppendLine("   }");

            return code.ToString();
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
            var variable = wd.VariableDefinitionCollection.Single(x => x.Name == this.MessagePath);

            // controller
            var controller = new Class
            {
                Name = $"{wd.WorkflowTypeName}Controller",
                FileName = $"{wd.WorkflowTypeName}Controller.{Name}.cs",
                Namespace = wd.CodeNamespace,
                IsPartial = true
            };
            controller.ImportCollection.Add("System.Web.Http");
            controller.ImportCollection.Add("System.Net");
            controller.ImportCollection.Add("System.Net.Http");
            controller.ImportCollection.Add("System.Net.Http.Formatting");
            controller.ImportCollection.Add("Bespoke.Sph.WebApi");
            controller.ImportCollection.Add(typeof(Exception).Namespace);
            controller.ImportCollection.Add(typeof(DomainObject).Namespace);
            controller.ImportCollection.Add(typeof(Task<>).Namespace);
            controller.ImportCollection.Add(typeof(Enumerable).Namespace);
            controller.ImportCollection.Add(typeof(JsonConvert).Namespace);

            var vt = Strings.GetType(variable.TypeName);
            var messageType = vt?.FullName;
            if (null == vt) messageType = variable.TypeName;

            var code = new StringBuilder();
            code.AppendLinf("//exec:{0}", this.WebId);
            code.AppendLine("       [HttpPost]");
            if (this.FollowingCorrelationSetCollection.Count == 0 && this.IsInitiator)
            {
                code.AppendLine($"      [Route(\"{this.Operation.ToIdFormat()}\")]");
                code.AppendLine($"      public async Task<IHttpActionResult> {this.Operation}([SourceEntity(\"{wd.Id}\")]WorkflowDefinition wd,[FromBody]{messageType} @message)");
                code.AppendLine("       {");
                code.AppendLine($"           var wf = new {wd.WorkflowTypeName}{{Id = Guid.NewGuid().ToString()}};");
                code.AppendLine("            await wf.LoadWorkflowDefinitionAsync();");
            }
            else
            {
                var correlationName = string.Join(";", this.FollowingCorrelationSetCollection);
                code.AppendLine($"      [Route(\"{this.Operation.ToIdFormat()}\")]");
                code.AppendLine($"      public async Task<IHttpActionResult> {this.Operation}([SourceEntity(\"{wd.Id}\")]WorkflowDefinition wd, [FromBody]{messageType} @message)");
                code.AppendLine("       {");

                var valuePath = this.CorrelationPropertyCollection.Where(x => x.Path.Contains("."))
                    .Select(x => "@message" + x.Path.Remove(0, x.Path.IndexOf(".", StringComparison.Ordinal)))
                    .ToList();
                code.AppendLine($@"           string correlationValue = {string.Join(" + \";\" + ", valuePath)};");
                code.AppendLine($@"           var self = wd.ActivityCollection.OfType<ReceiveActivity>().Single(x => x.WebId == ""{WebId}"");");
                code.AppendLine($"            var wf = await self.LoadInstanceAsync<{wd.WorkflowTypeName}>(wd, \"{correlationName}\", correlationValue);");
                code.AppendLine($"            if( null == wf)");
                code.AppendLine("            {");
                code.AppendLine($"                   return NotFound(\"There's no workflow with {correlationName}  value \" + correlationValue);");
                code.AppendLine("            }");
            }
            code.AppendLine(this.GenerateCanExecuteCode());

            code.AppendLine();
            code.AppendLinf($"            var result = await wf.{Name}Async(@message);");
            code.AppendLine($"            await wf.SaveAsync(\"{WebId}\", result);");
            // any business rules?            
            code.AppendLine("           return Accepted(new {success = true, status=\"OK\"});");
            code.AppendLine("       }"); // end SAVE action
            controller.MethodCollection.Add(new Method { Code = code.ToString() });


            @classes.Add(controller);

            return @classes;
        }


        private string GenerateCanExecuteCode()
        {
            var code = new StringBuilder();
            if (!this.IsInitiator)
            {
                code.AppendLinf("           var tracker = await wf.GetTrackerAsync();");
                code.AppendLine($"           if(!tracker.CanExecute(\"{WebId}\", \"{WebId}\" ))");
                code.AppendLine("           {");
                code.AppendLine("               return Invalid(HttpStatusCode.Forbidden, new { message = \"This endpoint is not in a valid state\"}); ");
                code.AppendLine("           }");

            }

            return code.ToString();
        }



        public override async Task CancelAsync(Workflow wf)
        {
            var baseUrl = ConfigurationManager.BaseUrl;
            var url = $"{ConfigurationManager.BaseUrl}/wf/{wf.WorkflowDefinitionId.ToIdFormat()}/v{wf.Version}/{this.Name.ToIdFormat()}/{wf.Id}";
            var cmb = this.CancelMessageBody ?? "@Model.Screen.Name task assigned to has been cancelled";
            var cms = this.CancelMessageSubject ?? "[Sph] @Model.Screen.Name  task is cancelled";

            await SendNotificationToPerformers(wf, baseUrl, url, cms, cmb);
            var tracker = await wf.GetTrackerAsync();

            tracker.CancelAsyncList(this.WebId);
            await tracker.SaveAsync();
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

        public async Task<string[]> GetUsersAsync(Workflow wf)
        {
            var script = ObjectBuilder.GetObject<IScriptEngine>();
            var context = new SphDataContext();
            var ad = ObjectBuilder.GetObject<IDirectoryService>();

            var unwrapValue = this.Performer.Value;
            var scriptingUsed = !string.IsNullOrWhiteSpace(unwrapValue) && unwrapValue.StartsWith("=");
            if (scriptingUsed)
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
                case "Everybody":
                case "Anynomous":
                    break;
            }
            return users.ToArray();
        }

        public async Task<T> LoadInstanceAsync<T>(WorkflowDefinition wd, string correlationName, string correlationValue) where T : Workflow
        {
            var repos = ObjectBuilder.GetObject<ICorrelationRepository>();
            return await repos.GetInstanceAsync<T>(wd, correlationName, correlationValue);
        }
    }
}