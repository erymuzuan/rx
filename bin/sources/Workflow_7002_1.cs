using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_7002_1
{
   [EntityType(typeof(Workflow))]
   public class TestScreenFormsElement_7002_1 : Bespoke.Sph.Domain.Workflow
   {
       public TestScreenFormsElement_7002_1()
       {
           this.Name = "Test Screen Forms Element";
           this.Version = 1;
           this.WorkflowDefinitionId = 7002;
           this.Pemohon = new Applicant();
       }
       public override Task<ActivityExecutionResult> StartAsync()
       {
           this.SerializedDefinitionStoreId = "wd.7002.1";
           return this.ExecScreenActivityScreen0_8655Async();
       }
       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
       {
           this.SerializedDefinitionStoreId = "wd.7002.1";
           ActivityExecutionResult result = null;
           switch(activityId)
           {
                   case "8655a85e-caea-4bd0-a498-157f0c032f54" : 
                       result = await this.ExecScreenActivityScreen0_8655Async();
                       break;
                   case "62a3c233-0d42-4e53-b896-0812be303ded" : 
                       result = await this.ExecExpressionActivityExpression1_62a3Async();
                       break;
                   case "af52a115-7edc-44e7-a59d-ff5703514087" : 
                       result = await this.ExecEndActivityEnd2_af52Async();
                       break;
           }
           result.Correlation = correlation;
           await this.SaveAsync(activityId, result);
           return result;
       }
//variable:WhatEver
       public System.String WhatEver{get;set;}
//variable:Kelas
       public System.String Kelas{get;set;}
//variable:Pemohon
       public Applicant Pemohon {get;set;}

//exec:8655a85e-caea-4bd0-a498-157f0c032f54
   public async Task<InitiateActivityResult> InitiateAsyncExecScreenActivityScreen0_8655Async()
   {
       var correlation = Guid.NewGuid().ToString();
       var self = this.GetActivity<ScreenActivity>("8655a85e-caea-4bd0-a498-157f0c032f54");
       var baseUrl = ConfigurationManager.BaseUrl;
       var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}?correlation={5}", baseUrl, this.WorkflowDefinitionId, this.Version, self.ActionName, this.WorkflowId, correlation);
       var imb = self.InvitationMessageBody ?? "@Model.Screen.Name task is assigned to you go here @Model.Url";
       var ims = self.InvitationMessageSubject ?? "[Sph] @Model.Screen.Name task is assigned to you";
       await self.SendNotificationToPerformers(this, baseUrl, url, ims, imb);
       return new InitiateActivityResult{ Correlation = correlation };
   }

   public async Task<ActivityExecutionResult> ExecScreenActivityScreen0_8655Async()
   {

       await Task.Delay(40);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{ "62a3c233-0d42-4e53-b896-0812be303ded"};

       return result;
   }


//exec:62a3c233-0d42-4e53-b896-0812be303ded
   public async Task<ActivityExecutionResult> ExecExpressionActivityExpression1_62a3Async()
   {
       await Task.Delay(50);
       
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var item = this;
       System.Console.WriteLine("doing everything i can");
       result.NextActivities = new[]{"af52a115-7edc-44e7-a59d-ff5703514087"};
       
       return result;
   }


//exec:af52a115-7edc-44e7-a59d-ff5703514087
   public Task<ActivityExecutionResult> ExecEndActivityEnd2_af52Async()
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


   public partial class Workflow_7002_1Controller : System.Web.Mvc.Controller
{
//exec:8655a85e-caea-4bd0-a498-157f0c032f54
       public async Task<System.Web.Mvc.ActionResult> Screen0()
       {
           var context = new SphDataContext();
           var wf =  new  TestScreenFormsElement_7002_1();
           await wf.LoadWorkflowDefinitionAsync();
           var profile = await context.LoadOneAsync<UserProfile>(u => u.Username == User.Identity.Name);
           var screen = wf.GetActivity<ScreenActivity>("8655a85e-caea-4bd0-a498-157f0c032f54");
           var vm = new Screen0ViewModel(){
                   Screen  = screen,
                   Instance  = wf as TestScreenFormsElement_7002_1,
                   Controller  = this.GetType().Name,
                   SaveAction  = "SaveScreen0",
                   Namespace  = "Bespoke.Sph.Workflows_7002_1"
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

//exec:8655a85e-caea-4bd0-a498-157f0c032f54
       [System.Web.Mvc.HttpPost]
       public async Task<System.Web.Mvc.ActionResult> SaveScreen0()
       {
           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<TestScreenFormsElement_7002_1>(this);
          var store = ObjectBuilder.GetObject<IBinaryStore>();
                                        var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", wf.WorkflowDefinitionId, wf.Version));
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }  
           var result = await wf.ExecuteAsync("8655a85e-caea-4bd0-a498-157f0c032f54");
           this.Response.ContentType = "application/javascript";
           var retVal = new {sucess = true, status = "OK", result = result,wf};
           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));
       }
   }
   public class Screen0ViewModel
   {
       public TestScreenFormsElement_7002_1 Instance {get;set;}
       public WorkflowDefinition WorkflowDefinition {get;set;}
       public ScreenActivity Screen {get;set;}
       public string Controller {get;set;}
       public string Namespace {get;set;}
       public string SaveAction {get;set;}
       public string Correlation {get;set;}
   }

   
   
public partial class Workflow_7002_1Controller : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = string.Format("{0}/{1}/workflow_7002_1/_search", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());
       }
}
public partial class Workflow_7002_1Controller : System.Web.Mvc.Controller
{
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var store = ObjectBuilder.GetObject<IBinaryStore>();
           var doc = await store.GetContentAsync("wd.7002.1");
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
