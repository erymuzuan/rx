using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
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
        public Performer Performer { get; set; }

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {

            var result = base.ValidateBuild(wd);

            if (this.FollowingCorrelationSetCollection.Count > 0 && this.IsInitiator)
                result.Errors.Add(new BuildError(this.WebId,$"[ReceiveActivity] : {this.Name} => Receive must follow a correlation or set to be a start activity but not both"));

            if (this.FollowingCorrelationSetCollection.Count == 0 && !this.IsInitiator)
                result.Errors.Add(new BuildError(this.WebId,$"[ReceiveActivity] : {this.Name} => Receive must follow a correlation or set to be a start activity"));

            if (string.IsNullOrWhiteSpace(this.Operation))
                result.Errors.Add(new BuildError(this.WebId,$"[ReceiveActivity] : {this.Name} => does not have Operation"));
            if (string.IsNullOrWhiteSpace(this.MessagePath))
                result.Errors.Add(new BuildError(this.WebId,$"[ReceiveActivity] : {this.Name} => does not have the MessagePath"));

            var variable = wd.VariableDefinitionCollection.SingleOrDefault(x => x.Name == this.MessagePath);
            if (null != variable)
            {
                var vt = Strings.GetType(variable.TypeName);
                if (null == vt)
                    result.Errors.Add(new BuildError(this.WebId,$"[ReceiveActivity] : {this.Name} => Cannot load the type for {variable.TypeName} in the MesssagePath"));
            }
            else
            {
                result.Errors.Add(new BuildError(this.WebId,$"[ReceiveActivity] : {this.Name} => Cannot find variable {this.MessagePath} in the VariableCollection"));
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
            if (null == vt) throw new InvalidOperationException(variable.TypeName + " is null");

            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}Async({1} message)", this.Name, vt.FullName);
            code.AppendLine("   {");

            code.AppendLinf("       this.{0} = message;", this.MessagePath);

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
                Name = $"{this.Name}Controller",
                FileName = this.Name + "Controller.cs",
                Namespace = wd.CodeNamespace,
                IsPartial = true,
                BaseClass = "BaseApiController"
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
            controller.ImportCollection.Add(typeof(MemoryStream).Namespace);
            controller.AttributeCollection.Add($"  [RoutePrefix(\"{wd.Id}\")]");


            var vt = Strings.GetType(variable.TypeName);
            if (null == vt) throw new InvalidOperationException(variable.TypeName + " is null");


            var code = new StringBuilder();
            code.AppendLinf("//exec:{0}", this.WebId);
            code.AppendLine("       [HttpPost]");
            code.AppendLine($"      [Route(\"{{correlationId}}{this.Operation.ToIdFormat()}\")]");
            code.AppendLine($"      public async Task<HttpResponseMessage> {this.Operation}([RawBody]string json, [FromUri]string correlationId)");
            code.AppendLine("       {");
            if (this.FollowingCorrelationSetCollection.Count == 0 && this.IsInitiator)
            {
                code.AppendLinf("           var wf = new {0}{{Id = Guid.NewGuid().ToString()}};", wd.WorkflowTypeName);
            }
            else
            {
                code.AppendLinf("           {0} wf = null;", wd.WorkflowTypeName);
                code.AppendLine(this.GenerateGetInstanceFromCorrelationSet(wd));
            }
            code.AppendLine();
            code.AppendLine("           await wf.LoadWorkflowDefinitionAsync();");

            code.AppendLine(this.GenerateCanExecuteCode());

            //JsonConvert.DeserializeObject()
            code.AppendLine();
            code.AppendLine($"           var @message = JsonConvert.DeserializeObject<{vt.FullName}>(json);");
            code.AppendLinf($"           var result = await wf.{Name}Async(@message);");
            code.AppendLinf("           await wf.SaveAsync(\"{0}\", result);", this.WebId);
            // any business rules?            
            code.AppendLine("           var  response = Request.CreateResponse(HttpStatusCode.Accepted, new {success = true, status=\"OK\"} );");
            code.AppendLine("           return response;");
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
                code.AppendLinf("           if(!tracker.CanExecute(\"{0}\", \"{0}\" ))", this.WebId);
                code.AppendLine("           {");
                code.AppendLine("               return Request.CreateResponse(HttpStatusCode.Forbidden); ");
                code.AppendLine("           }");

            }

            return code.ToString();
        }


        private string GenerateGetInstanceFromCorrelationSet(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            // get the correlation
            foreach (var c in this.FollowingCorrelationSetCollection)
            {
                var c1 = c;
                var cors = this.CorrelationPropertyCollection.Where(x => x.Name == c1);
                var valExpression = cors.Select(x => "string.Format(\"{0}\"," + x.Path + ")").ToArray();

                code.AppendLinf("           var cval = string.Join(\";\",new string[]{{{0}}});", string.Join(",", valExpression));
                code.AppendFormat(@"  
            var url = ConfigurationManager.ElasticSearchIndex + ""/correlationset/"";
            
            var query = @""{{
   """"query"""": {{
      """"filtered"""": {{
         """"filter"""": {{
            """"bool"""": {{
               """"must"""": [
                  {{
                     """"term"""": {{
                        """"wdid"""": """"{0}""""
                     }}
                  }},
                  {{
                      """"term"""": {{
                         """"value"""": """""" + cval + @""""""
                      }}
                  }},
                  {{
                      """"term"""": {{
                         """"name"""": """"{1}""""
                      }}
                  }}
               ]
            }}
         }}
      }}
   }}
}}"";
            using (var client = new HttpClient())
            {{
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var esresult = await client.PostAsync(url, new StringContent(query));
                var content = esresult.Content as StreamContent;
                var json2 = await content.ReadAsStringAsync();
                var wid = Newtonsoft.Json.Linq.JObject.Parse(json2).SelectToken(""hits.hits[0]._source.wid"");

                var context = new SphDataContext();
                wf = (await context.LoadOneAsync<Workflow>(x => x.Id == wid.ToString())) as {2};              
            }}

", wd.Id, c, wd.WorkflowTypeName);
            }

            return code.ToString();
        }

        public Task<IEnumerable<string>> GetUsersAsync(Workflow wf)
        {
            throw new NotImplementedException();
        }
    }
}