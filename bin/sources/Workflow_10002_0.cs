using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_10002_0
{
   [EntityType(typeof(Workflow))]
   public class SimpleScreenAndWait_10002_0 : Bespoke.Sph.Domain.Workflow
   {
       public SimpleScreenAndWait_10002_0()
       {
           this.Name = "Simple Screen and wait";
           this.Version = 0;
           this.WorkflowDefinitionId = 10002;
           this.applicant = new Applicant();
       }
       public override Task<ActivityExecutionResult> StartAsync()
       {
           this.SerializedDefinitionStoreId = "wd.10002.0";
           return this.ExecScheduledTriggerActivityEveryFiveMinutes_edb2Async();
       }
       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
       {
           this.SerializedDefinitionStoreId = "wd.10002.0";
           ActivityExecutionResult result = null;
           switch(activityId)
           {
                   case "77d593ea-0bc9-443c-80d6-0b6f069912d7" : 
                       result = await this.ExecScreenActivityVerification_77d5Async();
                       break;
                   case "edb22af4-9fc2-4f01-b0f1-8c949aea76fc" : 
                       result = await this.ExecScheduledTriggerActivityEveryFiveMinutes_edb2Async();
                       break;
                   case "5f7b4900-798c-43f4-8d8e-20d92c07c28a" : 
                       result = await this.ExecEndActivityEnd2_5f7bAsync();
                       break;
                   case "e56fda2b-5677-42e7-8da9-1e7fe0687d21" : 
                       result = await this.ExecListenActivityStartsVerification_e56fAsync();
                       break;
                   case "5d76fa1a-521e-432f-8d2a-ec36abf1d057" : 
                       result = await this.ExecDelayActivityDelayVerification_5d76Async();
                       break;
                   case "da9fdcd3-a029-4174-9167-eaf6c4c6363f" : 
                       result = await this.ExecNotificationActivityEmailLambatBuat_da9fAsync();
                       break;
           }
           result.Correlation = correlation;
           await this.SaveAsync(activityId, result);
           return result;
       }
//variable:Date
       public System.DateTime Date{get;set;}
//variable:applicant
       public Applicant applicant {get;set;}

//exec:77d593ea-0bc9-443c-80d6-0b6f069912d7
   public async Task<InitiateActivityResult> InitiateAsyncExecScreenActivityVerification_77d5Async()
   {
       var correlation = Guid.NewGuid().ToString();
       var self = this.GetActivity<ScreenActivity>("77d593ea-0bc9-443c-80d6-0b6f069912d7");
       var baseUrl = ConfigurationManager.BaseUrl;
       var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}?correlation={5}", baseUrl, this.WorkflowDefinitionId, this.Version, self.ActionName, this.WorkflowId, correlation);
       var imb = self.InvitationMessageBody ?? "@Model.Screen.Name task is assigned to you go here @Model.Url";
       var ims = self.InvitationMessageSubject ?? "[Sph] @Model.Screen.Name task is assigned to you";
       await self.SendNotificationToPerformers(this, baseUrl, url, ims, imb);
       return new InitiateActivityResult{ Correlation = correlation };
   }

   public async Task<ActivityExecutionResult> ExecScreenActivityVerification_77d5Async()
   {

       await Task.Delay(40);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{ "5f7b4900-798c-43f4-8d8e-20d92c07c28a"};
     await this.FireListenTriggerExecListenActivityStartsVerification_e56fAsync("77d593ea-0bc9-443c-80d6-0b6f069912d7");
       return result;
   }


//exec:edb22af4-9fc2-4f01-b0f1-8c949aea76fc
   public Task<ActivityExecutionResult> ExecScheduledTriggerActivityEveryFiveMinutes_edb2Async()
   {

       this.State = "Ready";
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success };
       result.NextActivities = new[]{"e56fda2b-5677-42e7-8da9-1e7fe0687d21"};

       return Task.FromResult(result);
   }


