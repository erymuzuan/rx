using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_5002_5
{
   [EntityType(typeof(Workflow))]
   public class TestSchedulerTrigger_5002_5 : Bespoke.Sph.Domain.Workflow
   {
       public TestSchedulerTrigger_5002_5()
       {
           this.Name = "Test Scheduler Trigger";
           this.Version = 5;
           this.WorkflowDefinitionId = 5002;
       }
       public override Task<ActivityExecutionResult> StartAsync()
       {
           this.SerializedDefinitionStoreId = "wd.5002.5";
           return this.ExecScheduledTriggerActivityStartsThisWork_6d0bAsync();
       }
       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
       {
           this.SerializedDefinitionStoreId = "wd.5002.5";
           ActivityExecutionResult result = null;
           switch(activityId)
           {
                   case "6d0baef3-6046-4a84-ab4b-4a17ce0fa6b5" : 
                       result = await this.ExecScheduledTriggerActivityStartsThisWork_6d0bAsync();
                       break;
                   case "1509dfd6-5c71-40dc-9b69-b0e6677154ba" : 
                       result = await this.ExecNotificationActivityNotify1_1509Async();
                       break;
                   case "7143d797-5526-438a-af0c-07f115c906ec" : 
                       result = await this.ExecEndActivityEnd2_7143Async();
                       break;
                   case "a6f2d7b4-ab5e-4b96-a01a-9a80df15aac6" : 
                       result = await this.ExecDelayActivityDelay3_a6f2Async();
                       break;
                   case "f031002d-aca5-4cc4-964c-d6b64ae079b3" : 
                       result = await this.ExecExpressionActivityExpression4_f031Async();
                       break;
                   case "0730cf3e-eeb1-42e0-88d4-e3bfe6095ce0" : 
                       result = await this.ExecDelayActivityDelay5_0730Async();
                       break;
                   case "b7a8a3ca-c385-46c6-8a2a-35a7663137b1" : 
                       result = await this.ExecExpressionActivityExpression6_b7a8Async();
                       break;
                   case "bfa5e2d9-f848-4c8c-8887-04abad70d127" : 
                       result = await this.ExecDecisionActivityDecision7_bfa5Async();
                       break;
                   case "0168e71f-a4d2-46cb-9f7d-775563ff098a" : 
                       result = await this.ExecScreenActivityScreen8_0168Async();
                       break;
                   case "c85f04f4-2131-4514-bc2e-434613f525da" : 
                       result = await this.ExecListenActivityListen9_c85fAsync();
                       break;
           }
           result.Correlation = correlation;
           await this.SaveAsync(activityId, result);
           return result;
       }
//variable:MachineName
       public System.String MachineName{get;set;}
//variable:Status
       public System.String Status{get;set;}

//exec:6d0baef3-6046-4a84-ab4b-4a17ce0fa6b5
   public Task<ActivityExecutionResult> ExecScheduledTriggerActivityStartsThisWork_6d0bAsync()
   {

       this.State = "Ready";
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success };
       result.NextActivities = new[]{"b7a8a3ca-c385-46c6-8a2a-35a7663137b1"};

       return Task.FromResult(result);
   }


//exec:1509dfd6-5c71-40dc-9b69-b0e6677154ba
   public async Task<ActivityExecutionResult> ExecNotificationActivityNotify1_1509Async()
   {
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var act = this.GetActivity<NotificationActivity>("1509dfd6-5c71-40dc-9b69-b0e6677154ba");
       result.NextActivities = new[]{"7143d797-5526-438a-af0c-07f115c906ec"};
       var @from = await this.TransformFromExecNotificationActivityNotify1_1509Async(act.From);
       var to = await this.TransformToExecNotificationActivityNotify1_1509Async(act.To);
       var subject = await this.TransformSubjectExecNotificationActivityNotify1_1509Async(act.Subject);
       var body = await this.TransformBodyExecNotificationActivityNotify1_1509Async(act.Body);
       var cc = await this.TransformBodyExecNotificationActivityNotify1_1509Async(act.Cc);
       var bcc = await this.TransformBodyExecNotificationActivityNotify1_1509Async(act.Bcc);
       System.Console.WriteLine("Sending email to : {0}", to);
       var client = new System.Net.Mail.SmtpClient();
       var mm = new System.Net.Mail.MailMessage();
       mm.Subject = subject;
       mm.Body = body;
       mm.From = new System.Net.Mail.MailAddress(@from);
       mm.To.Add(to);
       if (!string.IsNullOrWhiteSpace(cc))
           mm.CC.Add(cc);
       if (!string.IsNullOrWhiteSpace(bcc))
           mm.Bcc.Add(bcc);
       await client.SendMailAsync(mm).ConfigureAwait(false);
       return result;
   }

   public async Task<string> TransformFromExecNotificationActivityNotify1_1509Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformToExecNotificationActivityNotify1_1509Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformSubjectExecNotificationActivityNotify1_1509Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformBodyExecNotificationActivityNotify1_1509Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }



