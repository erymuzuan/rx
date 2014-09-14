﻿using System;
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

            var ed = await context.LoadOneAsync<EntityDefinition>(w => w.Id == id);
            if (null == ed) return new HttpNotFoundResult("Cannot find EntityDefinition with Id = " + id);
            var list = ed.GetMembersPath();
            return Json(list, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        public async Task<ActionResult> Save()
        {
            var ed = this.GetRequestJson<EntityDefinition>();
            var context = new SphDataContext();

            var existingItem = (await context.LoadOneAsync<EntityDefinition>(x => x.Id == ed.Name)) != null;
            if (!existingItem)
                ed.Id = ed.Name.ToLowerInvariant();

            if (existingItem)
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
                session.Attach(ed,form, view);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully saved ", id = ed.Id });


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
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully depublished", id = ed.Id });


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

        public ActionResult BusinessRuleDialog()
        {
            return View();
        }


    }
}