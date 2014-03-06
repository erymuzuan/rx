using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_2002_33
{
   [EntityType(typeof(Workflow))]
   public class PermohonanMewujudkanWakaf_2002_33 : Bespoke.Sph.Domain.Workflow
   {
       public PermohonanMewujudkanWakaf_2002_33()
       {
           this.Name = "Permohonan Mewujudkan Wakaf";
           this.Version = 33;
           this.WorkflowDefinitionId = 2002;
           this.Applicant = new Contact();
           this.Address = new Address();
           this.Witness = new Witness();
           this.MovableProperty = new MovableProperty();
           this.ImmovableProperty = new ImmovableProperty();
           this.WakafLand = new WakafLand();
       }
       public override Task<ActivityExecutionResult> StartAsync()
       {
           this.SerializedDefinitionStoreId = "wd.2002.33";
           return this.ExecScreenActivityApplyScreen_4122Async();
       }
       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
       {
           this.SerializedDefinitionStoreId = "wd.2002.33";
           ActivityExecutionResult result = null;
           switch(activityId)
           {
                   case "4122d09e-40cd-4558-93eb-ed18f6309f09" : 
                       result = await this.ExecScreenActivityApplyScreen_4122Async();
                       break;
                   case "3aa668ed-598b-4ac6-845a-230b7e1a7deb" : 
                       result = await this.ExecScreenActivityInvestigationScreen_3aa6Async();
                       break;
                   case "5cfb5560-2b44-4af1-8b7b-6d3a2913dd31" : 
                       result = await this.ExecEndActivityEndOfApplicationProcess_5cfbAsync();
                       break;
                   case "5547d4c1-b4c1-47d3-aa84-47cc30e91153" : 
                       result = await this.ExecUpdateEntityActivityUpdateRecord3_5547Async();
                       break;
           }
           result.Correlation = correlation;
           await this.SaveAsync(activityId, result);
           return result;
       }
//variable:Applicant
       public Contact Applicant {get;set;}
//variable:Purposed
       public System.String Purposed{get;set;}
//variable:PronouncementDate
       public System.DateTime PronouncementDate{get;set;}
//variable:Receiver
       public System.String Receiver{get;set;}
//variable:Address
       public Address Address {get;set;}
//variable:Witness
       public Witness Witness {get;set;}
//variable:MovableProperty
       public MovableProperty MovableProperty {get;set;}
//variable:ImmovableProperty
       public ImmovableProperty ImmovableProperty {get;set;}
//variable:WakafLand
       public WakafLand WakafLand {get;set;}
//variable:customer
          private Bespoke.Dev_1.Domain.Customer m_customer = new Bespoke.Dev_1.Domain.Customer();
   public Bespoke.Dev_1.Domain.Customer customer
   {
       get{ return m_customer;}
       set{ m_customer = value;}
   }

//variable:Ticket
       public System.String Ticket{get;set;}

//exec:4122d09e-40cd-4558-93eb-ed18f6309f09
   public async Task<InitiateActivityResult> InitiateAsyncExecScreenActivityApplyScreen_4122Async()
   {
       var correlation = Guid.NewGuid().ToString();
       var self = this.GetActivity<ScreenActivity>("4122d09e-40cd-4558-93eb-ed18f6309f09");
       var baseUrl = ConfigurationManager.BaseUrl;
       var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}?correlation={5}", baseUrl, this.WorkflowDefinitionId, this.Version, self.ActionName, this.WorkflowId, correlation);
       var imb = self.InvitationMessageBody ?? "@Model.Screen.Name task is assigned to you go here @Model.Url";
       var ims = self.InvitationMessageSubject ?? "[Sph] @Model.Screen.Name task is assigned to you";
       await self.SendNotificationToPerformers(this, baseUrl, url, ims, imb);
       return new InitiateActivityResult{ Correlation = correlation };
   }

   public async Task<ActivityExecutionResult> ExecScreenActivityApplyScreen_4122Async()
   {

       await Task.Delay(40);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{ "5547d4c1-b4c1-47d3-aa84-47cc30e91153"};

       return result;
   }


//exec:3aa668ed-598b-4ac6-845a-230b7e1a7deb
   public async Task<InitiateActivityResult> InitiateAsyncExecScreenActivityInvestigationScreen_3aa6Async()
   {
       var correlation = Guid.NewGuid().ToString();
       var self = this.GetActivity<ScreenActivity>("3aa668ed-598b-4ac6-845a-230b7e1a7deb");
       var baseUrl = ConfigurationManager.BaseUrl;
       var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}?correlation={5}", baseUrl, this.WorkflowDefinitionId, this.Version, self.ActionName, this.WorkflowId, correlation);
       var imb = self.InvitationMessageBody ?? "@Model.Screen.Name task is assigned to you go here @Model.Url";
       var ims = self.InvitationMessageSubject ?? "[Sph] @Model.Screen.Name task is assigned to you";
       await self.SendNotificationToPerformers(this, baseUrl, url, ims, imb);
       return new InitiateActivityResult{ Correlation = correlation };
   }

   public async Task<ActivityExecutionResult> ExecScreenActivityInvestigationScreen_3aa6Async()
   {

       await Task.Delay(40);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{ "5cfb5560-2b44-4af1-8b7b-6d3a2913dd31"};

       return result;
   }


