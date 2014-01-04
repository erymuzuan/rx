using System.Web.Mvc;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class WorkflowController : BaseAppController
    {
        public ActionResult ActivityScreenHtml()
        {
            this.Server.TransferRequest("/WorkflowDefinition/ScreenHtml");
            return Content("");

        }
        public ActionResult ActivityScreenJs()
        {
            this.Server.TransferRequest("/WorkflowDefinition/ScreenJs");
            return Content("");

        }
    }
}