//exec:5f7b4900-798c-43f4-8d8e-20d92c07c28a
   public Task<ActivityExecutionResult> ExecEndActivityEnd2_5f7bAsync()
   {
       var result = new ActivityExecutionResult{  Status = ActivityExecutionStatus.Success };
       result.NextActivities = new string[]{};
       this.State = "Completed";
       return Task.FromResult(result);
   }


//exec:e56fda2b-5677-42e7-8da9-1e7fe0687d21
   public async Task<InitiateActivityResult> InitiateAsyncExecListenActivityStartsVerification_e56fAsync()
   {
       var initiateTask1 = this.InitiateAsyncExecScreenActivityVerification_77d5Async();
       var initiateTask2 = this.InitiateAsyncExecDelayActivityDelayVerification_5d76Async();
       await Task.WhenAll(initiateTask1,initiateTask2);
       var tracker = await this.GetTrackerAsync();
       var act1 = this.GetActivity<Activity>("77d593ea-0bc9-443c-80d6-0b6f069912d7");
       var bc1 = await  initiateTask1;
       tracker.AddInitiateActivity(act1, bc1, System.DateTime.Now.AddSeconds(1));

       var act2 = this.GetActivity<Activity>("5d76fa1a-521e-432f-8d2a-ec36abf1d057");
       var bc2 = await  initiateTask2;
       tracker.AddInitiateActivity(act2, bc2, System.DateTime.Now.AddSeconds(1));

       var context = new Bespoke.Sph.Domain.SphDataContext();
       using(var session = context.OpenSession())
       {
           session.Attach(tracker);
           await session.SubmitChanges("ListenActivityChildInitiation");
       }
       return new InitiateActivityResult{Correlation = Guid.NewGuid().ToString() };
   }

   public async Task FireListenTriggerExecListenActivityStartsVerification_e56fAsync(string webId)
   {
       var self = this.GetActivity<ListenActivity>("e56fda2b-5677-42e7-8da9-1e7fe0687d21");
       var fired = this.GetActivity<Activity>(webId);
       var tracker = await this.GetTrackerAsync();
       tracker.AddExecutedActivity(fired);
      await self.CancelAsync(this);
 
                                    var cancelled = self.ListenBranchCollection
                                                    .Where(a =>a.NextActivityWebId != webId)
                                                    .Select(a => this.GetActivity<Activity>(a.NextActivityWebId))
                                                    .Select(act => act.CancelAsync(this));
       await Task.WhenAll(cancelled);
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{fired.NextActivityWebId};
       await this.SaveAsync("e56fda2b-5677-42e7-8da9-1e7fe0687d21", result);
   }
   public Task<ActivityExecutionResult> ExecListenActivityStartsVerification_e56fAsync()
   {
       throw new System.Exception("Listen activity is not supposed to be executed directly but through FireListenTrigger");
   }


//exec:5d76fa1a-521e-432f-8d2a-ec36abf1d057
   public async Task<InitiateActivityResult> InitiateAsyncExecDelayActivityDelayVerification_5d76Async()
   {
       var self = this.GetActivity<DelayActivity>("5d76fa1a-521e-432f-8d2a-ec36abf1d057");
       await self.CreateTaskSchedulerAsync(this);
       var result = new InitiateActivityResult{ Correlation = Guid.NewGuid().ToString() };
       return result;
   }

   public async Task<ActivityExecutionResult> ExecDelayActivityDelayVerification_5d76Async()
   {

       await Task.Delay(50);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{"da9fdcd3-a029-4174-9167-eaf6c4c6363f"};
     await this.FireListenTriggerExecListenActivityStartsVerification_e56fAsync("5d76fa1a-521e-432f-8d2a-ec36abf1d057");
       return result;
   }
   public System.DateTime EvaluateExpressionExecDelayActivityDelayVerification_5d76Async()
   {
       var item = this;
       return System.DateTime.Now.AddSeconds((this.WorkflowId % 30)+300);
   }


