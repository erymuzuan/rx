using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;

namespace Bespoke.Dev_2002.Domain
{
   public class Patient : Entity
   {
   private int m_patientId;
   public int PatientId
   {
       get{ return m_patientId;}
       set{ m_patientId = value;}
   }
     
        public override string ToString()
        {
            return "Patient:" + Mrn;
        }//member:Mrn
          private System.String m_mrn;
   public System.String Mrn
   {
       get{ return m_mrn;}
       set{ m_mrn = value;}
   }

//member:FullName
          private System.String m_fullName;
   public System.String FullName
   {
       get{ return m_fullName;}
       set{ m_fullName = value;}
   }

//member:Dob
          private System.DateTime m_dob;
   public System.DateTime Dob
   {
       get{ return m_dob;}
       set{ m_dob = value;}
   }

//member:Gender
          private System.String m_gender;
   public System.String Gender
   {
       get{ return m_gender;}
       set{ m_gender = value;}
   }

//member:Religion
          private System.String m_religion;
   public System.String Religion
   {
       get{ return m_religion;}
       set{ m_religion = value;}
   }

//member:Race
          private System.String m_race;
   public System.String Race
   {
       get{ return m_race;}
       set{ m_race = value;}
   }

//member:RegisteredDate
          private System.DateTime m_registeredDate;
   public System.DateTime RegisteredDate
   {
       get{ return m_registeredDate;}
       set{ m_registeredDate = value;}
   }

//member:IdentificationNo
          private System.String m_identificationNo;
   public System.String IdentificationNo
   {
       get{ return m_identificationNo;}
       set{ m_identificationNo = value;}
   }

//member:PassportNo
          private System.String m_passportNo;
   public System.String PassportNo
   {
       get{ return m_passportNo;}
       set{ m_passportNo = value;}
   }

//member:Nationality
          private System.String m_nationality;
   public System.String Nationality
   {
       get{ return m_nationality;}
       set{ m_nationality = value;}
   }

//member:NextOfKin
          private NextOfKin m_nextOfKin = new NextOfKin();
   public NextOfKin NextOfKin
   {
       get{ return m_nextOfKin;}
       set{ m_nextOfKin = value;}
   }

//member:HomeAddress
          private HomeAddress m_homeAddress = new HomeAddress();
   public HomeAddress HomeAddress
   {
       get{ return m_homeAddress;}
       set{ m_homeAddress = value;}
   }

//member:Occupation
          private System.String m_occupation;
   public System.String Occupation
   {
       get{ return m_occupation;}
       set{ m_occupation = value;}
   }

//member:Status
          private System.String m_status;
   public System.String Status
   {
       get{ return m_status;}
       set{ m_status = value;}
   }

//member:Age
          private System.Int32 m_age;
   public System.Int32 Age
   {
       get{ return m_age;}
       set{ m_age = value;}
   }

//member:Income
          private System.Decimal m_income;
   public System.Decimal Income
   {
       get{ return m_income;}
       set{ m_income = value;}
   }

//member:Empty
          private System.String m_empty;
   public System.String Empty
   {
       get{ return m_empty;}
       set{ m_empty = value;}
   }

//member:Spouse
          private System.String m_spouse;
   public System.String Spouse
   {
       get{ return m_spouse;}
       set{ m_spouse = value;}
   }

//member:MaritalStatus
          private System.String m_maritalStatus;
   public System.String MaritalStatus
   {
       get{ return m_maritalStatus;}
       set{ m_maritalStatus = value;}
   }

   }
//class:Mrn

//class:FullName

//class:Dob

//class:Gender

//class:Religion

//class:Race

//class:RegisteredDate

//class:IdentificationNo

//class:PassportNo

//class:Nationality

//class:NextOfKin
   public class NextOfKin: DomainObject
   {
   private System.String m_fullName;
   public System.String FullName
   {
       get{ return m_fullName;}
       set{ m_fullName = value;}
   }

   private System.String m_relationship;
   public System.String Relationship
   {
       get{ return m_relationship;}
       set{ m_relationship = value;}
   }

   private System.String m_mobilePhone;
   public System.String MobilePhone
   {
       get{ return m_mobilePhone;}
       set{ m_mobilePhone = value;}
   }

   private System.String m_email;
   public System.String Email
   {
       get{ return m_email;}
       set{ m_email = value;}
   }

