using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_9002_1
{
   [EntityType(typeof(Workflow))]
   public class TestPageToDiskWorkflow_9002_1 : Bespoke.Sph.Domain.Workflow
   {
       public TestPageToDiskWorkflow_9002_1()
       {
           this.Name = "Test page to disk workflow";
           this.Version = 1;
           this.WorkflowDefinitionId = 9002;
           this.applicant = new Applicant();
       }
       public override Task<ActivityExecutionResult> StartAsync()
       {
           this.SerializedDefinitionStoreId = "wd.9002.1";
           return this.ExecScreenActivityPermohonan_3448Async();
       }
       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
       {
           this.SerializedDefinitionStoreId = "wd.9002.1";
           ActivityExecutionResult result = null;
           switch(activityId)
           {
                   case "3448b2bd-fcb5-496f-a116-c702fe363349" : 
                       result = await this.ExecScreenActivityPermohonan_3448Async();
                       break;
                   case "e72a1cb3-4936-42d4-bab6-ebbcdee1c579" : 
                       result = await this.ExecNotificationActivityEmailAdmin_e72aAsync();
                       break;
                   case "89053589-7627-4c0b-aced-e01bef7ce32b" : 
                       result = await this.ExecEndActivityEnd2_8905Async();
                       break;
           }
           result.Correlation = correlation;
           await this.SaveAsync(activityId, result);
           return result;
       }
//variable:applicant
       public Applicant applicant {get;set;}
//variable:Title
       public System.String Title{get;set;}

//exec:3448b2bd-fcb5-496f-a116-c702fe363349
   public async Task<InitiateActivityResult> InitiateAsyncExecScreenActivityPermohonan_3448Async()
   {
       var correlation = Guid.NewGuid().ToString();
       var self = this.GetActivity<ScreenActivity>("3448b2bd-fcb5-496f-a116-c702fe363349");
       var baseUrl = ConfigurationManager.BaseUrl;
       var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}?correlation={5}", baseUrl, this.WorkflowDefinitionId, this.Version, self.ActionName, this.WorkflowId, correlation);
       var imb = self.InvitationMessageBody ?? "@Model.Screen.Name task is assigned to you go here @Model.Url";
       var ims = self.InvitationMessageSubject ?? "[Sph] @Model.Screen.Name task is assigned to you";
       await self.SendNotificationToPerformers(this, baseUrl, url, ims, imb);
       return new InitiateActivityResult{ Correlation = correlation };
   }

   public async Task<ActivityExecutionResult> ExecScreenActivityPermohonan_3448Async()
   {

       await Task.Delay(40);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{ "e72a1cb3-4936-42d4-bab6-ebbcdee1c579"};

       return result;
   }


//exec:e72a1cb3-4936-42d4-bab6-ebbcdee1c579
   public async Task<ActivityExecutionResult> ExecNotificationActivityEmailAdmin_e72aAsync()
   {
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var act = this.GetActivity<NotificationActivity>("e72a1cb3-4936-42d4-bab6-ebbcdee1c579");
       result.NextActivities = new[]{"89053589-7627-4c0b-aced-e01bef7ce32b"};
       var @from = await this.TransformFromExecNotificationActivityEmailAdmin_e72aAsync(act.From);
       var to = await this.TransformToExecNotificationActivityEmailAdmin_e72aAsync(act.To);
       var subject = await this.TransformSubjectExecNotificationActivityEmailAdmin_e72aAsync(act.Subject);
       var body = await this.TransformBodyExecNotificationActivityEmailAdmin_e72aAsync(act.Body);
       var cc = await this.TransformBodyExecNotificationActivityEmailAdmin_e72aAsync(act.Cc);
       var bcc = await this.TransformBodyExecNotificationActivityEmailAdmin_e72aAsync(act.Bcc);
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


       var context = new SphDataContext();
       foreach(var et in to.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries))
       {
           var et1 = et;
           var user = await context.LoadOneAsync<UserProfile>(u => u.Email == et1);
           if(null == user)continue;
           var message = new Message
                           {
                               Subject = subject,
                               UserName = user.UserName,
                               Body = body
                           };
           using (var session = context.OpenSession())
           {
               session.Attach(message);
               await session.SubmitChanges("Email admin").ConfigureAwait(false);
           }
       }
       return result;
   }

   public async Task<string> TransformFromExecNotificationActivityEmailAdmin_e72aAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformToExecNotificationActivityEmailAdmin_e72aAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformSubjectExecNotificationActivityEmailAdmin_e72aAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformBodyExecNotificationActivityEmailAdmin_e72aAsync(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }



//exec:89053589-7627-4c0b-aced-e01bef7ce32b
   public Task<ActivityExecutionResult> ExecEndActivityEnd2_8905Async()
   {
       var result = new ActivityExecutionResult{  Status = ActivityExecutionStatus.Success };
       result.NextActivities = new string[]{};
       this.State = "Completed";
       return Task.FromResult(result);
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


   public partial class Workflow_9002_1Controller : System.Web.Mvc.Controller
{
//exec:3448b2bd-fcb5-496f-a116-c702fe363349
       public async Task<System.Web.Mvc.ActionResult> Permohonan()
       {
           var context = new SphDataContext();
           var wf =  new  TestPageToDiskWorkflow_9002_1();
           await wf.LoadWorkflowDefinitionAsync();
           var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == User.Identity.Name);
           var screen = wf.GetActivity<ScreenActivity>("3448b2bd-fcb5-496f-a116-c702fe363349");
           var vm = new PermohonanViewModel(){
                   Screen  = screen,
                   Instance  = wf as TestPageToDiskWorkflow_9002_1,
                   Controller  = this.GetType().Name,
                   SaveAction  = "SavePermohonan",
                   Namespace  = "Bespoke.Sph.Workflows_9002_1"
               };
           var canview = screen.Performer.IsPublic;
           if(!screen.Performer.IsPublic)
           {
               var users = await screen.GetUsersAsync(wf);
               canview = this.User.Identity.IsAuthenticated && users.Contains(this.User.Identity.Name);
           }
           if(canview) return View(vm);
           return new System.Web.Mvc.HttpUnauthorizedResult();
       }

//exec:3448b2bd-fcb5-496f-a116-c702fe363349
       [System.Web.Mvc.HttpPost]
       public async Task<System.Web.Mvc.ActionResult> SavePermohonan()
       {
           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<TestPageToDiskWorkflow_9002_1>(this);
          var store = ObjectBuilder.GetObject<IBinaryStore>();
                                        var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", wf.WorkflowDefinitionId, wf.Version));
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }  
           var result = await wf.ExecuteAsync("3448b2bd-fcb5-496f-a116-c702fe363349");
           this.Response.ContentType = "application/javascript";
           var retVal = new {sucess = true, status = "OK", result = result,wf};
           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));
       }
   }
   public class PermohonanViewModel
   {
       public TestPageToDiskWorkflow_9002_1 Instance {get;set;}
       public WorkflowDefinition WorkflowDefinition {get;set;}
       public ScreenActivity Screen {get;set;}
       public string Controller {get;set;}
       public string Namespace {get;set;}
       public string SaveAction {get;set;}
       public string Correlation {get;set;}
   }

   
   
public partial class Workflow_9002_1Controller : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = string.Format("{0}/{1}/workflow_9002_1/_search", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());
       }
}
public partial class Workflow_9002_1Controller : System.Web.Mvc.Controller
{
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var store = ObjectBuilder.GetObject<IBinaryStore>();
           var doc = await store.GetContentAsync("wd.9002.1");
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
