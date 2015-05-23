using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Humanizer;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("entity-definition")]
    public class EntityDefinition2Controller : Controller
    {
        public const string ED_SCHEMA = "ed-schema";

        private void DeleteEdSchemaCache()
        {
            System.Web.HttpContext.Current.Cache.Remove(ED_SCHEMA);
        }

        [HttpGet]
        [Route("variable-path/{id}")]
        public async Task<ActionResult> GetVariablePath(string id)
        {
            var context = new SphDataContext();

            var ed = await context.LoadOneAsync<EntityDefinition>(w => w.Id == id);
            if (null == ed) return new HttpNotFoundResult("Cannot find EntityDefinition with Id = " + id);
            var list = ed.GetMembersPath();
            return Json(list, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save()
        {
            this.DeleteEdSchemaCache();
            var ed = this.GetRequestJson<EntityDefinition>();
            var context = new SphDataContext();
            var canSave = ed.CanSave();
            if (!canSave.Result)
            {
                return Json(new { success = false, status = "ERROR", message = "Your entity cannot be save", errors = canSave.Errors.ToArray() });
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
                return Json(new { success = true, status = "OK", message = "Your entity has been successfully saved ", id = ed.Id });

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
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully saved ", id = ed.Id });


        }

        [HttpGet]
        [Route("plural/{id}")]
        public ActionResult GetPlural(string id)
        {
            return Content(id.Pluralize());
        }


        [HttpGet]
        [Route("schema")]
        public async Task<ActionResult> Schemas()
        {
            this.Response.ContentType = "application/javascript";

            var cached = System.Web.HttpContext.Current.Cache.Get(ED_SCHEMA) as string;
            if (null != cached) return Content(cached);

            var context = new SphDataContext();
            var query = context.EntityDefinitions;
            var lo = await context.LoadAsync(query, includeTotalRows: true);
            var list = new ObjectCollection<EntityDefinition>(lo.ItemCollection);

            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true);
                list.AddRange(lo.ItemCollection);
            }

            var script = new StringBuilder();
            foreach (var ef in list)
            {
                var code = await ef.GenerateCustomXsdJavascriptClassAsync();
                script.AppendLine(code);
            }

            System.Web.HttpContext.Current.Cache.Insert(ED_SCHEMA, script.ToString());
            return Content(script.ToString());
        }



        [HttpPost]
        [Route("depublish")]
        public async Task<ActionResult> Depublish()
        {
            this.DeleteEdSchemaCache();
            var context = new SphDataContext();
            var ed = this.GetRequestJson<EntityDefinition>();

            ed.IsPublished = false;
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Depublish");
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully depublished", id = ed.Id });


        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            this.DeleteEdSchemaCache();
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == id);
            if (null == ed) return new HttpNotFoundResult("Cannot find entity definition to delete, id : " + id);

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
            return Json(new { success = true, status = "OK", message = "Your entity definition has been successfully deleted", id = ed.Id });

        }


        [HttpPost]
        [Route("publish")]
        public async Task<ActionResult> Publish()
        {
            this.DeleteEdSchemaCache();
            var context = new SphDataContext();
            var ed = this.GetRequestJson<EntityDefinition>();
            var buildValidation = await ed.ValidateBuildAsync();


            if (!buildValidation.Result)
                return Json(buildValidation);

            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.UserSourceDirectory
            };
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll"));

            var codes = ed.GenerateCode();
            var sources = ed.SaveSources(codes);
            var result = ed.Compile(options, sources);

            result.Errors.ForEach(Console.WriteLine);
            if (!result.Result)
                return Json(result);



            ed.IsPublished = true;
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully published", id = ed.Id });


        }

    }
}