using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_10003_0
{
   [EntityType(typeof(Workflow))]
   public class Test0_10003_0 : Bespoke.Sph.Domain.Workflow
   {
       public Test0_10003_0()
       {
           this.Name = "Test 0";
           this.Version = 0;
           this.WorkflowDefinitionId = 10003;
       }
       public override Task<ActivityExecutionResult> StartAsync()
       {
           this.SerializedDefinitionStoreId = "wd.10003.0";
           return this.ExecScheduledTriggerActivityScheduledTrigger0_8f61Async();
       }
       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
       {
           this.SerializedDefinitionStoreId = "wd.10003.0";
           ActivityExecutionResult result = null;
           switch(activityId)
           {
                   case "8f61b30d-eec9-427b-96f3-2e09b7b71153" : 
                       result = await this.ExecScheduledTriggerActivityScheduledTrigger0_8f61Async();
                       break;
                   case "949344ef-c282-4e88-ade7-20dee00db6dd" : 
                       result = await this.ExecEndActivityEnd1_9493Async();
                       break;
           }
           result.Correlation = correlation;
           await this.SaveAsync(activityId, result);
           return result;
       }

//exec:8f61b30d-eec9-427b-96f3-2e09b7b71153
   public Task<ActivityExecutionResult> ExecScheduledTriggerActivityScheduledTrigger0_8f61Async()
   {

       this.State = "Ready";
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success };
       result.NextActivities = new[]{"949344ef-c282-4e88-ade7-20dee00db6dd"};

       return Task.FromResult(result);
   }


//exec:949344ef-c282-4e88-ade7-20dee00db6dd
   public Task<ActivityExecutionResult> ExecEndActivityEnd1_9493Async()
   {
       var result = new ActivityExecutionResult{  Status = ActivityExecutionStatus.Success };
       result.NextActivities = new string[]{};
       this.State = "Completed";
       return Task.FromResult(result);
   }

   }
   [XmlType("Empty",  Namespace="")]
   public partial class Empty : DomainObject
   {
     [XmlAttribute]
     public string Name {get;set;}
   }


   
   
public partial class Workflow_10003_0Controller : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = string.Format("{0}/{1}/workflow_10003_0/_search", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());
       }
}
public partial class Workflow_10003_0Controller : System.Web.Mvc.Controller
{
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var store = ObjectBuilder.GetObject<IBinaryStore>();
           var doc = await store.GetContentAsync("wd.10003.0");
          WorkflowDefinition wd;
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wd = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }

                                        
           var script = await wd.GenerateCustomXsdJavascriptClassAsync();
           this.Response.ContentType = "application/javascript";
           return Content(script);
       }
   }
}