//exec:7143d797-5526-438a-af0c-07f115c906ec
   public Task<ActivityExecutionResult> ExecEndActivityEnd2_7143Async()
   {
       var result = new ActivityExecutionResult{  Status = ActivityExecutionStatus.Success };
       result.NextActivities = new string[]{};
       this.State = "Completed";
       return Task.FromResult(result);
   }


//exec:a6f2d7b4-ab5e-4b96-a01a-9a80df15aac6
   public async Task<InitiateActivityResult> InitiateAsyncExecDelayActivityDelay3_a6f2Async()
   {
       var self = this.GetActivity<DelayActivity>("a6f2d7b4-ab5e-4b96-a01a-9a80df15aac6");
       await self.CreateTaskSchedulerAsync(this);
       var result = new InitiateActivityResult{ Correlation = Guid.NewGuid().ToString() };
       return result;
   }

   public async Task<ActivityExecutionResult> ExecDelayActivityDelay3_a6f2Async()
   {

       await Task.Delay(50);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{"7143d797-5526-438a-af0c-07f115c906ec"};
     await this.FireListenTriggerExecListenActivityListen9_c85fAsync("a6f2d7b4-ab5e-4b96-a01a-9a80df15aac6");
       return result;
   }


//exec:f031002d-aca5-4cc4-964c-d6b64ae079b3
   public async Task<ActivityExecutionResult> ExecExpressionActivityExpression4_f031Async()
   {
       await Task.Delay(50);
       
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var item = this;
       System.Console.WriteLine("YYYYYYYYYYYYYYYYYYYYY");
this.MachineName = System.DateTime.Now.ToString();
       result.NextActivities = new[]{"0730cf3e-eeb1-42e0-88d4-e3bfe6095ce0"};
       
       return result;
   }


//exec:0730cf3e-eeb1-42e0-88d4-e3bfe6095ce0
   public async Task<InitiateActivityResult> InitiateAsyncExecDelayActivityDelay5_0730Async()
   {
       var self = this.GetActivity<DelayActivity>("0730cf3e-eeb1-42e0-88d4-e3bfe6095ce0");
       await self.CreateTaskSchedulerAsync(this);
       var result = new InitiateActivityResult{ Correlation = Guid.NewGuid().ToString() };
       return result;
   }

   public async Task<ActivityExecutionResult> ExecDelayActivityDelay5_0730Async()
   {

       await Task.Delay(50);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{"1509dfd6-5c71-40dc-9b69-b0e6677154ba"};

       return result;
   }
   public System.DateTime EvaluateExpressionExecDelayActivityDelay5_0730Async()
   {
       var item = this;
       var wait = System.DateTime.Now.Second + 52;
return System.DateTime.Now.AddSeconds(wait);
   }


//exec:b7a8a3ca-c385-46c6-8a2a-35a7663137b1
   public async Task<ActivityExecutionResult> ExecExpressionActivityExpression6_b7a8Async()
   {
       await Task.Delay(50);
       
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var item = this;
       System.Console.WriteLine("XXX");
       result.NextActivities = new[]{"c85f04f4-2131-4514-bc2e-434613f525da"};
       
       return result;
   }


//exec:bfa5e2d9-f848-4c8c-8887-04abad70d127
   public async Task<ActivityExecutionResult> ExecDecisionActivityDecision7_bfa5Async()
   {
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       await Task.Delay(50);
       var branch1 = this.Decision7End();
       if(branch1)
       {
           result.NextActivities = new []{"7143d797-5526-438a-af0c-07f115c906ec"};
           return result;
       }
       result.NextActivities = new[]{"f031002d-aca5-4cc4-964c-d6b64ae079b3"};
       return result;
   }
   [System.Diagnostics.Contracts.PureAttribute]
   private bool Decision7End()
   {
       var item = this;
       return Status == "End";
   }


