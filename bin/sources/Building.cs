using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Dev_2001.Domain
{
   public class Building : Entity
   {
   private int m_buildingId;
   public int BuildingId
   {
       get{ return m_buildingId;}
       set{ m_buildingId = value;}
   }
     
        public override string ToString()
        {
            return "Building:" + Name;
        }     
        public override void SetId(int id)
        {
            m_buildingId = id;
        }     
        public override int GetId()
        {
            return m_buildingId;
        }
//member:Name
          private System.String m_name;
   public System.String Name
   {
       get{ return m_name;}
       set{ m_name = value;}
   }

//member:Name2
          private System.String m_name2;
   public System.String Name2
   {
       get{ return m_name2;}
       set{ m_name2 = value;}
   }

//member:Contact
          private Contact m_contact = new Contact();
   public Contact Contact
   {
       get{ return m_contact;}
       set{ m_contact = value;}
   }

//member:Owner
          private Owner m_owner = new Owner();
   public Owner Owner
   {
       get{ return m_owner;}
       set{ m_owner = value;}
   }

//member:BlockCollection
          private readonly ObjectCollection<Block> m_blockCollection = new ObjectCollection<Block>();
   public ObjectCollection<Block> BlockCollection
   {
       get{ return m_blockCollection;}
   }

//member:BuildingAddress
          private BuildingAddress m_buildingAddress = new BuildingAddress();
   public BuildingAddress BuildingAddress
   {
       get{ return m_buildingAddress;}
       set{ m_buildingAddress = value;}
   }

   }
//class:Name

//class:Name2

//class:Contact
   public class Contact: DomainObject
   {
   private System.String m_name;
   public System.String Name
   {
       get{ return m_name;}
       set{ m_name = value;}
   }

   private ContactAddress m_contactAddress = new ContactAddress();
   public ContactAddress ContactAddress
   {
       get{ return m_contactAddress;}
       set{ m_contactAddress = value;}
   }

   }

   public class ContactAddress: DomainObject
   {
   private System.String m_street;
   public System.String Street
   {
       get{ return m_street;}
       set{ m_street = value;}
   }

   private System.String m_city;
   public System.String City
   {
       get{ return m_city;}
       set{ m_city = value;}
   }

   private System.String m_postcode;
   public System.String Postcode
   {
       get{ return m_postcode;}
       set{ m_postcode = value;}
   }

   private System.String m_state;
   public System.String State
   {
       get{ return m_state;}
       set{ m_state = value;}
   }

   }






//class:Owner
   public class Owner: DomainObject
   {
   private System.String m_name;
   public System.String Name
   {
       get{ return m_name;}
       set{ m_name = value;}
   }

   private Address m_address = new Address();
   public Address Address
   {
       get{ return m_address;}
       set{ m_address = value;}
   }

   }

   public class Address: DomainObject
   {
   private System.String m_street;
   public System.String Street
   {
       get{ return m_street;}
       set{ m_street = value;}
   }

   private System.String m_city;
   public System.String City
   {
       get{ return m_city;}
       set{ m_city = value;}
   }

   private System.String m_postcode;
   public System.String Postcode
   {
       get{ return m_postcode;}
       set{ m_postcode = value;}
   }

   private System.String m_state;
   public System.String State
   {
       get{ return m_state;}
       set{ m_state = value;}
   }

   }






//class:BlockCollection
   public class Block: DomainObject
   {
   private System.String m_name;
   public System.String Name
   {
       get{ return m_name;}
       set{ m_name = value;}
   }

   private System.Int32 m_size;
   public System.Int32 Size
   {
       get{ return m_size;}
       set{ m_size = value;}
   }

   }



//class:BuildingAddress
   public class BuildingAddress: DomainObject
   {
   private System.String m_street;
   public System.String Street
   {
       get{ return m_street;}
       set{ m_street = value;}
   }

   private System.String m_city;
   public System.String City
   {
       get{ return m_city;}
       set{ m_city = value;}
   }

   private System.String m_postocde;
   public System.String Postocde
   {
       get{ return m_postocde;}
       set{ m_postocde = value;}
   }

   }




public partial class BuildingController : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = "dev/building/_search";

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
       public async Task<System.Web.Mvc.ActionResult> Save([RequestBody]Building item)
       {

            if(null == item) throw new ArgumentNullException("item");
            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("save");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new {success = true, status="OK", id = item.BuildingId});
       }
//exec:validate
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> Validate(string id,[RequestBody]Building item)
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           if(null == item) throw new ArgumentNullException("item");
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Building");
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

   
            return Json(new {success = true, status="OK", id = item.BuildingId});
       }
//exec:Remove
       [HttpDelete]
       public async Task<System.Web.Mvc.ActionResult> Remove(int id)
       {

            var repos = ObjectBuilder.GetObject<IRepository<Building>>();
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
            return Json(new {success = true, status="OK", id = item.BuildingId});
       }
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var context = new SphDataContext();
           var ed = await context.LoadOneAsync<EntityDefinition>(t => t.Name == "Building");
           var script = await ed.GenerateCustomXsdJavascriptClassAsync();
           this.Response.ContentType = "application/javascript";
           return Content(script);
       }
}
}
