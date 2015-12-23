using System.ComponentModel.Composition;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Dependencies;

namespace Bespoke.Sph.Web.Controllers
{
    public class BaseController : Controller
    {
        static BaseController()
        {
            DeveloperService.Init();
        }
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }

        public BaseController()
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            this.BuildDiagnostics = ds.BuildDiagnostics;
        }
    }
}