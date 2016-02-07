using System.Web.Mvc;
using Bespoke.Sph.WebApi;

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