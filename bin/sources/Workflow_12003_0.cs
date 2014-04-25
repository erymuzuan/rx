using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_12003_0
{
   [EntityType(typeof(Workflow))]
   public class CreateBillingAccountForNewPatient_12003_0 : Bespoke.Sph.Domain.Workflow
   {
       public CreateBillingAccountForNewPatient_12003_0()
       {
           this.Name = "Create billing account for new patient";
           this.Version = 0;
           this.WorkflowDefinitionId = 12003;
       }
       public override Task<ActivityExecutionResult> StartAsync()
       {
           this.SerializedDefinitionStoreId = "wd.12003.0";
           return this.ExecNotificationActivityTellMyBoss_68b2Async();
       }
       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
       {
           this.SerializedDefinitionStoreId = "wd.12003.0";
           ActivityExecutionResult result = null;
           switch(activityId)
           {
                   case "cb58e7bb-5c9d-4a6b-a141-8b256c7085e1" : 
                       result = await this.ExecExpressionActivityCheckIfPatientIsNull_cb58Async();
                       break;
                   case "5e796abc-6629-4e40-99a2-d6e10db378b4" : 
                       result = await this.ExecScreenActivityCreateAccount_5e79Async();
                       break;
                   case "36dab4d5-ae32-43bd-a434-45facebbb3aa" : 
                       result = await this.ExecEndActivityEnd2_36daAsync();
                       break;
                   case "68b2e916-bdc7-4ee3-9873-b0775d637d10" : 
                       result = await this.ExecNotificationActivityTellMyBoss_68b2Async();
                       break;
           }
           result.Correlation = correlation;
           await this.SaveAsync(activityId, result);
           return result;
       }
//variable:Patient
          private Bespoke.Dev_2002.Domain.Patient m_Patient = new Bespoke.Dev_2002.Domain.Patient();
   public Bespoke.Dev_2002.Domain.Patient Patient
   {
       get{ return m_Patient;}
       set{ m_Patient = value;}
   }


//exec:cb58e7bb-5c9d-4a6b-a141-8b256c7085e1
   public async Task<ActivityExecutionResult> ExecExpressionActivityCheckIfPatientIsNull_cb58Async()
   {
       await Task.Delay(50);
       
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var item = this;
       var admin = System.Web.Security.Membership.GetUser("admin");
Console.WriteLine(admin.UserName);

if(null == this.Patient)
    throw new Exception("You cannot register billing on null patient");
       result.NextActivities = new[]{"5e796abc-6629-4e40-99a2-d6e10db378b4"};
       
       return result;
   }


//exec:5e796abc-6629-4e40-99a2-d6e10db378b4
   public async Task<InitiateActivityResult> InitiateAsyncExecScreenActivityCreateAccount_5e79Async()
   {
       var correlation = Guid.NewGuid().ToString();
       var self = this.GetActivity<ScreenActivity>("5e796abc-6629-4e40-99a2-d6e10db378b4");
       var baseUrl = ConfigurationManager.BaseUrl;
       var url = string.Format("{0}/Workflow_{1}_{2}/{3}/{4}?correlation={5}", baseUrl, this.WorkflowDefinitionId, this.Version, self.ActionName, this.WorkflowId, correlation);
       var imb = self.InvitationMessageBody ?? "@Model.Screen.Name task is assigned to you go here @Model.Url";
       var ims = self.InvitationMessageSubject ?? "[Sph] @Model.Screen.Name task is assigned to you";
       await self.SendNotificationToPerformers(this, baseUrl, url, ims, imb);
       return new InitiateActivityResult{ Correlation = correlation };
   }

   public async Task<ActivityExecutionResult> ExecScreenActivityCreateAccount_5e79Async()
   {

       await Task.Delay(40);
       this.State = "Ready";
       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};
       result.NextActivities = new[]{ "36dab4d5-ae32-43bd-a434-45facebbb3aa"};

       return result;
   }


//exec:36dab4d5-ae32-43bd-a434-45facebbb3aa
   public Task<ActivityExecutionResult> ExecEndActivityEnd2_36daAsync()
   {
       var result = new ActivityExecutionResult{  Status = ActivityExecutionStatus.Success };
       result.NextActivities = new string[]{};
       this.State = "Completed";
       return Task.FromResult(result);
   }


//exec:68b2e916-bdc7-4ee3-9873-b0775d637d10
   public async Task<ActivityExecutionResult> ExecNotificationActivityTellMyBoss_68b2Async()
   {
       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};
       var act = this.GetActivity<NotificationActivity>("68b2e916-bdc7-4ee3-9873-b0775d637d10");
       result.NextActivities = new[]{"cb58e7bb-5c9d-4a6b-a141-8b256c7085e1"};
       var @from = await this.TransformFromExecNotificationActivityTellMyBoss_68b2Async(act.From);
       var to = await this.TransformToExecNotificationActivityTellMyBoss_68b2Async(act.To);
       var subject = await this.TransformSubjectExecNotificationActivityTellMyBoss_68b2Async(act.Subject);
       var body = await this.TransformBodyExecNotificationActivityTellMyBoss_68b2Async(act.Body);
       var cc = await this.TransformBodyExecNotificationActivityTellMyBoss_68b2Async(act.Cc);
       var bcc = await this.TransformBodyExecNotificationActivityTellMyBoss_68b2Async(act.Bcc);
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
               await session.SubmitChanges("Tell my boss").ConfigureAwait(false);
           }
       }
       return result;
   }

   public async Task<string> TransformFromExecNotificationActivityTellMyBoss_68b2Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformToExecNotificationActivityTellMyBoss_68b2Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformSubjectExecNotificationActivityTellMyBoss_68b2Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }

   public async Task<string> TransformBodyExecNotificationActivityTellMyBoss_68b2Async(string template)
   {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);
   }


   }
   [XmlType("Empty",  Namespace="")]
   public partial class Empty : DomainObject
   {
     [XmlAttribute]
     public string Name {get;set;}
   }


   
   public partial class Workflow_12003_0Controller : System.Web.Mvc.Controller
{
//exec:5e796abc-6629-4e40-99a2-d6e10db378b4
       public async Task<System.Web.Mvc.ActionResult> CreateAccount(int id, string correlation)
       {
           var context = new SphDataContext();
           var wf =await context.LoadOneAsync<Workflow>(w => w.WorkflowId == id);
           await wf.LoadWorkflowDefinitionAsync();
           var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == User.Identity.Name);
           var screen = wf.GetActivity<ScreenActivity>("5e796abc-6629-4e40-99a2-d6e10db378b4");
           var vm = new CreateAccountViewModel(){
                   Screen  = screen,
                   Instance  = wf as CreateBillingAccountForNewPatient_12003_0,
                   Controller  = this.GetType().Name,
                   SaveAction  = "SaveCreateAccount",
                   Namespace  = "Bespoke.Sph.Workflows_12003_0"
               };
           if(id == 0) throw new ArgumentException("id cannot be zero for none initiator");
           var tracker = await wf.GetTrackerAsync();
           if(!tracker.CanExecute("5e796abc-6629-4e40-99a2-d6e10db378b4", correlation ))
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

//exec:5e796abc-6629-4e40-99a2-d6e10db378b4
       [System.Web.Mvc.HttpPost]
       public async Task<System.Web.Mvc.ActionResult> SaveCreateAccount()
       {
           var wf = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<CreateBillingAccountForNewPatient_12003_0>(this);
          var store = ObjectBuilder.GetObject<IBinaryStore>();
                                        var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", wf.WorkflowDefinitionId, wf.Version));
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }  
           var result = await wf.ExecuteAsync("5e796abc-6629-4e40-99a2-d6e10db378b4");
           this.Response.ContentType = "application/javascript";
           var retVal = new {sucess = true, status = "OK", result = result,wf};
           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));
       }
   }
   public class CreateAccountViewModel
   {
       public CreateBillingAccountForNewPatient_12003_0 Instance {get;set;}
       public WorkflowDefinition WorkflowDefinition {get;set;}
       public ScreenActivity Screen {get;set;}
       public string Controller {get;set;}
       public string Namespace {get;set;}
       public string SaveAction {get;set;}
       public string Correlation {get;set;}
   }

   
   
public partial class Workflow_12003_0Controller : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = string.Format("{0}/{1}/workflow_12003_0/_search", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());
       }
}
public partial class Workflow_12003_0Controller : System.Web.Mvc.Controller
{
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var store = ObjectBuilder.GetObject<IBinaryStore>();
           var doc = await store.GetContentAsync("wd.12003.0");
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
