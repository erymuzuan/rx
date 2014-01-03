using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public class TriggerController : Controller
    {

        public async Task<ActionResult> Save()
        {
            var trigger = this.GetRequestJson<Trigger>();

            var newItem = trigger.TriggerId == 0;
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Submit trigger");
            }

            if (newItem)
            {
                trigger.ActionCollection.OfType<SetterAction>()
                    .ToList()
                    .ForEach(s => s.TriggerId = trigger.TriggerId);

                using (var session = context.OpenSession())
                {
                    session.Attach(trigger);
                    await session.SubmitChanges("Submit trigger");
                }
            }

            return Json(trigger.TriggerId);
        }

    }
}
