using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class BusinessRuleController : Controller
    {

        public async Task<ActionResult> Validate()
        {
            var id = this.Request.QueryString[0];
            var edName = id.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries).First();
            var rules = id.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);

            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == edName);
            var assembly = Assembly.Load(string.Format("{0}.{1}", ConfigurationManager.ApplicationName, edName));
            var type = assembly.GetType(string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed.Id, id));
            
            var json = this.GetRequestBody();
            dynamic item = JsonConvert.DeserializeObject(json, type, new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.All});
            
            var appliedRules = ed.BusinessRuleCollection.Where(b => rules.Contains(b.Name.Dehumanize()));
            ValidationResult result = item.ValidateBusinessRule(appliedRules);

            var setting = new JsonSerializerSettings{ContractResolver = new CamelCasePropertyNamesContractResolver()};
            var resultJson = JsonConvert.SerializeObject(result, setting);

            this.Response.ContentType = "application/javascript";
            return Content(resultJson);

        }
    }
}