using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Controllers
{
    public class WorkflowDefinitionController : Controller
    {
        public const string APPLICATION_JAVASCRIPT = "application/javascript";
        public const string TEXT_HTML = "text/html";

        public async Task<ActionResult> Save()
        {
            var wd = this.GetRequestJson<WorkflowDefinition>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(wd);
                await session.SubmitChanges();
            }
            return Json(wd.WorkflowDefinitionId);
        }
    
    }
}