//exec:5cfb5560-2b44-4af1-8b7b-6d3a2913dd31
   public Task<ActivityExecutionResult> ExecEndActivityEndOfApplicationProcess_5cfbAsync()
   {
       var result = new ActivityExecutionResult{  Status = ActivityExecutionStatus.Success };
       result.NextActivities = new string[]{};
       this.State = "Completed";
       return Task.FromResult(result);
   }


//exec:5547d4c1-b4c1-47d3-aa84-47cc30e91153
   public async Task<ActivityExecutionResult> ExecUpdateEntityActivityUpdateRecord3_5547Async()
   {
       var context = new Bespoke.Sph.Domain.SphDataContext();
       var item = this.customer;
if(item.CustomerId == 0)throw new InvalidOperationException("CustomerId is 0");
       var self = this.WorkflowDefinition.ActivityCollection.OfType<UpdateEntityActivity>().Single(a => a.WebId == "5547d4c1-b4c1-47d3-aa84-47cc30e91153");
       var functoid1 =  self.PropertyMappingCollection.SingleOrDefault(m => m.WebId == "4fecf979-efc8-4aec-ae60-fb882f015ff1") as FunctoidMapping;
       if(null != functoid1)
           item.FullName = functoid1.Functoid.Convert<string,string>(this.Ticket);
       else
           item.FullName = this.Ticket;
      using (var session = context.OpenSession())
      {
          session.Attach(item);
          await session.SubmitChanges();
      }
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{"3aa668ed-598b-4ac6-845a-230b7e1a7deb"};
       return result;
   }

   }
   [XmlType("Attachment",  Namespace="http://www.maim.gov.my/")]
   public partial class Attachment : DomainObject
   {
     [XmlAttribute]
     public string Type {get;set;}
     [XmlAttribute]
     public string Name {get;set;}
     [XmlAttribute]
     public bool IsReceived {get;set;}
     [XmlAttribute]
     public string StoreId {get;set;}
     [XmlAttribute]
     public bool IsCompleted {get;set;}
     [XmlAttribute]
     public bool IsRequired {get;set;}
   }

   [XmlType("MovableProperty",  Namespace="http://www.maim.gov.my/")]
   public partial class MovableProperty : DomainObject
   {
     [XmlAttribute]
     public int LandId {get;set;}
     [XmlAttribute]
     public string PropertyType {get;set;}
     [XmlAttribute]
     public string Location {get;set;}
     [XmlAttribute]
     public bool CompleteIcNo {get;set;}
     [XmlAttribute]
     public bool CompleteGrant {get;set;}
     [XmlAttribute]
     public bool ComplateTax {get;set;}
     [XmlAttribute]
     public bool CompleteLandPlan {get;set;}
      private  Address m_Address = new Address();
      public Address Address{get{ return m_Address;} set{ m_Address = value;} }
      private  Contact m_Contact = new Contact();
      public Contact Contact{get{ return m_Contact;} set{ m_Contact = value;} }
   }

   [XmlType("ImmovableProperty",  Namespace="http://www.maim.gov.my/")]
   public partial class ImmovableProperty : DomainObject
   {
     [XmlAttribute]
     public int LandId {get;set;}
     [XmlAttribute]
     public double Size {get;set;}
     [XmlAttribute]
     public string Mukim {get;set;}
     [XmlAttribute]
     public string LotNo {get;set;}
     [XmlAttribute]
     public string TypeAndOwnershipNo {get;set;}
      private  Address m_Address = new Address();
      public Address Address{get{ return m_Address;} set{ m_Address = value;} }
      private  Contact m_Contact = new Contact();
      public Contact Contact{get{ return m_Contact;} set{ m_Contact = value;} }
   }

   [XmlType("Address",  Namespace="http://www.maim.gov.my/")]
   public partial class Address : DomainObject
   {
     [XmlAttribute]
     public string Street {get;set;}
     [XmlAttribute]
     public string City {get;set;}
     [XmlAttribute]
     public string Postcode {get;set;}
     [XmlAttribute]
     public string State {get;set;}
     [XmlAttribute]
     public string Country {get;set;}
   }

   [XmlType("Contact",  Namespace="http://www.maim.gov.my/")]
   public partial class Contact : DomainObject
   {
     [XmlAttribute]
     public string Name {get;set;}
     [XmlAttribute]
     public string MobileNo {get;set;}
     [XmlAttribute]
     public string TelephoneNo {get;set;}
     [XmlAttribute]
     public string Email {get;set;}
     [XmlAttribute]
     public string IcNo {get;set;}
   }

   [XmlType("PropertyScheduleAndInterest",  Namespace="http://www.maim.gov.my/")]
   public partial class PropertyScheduleAndInterest : DomainObject
   {
     [XmlAttribute]
     public string Mukim {get;set;}
     [XmlAttribute]
     public string LotNo {get;set;}
     [XmlAttribute]
     public string TypeAndOwnershipNo {get;set;}
     [XmlAttribute]
     public string LandShare {get;set;}
     [XmlAttribute]
     public string LeasingRegistrationNo {get;set;}
     [XmlAttribute]
     public string MortgageRegistrationNo {get;set;}
   }

   [XmlType("WakafLand",  Namespace="http://www.maim.gov.my/")]
   public partial class WakafLand : DomainObject
   {
     [XmlAttribute]
     public string Purposed {get;set;}
     [XmlAttribute]
     public DateTime PronouncementDate {get;set;}
     [XmlAttribute]
     public string Signature {get;set;}
     [XmlAttribute]
     public string ReceiverParty {get;set;}
      private  MovableProperty m_MovableProperty = new MovableProperty();
      public MovableProperty MovableProperty{get{ return m_MovableProperty;} set{ m_MovableProperty = value;} }
      private  ImmovableProperty m_ImmovableProperty = new ImmovableProperty();
      public ImmovableProperty ImmovableProperty{get{ return m_ImmovableProperty;} set{ m_ImmovableProperty = value;} }
      private  Witness m_Witness = new Witness();
      public Witness Witness{get{ return m_Witness;} set{ m_Witness = value;} }
      private  Contact m_Contact = new Contact();
      public Contact Contact{get{ return m_Contact;} set{ m_Contact = value;} }
      private  Address m_Address = new Address();
      public Address Address{get{ return m_Address;} set{ m_Address = value;} }
      private  RegistrationOfficer m_RegistrationOfficer = new RegistrationOfficer();
      public RegistrationOfficer RegistrationOfficer{get{ return m_RegistrationOfficer;} set{ m_RegistrationOfficer = value;} }
   }

   [XmlType("Witness",  Namespace="http://www.maim.gov.my/")]
   public partial class Witness : DomainObject
   {
      public string FirstWitnessSignature {get;set;}
      public string SecondWitnessSignature {get;set;}
      private  Contact m_Contact = new Contact();
      public Contact Contact{get{ return m_Contact;} set{ m_Contact = value;} }
      private  Address m_Address = new Address();
      public Address Address{get{ return m_Address;} set{ m_Address = value;} }
   }

   [XmlType("RegistrationOfficer",  Namespace="http://www.maim.gov.my/")]
   public partial class RegistrationOfficer : DomainObject
   {
     [XmlAttribute]
     public string Name {get;set;}
     [XmlAttribute]
     public string IcNo {get;set;}
     [XmlAttribute]
     public DateTime ApplicationReceiveDate {get;set;}
     [XmlAttribute]
     public bool IsApplicationTrue {get;set;}
     [XmlAttribute]
     public string Note {get;set;}
     [XmlAttribute]
     public string Signature {get;set;}
     [XmlAttribute]
     public DateTime SignatureDate {get;set;}
   }

   [XmlType("Investigation",  Namespace="http://www.maim.gov.my/")]
   public partial class Investigation : DomainObject
   {
     [XmlAttribute]
     public DateTime InvestigationDate {get;set;}
     [XmlAttribute]
     public string Time {get;set;}
     [XmlAttribute]
     public string Purposed {get;set;}
     [XmlAttribute]
     public string InvestigationReport {get;set;}
     [XmlAttribute]
     public DateTime ReportDate {get;set;}
     [XmlAttribute]
     public string PreparedBy {get;set;}
     [XmlAttribute]
     public string HeadUnitReview {get;set;}
     [XmlAttribute]
     public DateTime HeadUnitReviewDate {get;set;}
     [XmlAttribute]
     public string PsuReview {get;set;}
     [XmlAttribute]
     public DateTime PsuReviewDate {get;set;}
      private  WakafLand m_WakafLand = new WakafLand();
      public WakafLand WakafLand{get{ return m_WakafLand;} set{ m_WakafLand = value;} }
   }

   [XmlType("Investigator",  Namespace="http://www.maim.gov.my/")]
   public partial class Investigator : DomainObject
   {
     [XmlAttribute]
     public string Name {get;set;}
   }

   [XmlType("PresenterParty",  Namespace="http://www.maim.gov.my/")]
   public partial class PresenterParty : DomainObject
   {
     [XmlAttribute]
     public string Name {get;set;}
     [XmlAttribute]
     public string Designation {get;set;}
     [XmlAttribute]
     public string TelephoneNo {get;set;}
   }


   public partial class Workflow_2002_33Controller : System.Web.Mvc.Controller
{
//exec:4122d09e-40cd-4558-93eb-ed18f6309f09
       public async Task<System.Web.Mvc.ActionResult> ApplyScreen()
       {
           var context = new SphDataContext();
           var wf =  new  PermohonanMewujudkanWakaf_2002_33();
           await wf.LoadWorkflowDefinitionAsync();
           var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == User.Identity.Name);
           var screen = wf.GetActivity<ScreenActivity>("4122d09e-40cd-4558-93eb-ed18f6309f09");
           var vm = new ApplyScreenViewModel(){
                   Screen  = screen,
                   Instance  = wf as PermohonanMewujudkanWakaf_2002_33,
                   Controller  = this.GetType().Name,
                   SaveAction  = "SaveApplyScreen",
                   Namespace  = "Bespoke.Sph.Workflows_2002_33"
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

//exec:4122d09e-40cd-4558-93eb-ed18f6309f09
       [System.Web.Mvc.HttpPost]
       public async Task<System.Web.Mvc.ActionResult> SaveApplyScreen()
       {
           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<PermohonanMewujudkanWakaf_2002_33>(this);
          var store = ObjectBuilder.GetObject<IBinaryStore>();
                                        var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", wf.WorkflowDefinitionId, wf.Version));
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }  
           var result = await wf.ExecuteAsync("4122d09e-40cd-4558-93eb-ed18f6309f09");
           this.Response.ContentType = "application/javascript";
           var retVal = new {sucess = true, status = "OK", result = result,wf};
           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));
       }
   }
   public class ApplyScreenViewModel
   {
       public PermohonanMewujudkanWakaf_2002_33 Instance {get;set;}
       public WorkflowDefinition WorkflowDefinition {get;set;}
       public ScreenActivity Screen {get;set;}
       public string Controller {get;set;}
       public string Namespace {get;set;}
       public string SaveAction {get;set;}
       public string Correlation {get;set;}
   }

   public partial class Workflow_2002_33Controller : System.Web.Mvc.Controller
{
//exec:3aa668ed-598b-4ac6-845a-230b7e1a7deb
       public async Task<System.Web.Mvc.ActionResult> InvestigationScreen(int id, string correlation)
       {
           var context = new SphDataContext();
           var wf =await context.LoadOneAsync<Workflow>(w => w.WorkflowId == id);
           await wf.LoadWorkflowDefinitionAsync();
           var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == User.Identity.Name);
           var screen = wf.GetActivity<ScreenActivity>("3aa668ed-598b-4ac6-845a-230b7e1a7deb");
           var vm = new InvestigationScreenViewModel(){
                   Screen  = screen,
                   Instance  = wf as PermohonanMewujudkanWakaf_2002_33,
                   Controller  = this.GetType().Name,
                   SaveAction  = "SaveInvestigationScreen",
                   Namespace  = "Bespoke.Sph.Workflows_2002_33"
               };
           if(id == 0) throw new ArgumentException("id cannot be zero for none initiator");
           var tracker = await wf.GetTrackerAsync();
           if(!tracker.CanExecute("3aa668ed-598b-4ac6-845a-230b7e1a7deb", correlation ))
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

//exec:3aa668ed-598b-4ac6-845a-230b7e1a7deb
       [System.Web.Mvc.HttpPost]
       public async Task<System.Web.Mvc.ActionResult> SaveInvestigationScreen()
       {
           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<PermohonanMewujudkanWakaf_2002_33>(this);
          var store = ObjectBuilder.GetObject<IBinaryStore>();
                                        var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", wf.WorkflowDefinitionId, wf.Version));
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }  
           var result = await wf.ExecuteAsync("3aa668ed-598b-4ac6-845a-230b7e1a7deb");
           this.Response.ContentType = "application/javascript";
           var retVal = new {sucess = true, status = "OK", result = result,wf};
           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));
       }
   }
   public class InvestigationScreenViewModel
   {
       public PermohonanMewujudkanWakaf_2002_33 Instance {get;set;}
       public WorkflowDefinition WorkflowDefinition {get;set;}
       public ScreenActivity Screen {get;set;}
       public string Controller {get;set;}
       public string Namespace {get;set;}
       public string SaveAction {get;set;}
       public string Correlation {get;set;}
   }

   
   
public partial class Workflow_2002_33Controller : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = string.Format("{0}/{1}/workflow_2002_33/_search", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());
       }
}
public partial class Workflow_2002_33Controller : System.Web.Mvc.Controller
{
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var store = ObjectBuilder.GetObject<IBinaryStore>();
           var doc = await store.GetContentAsync("wd.2002.33");
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
