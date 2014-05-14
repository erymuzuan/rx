using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Dev_1.Domain
{
   public class Customer : Entity
   {
       private int m_customerId;
       [XmlAttribute]
       public int CustomerId
       {
           get{ return m_customerId;}
           set{ m_customerId = value;}
       }
       public Customer()
       {
           var rc = new RuleContext(this);

           var mj1 = "{\"$type\":\"Bespoke.Sph.Domain.ConstantField, domain.sph\",\"TypeName\":\"System.String, mscorlib\",\"Value\":\"Male\",\"Name\":\"Male\",\"Note\":\"\",\"WebId\":\"bcccb11c-fb6f-4b91-9591-14951a451a42\"}";
           var field1 = mj1.DeserializeFromJson<ConstantField>();
           var val1 = field1.GetValue(rc);
           m_gender = (System.String)val1;

           var mj2 = "{\"$type\":\"Bespoke.Sph.Domain.ConstantField, domain.sph\",\"TypeName\":\"System.String, mscorlib\",\"Value\":\"Sales\",\"Name\":\"Sales\",\"Note\":\"\",\"WebId\":\"95df2857-f940-4066-95b7-bc5ccc6bc155\"}";
           var field2 = mj2.DeserializeFromJson<ConstantField>();
           var val2 = field2.GetValue(rc);
           m_department = (System.String)val2;
       }
     
        public override string ToString()
        {
            return "Customer:" + FullName;
        }     
        public override void SetId(int id)
        {
            m_customerId = id;
        }     
        public override int GetId()
        {
            return m_customerId;
        }
//member:FullName
      private System.String m_fullName;
      [XmlAttribute]
      public System.String FullName
      {
          get{ return m_fullName;}
          set{ m_fullName = value;}
      }

//member:Address
      private Address m_address = new Address();
      public Address Address
      {
          get{ return m_address;}
          set{ m_address = value;}
      }

//member:Age
      private System.Int32? m_age;
      public System.Int32? Age
      {
          get{ return m_age;}
          set{ m_age = value;}
      }

//member:Gender
      private System.String m_gender;
      [XmlAttribute]
      public System.String Gender
      {
          get{ return m_gender;}
          set{ m_gender = value;}
      }

//member:IsPriority
      private System.Boolean m_isPriority;
      [XmlAttribute]
      public System.Boolean IsPriority
      {
          get{ return m_isPriority;}
          set{ m_isPriority = value;}
      }

//member:Contact
      private Contact m_contact = new Contact();
      public Contact Contact
      {
          get{ return m_contact;}
          set{ m_contact = value;}
      }

//member:PhotoStoreId
      private System.String m_photoStoreId;
      [XmlAttribute]
      public System.String PhotoStoreId
      {
          get{ return m_photoStoreId;}
          set{ m_photoStoreId = value;}
      }

//member:RegisteredDate
      private System.DateTime m_registeredDate;
      [XmlAttribute]
      public System.DateTime RegisteredDate
      {
          get{ return m_registeredDate;}
          set{ m_registeredDate = value;}
      }

//member:Rating
      private System.Int32 m_rating;
      [XmlAttribute]
      public System.Int32 Rating
      {
          get{ return m_rating;}
          set{ m_rating = value;}
      }

//member:AnnualRevenue
      private System.Decimal m_annualRevenue;
      [XmlAttribute]
      public System.Decimal AnnualRevenue
      {
          get{ return m_annualRevenue;}
          set{ m_annualRevenue = value;}
      }

//member:ProfileStoreId
      private System.String m_profileStoreId;
      [XmlAttribute]
      public System.String ProfileStoreId
      {
          get{ return m_profileStoreId;}
          set{ m_profileStoreId = value;}
      }

//member:PrimaryContact
      private System.String m_primaryContact;
      [XmlAttribute]
      public System.String PrimaryContact
      {
          get{ return m_primaryContact;}
          set{ m_primaryContact = value;}
      }

//member:Revenue
      private System.Decimal? m_revenue;
      public System.Decimal? Revenue
      {
          get{ return m_revenue;}
          set{ m_revenue = value;}
      }

//member:LogoStoreId
      private System.String m_logoStoreId;
      [XmlAttribute]
      public System.String LogoStoreId
      {
          get{ return m_logoStoreId;}
          set{ m_logoStoreId = value;}
      }

//member:Department
      private System.String m_department;
      [XmlAttribute]
      public System.String Department
      {
          get{ return m_department;}
          set{ m_department = value;}
      }

   }
//class:FullName

//class:Address
   public class Address: DomainObject
   {
      private System.String m_street1;
      [XmlAttribute]
      public System.String Street1
      {
          get{ return m_street1;}
          set{ m_street1 = value;}
      }

      private System.String m_street2;
      [XmlAttribute]
      public System.String Street2
      {
          get{ return m_street2;}
          set{ m_street2 = value;}
      }

      private System.String m_postcode;
      [XmlAttribute]
      public System.String Postcode
      {
          get{ return m_postcode;}
          set{ m_postcode = value;}
      }

      private System.String m_state;
      [XmlAttribute]
      public System.String State
      {
          get{ return m_state;}
          set{ m_state = value;}
      }

      private System.String m_locality;
      [XmlAttribute]
      public System.String Locality
      {
          get{ return m_locality;}
          set{ m_locality = value;}
      }

      private System.String m_district;
      [XmlAttribute]
      public System.String District
      {
          get{ return m_district;}
          set{ m_district = value;}
      }

   }







//class:Age

//class:Gender

//class:IsPriority

//class:Contact
   public class Contact: DomainObject
   {
      private System.String m_name;
      [XmlAttribute]
      public System.String Name
      {
          get{ return m_name;}
          set{ m_name = value;}
      }

      private System.String m_email;
      [XmlAttribute]
      public System.String Email
      {
          get{ return m_email;}
          set{ m_email = value;}
      }

      private System.String m_telephone;
      [XmlAttribute]
      public System.String Telephone
      {
          get{ return m_telephone;}
          set{ m_telephone = value;}
      }

      private readonly ObjectCollection<Attachment> m_attachmentCollection = new ObjectCollection<Attachment>();
      public ObjectCollection<Attachment> AttachmentCollection
      {
          get{ return m_attachmentCollection;}
      }

      private System.String m_title;
      [XmlAttribute]
      public System.String Title
      {
          get{ return m_title;}
          set{ m_title = value;}
      }

   }



   public class Attachment: DomainObject
   {
      private System.String m_title;
      [XmlAttribute]
      public System.String Title
      {
          get{ return m_title;}
          set{ m_title = value;}
      }

      private System.String m_extension;
      [XmlAttribute]
      public System.String Extension
      {
          get{ return m_extension;}
          set{ m_extension = value;}
      }

      private System.String m_note;
      [XmlAttribute]
      public System.String Note
      {
          get{ return m_note;}
          set{ m_note = value;}
      }

      private System.String m_storeId;
      [XmlAttribute]
      public System.String StoreId
      {
          get{ return m_storeId;}
          set{ m_storeId = value;}
      }

   }







//class:PhotoStoreId

//class:RegisteredDate

//class:Rating

//class:AnnualRevenue

//class:ProfileStoreId

//class:PrimaryContact

//class:Revenue

//class:LogoStoreId

//class:Department

public partial class CustomerController : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = "dev/customer/_search";

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
       public async Task<System.Web.Mvc.ActionResult> Save([RequestBody]Customer item)
       {

            if(null == item) throw new ArgumentNullException("item");
            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("save");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new {success = true, status="OK", id = item.CustomerId});
       }
