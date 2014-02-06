using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_8002_0
{
   [EntityType(typeof(Workflow))]
   public class Test321_8002_0 : Bespoke.Sph.Domain.Workflow
   {
       public Test321_8002_0()
       {
           this.Name = "Test 321";
           this.Version = 0;
           this.WorkflowDefinitionId = 8002;
       }
       public override Task<ActivityExecutionResult> StartAsync()
       {
           this.SerializedDefinitionStoreId = "wd.8002.0";
           return this.ExecScreenActivityPermohonanBaru_affbAsync();
       }
       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
       {
           this.SerializedDefinitionStoreId = "wd.8002.0";
           ActivityExecutionResult result = null;
           switch(activityId)
           {
                   case "affb4e73-6669-41aa-a641-d218b0c5aa74" : 
                       result = await this.ExecScreenActivityPermohonanBaru_affbAsync();
                       break;
                   case "438db219-7bc9-45b6-b822-110ca9e0225f" : 
                       result = await this.ExecDecisionActivityCheckSize_438dAsync();
                       break;
                   case "aaf3bb9d-b1b3-474d-a83b-e4d28ed132ef" : 
                       result = await this.ExecNotificationActivityNotify2_aaf3Async();
                       break;
                   case "f7a63bc1-04e2-4b08-ba7d-df7cf1baf5a2" : 
                       result = await this.ExecNotificationActivityNotify3_f7a6Async();
                       break;
                   case "cb0db5a2-4e49-405c-81c0-6c3bcfddaddd" : 
                       result = await this.ExecEndActivityEnd4_cb0dAsync();
                       break;
           }
           result.Correlation = correlation;
           await this.SaveAsync(activityId, result);
           return result;
       }
//variable:Size
       public System.Int32 Size{get;set;}

//exec:affb4e73-6669-41aa-a641-d218b0c5aa74
   public async Task<InitiateActivityResult> InitiateAsyncExecScreenActivityPermohonanBaru_affbAsync()
   {
       var correlation = Guid.NewGuid().ToString();
       var self = this.GetActivity<ScreenActivity>("affb4e73-6669-41aa-a641-d218b0c5aa74");
       var baseUrl = ConfigurationManager.BaseUrl;
       var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}?correlation={5}", baseUrl, this.WorkflowDefinitionId, this.Version, self.ActionName, this.WorkflowId, correlation);
       var imb = self.InvitationMessageBody ?? "@Model.Screen.Name task is assigned to you go here @Model.Url";
       var ims = self.InvitationMessageSubject ?? "[Sph] @Model.Screen.Name task is assigned to you";
       await self.SendNotificationToPerformers(this, baseUrl, url, ims, imb);
       return new InitiateActivityResult{ Correlation = correlation };
   }

   public async Task<ActivityExecutionResult> ExecScreenActivityPermohonanBaru_affbAsync()
   {

       await Task.Delay(40);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{ "438db219-7bc9-45b6-b822-110ca9e0225f"};

       return result;
   }


//exec:438db219-7bc9-45b6-b822-110ca9e0225f
   public async Task<ActivityExecutionResult> ExecDecisionActivityCheckSize_438dAsync()
   {
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       await Task.Delay(50);
       var branch1 = this.CheckSizeBesar();
       if(branch1)
       {
           result.NextActivities = new []{"aaf3bb9d-b1b3-474d-a83b-e4d28ed132ef"};
           return result;
       }
       result.NextActivities = new[]{"f7a63bc1-04e2-4b08-ba7d-df7cf1baf5a2"};
       return result;
   }
   [System.Diagnostics.Contracts.PureAttribute]
   private bool CheckSizeBesar()
   {
       var item = this;
       return Size > 12;
   }


//exec:aaf3bb9d-b1b3-474d-a83b-e4d28ed132ef
   public async Task<ActivityExecutionResult> ExecNotificationActivityNotify2_aaf3Async()
   {
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var act = this.GetActivity<NotificationActivity>("aaf3bb9d-b1b3-474d-a83b-e4d28ed132ef");
       result.NextActivities = new[]{"cb0db5a2-4e49-405c-81c0-6c3bcfddaddd"};
       var @from = await this.TransformFromExecNotificationActivityNotify2_aaf3Async(act.From);
       var to = await this.TransformToExecNotificationActivityNotify2_aaf3Async(act.To);
       var subject = await this.TransformSubjectExecNotificationActivityNotify2_aaf3Async(act.Subject);
       var body = await this.TransformBodyExecNotificationActivityNotify2_aaf3Async(act.Body);
       var cc = await this.TransformBodyExecNotificationActivityNotify2_aaf3Async(act.Cc);
       var bcc = await this.TransformBodyExecNotificationActivityNotify2_aaf3Async(act.Bcc);
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

   public async Task<string> TransformFromExecNotificationActivityNotify2_aaf3Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformToExecNotificationActivityNotify2_aaf3Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformSubjectExecNotificationActivityNotify2_aaf3Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformBodyExecNotificationActivityNotify2_aaf3Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }



//exec:f7a63bc1-04e2-4b08-ba7d-df7cf1baf5a2
   public async Task<ActivityExecutionResult> ExecNotificationActivityNotify3_f7a6Async()
   {
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var act = this.GetActivity<NotificationActivity>("f7a63bc1-04e2-4b08-ba7d-df7cf1baf5a2");
       result.NextActivities = new[]{"cb0db5a2-4e49-405c-81c0-6c3bcfddaddd"};
       var @from = await this.TransformFromExecNotificationActivityNotify3_f7a6Async(act.From);
       var to = await this.TransformToExecNotificationActivityNotify3_f7a6Async(act.To);
       var subject = await this.TransformSubjectExecNotificationActivityNotify3_f7a6Async(act.Subject);
       var body = await this.TransformBodyExecNotificationActivityNotify3_f7a6Async(act.Body);
       var cc = await this.TransformBodyExecNotificationActivityNotify3_f7a6Async(act.Cc);
       var bcc = await this.TransformBodyExecNotificationActivityNotify3_f7a6Async(act.Bcc);
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

   public async Task<string> TransformFromExecNotificationActivityNotify3_f7a6Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformToExecNotificationActivityNotify3_f7a6Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformSubjectExecNotificationActivityNotify3_f7a6Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformBodyExecNotificationActivityNotify3_f7a6Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }



//exec:cb0db5a2-4e49-405c-81c0-6c3bcfddaddd
   public Task<ActivityExecutionResult> ExecEndActivityEnd4_cb0dAsync()
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


   public partial class Workflow_8002_0Controller : System.Web.Mvc.Controller
{
//exec:affb4e73-6669-41aa-a641-d218b0c5aa74
       public async Task<System.Web.Mvc.ActionResult> PermohonanBaru()
       {
           var context = new SphDataContext();
           var wf =  new  Test321_8002_0();
           await wf.LoadWorkflowDefinitionAsync();
           var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == User.Identity.Name);
           var screen = wf.GetActivity<ScreenActivity>("affb4e73-6669-41aa-a641-d218b0c5aa74");
           var vm = new PermohonanBaruViewModel(){
                   Screen  = screen,
                   Instance  = wf as Test321_8002_0,
                   Controller  = this.GetType().Name,
                   SaveAction  = "SavePermohonanBaru",
                   Namespace  = "Bespoke.Sph.Workflows_8002_0"
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

//exec:affb4e73-6669-41aa-a641-d218b0c5aa74
       [System.Web.Mvc.HttpPost]
       public async Task<System.Web.Mvc.ActionResult> SavePermohonanBaru()
       {
           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Test321_8002_0>(this);
          var store = ObjectBuilder.GetObject<IBinaryStore>();
                                        var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", wf.WorkflowDefinitionId, wf.Version));
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }  
           var result = await wf.ExecuteAsync("affb4e73-6669-41aa-a641-d218b0c5aa74");
           this.Response.ContentType = "application/javascript";
           var retVal = new {sucess = true, status = "OK", result = result,wf};
           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));
       }
   }
   public class PermohonanBaruViewModel
   {
       public Test321_8002_0 Instance {get;set;}
       public WorkflowDefinition WorkflowDefinition {get;set;}
       public ScreenActivity Screen {get;set;}
       public string Controller {get;set;}
       public string Namespace {get;set;}
       public string SaveAction {get;set;}
       public string Correlation {get;set;}
   }

   
   
   
   
public partial class Workflow_8002_0Controller : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = string.Format("{0}/{1}/workflow_8002_0/_search", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());
       }
}
public partial class Workflow_8002_0Controller : System.Web.Mvc.Controller
{
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var store = ObjectBuilder.GetObject<IBinaryStore>();
           var doc = await store.GetContentAsync("wd.8002.0");
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