//exec:0168e71f-a4d2-46cb-9f7d-775563ff098a
   public async Task<InitiateActivityResult> InitiateAsyncExecScreenActivityScreen8_0168Async()
   {
       var correlation = Guid.NewGuid().ToString();
       var self = this.GetActivity<ScreenActivity>("0168e71f-a4d2-46cb-9f7d-775563ff098a");
       var baseUrl = ConfigurationManager.BaseUrl;
       var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}?correlation={5}", baseUrl, this.WorkflowDefinitionId, this.Version, self.ActionName, this.WorkflowId, correlation);
       var imb = self.InvitationMessageBody ?? "@Model.Screen.Name task is assigned to you go here @Model.Url";
       var ims = self.InvitationMessageSubject ?? "[Sph] @Model.Screen.Name task is assigned to you";
       await self.SendNotificationToPerformers(this, baseUrl, url, ims, imb);
       return new InitiateActivityResult{ Correlation = correlation };
   }

   public async Task<ActivityExecutionResult> ExecScreenActivityScreen8_0168Async()
   {

       await Task.Delay(40);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{ "bfa5e2d9-f848-4c8c-8887-04abad70d127"};
     await this.FireListenTriggerExecListenActivityListen9_c85fAsync("0168e71f-a4d2-46cb-9f7d-775563ff098a");
       return result;
   }


