using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    public class WorkflowController : Controller
    {
        public async Task<ActionResult> StartWorkflow()
        {
            var json = this.GetRequestJson<string>();
            var context = new SphDataContext();
            dynamic obj = JsonConvert.DeserializeObject(json);
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.WorkflowDefinitionId == 1);
            var wf = new Workflow();
            foreach (var w in wd.VariableDefinitionCollection)
            {
                var name = w.Name;
                var value = obj[w.Name];
                var v = new VariableValue { Name = name, Value = value };
                wf.VariableValueCollection.Add(v);
            }
            
            return Content("");
        }

        public async Task<ActionResult> Start(int id)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.WorkflowDefinitionId == id);
            var screen = wd.GetInitiatorActivity() as ScreenActivity;
            if(null == screen)throw new InvalidOperationException("The start activity is not of type ScreenActivity for " + wd.Name);
            var vm = new WorkflowStartViewModel { WorkflowDefinition = wd, Screen = screen };
         
            return View(vm);
        }

    }

    public class WorkflowStartViewModel
    {
        public WorkflowDefinition WorkflowDefinition { get; set; }
        public ScreenActivity Screen { get; set; }
    }
}