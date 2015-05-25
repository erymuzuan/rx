using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.DevV1_appointment.Domain
{
    public partial class AppointmentController : System.Web.Mvc.Controller
    {
        //exec:Search
        public async Task<System.Web.Mvc.ActionResult> Search()
        {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = "devv1/appointment/_search";

            using (var client = new System.Net.Http.HttpClient())
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

            if (null == item) throw new ArgumentNullException("item");
            var context = new Bespoke.Sph.Domain.SphDataContext();
            if (item.IsNewItem) item.Id = Guid.NewGuid().ToString();

            using (var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("save");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new { success = true, status = "OK", id = item.Id, href = "appointment/" + item.Id });
        }
        //exec:Register
        [HttpPost]
        public async Task<System.Web.Mvc.ActionResult> Register([RequestBody]Appointment item)
        {
            var context = new Bespoke.Sph.Domain.SphDataContext();
            if (null == item) throw new ArgumentNullException("item");
            var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Appointment");
            var brokenRules = new ObjectCollection<ValidationResult>();

            var appliedRules1 = ed.BusinessRuleCollection.Where(b => b.Name == "Date");
            ValidationResult result1 = item.ValidateBusinessRule(appliedRules1);

            if (!result1.Success)
            {
                brokenRules.Add(result1);
            }
            if (brokenRules.Count > 0) return Json(new { success = false, rules = brokenRules.ToArray() });

            var operation = ed.EntityOperationCollection.Single(o => o.WebId == "b2dc31c8-955d-4d54-a9ef-6799e9775d96");
            var rc = new RuleContext(item);
            var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "51201976-4ff9-4c66-a7d6-9d266b5aa2b0");
            item.ReferenceNo = (System.String)setter1.Field.GetValue(rc);

            if (item.IsNewItem) item.Id = Guid.NewGuid().ToString();

            using (var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("Register");
            }
            return Json(new { success = true, message = "Done", status = "OK", id = item.Id });
        }
        //exec:validate
        [HttpPost]
        public async Task<System.Web.Mvc.ActionResult> Validate(string id, [RequestBody]Appointment item)
        {
            var context = new Bespoke.Sph.Domain.SphDataContext();
            if (null == item) throw new ArgumentNullException("item");
            var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Appointment");
            var brokenRules = new ObjectCollection<ValidationResult>();
            var rules = id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var r in rules)
            {
                var appliedRules = ed.BusinessRuleCollection.Where(b => b.Name == r);
                ValidationResult result = item.ValidateBusinessRule(appliedRules);

                if (!result.Success)
                {
                    brokenRules.Add(result);
                }
            }
            if (brokenRules.Count > 0) return Json(new { success = false, rules = brokenRules.ToArray() });


            return Json(new { success = true, status = "OK", id = item.Id });
        }
        //exec:Remove
        [HttpDelete]
        public async Task<System.Web.Mvc.ActionResult> Remove(string id)
        {

            var repos = ObjectBuilder.GetObject<IRepository<Appointment>>();
            var item = await repos.LoadOneAsync(id);
            if (null == item)
                return new HttpNotFoundResult();

            var context = new Bespoke.Sph.Domain.SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Delete(item);
                await session.SubmitChanges("delete");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new { success = true, status = "OK", id = item.Id });
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