//exec:da9fdcd3-a029-4174-9167-eaf6c4c6363f
   public async Task<ActivityExecutionResult> ExecNotificationActivityEmailLambatBuat_da9fAsync()
   {
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var act = this.GetActivity<NotificationActivity>("da9fdcd3-a029-4174-9167-eaf6c4c6363f");
       result.NextActivities = new[]{"5f7b4900-798c-43f4-8d8e-20d92c07c28a"};
       var @from = await this.TransformFromExecNotificationActivityEmailLambatBuat_da9fAsync(act.From);
       var to = await this.TransformToExecNotificationActivityEmailLambatBuat_da9fAsync(act.To);
       var subject = await this.TransformSubjectExecNotificationActivityEmailLambatBuat_da9fAsync(act.Subject);
       var body = await this.TransformBodyExecNotificationActivityEmailLambatBuat_da9fAsync(act.Body);
       var cc = await this.TransformBodyExecNotificationActivityEmailLambatBuat_da9fAsync(act.Cc);
       var bcc = await this.TransformBodyExecNotificationActivityEmailLambatBuat_da9fAsync(act.Bcc);
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

   public async Task<string> TransformFromExecNotificationActivityEmailLambatBuat_da9fAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformToExecNotificationActivityEmailLambatBuat_da9fAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformSubjectExecNotificationActivityEmailLambatBuat_da9fAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformBodyExecNotificationActivityEmailLambatBuat_da9fAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
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


   public partial class Workflow_10002_0Controller : System.Web.Mvc.Controller
{
//exec:77d593ea-0bc9-443c-80d6-0b6f069912d7
       public async Task<System.Web.Mvc.ActionResult> Verification(int id, string correlation)
       {
           var context = new SphDataContext();
           var wf =await context.LoadOneAsync<Workflow>(w => w.WorkflowId == id);
           await wf.LoadWorkflowDefinitionAsync();
           var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == User.Identity.Name);
           var screen = wf.GetActivity<ScreenActivity>("77d593ea-0bc9-443c-80d6-0b6f069912d7");
           var vm = new VerificationViewModel(){
                   Screen  = screen,
                   Instance  = wf as SimpleScreenAndWait_10002_0,
                   Controller  = this.GetType().Name,
                   SaveAction  = "SaveVerification",
                   Namespace  = "Bespoke.Sph.Workflows_10002_0"
               };
           if(id == 0) throw new ArgumentException("id cannot be zero for none initiator");
           var tracker = await wf.GetTrackerAsync();
           if(!tracker.CanExecute("77d593ea-0bc9-443c-80d6-0b6f069912d7", correlation ))
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

//exec:77d593ea-0bc9-443c-80d6-0b6f069912d7
       [System.Web.Mvc.HttpPost]
       public async Task<System.Web.Mvc.ActionResult> SaveVerification()
       {
           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<SimpleScreenAndWait_10002_0>(this);
          var store = ObjectBuilder.GetObject<IBinaryStore>();
                                        var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", wf.WorkflowDefinitionId, wf.Version));
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }  
           var result = await wf.ExecuteAsync("77d593ea-0bc9-443c-80d6-0b6f069912d7");
           this.Response.ContentType = "application/javascript";
           var retVal = new {sucess = true, status = "OK", result = result,wf};
           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));
       }
   }
   public class VerificationViewModel
   {
       public SimpleScreenAndWait_10002_0 Instance {get;set;}
       public WorkflowDefinition WorkflowDefinition {get;set;}
       public ScreenActivity Screen {get;set;}
       public string Controller {get;set;}
       public string Namespace {get;set;}
       public string SaveAction {get;set;}
       public string Correlation {get;set;}
   }

   
   
   
   
   
public partial class Workflow_10002_0Controller : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = string.Format("{0}/{1}/workflow_10002_0/_search", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());
       }
}
public partial class Workflow_10002_0Controller : System.Web.Mvc.Controller
{
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var store = ObjectBuilder.GetObject<IBinaryStore>();
           var doc = await store.GetContentAsync("wd.10002.0");
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
