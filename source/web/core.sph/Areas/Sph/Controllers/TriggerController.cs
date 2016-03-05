using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize(Roles = "administrators,developers")]
    [RoutePrefix("api/triggers")]
    public class TriggerController : BaseApiController
    {
        [HttpGet]
        [Route("actions")]
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


        [HttpGet]
        [Route("actions/{id}/image")]
        public IHttpActionResult GetImage(string id)
        {
            var action = this.DeveloperService.ActionOptions.Single(
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
        [HttpGet]
        [Route("actions/{type}/viewmodels")]
        public IHttpActionResult GetActionViewModel(string type)
        {
            var action = this.DeveloperService.ActionOptions.SingleOrDefault(x => x.GetTypeName().ToLowerInvariant() == type.Replace(",", ", "));
            if (null == action) return NotFound($"Cannot find viewmodels for action dialog : {type}");

            var js = action.Value.GetEditorViewModel();
            return Javascript(js);
        }

        [HttpGet]
        [Route("actions/{type}/views")]
        public IHttpActionResult GetActionViewl(string type)
        {
            var action = this.DeveloperService.ActionOptions.SingleOrDefault(x => x.GetTypeName().ToLowerInvariant() == type.Replace(",", ", "));
            if (null == action) return NotFound($"Cannot find views for action dialog : {type}");

            var html = action.Value.GetEditorView();
            return Html(html);
        }



        [HttpPut]
        [Route("{id}/publish")]
        public async Task<IHttpActionResult> Publish([JsonBody]Trigger trigger, string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new InvalidOperationException("You cannot publish unsaved trigger");
            trigger.IsActive = true;

            var result = await trigger.CompileAsync();
            if (result.Result)
                return Json(new {success = false, errors = result.Errors});

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Publish");
            }


            return Ok(new { success = true, status = "OK", output = result.Output, message = "Your trigger publishing is ACCEPTEP, please check your output directory" });
        }

        [HttpPut]
        [Route("{id}/depublish")]
        public async Task<IHttpActionResult> Depublish([JsonBody]Trigger trigger, string id)
        {
            trigger.IsActive = false;
            if (string.IsNullOrWhiteSpace(id)) throw new InvalidOperationException("You cannot depublish unsaved trigger");
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
            return Created($"/api/triggers/{trigger.Id}", new { id =  trigger.Id});

        }

    }
}
