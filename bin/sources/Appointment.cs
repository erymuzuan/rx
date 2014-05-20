using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Dev_6003.Domain
{
   public class Appointment : Entity
   {
       private int m_appointmentId;
       [XmlAttribute]
       public int AppointmentId
       {
           get{ return m_appointmentId;}
           set{ m_appointmentId = value;}
       }
       public Appointment()
       {
           var rc = new RuleContext(this);
       }
     
        public override string ToString()
        {
            return "Appointment:" + ReferenceNo;
        }     
        public override void SetId(int id)
        {
            m_appointmentId = id;
        }     
        public override int GetId()
        {
            return m_appointmentId;
        }
//member:ReferenceNo
      private System.String m_referenceNo;
      [XmlAttribute]
      public System.String ReferenceNo
      {
          get{ return m_referenceNo;}
          set{ m_referenceNo = value;}
      }

//member:Mrn
      private System.String m_mrn;
      [XmlAttribute]
      public System.String Mrn
      {
          get{ return m_mrn;}
          set{ m_mrn = value;}
      }

//member:DateTime
      private System.DateTime m_dateTime;
      [XmlAttribute]
      public System.DateTime DateTime
      {
          get{ return m_dateTime;}
          set{ m_dateTime = value;}
      }

//member:Doctor
      private System.String m_doctor;
      [XmlAttribute]
      public System.String Doctor
      {
          get{ return m_doctor;}
          set{ m_doctor = value;}
      }

//member:Ward
      private System.String m_ward;
      [XmlAttribute]
      public System.String Ward
      {
          get{ return m_ward;}
          set{ m_ward = value;}
      }

//member:Location
      private System.String m_location;
      [XmlAttribute]
      public System.String Location
      {
          get{ return m_location;}
          set{ m_location = value;}
      }

//member:Note
      private System.String m_note;
      [XmlAttribute]
      public System.String Note
      {
          get{ return m_note;}
          set{ m_note = value;}
      }

//member:Referral
      private System.String m_referral;
      [XmlAttribute]
      public System.String Referral
      {
          get{ return m_referral;}
          set{ m_referral = value;}
      }

   }
//class:ReferenceNo

//class:Mrn

//class:DateTime

//class:Doctor

//class:Ward

//class:Location

//class:Note

//class:Referral

public partial class AppointmentController : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = "dev/appointment/_search";

            using(var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PostAsync(url, request);
                var content = response.Content as System.Net.Http.StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es " + request);
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(await content.ReadAsStringAsync());
            }
                   }
//exec:Save
       public async Task<System.Web.Mvc.ActionResult> Save([RequestBody]Appointment item)
       {

            if(null == item) throw new ArgumentNullException("item");
            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("save");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new {success = true, status="OK", id = item.AppointmentId});
       }
//exec:Register
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> Register([RequestBody]Appointment item)
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           if(null == item) throw new ArgumentNullException("item");
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Appointment");
           var brokenRules = new ObjectCollection<ValidationResult>();

            var appliedRules1 = ed.BusinessRuleCollection.Where(b => b.Name == "Date");
            ValidationResult result1 = item.ValidateBusinessRule(appliedRules1);

            if(!result1.Success){
                brokenRules.Add(result1);
            }
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "b2dc31c8-955d-4d54-a9ef-6799e9775d96");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "51201976-4ff9-4c66-a7d6-9d266b5aa2b0");
           item.ReferenceNo = (System.String)setter1.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("Register");
            }
            return Json(new {success = true, status="OK", id = item.AppointmentId});
       }
//exec:validate
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> Validate(string id,[RequestBody]Appointment item)
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           if(null == item) throw new ArgumentNullException("item");
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Appointment");
           var brokenRules = new ObjectCollection<ValidationResult>();
           var rules = id.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);

           foreach(var r in rules)
           {
                var appliedRules = ed.BusinessRuleCollection.Where(b => b.Name == r);
                ValidationResult result = item.ValidateBusinessRule(appliedRules);

                if(!result.Success){
                    brokenRules.Add(result);
                }
           }
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

   
            return Json(new {success = true, status="OK", id = item.AppointmentId});
       }
//exec:Remove
       [HttpDelete]
       public async Task<System.Web.Mvc.ActionResult> Remove(int id)
       {

            var repos = ObjectBuilder.GetObject<IRepository<Appointment>>();
            var item = await repos.LoadOneAsync(id);
            if(null == item)
                return new HttpNotFoundResult();

            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {
                session.Delete(item);
                await session.SubmitChanges("delete");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new {success = true, status="OK", id = item.AppointmentId});
       }
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var context = new SphDataContext();
           var ed = await context.LoadOneAsync<EntityDefinition>(t => t.Name == "Appointment");
           var script = await ed.GenerateCustomXsdJavascriptClassAsync();
           this.Response.ContentType = "application/javascript";
           return Content(script);
       }
}
}
