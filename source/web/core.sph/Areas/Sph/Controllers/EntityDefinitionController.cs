using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Humanizer;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EntityDefinitionController : Controller
    {
        public async Task<ActionResult> GetVariablePath(string id)
        {
            var context = new SphDataContext();
            int eid;
            if (int.TryParse(id, out eid))
            {
                var ed = await context.LoadOneAsync<EntityDefinition>(w => w.EntityDefinitionId == eid);
                if (null == ed) return new HttpNotFoundResult("Cannot find EntityDefinition with Id = " + eid);
                var list = ed.GetMembersPath();
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            var ed2 = await context.LoadOneAsync<EntityDefinition>(w => w.Name == id);
            if (null == ed2) return new HttpNotFoundResult("Cannot find EntityDefinition with Nmae = " + id);

            var list2 = ed2.GetMembersPath();
            return Json(list2, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Save()
        {
            var ed = this.GetRequestJson<EntityDefinition>();
            var context = new SphDataContext();

            var newItem = ed.EntityDefinitionId == 0;

            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Save");
            }
            if (newItem)
            {
                var form = new EntityForm
                {
                    Name = ed.Name + " details",
                    Route = ed.Name.ToLowerInvariant() + "-details",
                    EntityDefinitionId = ed.EntityDefinitionId,
                    IsDefault = true
                };
                var view = new EntityView
                {
                    Name = "All " + ed.Plural,
                    Route = ed.Plural.ToLowerInvariant() + "-all",
                    EntityDefinitionId = ed.EntityDefinitionId,
                };

                using (var session = context.OpenSession())
                {
                    session.Attach(form, view);
                    await session.SubmitChanges("Save");
                }
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully saved ", id = ed.EntityDefinitionId });


        }


        public ActionResult GetPlural(string id)
        {
            return Content(id.Pluralize());
        }
        public async Task<ActionResult> Schemas()
        {
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

            this.Response.ContentType = "application/javascript";
            return Content(script.ToString());
        }

        public async Task<ActionResult> Depublish()
        {
            var context = new SphDataContext();
            var ed = this.GetRequestJson<EntityDefinition>();

            ed.IsPublished = false;
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Depublish");
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully depublished", id = ed.EntityDefinitionId });


        }
        public async Task<ActionResult> Publish()
        {
            var context = new SphDataContext();
            var ed = this.GetRequestJson<EntityDefinition>();
            var buildValidation = await ed.ValidateBuildAsync();


            if (!buildValidation.Result)
                return Json(buildValidation);

            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.WorkflowSourceDirectory
            };
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll"));


            var result = ed.Compile(options);
            result.Errors.ForEach(Console.WriteLine);
            if (!result.Result)
                return Json(result);



            ed.IsPublished = true;
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully published", id = ed.EntityDefinitionId });


        }

        public ActionResult BusinessRuleDialog()
        {
            return View();
        }


    }
}