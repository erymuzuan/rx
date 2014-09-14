using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
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
            if (string.IsNullOrWhiteSpace(dt.Id) || dt.Id == "0")
                dt.Id = Guid.NewGuid().ToString();

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(dt);
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

            var sqlAssembly = Assembly.Load("sql.repository");
            var sqlRepositoryType = sqlAssembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");

            var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + ed.Name);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed.Id, ed.Name);
            var edType = edAssembly.GetType(edTypeName);
            if (null == edType)
                Console.WriteLine("Cannot create type " + edTypeName);


            var reposType = sqlRepositoryType.MakeGenericType(edType);
            dynamic repository = Activator.CreateInstance(reposType);
            var item = await repository.LoadOneAsync(itemId);

            var file = System.IO.Path.GetTempFileName() + ".docx";
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = await store.GetContentAsync(template.WordTemplateStoreId);
            System.IO.File.WriteAllBytes(file, content.Content);

            var wordGen = ObjectBuilder.GetObject<IDocumentGenerator>();
            wordGen.Generate(file, item);


            return File(System.IO.File.ReadAllBytes(file), MimeMapping.GetMimeMapping(file), string.Format("{0}.{1}.docx", template.Name, item.GetId()));

        }
    }
}