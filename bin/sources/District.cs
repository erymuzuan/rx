using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Dev_5004.Domain
{
   public class District : Entity
   {
   private int m_districtId;
   public int DistrictId
   {
       get{ return m_districtId;}
       set{ m_districtId = value;}
   }
     
        public override string ToString()
        {
            return "District:" + Name;
        }     
        public override void SetId(int id)
        {
            m_districtId = id;
        }     
        public override int GetId()
        {
            return m_districtId;
        }
//member:Name
          private System.String m_name;
   public System.String Name
   {
       get{ return m_name;}
       set{ m_name = value;}
   }

//member:Postcode
          private System.String m_postcode;
   public System.String Postcode
   {
       get{ return m_postcode;}
       set{ m_postcode = value;}
   }

//member:State
          private System.String m_state;
   public System.String State
   {
       get{ return m_state;}
       set{ m_state = value;}
   }

   }
//class:Name

//class:Postcode

//class:State

public partial class DistrictController : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = "dev/district/_search";

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
       public async Task<System.Web.Mvc.ActionResult> Save([RequestBody]District item)
       {

            if(null == item) throw new ArgumentNullException("item");
            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("save");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new {success = true, status="OK", id = item.DistrictId});
       }
//exec:validate
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> Validate(string id,[RequestBody]District item)
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           if(null == item) throw new ArgumentNullException("item");
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "District");
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

   
            return Json(new {success = true, status="OK", id = item.DistrictId});
       }
//exec:Remove
       [HttpDelete]
       public async Task<System.Web.Mvc.ActionResult> Remove(int id)
       {

            var repos = ObjectBuilder.GetObject<IRepository<District>>();
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
            return Json(new {success = true, status="OK", id = item.DistrictId});
       }
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var context = new SphDataContext();
           var ed = await context.LoadOneAsync<EntityDefinition>(t => t.Name == "District");
           var script = await ed.GenerateCustomXsdJavascriptClassAsync();
           this.Response.ContentType = "application/javascript";
           return Content(script);
       }
}
}
