using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_9002_0
{
   [EntityType(typeof(Workflow))]
   public class MyScheduledWorkflow_9002_0 : Bespoke.Sph.Domain.Workflow
   {
       public MyScheduledWorkflow_9002_0()
       {
           this.Name = "My scheduled workflow";
           this.Version = 0;
           this.WorkflowDefinitionId = 9002;
           this.app = new Applicant();
       }
       public override Task<ActivityExecutionResult> StartAsync()
       {
           this.SerializedDefinitionStoreId = "wd.9002.0";
           return this.ExecScheduledTriggerActivityScheduledTrigger1_b7c4Async();
       }
       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
       {
           this.SerializedDefinitionStoreId = "wd.9002.0";
           ActivityExecutionResult result = null;
           switch(activityId)
           {
                   case "b7c4b7a6-fd22-4143-be20-8752eabdb463" : 
                       result = await this.ExecScheduledTriggerActivityScheduledTrigger1_b7c4Async();
                       break;
                   case "56fb80e3-d10f-4a2d-8268-e2118de28800" : 
                       result = await this.ExecNotificationActivityNotify2_56fbAsync();
                       break;
                   case "15dffee5-f38b-4efd-a21f-eb75f99682b5" : 
                       result = await this.ExecEndActivityEnd3_15dfAsync();
                       break;
                   case "b43ee476-563a-4e0b-84f4-021b82abf0d0" : 
                       result = await this.ExecExpressionActivityExpression3_b43eAsync();
                       break;
                   case "ee090c8f-ac80-4444-a882-837dd81ecac4" : 
                       result = await this.ExecScreenActivityScreen4_ee09Async();
                       break;
           }
           result.Correlation = correlation;
           await this.SaveAsync(activityId, result);
           return result;
       }
//variable:app
       public Applicant app {get;set;}
//variable:customer
          private Bespoke.Dev_1001.Domain.Car m_customer = new Bespoke.Dev_1001.Domain.Car();
   public Bespoke.Dev_1001.Domain.Car customer
   {
       get{ return m_customer;}
       set{ m_customer = value;}
   }


//exec:b7c4b7a6-fd22-4143-be20-8752eabdb463
   public Task<ActivityExecutionResult> ExecScheduledTriggerActivityScheduledTrigger1_b7c4Async()
   {

       this.State = "Ready";
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success };
       result.NextActivities = new[]{"b43ee476-563a-4e0b-84f4-021b82abf0d0"};

       return Task.FromResult(result);
   }


//exec:56fb80e3-d10f-4a2d-8268-e2118de28800
   public async Task<ActivityExecutionResult> ExecNotificationActivityNotify2_56fbAsync()
   {
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var act = this.GetActivity<NotificationActivity>("56fb80e3-d10f-4a2d-8268-e2118de28800");
       result.NextActivities = new[]{"15dffee5-f38b-4efd-a21f-eb75f99682b5"};
       var @from = await this.TransformFromExecNotificationActivityNotify2_56fbAsync(act.From);
       var to = await this.TransformToExecNotificationActivityNotify2_56fbAsync(act.To);
       var subject = await this.TransformSubjectExecNotificationActivityNotify2_56fbAsync(act.Subject);
       var body = await this.TransformBodyExecNotificationActivityNotify2_56fbAsync(act.Body);
       var cc = await this.TransformBodyExecNotificationActivityNotify2_56fbAsync(act.Cc);
       var bcc = await this.TransformBodyExecNotificationActivityNotify2_56fbAsync(act.Bcc);
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

   public async Task<string> TransformFromExecNotificationActivityNotify2_56fbAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformToExecNotificationActivityNotify2_56fbAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformSubjectExecNotificationActivityNotify2_56fbAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformBodyExecNotificationActivityNotify2_56fbAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }



//exec:15dffee5-f38b-4efd-a21f-eb75f99682b5
   public Task<ActivityExecutionResult> ExecEndActivityEnd3_15dfAsync()
   {
       var result = new ActivityExecutionResult{  Status = ActivityExecutionStatus.Success };
       result.NextActivities = new string[]{};
       this.State = "Completed";
       return Task.FromResult(result);
   }


//exec:b43ee476-563a-4e0b-84f4-021b82abf0d0
   public async Task<ActivityExecutionResult> ExecExpressionActivityExpression3_b43eAsync()
   {
       await Task.Delay(50);
       
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var item = this;
       //Do this
Console.WriteLine("Test");//Do this
Console.WriteLine("Test");//Do this
Console.WriteLine("Test");//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
//Do this
Console.WriteLine("Test");
       result.NextActivities = new[]{"ee090c8f-ac80-4444-a882-837dd81ecac4"};
       
       return result;
   }


//exec:ee090c8f-ac80-4444-a882-837dd81ecac4
   public async Task<InitiateActivityResult> InitiateAsyncExecScreenActivityScreen4_ee09Async()
   {
       var correlation = Guid.NewGuid().ToString();
       var self = this.GetActivity<ScreenActivity>("ee090c8f-ac80-4444-a882-837dd81ecac4");
       var baseUrl = ConfigurationManager.BaseUrl;
       var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}?correlation={5}", baseUrl, this.WorkflowDefinitionId, this.Version, self.ActionName, this.WorkflowId, correlation);
       var imb = self.InvitationMessageBody ?? "@Model.Screen.Name task is assigned to you go here @Model.Url";
       var ims = self.InvitationMessageSubject ?? "[Sph] @Model.Screen.Name task is assigned to you";
       await self.SendNotificationToPerformers(this, baseUrl, url, ims, imb);
       return new InitiateActivityResult{ Correlation = correlation };
   }

   public async Task<ActivityExecutionResult> ExecScreenActivityScreen4_ee09Async()
   {

       await Task.Delay(40);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{ "56fb80e3-d10f-4a2d-8268-e2118de28800"};

       return result;
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


   
   
   
   
   public partial class Workflow_9002_0Controller : System.Web.Mvc.Controller
{
//exec:ee090c8f-ac80-4444-a882-837dd81ecac4
       public async Task<System.Web.Mvc.ActionResult> Screen4(int id, string correlation)
       {
           var context = new SphDataContext();
           var wf =await context.LoadOneAsync<Workflow>(w => w.WorkflowId == id);
           await wf.LoadWorkflowDefinitionAsync();
           var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == User.Identity.Name);
           var screen = wf.GetActivity<ScreenActivity>("ee090c8f-ac80-4444-a882-837dd81ecac4");
           var vm = new Screen4ViewModel(){
                   Screen  = screen,
                   Instance  = wf as MyScheduledWorkflow_9002_0,
                   Controller  = this.GetType().Name,
                   SaveAction  = "SaveScreen4",
                   Namespace  = "Bespoke.Sph.Workflows_9002_0"
               };
           if(id == 0) throw new ArgumentException("id cannot be zero for none initiator");
           var tracker = await wf.GetTrackerAsync();
           if(!tracker.CanExecute("ee090c8f-ac80-4444-a882-837dd81ecac4", correlation ))
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

//exec:ee090c8f-ac80-4444-a882-837dd81ecac4
       [System.Web.Mvc.HttpPost]
       public async Task<System.Web.Mvc.ActionResult> SaveScreen4()
       {
           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<MyScheduledWorkflow_9002_0>(this);
          var store = ObjectBuilder.GetObject<IBinaryStore>();
                                        var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", wf.WorkflowDefinitionId, wf.Version));
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }  
           var result = await wf.ExecuteAsync("ee090c8f-ac80-4444-a882-837dd81ecac4");
           this.Response.ContentType = "application/javascript";
           var retVal = new {sucess = true, status = "OK", result = result,wf};
           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));
       }
   }
   public class Screen4ViewModel
   {
       public MyScheduledWorkflow_9002_0 Instance {get;set;}
       public WorkflowDefinition WorkflowDefinition {get;set;}
       public ScreenActivity Screen {get;set;}
       public string Controller {get;set;}
       public string Namespace {get;set;}
       public string SaveAction {get;set;}
       public string Correlation {get;set;}
   }

public partial class Workflow_9002_0Controller : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = string.Format("{0}/{1}/workflow_9002_0/_search", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());
       }
}
public partial class Workflow_9002_0Controller : System.Web.Mvc.Controller
{
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var store = ObjectBuilder.GetObject<IBinaryStore>();
           var doc = await store.GetContentAsync("wd.9002.0");
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
