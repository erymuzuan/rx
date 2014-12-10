using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Receive", TypeName = "Receive", Description = "Wait for a message to be delivered")]
    public partial class ReceiveActivity : Activity
    {
        public override bool IsAsync
        {
            get { return true; }
        }
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {

            var result = base.ValidateBuild(wd);

            if (this.FollowingCorrelationSetCollection.Count > 0 && this.IsInitiator) 
                result.Errors.Add(new BuildError(this.WebId, string.Format("[ReceiveActivity] : {0} => Receive must follow a correlation or set to be a start activity but not both", this.Name)));
         
            if (this.FollowingCorrelationSetCollection.Count == 0 && !this.IsInitiator) 
                result.Errors.Add(new BuildError(this.WebId, string.Format("[ReceiveActivity] : {0} => Receive must follow a correlation or set to be a start activity", this.Name)));
         
            if (!string.IsNullOrWhiteSpace(this.Operation))
                result.Errors.Add(new BuildError(this.WebId, string.Format("[ReceiveActivity] : {0} => does not have Operation", this.Name)));
            if (string.IsNullOrWhiteSpace(this.MessagePath))
                result.Errors.Add(new BuildError(this.WebId, string.Format("[ReceiveActivity] : {0} => does not have the MessagePath", this.Name)));

            return result;
        }


        public override string GeneratedInitiateAsyncCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendLinf("   public async Task<InitiateActivityResult> InitiateAsync{0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLinf("       var correlation = Guid.NewGuid().ToString();", this.WebId);
            code.AppendLinf("       await Task.Delay(50);");
            code.AppendLine("       return new InitiateActivityResult{ Correlation = correlation };");
            code.AppendLine("   }");

            return code.ToString();
        }
        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);

            var variable = wd.VariableDefinitionCollection.Single(x => x.Name == this.MessagePath);

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


            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}Async({1} message)", this.Name, variable.TypeName);
            code.AppendLine("   {");
            code.AppendLinf("       this.{0} = message;", this.MessagePath);
            code.AppendLinf("       return await this.{0}();", this.MethodName);
            code.AppendLine("   }");

            return code.ToString();
        }



        public override IEnumerable<Class> GeneratedCustomTypeCode(WorkflowDefinition wd)
        {
            var @classes = new List<Class>();
            var variable = wd.VariableDefinitionCollection.Single(x => x.Name == this.MessagePath);

            // controller
            string name = this.Name.Dehumanize().Replace(" ", string.Empty);
            var controller = new Class
            {
                Name = string.Format("{0}Controller", this.Name),
                FileName = this.Name + "Controller.cs",
                Namespace = wd.CodeNamespace,
                IsPartial = true,
                BaseClass = "ApiController"
            };
            controller.ImportCollection.Add("System.Web.Http");
            controller.ImportCollection.Add("System.Net");
            controller.ImportCollection.Add("System.Net.Http");
            controller.ImportCollection.Add("System.Net.Http.Formatting");
            controller.ImportCollection.Add(typeof(Exception).Namespace);
            controller.ImportCollection.Add(typeof(DomainObject).Namespace);
            controller.ImportCollection.Add(typeof(Task<>).Namespace);
            controller.ImportCollection.Add(typeof(Enumerable).Namespace);
            controller.ImportCollection.Add(typeof(JsonConvert).Namespace);
            controller.ImportCollection.Add(typeof(MemoryStream).Namespace);
            controller.AttributeCollection.Add(string.Format("[RoutePrefix(\"{0}\")]", wd.Id));




            var code = new StringBuilder();
            code.AppendLinf("//exec:{0}", this.WebId);
            code.AppendLine("       [HttpPost]");
            code.AppendLinf("       [Route(\"{0}\")]", this.Operation.ToIdFormat());
            code.AppendLine("       public async Task<HttpResponseMessage> " + this.Operation + "([FromBody]" + variable.TypeName + " item)");
            code.AppendLine("       {");

            // get the correlation
            foreach (var c in this.FollowingCorrelationSetCollection)
            {
                code.AppendFormat(@"  var url = string.Format(""{{0}}/correlationset/"", ConfigurationManager.ElasticSearchIndex, id);
            using (var client = new HttpClient())
            {{
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PostAsync(url, new StringContent(json));
              
            }}");
            }

            if (this.FollowingCorrelationSetCollection.Count == 0 && this.IsInitiator)
            {
                code.AppendLinf("           var wf = new  {0}();", wd.WorkflowTypeName);
            }
            code.AppendFormat(@"           
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(""wd.{0}.{1}"");
            using (var stream = new MemoryStream(doc.Content))
            {{
                wf.WorkflowDefinition = stream.DeserializeFromJson<WorkflowDefinition>();
            }}  
",
 wd.Id, wd.Version);

            code.Append("               ");
            code.AppendLinf("           var result = await wf.{0}Async(item);", this.Name);
            code.AppendLinf("           await wf.SaveAsync(\"{0}\", result);", this.WebId);
            // any business rules?            
            code.AppendLine("           var  response = Request.CreateResponse(HttpStatusCode.Accepted, new {success = true, status=\"OK\", item} );");
            code.AppendLine("           return response;");
            code.AppendLine("       }"); // end SAVE action 09-9558328

            controller.MethodCollection.Add(new Method { Code = code.ToString() });


            @classes.Add(controller);

            return @classes;
        }
    }
}