//exec:PromoteTo
       [HttpPost]
       [Authorize]
       public async Task<System.Web.Mvc.ActionResult> PromoteTo([RequestBody]Customer item)
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           if(null == item) throw new ArgumentNullException("item");
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Customer");
           var brokenRules = new ObjectCollection<ValidationResult>();

            var appliedRules1 = ed.BusinessRuleCollection.Where(b => b.Name == "Must be Malaysian");
            ValidationResult result1 = item.ValidateBusinessRule(appliedRules1);

            if(!result1.Success){
                brokenRules.Add(result1);
            }

            var appliedRules2 = ed.BusinessRuleCollection.Where(b => b.Name == "Verify the age");
            ValidationResult result2 = item.ValidateBusinessRule(appliedRules2);

            if(!result2.Success){
                brokenRules.Add(result2);
            }
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "84d65cfd-a054-413f-8ea6-3fe2660a8564");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "f1eb0f44-e235-4a3f-9444-6116717a5eb3");
           item.Rating = (System.Int32)setter1.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("PromoteTo");
            }
            return Json(new {success = true, status="OK", id = item.CustomerId});
       }
//exec:Demote
       [HttpPost]
       [Authorize(Roles="developers,administrators")]
       public async Task<System.Web.Mvc.ActionResult> Demote([RequestBody]Customer item)
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           if(null == item) throw new ArgumentNullException("item");
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Customer");
           var brokenRules = new ObjectCollection<ValidationResult>();

            var appliedRules1 = ed.BusinessRuleCollection.Where(b => b.Name == "Verify the age");
            ValidationResult result1 = item.ValidateBusinessRule(appliedRules1);

            if(!result1.Success){
                brokenRules.Add(result1);
            }
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "4d265ce9-c455-4c94-ab0a-97794f203ec8");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "98f018ea-75f4-4394-a2d1-6d0ec80f3fbc");
           item.Rating = (System.Int32)setter1.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("Demote");
            }
            return Json(new {success = true, status="OK", id = item.CustomerId});
       }
//exec:CreateOrder
       [HttpPost]
       [Authorize]
       public async Task<System.Web.Mvc.ActionResult> CreateOrder([RequestBody]Customer item)
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           if(null == item) throw new ArgumentNullException("item");
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Customer");
           var brokenRules = new ObjectCollection<ValidationResult>();
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "31952c83-2fdc-4d33-8360-8ac6750a0cb9");
           var rc = new RuleContext(item);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("CreateOrder");
            }
            return Json(new {success = true, status="OK", id = item.CustomerId});
       }
//exec:i7
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> i7([RequestBody]Customer item)
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           if(null == item) throw new ArgumentNullException("item");
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Customer");
           var brokenRules = new ObjectCollection<ValidationResult>();
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "6ce2606f-d5e0-4365-8e0f-52644cae339a");
           var rc = new RuleContext(item);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("i7");
            }
            return Json(new {success = true, status="OK", id = item.CustomerId});
       }
//exec:validate
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> Validate(string id,[RequestBody]Customer item)
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           if(null == item) throw new ArgumentNullException("item");
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Customer");
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

   
            return Json(new {success = true, status="OK", id = item.CustomerId});
       }
//exec:Remove
       [HttpDelete]
       public async Task<System.Web.Mvc.ActionResult> Remove(int id)
       {

            var repos = ObjectBuilder.GetObject<IRepository<Customer>>();
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
            return Json(new {success = true, status="OK", id = item.CustomerId});
       }
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var context = new SphDataContext();
           var ed = await context.LoadOneAsync<EntityDefinition>(t => t.Name == "Customer");
           var script = await ed.GenerateCustomXsdJavascriptClassAsync();
           this.Response.ContentType = "application/javascript";
           return Content(script);
       }
}
}
