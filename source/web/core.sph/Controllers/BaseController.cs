using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    public class BaseController : Controller
    {
        protected ILogger Logger => ObjectBuilder.GetObject<ILogger>();
        static BaseController()
        {
            DeveloperService.Init();
        }
    }
}