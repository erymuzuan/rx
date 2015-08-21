using System.ComponentModel.Composition;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public class BaseController : Controller
    {
        [ImportMany(typeof(IBuildDiagnostics))]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }

        public BaseController()
        {
            ObjectBuilder.ComposeMefCatalog(this);
        }
    }
}