//exec:c85f04f4-2131-4514-bc2e-434613f525da
   public async Task<InitiateActivityResult> InitiateAsyncExecListenActivityListen9_c85fAsync()
   {
       var initiateTask1 = this.InitiateAsyncExecScreenActivityScreen8_0168Async();
       var initiateTask2 = this.InitiateAsyncExecDelayActivityDelay3_a6f2Async();
       await Task.WhenAll(initiateTask1,initiateTask2);
       var tracker = await this.GetTrackerAsync();
       var act1 = this.GetActivity<Activity>("0168e71f-a4d2-46cb-9f7d-775563ff098a");
       var bc1 = await  initiateTask1;
       tracker.AddInitiateActivity(act1, bc1);

       var act2 = this.GetActivity<Activity>("a6f2d7b4-ab5e-4b96-a01a-9a80df15aac6");
       var bc2 = await  initiateTask2;
       tracker.AddInitiateActivity(act2, bc2);

       var context = new Bespoke.Sph.Domain.SphDataContext();
       using(var session = context.OpenSession())
       {
           session.Attach(tracker);
           await session.SubmitChanges("ListenActivityChildInitiation");
       }
       return new InitiateActivityResult{Correlation = Guid.NewGuid().ToString() };
   }

   public async Task FireListenTriggerExecListenActivityListen9_c85fAsync(string webId)
   {
       var self = this.GetActivity<ListenActivity>("c85f04f4-2131-4514-bc2e-434613f525da");
       var fired = this.GetActivity<Activity>(webId);
      await self.CancelAsync(this);
 
                                    var cancelled = self.ListenBranchCollection
                                                    .Where(a =>a.NextActivityWebId != webId)
                                                    .Select(a => this.GetActivity<Activity>(a.NextActivityWebId))
                                                    .Select(act => act.CancelAsync(this));
       await Task.WhenAll(cancelled);
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{fired.NextActivityWebId};
       await this.SaveAsync("c85f04f4-2131-4514-bc2e-434613f525da", result);
   }
   public Task<ActivityExecutionResult> ExecListenActivityListen9_c85fAsync()
   {
       throw new System.Exception("Listen activity is not supposed to be executed directly but through FireListenTrigger");
   }

   }
   [XmlType("Vehicle",  Namespace="http://www.maim.gov.my/wakaf")]
   public partial class Vehicle : DomainObject
   {
     [XmlAttribute]
     public int Power {get;set;}
     [XmlAttribute]
     public decimal Cost {get;set;}
     [XmlAttribute]
     public string Name {get;set;}
   }

   [XmlType("Address",  Namespace="http://www.maim.gov.my/wakaf")]
   public partial class Address : DomainObject
   {
     [XmlAttribute]
     public string Street {get;set;}
     [XmlAttribute]
     public string Postcode {get;set;}
     [XmlAttribute]
     public string State {get;set;}
     [XmlAttribute]
     public string City {get;set;}
   }

   [XmlType("Applicant",  Namespace="http://www.maim.gov.my/wakaf")]
   public partial class Applicant : DomainObject
   {
     [XmlAttribute]
     public string Name {get;set;}
     [XmlAttribute]
     public string MyKad {get;set;}
     [XmlAttribute]
     public DateTime RegisteredDate {get;set;}
      public int? Age {get;set;}
      public DateTime? Dob {get;set;}
      public Vehicle Ride {get;set;}
         private readonly ObjectCollection<Car> m_Taxis = new ObjectCollection<Car>();
         public ObjectCollection<Car> Taxis {get { return m_Taxis;} }
         private readonly ObjectCollection<Vehicle> m_PastVehicles = new ObjectCollection<Vehicle>();
         public ObjectCollection<Vehicle> PastVehicles {get { return m_PastVehicles;} }
      private  Address m_Address = new Address();
      public Address Address{get{ return m_Address;} set{ m_Address = value;} }
      private  Contact m_Contact = new Contact();
      public Contact Contact{get{ return m_Contact;} set{ m_Contact = value;} }
   }

   [XmlType("Contact",  Namespace="http://www.maim.gov.my/wakaf")]
   public partial class Contact : DomainObject
   {
     [XmlAttribute]
     public string Telephone {get;set;}
      private  Address m_Address = new Address();
      public Address Address{get{ return m_Address;} set{ m_Address = value;} }
   }

   [XmlType("Car",  Namespace="http://www.maim.gov.my/wakaf")]
   public partial class Car : Vehicle
   {
     [XmlAttribute]
     public int Seating {get;set;}
   }

   [XmlType("Bike",  Namespace="http://www.maim.gov.my/wakaf")]
   public partial class Bike : Vehicle
   {
     [XmlAttribute]
     public bool IsLegal {get;set;}
   }


   
   
   
   
   
   
   
   
   public partial class Workflow_5002_5Controller : System.Web.Mvc.Controller
{
//exec:0168e71f-a4d2-46cb-9f7d-775563ff098a
       public async Task<System.Web.Mvc.ActionResult> Screen8(int id, string correlation)
       {
           var context = new SphDataContext();
           var wf =await context.LoadOneAsync<Workflow>(w => w.WorkflowId == id);
           await wf.LoadWorkflowDefinitionAsync();
           var profile = await context.LoadOneAsync<UserProfile>(u => u.Username == User.Identity.Name);
           var screen = wf.GetActivity<ScreenActivity>("0168e71f-a4d2-46cb-9f7d-775563ff098a");
           var vm = new Screen8ViewModel(){
                   Screen  = screen,
                   Instance  = wf as TestSchedulerTrigger_5002_5,
                   Controller  = this.GetType().Name,
                   SaveAction  = "SaveScreen8",
                   Namespace  = "Bespoke.Sph.Workflows_5002_5"
               };
           if(id == 0) throw new ArgumentException("id cannot be zero for none initiator");
           var tracker = await wf.GetTrackerAsync();
           if(!tracker.CanExecute("0168e71f-a4d2-46cb-9f7d-775563ff098a", correlation ))
           {
               return RedirectToAction("InvalidState","Workflow");
           }
           vm.Correlation = correlation;
           var canview = screen.Performer.IsPublic;
           if(!screen.Performer.IsPublic)
           {
               var users = await screen.GetUsersAsync(wf);
               canview = this.User.Identity.IsAuthenticated && users.Contains(this.User.Identity.Name);
           }
           if(canview) return View(vm);
           return new System.Web.Mvc.HttpUnauthorizedResult();
       }

//exec:0168e71f-a4d2-46cb-9f7d-775563ff098a
       [System.Web.Mvc.HttpPost]
       public async Task<System.Web.Mvc.ActionResult> SaveScreen8()
       {
           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<TestSchedulerTrigger_5002_5>(this);
          var store = ObjectBuilder.GetObject<IBinaryStore>();
                                        var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", wf.WorkflowDefinitionId, wf.Version));
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }  
           var result = await wf.ExecuteAsync("0168e71f-a4d2-46cb-9f7d-775563ff098a");
           this.Response.ContentType = "application/javascript";
           var retVal = new {sucess = true, status = "OK", result = result,wf};
           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));
       }
   }
   public class Screen8ViewModel
   {
       public TestSchedulerTrigger_5002_5 Instance {get;set;}
       public WorkflowDefinition WorkflowDefinition {get;set;}
       public ScreenActivity Screen {get;set;}
       public string Controller {get;set;}
       public string Namespace {get;set;}
       public string SaveAction {get;set;}
       public string Correlation {get;set;}
   }

   
public partial class Workflow_5002_5Controller : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = string.Format("{0}/{1}/workflow_5002_5/_search", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());
       }
}
public partial class Workflow_5002_5Controller : System.Web.Mvc.Controller
{
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var store = ObjectBuilder.GetObject<IBinaryStore>();
           var doc = await store.GetContentAsync("wd.5002.5");
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
