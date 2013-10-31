using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public class WorkflowController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Start()
        {
            return Content("");
        }

        [HttpGet]
        public async Task<ActionResult> Start(int id)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.WorkflowDefinitionId == id);
            var screen = wd.GetInititorScreen();

            var vm = new WorkflowStartViewModel {WorkflowDefinition = wd, Screen = screen};

            return View(vm);
        } 

    }

    public class WorkflowStartViewModel 
    {
        public WorkflowDefinition WorkflowDefinition { get; set; }
        public ScreenActivity Screen { get; set; }
    }
}