using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Humanizer;
using System.Web.Http;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("entity-definition")]
    public class EntityDefinition2Controller : ApiController
    {
        [HttpGet]
        [Route("variable-path/{id}")]
        public async Task<HttpResponseMessage> GetVariablePath(string id)
        {
            var context = new SphDataContext();

            var ed = await context.LoadOneAsync<EntityDefinition>(w => w.Id == id);
            if (null == ed) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cannot find EntityDefinition with Id = " + id);
            var list = ed.GetMembersPath();
            return Request.CreateResponse(HttpStatusCode.OK, list);

        }



        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Save([FromBody2]EntityDefinition ed)
        {
            ed = this.GetRequestBody(ed, t => !string.IsNullOrWhiteSpace(t.Name));

            var context = new SphDataContext();
            var canSave = ed.CanSave();
            if (!canSave.Result)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    status = "ERROR",
                    message = "Your entity cannot be save",
                    errors = canSave.Errors.ToArray()
                });
            }

            var brandNewItem = ed.IsNewItem;
            if (brandNewItem)
            {
                ed.Id = ed.Name.ToIdFormat();
            }
            else
            {
                using (var session = context.OpenSession())
                {
                    session.Attach(ed);
                    await session.SubmitChanges("Save");
                }
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    status = "OK",
                    message = "Your entity has been successfully saved ",
                    id = ed.Id
                });

            }

            var form = new EntityForm
            {
                Id = Guid.NewGuid().ToString(),
                Name = ed.Name + " details",
                Entity = ed.Name,
                Route = ed.Name.ToLowerInvariant() + "-details",
                EntityDefinitionId = ed.Id,
                IsDefault = true
            };
            var view = new EntityView
            {
                Id = Guid.NewGuid().ToString(),
                Entity = ed.Name,
                Name = "All " + ed.Plural,
                Route = ed.Plural.ToLowerInvariant() + "-all",
                EntityDefinitionId = ed.Id,
            };

            using (var session = context.OpenSession())
            {
                session.Attach(ed, form, view);
                await session.SubmitChanges("Save");
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, status = "OK", message = "Your entity has been successfully saved ", id = ed.Id });


        }

        [HttpGet]
        [Route("plural/{name}")]
        public HttpResponseMessage GetPlural(string name)
        {
            return Request.CreateResponse(HttpStatusCode.OK, name.Pluralize());
        }



        [HttpPost]
        [Route("depublish")]
        public async Task<HttpResponseMessage> Depublish([FromBody]EntityDefinition ed)
        {
            ed = this.GetRequestBody(ed, t => !string.IsNullOrWhiteSpace(t.Name));
            var context = new SphDataContext();
            ed.IsPublished = false;
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Depublish");
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, status = "OK", message = "Your entity has been successfully depublished", id = ed.Id });


        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == id);
            if (null == ed) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cannot find entity definition to delete, id : " + id);

            var formsTask = context.LoadAsync(context.EntityForms.Where(f => f.EntityDefinitionId == id));
            var viewsTask = context.LoadAsync(context.EntityViews.Where(f => f.EntityDefinitionId == id));
            var triggersTask = context.LoadAsync(context.Triggers.Where(f => f.Entity == id));
            await Task.WhenAll(formsTask, viewsTask, triggersTask);

            using (var session = context.OpenSession())
            {
                session.Delete(ed);
                session.Delete((await formsTask).ItemCollection.Cast<Entity>().ToArray());
                session.Delete((await viewsTask).ItemCollection.Cast<Entity>().ToArray());
                session.Delete((await triggersTask).ItemCollection.Cast<Entity>().ToArray());
                // TODO : drop the tables and elastic search mappings
                await session.SubmitChanges("delete");
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, status = "OK", message = "Your entity definition has been successfully deleted", id = ed.Id });

        }


  


        [HttpPost]
        [Route("publish/{generateSource:bool=false}")]
        public async Task<HttpResponseMessage> Publish([FromBody]EntityDefinition ed, bool generateSource = false)
        {
            ed = this.GetRequestBody(ed, t => !string.IsNullOrWhiteSpace(t.Name));

            var context = new SphDataContext();
            var buildValidation = await ed.ValidateBuildAsync();

            if (!buildValidation.Result)
                return Request.CreateResponse(HttpStatusCode.OK, buildValidation);

            var path = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath,
                string.Format("{0}.{1}", ConfigurationManager.ApplicationName, ed.Id));
            using (var stream = new FileStream(path, FileMode.Create))
            {
                var options = new CompilerOptions
                {
                    Emit = true,
                    Stream = stream
                };
                var result = await ed.CompileAsync(options);
                result.Errors.ForEach(Console.WriteLine);
                if (!result.Result)
                    return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            if (generateSource)
            {
                var codes = await ed.GenerateCodeAsync();
                ed.SaveSources(codes);
            }

            ed.IsPublished = true;
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Publish");
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, status = "OK", message = "Your entity has been successfully published", id = ed.Id });


        }

        
    }
}