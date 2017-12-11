using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Extensions;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("document-template")]
    public class DocumentTemplateController : Controller
    {
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save()
        {
            var dt = this.GetRequestJson<DocumentTemplate>();
            if (string.IsNullOrWhiteSpace(dt.Name))
                throw new InvalidOperationException("Document template name cannot be empty");
            if (dt.IsNewItem)
                dt.Id = dt.Name.ToIdFormat();

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(dt);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = dt.Id });
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            var context = new SphDataContext();
            var dt = context.LoadOneFromSources<DocumentTemplate>(x => x.Id == id);
            using (var session = context.OpenSession())
            {
                session.Delete(dt);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = dt.Id });
        }


        [HttpPost]
        [Route("publish")]
        public async Task<ActionResult> Publish()
        {
            var context = new SphDataContext();
            var template = this.GetRequestJson<DocumentTemplate>();
            template.IsPublished = true;
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Name == template.Entity);

            var buildValidation = await template.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your template has been successfully published", id = template.Id });

        }

        [HttpGet]
        [Route("transform/{entity}/{itemId}/{templateId}")]
        public async Task<ActionResult> Transform(string itemId, string entity, string templateId)
        {

            var context = new SphDataContext();
            var template = await context.LoadOneAsync<DocumentTemplate>(e => e.Id == templateId);
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Name == entity);

            var buildValidation = await template.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            var resolved = ObjectBuilder.GetObject<ICustomEntityDependenciesResolver>()
                .ResolveRepository(ed);
            var item = await resolved.Implementation.LoadOneAsync(itemId);

            var file = System.IO.Path.GetTempFileName() + ".docx";
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = await store.GetContentAsync(template.WordTemplateStoreId);
            System.IO.File.WriteAllBytes(file, content.Content);

            var wordGen = ObjectBuilder.GetObject<IDocumentGenerator>();
            wordGen.Generate(file, item);


            return File(System.IO.File.ReadAllBytes(file), MimeMapping.GetMimeMapping(file),$"{template.Name}.{item.Id}.docx");

        }
    }
}