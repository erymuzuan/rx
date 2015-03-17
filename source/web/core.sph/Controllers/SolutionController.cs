using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("solution")]
    public class SolutionController : Controller
    {
        [Route("open/{path}")]
        [HttpGet]
        public async Task<ActionResult> OpenAsync(string path)
        {
            var solution = await Solution.LoadAsync(path);
            var json = solution.ToJsonString(true);
            this.Response.ContentType = "application/json";
            return Content( json);

        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult> NewSolution([RequestBody]Solution solution)
        {
            await solution.CreateNewAsync();
            return Json(new { success = "true" });
        }
    }
}
