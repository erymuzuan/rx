using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize(Roles = "administrators,developers")]
    public class TriggerController : BaseApiController
    {
        public IHttpActionResult Actions()
        {
            var actions = from a in this.DeveloperService.ActionOptions
                          select
                              $@"
{{
    ""designer"" : {JsonConvert.SerializeObject(a.Metadata)},
    ""action"" : {a.Value.ToJsonString()}
}}";


            return Json($"[{string.Join(",", actions)}]");
        }


        public IHttpActionResult Action(string id, string type)
        {
            var action = this.DeveloperService.ActionOptions.Single(x => x.Value.GetType().GetShortAssemblyQualifiedName()
                .ToLowerInvariant() == type.Replace(",", ", ")).Value;
            if (id == "js")
            {
                var js = action.GetEditorViewModel();
                return Javascript(js);
            }
            var html = action.GetEditorView();
            return Html(html);
        }


        public IHttpActionResult Image(string id)
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
                return File(byteArray, "image/png");
            }
        }

        public async Task<IHttpActionResult> Publish([JsonBody]Trigger trigger)
        {
            if (string.IsNullOrWhiteSpace(trigger.Id)) throw new InvalidOperationException("You cannot publish unsaved trigger");
            trigger.IsActive = true;
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Publish");
            }


            return Ok();
        }

        public async Task<IHttpActionResult> Depublish([JsonBody]Trigger trigger)
        {
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

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var context = new SphDataContext();
            var trigger = await context.LoadOneAsync<Trigger>(x => x.Id == id);
            if (null == trigger) return NotFound();


            trigger.IsActive = false;
            if (string.IsNullOrWhiteSpace(trigger.Id)) throw new InvalidOperationException("You cannot depublish unsaved trigger");
            using (var session = context.OpenSession())
            {
                session.Delete(trigger);
                await session.SubmitChanges("Depublish");
            }


            return Json(trigger.Id);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Save([JsonBody]Trigger trigger)
        {
            if (null == trigger) return BadRequest("Cannot read trigger from the request body");

            var baru = trigger.IsNewItem;
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Name == trigger.Entity);
            trigger.TypeOf = ed.FullTypeName;

            if (baru)
                trigger.Id = (trigger.Entity + "-" + trigger.Name).ToIdFormat();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Save");
            }

            if (!baru) return Ok(trigger.Id);

            trigger.ActionCollection.OfType<SetterAction>()
                .ToList()
                .ForEach(s => s.TriggerId = trigger.Id);

            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Submit trigger");
            }
            return Created($"/api/triggers/{trigger.Id}", new { });

        }

    }
}
