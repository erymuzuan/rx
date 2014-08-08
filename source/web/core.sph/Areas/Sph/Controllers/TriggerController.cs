﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize(Roles = "administrators,developers")]
    public class TriggerController : Controller
    {
        [ImportMany(typeof(CustomAction), AllowRecomposition = true)]
        public Lazy<CustomAction, IDesignerMetadata>[] ActionOptions { get; set; }

        public ActionResult Actions()
        {
            if (null == this.ActionOptions)
                ObjectBuilder.ComposeMefCatalog(this);

            var actions = from a in this.ActionOptions
                          select string.Format(@"
{{
    ""designer"" : {0},
    ""action"" : {1}
}}", JsonConvert.SerializeObject(a.Metadata), a.Value.ToJsonString());


            return Content("[" + string.Join(",", actions) + "]", "application/json", Encoding.UTF8);
        }


        public ActionResult Action(string id, string type)
        {
            if (null == this.ActionOptions)
                ObjectBuilder.ComposeMefCatalog(this);

            var action = this.ActionOptions.Single(x => x.Value.GetType().GetShortAssemblyQualifiedName()
                .ToLowerInvariant() == type.Replace(",",", ")).Value;
            if (id == "js")
            {
                this.Response.ContentType = "application/javascript";
                var js = action.GetEditorViewModel();
                return Content(js);
            }
            this.Response.ContentType = "text/html";
            var html = action.GetEditorView();
            return Content(html);
        }

        public async Task<ActionResult> Publish()
        {
            var trigger = this.GetRequestJson<Trigger>();
            if (trigger.TriggerId == 0) throw new InvalidOperationException("You cannot publish unsaved trigger");
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
            if (trigger.TriggerId == 0) throw new InvalidOperationException("You cannot depublish unsaved trigger");
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Depublish");
            }


            return Json(trigger.TriggerId);
        }

        public async Task<ActionResult> Remove()
        {
            var trigger = this.GetRequestJson<Trigger>();
            trigger.IsActive = false;
            if (trigger.TriggerId == 0) throw new InvalidOperationException("You cannot depublish unsaved trigger");
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Delete(trigger);
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
