using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class TriggerController : Controller
    {

        public async Task<ActionResult> Publish()
        {
            var trigger = this.GetRequestJson<Trigger>();
            if(trigger.TriggerId == 0)throw new InvalidOperationException("You cannot publish unsaved trigger");
            trigger.IsActive = true;
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Publish");
            }


            return Json(trigger.TriggerId);
        }
        public async Task<ActionResult> Depublish()
        {
            var trigger = this.GetRequestJson<Trigger>();
            trigger.IsActive = false;
            if(trigger.TriggerId == 0)throw new InvalidOperationException("You cannot depublish unsaved trigger");
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Depublish");
            }


            return Json(trigger.TriggerId);
        }

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
