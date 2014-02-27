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
           }
           result.Correlation = correlation;
           await this.SaveAsync(activityId, result);
           return result;
       }
//variable:app
       public Applicant app {get;set;}

//exec:b7c4b7a6-fd22-4143-be20-8752eabdb463
   public Task<ActivityExecutionResult> ExecScheduledTriggerActivityScheduledTrigger1_b7c4Async()
   {

       this.State = "Ready";
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success };
       result.NextActivities = new[]{"56fb80e3-d10f-4a2d-8268-e2118de28800"};

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
