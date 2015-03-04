using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

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
    }
}
