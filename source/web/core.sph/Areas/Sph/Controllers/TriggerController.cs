﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Dependencies;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize(Roles = "administrators,developers")]
    public class TriggerController : Controller
    {
        static TriggerController()
        {
            DeveloperService.Init();
        }

        public ActionResult Actions()
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var actions = from a in ds.ActionOptions
                          select
                              $@"
{{
    ""designer"" : {JsonConvert.SerializeObject(a.Metadata)},
    ""action"" : {a.Value.ToJsonString()}
}}";


            return Content($"[{string.Join(",", actions)}]", "application/json", Encoding.UTF8);
        }


        public ActionResult Action(string id, string type)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var action = ds.ActionOptions.Single(x => x.Value.GetType().GetShortAssemblyQualifiedName()
                .ToLowerInvariant() == type.Replace(",", ", ")).Value;
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


        public ActionResult Image(string id)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var action = ds.ActionOptions.Single(
                x => string.Equals(x.Metadata.TypeName, id, StringComparison.InvariantCultureIgnoreCase)).Value;



            using (var stream = new MemoryStream())
            {
                var img = action.GetPngIcon();
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                var byteArray = stream.ToArray();
                this.Response.ContentType = "image/png";
                return File(byteArray, "image/png");
            }
        }

        public async Task<ActionResult> Publish()
        {
            var trigger = this.GetRequestJson<Trigger>();
            if (string.IsNullOrWhiteSpace(trigger.Id)) throw new InvalidOperationException("You cannot publish unsaved trigger");
            trigger.IsActive = true;
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Publish");
            }


            return Json(trigger.Id);
        }

        public async Task<ActionResult> Depublish()
        {
            var trigger = this.GetRequestJson<Trigger>();
            trigger.IsActive = false;
            if (string.IsNullOrWhiteSpace(trigger.Id)) throw new InvalidOperationException("You cannot depublish unsaved trigger");
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Depublish");
            }


            return Json(trigger.Id);
        }

        public async Task<ActionResult> Remove()
        {
            var trigger = this.GetRequestJson<Trigger>();
            trigger.IsActive = false;
            if (string.IsNullOrWhiteSpace(trigger.Id)) throw new InvalidOperationException("You cannot depublish unsaved trigger");
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Delete(trigger);
                await session.SubmitChanges("Depublish");
            }


            return Json(trigger.Id);
        }

        public async Task<ActionResult> Save()
        {
            var trigger = this.GetRequestJson<Trigger>();

            var newItem = trigger.IsNewItem;
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Name == trigger.Entity);
            trigger.TypeOf = ed.FullTypeName;

            if (newItem)
                trigger.Id = (trigger.Entity + "-" + trigger.Name).ToIdFormat();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Save");
            }

            if (newItem)
            {
                trigger.ActionCollection.OfType<SetterAction>()
                    .ToList()
                    .ForEach(s => s.TriggerId = trigger.Id);

                using (var session = context.OpenSession())
                {
                    session.Attach(trigger);
                    await session.SubmitChanges("Submit trigger");
                }
            }

            return Json(trigger.Id);
        }

    }
}