   private System.String m_telephone;
   public System.String Telephone
   {
       get{ return m_telephone;}
       set{ m_telephone = value;}
   }

   }






//class:HomeAddress
   public class HomeAddress: DomainObject
   {
   private System.String m_street;
   public System.String Street
   {
       get{ return m_street;}
       set{ m_street = value;}
   }

   private System.String m_street2;
   public System.String Street2
   {
       get{ return m_street2;}
       set{ m_street2 = value;}
   }

   private System.String m_postcode;
   public System.String Postcode
   {
       get{ return m_postcode;}
       set{ m_postcode = value;}
   }

   private System.String m_city;
   public System.String City
   {
       get{ return m_city;}
       set{ m_city = value;}
   }

   private System.String m_state;
   public System.String State
   {
       get{ return m_state;}
       set{ m_state = value;}
   }

   private System.String m_country;
   public System.String Country
   {
       get{ return m_country;}
       set{ m_country = value;}
   }

   }







//class:Occupation

//class:Status

//class:Age

//class:Income

//class:Empty

//class:Spouse

//class:MaritalStatus

public partial class PatientController : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = "dev/patient/_search";

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
       public async Task<System.Web.Mvc.ActionResult> Save()
       {

            var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Patient>(this);
            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("save");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new {success = true, status="OK", id = item.PatientId});
       }
       public Patient Item{get;set;}
//exec:Register
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> Register()
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Patient>(this);
           if(null == item) item = this.Item;
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Patient");
           var brokenRules = new ObjectCollection<ValidationResult>();

            var appliedRules1 = ed.BusinessRuleCollection.Where(b => b.Name == "Dob");
            ValidationResult result1 = item.ValidateBusinessRule(appliedRules1);

            if(!result1.Success){
                brokenRules.Add(result1);
            }
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "2dfe6dc4-2757-4b67-ae7d-7d57e6a2dc0d");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "ec52a232-a300-4671-97c3-debd580576da");
           item.Mrn = (System.String)setter1.Field.GetValue(rc);
           var setter2 = operation.SetterActionChildCollection.Single(a => a.WebId == "b414e6e9-9acb-4ba1-86ff-13f78db875f6");
           item.Status = (System.String)setter2.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("Register");
            }
            return Json(new {success = true, status="OK", id = item.PatientId});
       }
//exec:Discharge
       [HttpPost]
       [Authorize]
       public async Task<System.Web.Mvc.ActionResult> Discharge()
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Patient>(this);
           if(null == item) item = this.Item;
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Patient");
           var brokenRules = new ObjectCollection<ValidationResult>();
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "3fd43fb0-ee29-4933-875c-9c2330e81bbe");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "0e22d6e6-f3af-46ff-ad65-d27a15c48f06");
           item.Status = (System.String)setter1.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("Discharge");
            }
            return Json(new {success = true, status="OK", id = item.PatientId});
       }
//exec:Transfer
       [HttpPost]
       [Authorize]
       public async Task<System.Web.Mvc.ActionResult> Transfer()
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Patient>(this);
           if(null == item) item = this.Item;
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Patient");
           var brokenRules = new ObjectCollection<ValidationResult>();
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "f017a0c7-b152-4cb4-8914-188b182a046a");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "3a91284f-ca47-4976-9a93-dab80642536c");
           item.Status = (System.String)setter1.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("Transfer");
            }
            return Json(new {success = true, status="OK", id = item.PatientId});
       }
//exec:Admit
       [HttpPost]
       [Authorize]
       public async Task<System.Web.Mvc.ActionResult> Admit()
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Patient>(this);
           if(null == item) item = this.Item;
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Patient");
           var brokenRules = new ObjectCollection<ValidationResult>();
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "c9da7b5e-2a70-46a9-9b04-cd962a3bf368");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "1b76de49-5a32-45d2-b037-9ef091c8b5b2");
           item.Status = (System.String)setter1.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("Admit");
            }
            return Json(new {success = true, status="OK", id = item.PatientId});
       }
//exec:Remove
       [HttpDelete]
       public async Task<System.Web.Mvc.ActionResult> Remove(int id)
       {

            var repos = ObjectBuilder.GetObject<IRepository<Patient>>();
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
            return Json(new {success = true, status="OK", id = item.PatientId});
       }
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var context = new SphDataContext();
           var ed = await context.LoadOneAsync<EntityDefinition>(t => t.Name == "Patient");
           var script = await ed.GenerateCustomXsdJavascriptClassAsync();
           this.Response.ContentType = "application/javascript";
           return Content(script);
       }
}
}
