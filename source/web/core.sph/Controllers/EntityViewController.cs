using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/entity-views")]
    public class EntityViewController : BaseApiController
    {
        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> Save([JsonBody]EntityView view)
        {
            var context = new SphDataContext();
            view.Entity = view.EntityDefinitionId;

            var baru = string.IsNullOrWhiteSpace(view.Id) || view.Id == "0";
            if (baru)
                view.Id = $"{view.EntityDefinitionId}-{view.Route.ToIdFormat()}";

            using (var session = context.OpenSession())
            {
                session.Attach(view);
                await session.SubmitChanges("Save");
            }
            var content = new
            {
                id = view.Id,
                success = true,
                status = "OK",
                message = $"You view has been successfuly saved : {view.Id}",
                _links = new[]
                {
                    new { rel = "self", href = $"/api/entity-views/{view.Id}", method = "GET"},
                    new { rel = "publish", href = $"/api/entity-views/{view.Id}/publish", method = "PUT"},
                    new { rel = "depublish", href = $"/api/entity-views/{view.Id}/depublish", method = "PUT"},
                    new { rel = "delete", href = $"/api/entity-views/{view.Id}", method = "DELECT"}
                }
            };
            if (baru)
            {
                return Created($"/api/entity-views/{view.Id}", content);
            }
            return Ok(content);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var context = new SphDataContext();
            var view = context.LoadOneFromSources<EntityView>(x => x.Id == id);
            if (null == view) return NotFound();


            using (var session = context.OpenSession())
            {
                session.Delete(view);
                await session.SubmitChanges("Delete");
            }
            return Json(new { success = true, status = "OK", id = view.Id });
        }

        [HttpPut]
        [Route("{id}/depublish")]
        public async Task<IHttpActionResult> Depublish([JsonBody]EntityView view, string id)
        {
            var context = new SphDataContext();

            view.IsPublished = false;
            using (var session = context.OpenSession())
            {
                session.Attach(view);
                await session.SubmitChanges("Depublish");
            }
            var content = new
            {
                id = view.Id,
                success = true,
                status = "OK",
                message = $"You view has been successfuly depublished : {id}",
                _links = new[]
             {
                    new { rel = "self", href = $"/api/entity-views/{view.Id}", method = "GET"},
                    new { rel = "publish", href = $"/api/entity-views/{view.Id}/publish", method = "PUT"},
                    new { rel = "depublish", href = $"/api/entity-views/{view.Id}/depublish", method = "PUT"},
                    new { rel = "delete", href = $"/api/entity-views/{view.Id}", method = "DELECT"}
                }
            };
            return Ok(content);


        }


        [HttpPut]
        [Route("{id}/publish")]
        public async Task<IHttpActionResult> Publish([JsonBody]EntityView view, string id)
        {
            view.Entity = view.EntityDefinitionId;
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == view.EntityDefinitionId);

            var buildValidation = await view.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            view.IsPublished = true;
            using (var session = context.OpenSession())
            {
                session.Attach(view);
                await session.SubmitChanges("Publish");
            }
            var content = new
            {
                id = view.Id,
                success = true,
                status = "OK",
                message = $"You view has been successfuly published : {id}",
                _links = new[]
  {
                    new { rel = "self", href = $"/api/entity-views/{view.Id}", method = "GET"},
                    new { rel = "publish", href = $"/api/entity-views/{view.Id}/publish", method = "PUT"},
                    new { rel = "depublish", href = $"/api/entity-views/{view.Id}/depublish", method = "PUT"},
                    new { rel = "delete", href = $"/api/entity-views/{view.Id}", method = "DELECT"}
                }
            };
            return Ok(content);
        }




        [AllowAnonymous]
        //[RxSourceOutputCache(SourceType = typeof(EntityView))]
        [Route("{entity}/dashboard")]
        [HttpGet]
        public async Task<IHttpActionResult> Dashboard(string id)
        {
            var user = User.Identity.Name;
            var views =
                from f in Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\EntityView\\", "*.json")
                let v = f.DeserializeFromJsonFile<EntityView>()
                where v.IsPublished
                && v.DisplayOnDashboard
                && string.Equals(v.EntityDefinitionId, id, StringComparison.InvariantCultureIgnoreCase)
                select v;

            var list = new ObjectCollection<EntityView>();
            foreach (var v in views)
            {
                if (!v.Performer.Validate()) continue;
                if (v.RouteParameterCollection.Any()) continue;

                if (v.Performer.IsPublic)
                {
                    list.Add(v);
                    continue;
                }
                var users = await v.Performer.GetUsersAsync(v);
                if (users.Contains(user))
                    list.Add(v);
            }

            var jsonViews = string.Join(",", list.Select(c => c.ToJsonString(Newtonsoft.Json.Formatting.Indented)));
            return Ok($"[{jsonViews}]");
        }
    }
}