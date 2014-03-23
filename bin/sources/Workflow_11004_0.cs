using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_11004_0
{
   [EntityType(typeof(Workflow))]
   public class TestSchema2_11004_0 : Bespoke.Sph.Domain.Workflow
   {
       public TestSchema2_11004_0()
       {
           this.Name = "Test schema 2";
           this.Version = 0;
           this.WorkflowDefinitionId = 11004;
       }
       public override Task<ActivityExecutionResult> StartAsync()
       {
           this.SerializedDefinitionStoreId = "wd.11004.0";
           return this.ExecScheduledTriggerActivityStart_03f0Async();
       }
       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
       {
           this.SerializedDefinitionStoreId = "wd.11004.0";
           ActivityExecutionResult result = null;
           switch(activityId)
           {
                   case "03f0cf87-240b-4c19-b94b-fdc3e28e46b9" : 
                       result = await this.ExecScheduledTriggerActivityStart_03f0Async();
                       break;
                   case "23ea0076-94fc-4023-881f-fe627a6eb7b2" : 
                       result = await this.ExecEndActivityEnd1_23eaAsync();
                       break;
           }
           result.Correlation = correlation;
           await this.SaveAsync(activityId, result);
           return result;
       }

//exec:03f0cf87-240b-4c19-b94b-fdc3e28e46b9
   public Task<ActivityExecutionResult> ExecScheduledTriggerActivityStart_03f0Async()
   {

       this.State = "Ready";
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success };
       result.NextActivities = new[]{"23ea0076-94fc-4023-881f-fe627a6eb7b2"};

       return Task.FromResult(result);
   }


//exec:23ea0076-94fc-4023-881f-fe627a6eb7b2
   public Task<ActivityExecutionResult> ExecEndActivityEnd1_23eaAsync()
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


   
   
public partial class Workflow_11004_0Controller : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = string.Format("{0}/{1}/workflow_11004_0/_search", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());
       }
}
public partial class Workflow_11004_0Controller : System.Web.Mvc.Controller
{
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var store = ObjectBuilder.GetObject<IBinaryStore>();
           var doc = await store.GetContentAsync("wd.11004.0");
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
