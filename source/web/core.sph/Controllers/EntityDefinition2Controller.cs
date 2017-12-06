using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Domain.Extensions;
using Bespoke.Sph.Domain.Management;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.WebApi;
using Humanizer;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("entity-definition")]
    public class EntityDefinition2Controller : BaseApiController
    {
        [HttpGet]
        [Route("variable-path/{id}")]
        [RxSourceOutputCache(SourceType = typeof(EntityDefinition))]
        public async Task<IHttpActionResult> GetVariablePath(string id)
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();

            var ed = await repos.LoadOneAsync<EntityDefinition>(w => w.Id == id) ??
                     (await repos.LoadOneAsync<EntityDefinition>(w => w.Name == id));
            if (null == ed) return NotFound("Cannot find EntityDefinition with Id = " + id);
            var list = ed.GetMembersPath();
            return Json(list);

        }

        [HttpGet]
        [Route("{id}/members/{path}")]
        [RxSourceOutputCache(SourceType = typeof(EntityDefinition))]
        public async Task<IHttpActionResult> GetMembersPath(string id, string path)
        {
            var context = new SphDataContext();

            var ed = await context.LoadOneAsync<EntityDefinition>(w => w.Id == id);
            if (null == ed) return NotFound("Cannot find EntityDefinition with Id = " + id);
            var list = ed.GetMember(path);
            return Json(list.ToJsonString(true));

        }



        [HttpPost]
        [Route("publish-dashboard")]
        public async Task<IHttpActionResult> PublishDashboardAsync([JsonBody]EntityDefinition ed)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("PublishDashboard");
            }
            return Json(new { success = true, status = "OK", message = "Your dashboard has been sent for publishing ", id = ed.Id });


        }


        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Save([FromJsonProperty]EntityDefinition item,
            [FromJsonProperty("$.attachedProperties")]AttachedProperty[] properties)
        {
            (var _, object result) = await this.SaveAsync(item, properties);
            return Json(result);

        }

        [HttpPost]
        [Route("publish")]
        public async Task<IHttpActionResult> Publish([FromJsonProperty]EntityDefinition item,
            [FromJsonProperty]AttachedProperty[] attachedProperties)
        {
            await this.SaveAsync(item, attachedProperties);
            var result = await item.CompileAsync();
            if (!result.Result)
                return Json(result);


            return Json(new { success = true, status = "OK", message = "Your entity has been successfully published" });

        }

        private async Task<(bool, object)> SaveAsync(EntityDefinition ed, AttachedProperty[] properties)
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var canSave = ed.CanSave();
            if (!canSave.Result)
            {
                return (false, new { success = false, status = "ERROR", message = "Your entity cannot be save", errors = canSave.Errors.ToArray() });
            }

            var brandNewItem = ed.IsNewItem;
            if (brandNewItem)
                ed.Id = ed.Name.ToIdFormat();
            await repos.SavedAsync(ed, properties);

            var result = await ed.CompileDesignModeAsync();
            ObjectBuilder.GetObject<ILogger>().WriteInfo(result.ToString());

            return (brandNewItem, new { success = true, status = "OK", message = "Your entity has been successfully saved ", id = ed.Id });

        }

        [HttpGet]
        [Route("plural/{id}")]
        public IHttpActionResult GetPlural(string id)
        {
            return Ok(id.Pluralize());
        }

        [HttpGet]
        [Route("singular/{id}")]
        public IHttpActionResult GetSingular(string id)
        {
            return Ok(id.Singularize(false));
        }


        [HttpGet]
        [Route("schema")]
        [RxSourceOutputCache(SourceType = typeof(EntityDefinition))]
        public async Task<IHttpActionResult> Schemas()
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var list = await repos.LoadAsync<EntityDefinition>(x => x.IsPublished);

            var script = new StringBuilder();
            var provider = CodeGeneratorFactory.Instance.LanguageProviders.Single(x => x.Language == "javascript");
            foreach (var ef in list)
            {
                var sources = await provider.GenerateCodeAsync(ef);
                sources.ToList().ForEach(src => script.AppendLine(src.Value));
            }
            return Javascript(script.ToString());
        }

        [HttpGet]
        [Route("export/{id}")]
        public async Task<IHttpActionResult> Export(string id, [QueryString] bool includeData = false)
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var entity = await repos.LoadOneAsync<EntityDefinition>(e => e.Id == id);
            var package = new EntityDefinitionPackage();
            var zip = await package.PackAsync(entity, includeData);
            var file = $"{Path.GetFileNameWithoutExtension(zip)}_{Environment.MachineName}_{DateTime.Now:s}{(includeData ? "_data" : "")}.zip";
            return File(System.IO.File.ReadAllBytes(zip), MimeMapping.GetMimeMapping(zip), file);
        }

        [Route("upload")]
        public async Task<IHttpActionResult> Upload(IEnumerable<HttpPostedFileBase> files)
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

                    var result = new { success = true, zip, ed, folder };
                    return Json(result.ToJsonString());

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
        public async Task<IHttpActionResult> Import(string folder)
        {
            try
            {
                var packager = new EntityDefinitionPackage();
                var ed = await packager.ImportAsync(folder);

                var result = new { success = true, ed };
                return Javascript(result.ToJsonString());
            }
            catch (Exception e)
            {
                return Json(new { success = false, exception = e.GetType().FullName, message = e.Message, stack = e.StackTrace });
            }
        }

        [HttpPost]
        [Route("import-data")]
        public async Task<IHttpActionResult> ImportData(string folder)
        {
            try
            {
                var packager = new EntityDefinitionPackage();
                await packager.ImportDataAsync(folder);

                var result = new { success = true };
                return Json(result.ToJson());
            }
            catch (Exception e)
            {
                return Json(new { success = false, exception = e.GetType().FullName, message = e.Message, stack = e.StackTrace });
            }
        }

        [HttpDelete]
        [Route("{name}/contents")]
        public async Task<IHttpActionResult> TruncateData(string name)
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
        public async Task<IHttpActionResult> Depublish([JsonBody]EntityDefinition ed)
        {
            var context = new SphDataContext();

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
        public async Task<IHttpActionResult> Delete(string id)
        {
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == id);
            if (null == ed) return NotFound("Cannot find entity definition to delete, id : " + id);

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
        [Route("publish/service-contract")]
        public async Task<IHttpActionResult> PublishServiceContract([JsonBody]EntityDefinition ed)
        {
            var context = new SphDataContext();
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