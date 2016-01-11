using System.Web.Mvc;
using Bespoke.Sph.Web.Dependencies;

namespace Bespoke.Sph.Web.Controllers
{
    public class BaseController : Controller
    {
        static BaseController()
        {
            DeveloperService.Init();
        }
    }
}