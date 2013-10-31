using System.Web.Mvc;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class AppController
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