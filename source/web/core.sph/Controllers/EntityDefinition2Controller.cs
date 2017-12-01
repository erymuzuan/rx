using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Domain.Extensions;
using Bespoke.Sph.Domain.Management;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
using Humanizer;
using WebGrease.Css.Extensions;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("entity-definition")]
    public class EntityDefinition2Controller : BaseController
    {
        [HttpGet]
        [Route("variable-path/{id}")]
        [RxSourceOutputCache(SourceType = typeof(EntityDefinition))]
        public async Task<ActionResult> GetVariablePath(string id)
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();

            var ed = await repos.LoadOneAsync<EntityDefinition>(w => w.Id == id) ??
                     (await repos.LoadOneAsync<EntityDefinition>(w => w.Name == id));
            if (null == ed) return new HttpNotFoundResult("Cannot find EntityDefinition with Id = " + id);
            var list = ed.GetMembersPath();
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [Route("{id}/members/{path}")]
        [RxSourceOutputCache(SourceType = typeof(EntityDefinition))]
        public async Task<ActionResult> GetMembersPath(string id, string path)
        {
            var context = new SphDataContext();

            var ed = await context.LoadOneAsync<EntityDefinition>(w => w.Id == id);
            if (null == ed) return new HttpNotFoundResult("Cannot find EntityDefinition with Id = " + id);
            var list = ed.GetMember(path);
            return Content(list.ToJsonString(true), "application/json");

        }



        [HttpPost]
        [Route("publish-dashboard")]
        public async Task<ActionResult> PublishDashboardAsync()
        {
            var context = new SphDataContext();
            var ed = this.GetRequestJson<EntityDefinition>();

            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("PublishDashboard");
            }
            return Json(new { success = true, status = "OK", message = "Your dashboard has been sent for publishing ", id = ed.Id });


        }


        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save()
        {
            // TODO : get the AttachedProperties, for the ed and members
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
                this.Response.StatusCode = (int)HttpStatusCode.Created;
            }
            else
            {
                using (var session = context.OpenSession())
                {
                    session.Attach(ed);
                    await session.SubmitChanges("Save");
                }

                this.Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { success = true, status = "OK", message = "Your entity has been successfully saved ", id = ed.Id });

            }


            var formId = ed.Name.ToIdFormat() + "-details";
            var form = new EntityForm
            {
                Id = formId,
                Name = ed.Name + " details",
                Entity = ed.Name,
                Route = formId,
                EntityDefinitionId = ed.Id,
                IsDefault = true
            };
            var viewId = ed.Name.ToIdFormat() + "-all";
            var view = new EntityView
            {
                Id = viewId,
                Entity = ed.Name,
                Name = "All " + ed.Plural,
                Route = viewId,
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
        [Route("singular/{id}")]
        public ActionResult GetSingular(string id)
        {
            return Content(id.Singularize(false));
        }


        [HttpGet]
        [Route("schema")]
        [RxSourceOutputCache(SourceType = typeof(EntityDefinition))]
        public async Task<ActionResult> Schemas()
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var list = await repos.LoadAsync<EntityDefinition>(x => x.IsPublished);

            var script = new StringBuilder();
            var provider = CodeGeneratorFactory.Instance.LanguageProviders.Single(x => x.Language == "javascript");
            foreach (var ef in list)
            {
                var sources = await provider.GenerateCodeAsync(ef);
                sources.ForEach(src => script.AppendLine(src.Value));
            }
            return JavaScript(script.ToString());
        }

        [HttpGet]
        [Route("export/{id}")]
        public async Task<ActionResult> Export(string id, [QueryString] bool includeData = false)
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var entity = await repos.LoadOneAsync<EntityDefinition>(e => e.Id == id);
            var package = new EntityDefinitionPackage();
            var zip = await package.PackAsync(entity, includeData);
            var file = $"{Path.GetFileNameWithoutExtension(zip)}_{Environment.MachineName}_{DateTime.Now:s}{(includeData ? "_data" : "")}.zip";
            return File(System.IO.File.ReadAllBytes(zip), MimeMapping.GetMimeMapping(zip), file);
        }

        [Route("upload")]
        public async Task<ActionResult> Upload(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                foreach (var postedFile in files)
                {
                    var fileName = Path.GetFileName(postedFile.FileName);
                    if (string.IsNullOrWhiteSpace(fileName)) throw new Exception("Filename is empty or null");


                    var zip = Path.Combine(Path.GetTempPath(), fileName);
                    postedFile.SaveAs(zip);

                    var folder = Directory.CreateDirectory(Path.GetTempFileName() + "extract").FullName;
                    var packager = new EntityDefinitionPackage();
                    var ed = await packager.UnpackAsync(zip, folder);

                    this.Response.ContentType = "application/javascript";
                    var result = new { success = true, zip, ed, folder };
                    return Content(result.ToJsonString());

                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, exception = e.GetType().FullName, message = e.Message, stack = e.StackTrace });
            }
            return Json(new { success = false });


        }

        [HttpPost]
        [Route("import")]
        public async Task<ActionResult> Import(string folder)
        {
            try
            {
                var packager = new EntityDefinitionPackage();
                var ed = await packager.ImportAsync(folder);

                this.Response.ContentType = "application/javascript";
                var result = new { success = true, ed };
                return Content(result.ToJsonString());
            }
            catch (Exception e)
            {
                return Json(new { success = false, exception = e.GetType().FullName, message = e.Message, stack = e.StackTrace });
            }
        }

        [HttpPost]
        [Route("import-data")]
        public async Task<ActionResult> ImportData(string folder)
        {
            try
            {
                var packager = new EntityDefinitionPackage();
                await packager.ImportDataAsync(folder);

                this.Response.ContentType = "application/javascript";
                var result = new { success = true };
                return Content(result.ToJson());
            }
            catch (Exception e)
            {
                return Json(new { success = false, exception = e.GetType().FullName, message = e.Message, stack = e.StackTrace });
            }
        }

        [HttpDelete]
        [Route("{name}/contents")]
        public async Task<ActionResult> TruncateData(string name)
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var ed = await repos.LoadOneAsync<EntityDefinition>(x => x.Name == name);

            if (ed.StoreInElasticsearch ?? false)
            {
                await ObjectBuilder.GetObject<IReadOnlyRepository>()
                       .TruncateAsync(ed.Name);
            }

            if (!ed.Transient)
            {
                // truncate SQL Table
                var management = ObjectBuilder.GetObject<IRepositoryManagement>();
                await management.TruncateDataAsync(ed);
                
            }
            return Json(new { success = true, message = "Data has been truncated", status = "OK" });

        }


        [HttpPost]
        [Route("depublish")]
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

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == id);
            if (null == ed) return new HttpNotFoundResult("Cannot find entity definition to delete, id : " + id);

            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var forms = await repos.LoadAsync<EntityForm>(f => f.EntityDefinitionId == id);
            var views = await repos.LoadAsync<EntityView>(f => f.EntityDefinitionId == id);
            var triggers = await repos.LoadAsync<Trigger>(f => f.Entity == id);

            using (var session = context.OpenSession())
            {
                session.Delete(ed);
                session.Delete(forms.Cast<Entity>().ToArray());
                session.Delete(views.Cast<Entity>().ToArray());
                session.Delete(triggers.Cast<Entity>().ToArray());
                // TODO : drop the repository and readonly repository data store and schema
                await session.SubmitChanges("delete");
            }
            return Json(new { success = true, status = "OK", message = "Your entity definition has been successfully deleted", id = ed.Id });

        }


        [HttpPost]
        [Route("publish")]
        public async Task<ActionResult> Publish()
        {
            var context = new SphDataContext();
            var ed = this.GetRequestJson<EntityDefinition>();
         
            var final =await ed.CompileAsync();
            if (!final.Result)
                return Json(final);
            
            ed.IsPublished = true;
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully published", id = ed.Id });


        }



        [HttpPost]
        [Route("publish/service-contract")]
        public async Task<ActionResult> PublishServiceContract()
        {
            var context = new SphDataContext();
            var ed = this.GetRequestJson<EntityDefinition>();
           /* TODO : find the service contract diagnostics
            ed.BuildDiagnostics = ObjectBuilder.GetObject<IDeveloperService>().BuildDiagnostics;

            var buildValidation = await ed.ValidateBuildAsync();
            if (!buildValidation.Result)
                return Json(buildValidation);*/
            var result = await ed.ServiceContract.CompileAsync(ed);

            result.Errors.ForEach(Console.WriteLine);
            if (!result.Result)
                return Json(result);



            ed.IsPublished = true;
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("PublishServiceContract");
            }
            return Json(new { success = true, status = "OK", message = "Your service contract has been successfully published", id = ed.Id });


        }

    }
}