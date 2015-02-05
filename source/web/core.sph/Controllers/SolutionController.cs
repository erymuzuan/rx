using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("solution")]
    public class SolutionController : ApiController
    {
        [Route("open/{path}")]
        [HttpGet]
        public async Task<Solution> OpenAsync(string path)
        {
            var solution = await Solution.LoadAsync(path);
            return solution;

        }
    }
}
