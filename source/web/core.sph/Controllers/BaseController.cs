using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    // TODO : move to BaseApiController, use MVC for Razor/js/html/css etc..
    public class BaseController : Controller
    {
        protected ILogger Logger => ObjectBuilder.GetObject<ILogger>();
        static BaseController()
        {
            DeveloperService.Init();
        }
    }
}