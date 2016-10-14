﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Mono.Cecil;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [RoutePrefix("api/workflow-definitions")]
    [Authorize]
    public class WorkflowDefinitionController : BaseApiController
    {
        [HttpGet]
        [Route("import")]
        public async Task<IHttpActionResult> Import(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                foreach (var postedFile in files)
                {
                    var fileName = Path.GetFileName(postedFile.FileName);
                    if (string.IsNullOrWhiteSpace(fileName)) throw new Exception("Filename is empty or null");

                    var zip = Path.Combine(Path.GetTempPath(), fileName);
                    postedFile.SaveAs(zip);

                    var packager = new WorkflowDefinitionPackage();
                    var wd = await packager.UnpackAsync(zip);

                    var result = new { success = true, wd };
                    return Javascript(result.ToJsonString());

                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, exception = e.GetType().FullName, message = e.Message, stack = e.StackTrace });
            }
            return Json(new { success = false });


        }


        [HttpPost]
        [Route("export")]
        public async Task<IHttpActionResult> Export([JsonBody]WorkflowDefinition wd)
        {
            var package = new WorkflowDefinitionPackage();
            var zd = await package.PackAsync(wd);
            return Json(new { success = true, status = "OK", url = $"/binarystore/{zd.Id}" });
        }


        [HttpGet]
        [Route("xsd-elements/{id}")]
        public async Task<IHttpActionResult> GetXsdElementName(string id)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = await store.GetContentAsync(id);
            if (null == content)
                return NotFound("Cannot find WorkflowDefinition XSD with id " + id);
            using (var stream = new MemoryStream(content.Content))
            {
                var xsd = XElement.Load(stream);

                XNamespace x = "http://www.w3.org/2001/XMLSchema";
                var elements = xsd.Elements(x + "element").Select(e => e.Attribute("name").Value).ToList();
                return Json(elements);

            }
        }

        [HttpPost]
        [Route("compile")]
        public async Task<IHttpActionResult> Compile([JsonBody]WorkflowDefinition wd)
        {
            var buildValidation = wd.ValidateBuild();

            if (!buildValidation.Result)
                return Json(buildValidation);

            await this.Save("Compile", wd);
            var result = await wd.CompileAsync();
            if (!result.Result || !System.IO.File.Exists(result.Output))
            {
                return Json(new { success = false, version = wd.Version, status = "ERROR", result.Errors });
            }

            return Json(new { success = true, status = "OK", message = "Your workflow has been successfully compiled  : " + Path.GetFileName(result.Output) });

        }

        [HttpPost]
        [Route("publish")]
        public async Task<IHttpActionResult> Publish([JsonBody]WorkflowDefinition wd)
        {
            var buildValidation = wd.ValidateBuild();

            if (!buildValidation.Result)
                return Json(buildValidation);


            var result = await wd.CompileAsync();
            if (!result.Result || !System.IO.File.Exists(result.Output))
            {
                return Json(new { success = false, version = wd.Version, status = "ERROR", result.Errors });
            }

            // save
            //archive the WD
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var archived = new BinaryStore
            {
                Id = $"wd.{wd.Id}.{wd.Version}",
                Content = Encoding.Unicode.GetBytes(wd.ToJsonString(true)),
                Extension = ".json",
                FileName = $"wd.{wd.Id}.{wd.Version}.json"
            };
            await store.DeleteAsync(archived.Id);
            await store.AddAsync(archived);
            await this.Save("Publish", wd);

            return Json(new { success = true, version = wd.Version, status = "OK", message = "Your workflow has been successfully compiled and published : " + Path.GetFileName(result.Output) });
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Save([JsonBody]WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(wd.Name))
                return Json(new { success = false, status = "Not OK", message = "Name cannot be empty" });
            if (wd.Name.Trim() == "0")
                return Json(new { success = false, status = "Not OK", message = "Name \"0\" is invalid" });
            if (wd.IsNewItem && string.IsNullOrWhiteSpace(wd.SchemaStoreId))
            {
                wd.Id = wd.Name.ToIdFormat();
                // get the empty schema
                var store = ObjectBuilder.GetObject<IBinaryStore>();
                var xsd = new BinaryStore
                {
                    Extension = ".xsd",
                    FileName = "Empty.xsd",
                    WebId = Guid.NewGuid().ToString(),
                    Id = Guid.NewGuid().ToString(),
                    Content = System.IO.File.ReadAllBytes($@"{ConfigurationManager.WebPath}/App_Data/empty.xsd")
                };
                await store.AddAsync(xsd);
                wd.SchemaStoreId = xsd.Id;

            }
            var id = await this.Save(string.IsNullOrWhiteSpace(wd.Id) ? "Add" : "Update", wd);
            return Json(new { success = !string.IsNullOrWhiteSpace(wd.Id), id, status = "OK" });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Remove(string id)
        {
            var context = new SphDataContext();
            var wd = context.LoadOneFromSources<WorkflowDefinition>(x => x.Id == id);
            if (null == wd) return NotFound();

            //TODO : remove all the related assets, like the dll, table , and forms
            using (var session = context.OpenSession())
            {
                session.Delete(wd);
                await session.SubmitChanges("Delete");
            }

            return Json(new { success = true, id = wd.Id, status = "OK" });
        }

        [HttpGet]
        [Route("{id}/variable-path")]
        public async Task<IHttpActionResult> GetVariablePath(string id)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.Id == id);
            if (null == wd)
                return NotFound("No WorkflowDefinition is found with the id " + id);

            var list = new List<string>();
            foreach (var @var in wd.VariableDefinitionCollection)
            {
                list.Add(@var.Name);
                var members = await @var.GetMembersPathAsync(wd);
                list.AddRange(members);
            }
            return Json(list.Select(d => new { Path = d }).ToArray());
        }



        private async Task<string> Save(string operation, WorkflowDefinition wd, params Entity[] entities)
        {
            var context = new SphDataContext();
            if (null == wd) throw new ArgumentNullException(nameof(wd));

            if (string.IsNullOrWhiteSpace(wd.Name))
                throw new InvalidOperationException("Cannot save WorkflowDefinition with empty Name");


            using (var session = context.OpenSession())
            {
                if (entities.Any())
                    session.Attach(entities);

                session.Attach(wd);
                await session.SubmitChanges(operation);
            }
            return wd.Id;
        }
        public static readonly string[] Ignores =
        {
            "App_global",
            "App_Code",
            "App_Web",
            "ff",
            "subscriber"
        };

        [HttpGet]
        [Route("assemblies")]
        public IHttpActionResult GetLoadedAssemblies()
        {
            var files = Directory.GetFiles($"{ConfigurationManager.CompilerOutputPath}", "*.dll")
                    .Concat(Directory.GetFiles($"{ConfigurationManager.WebPath}\\bin", "*.dll"))
                    .Concat(Directory.GetFiles($"{ConfigurationManager.Home}\\packages", "*.dll", SearchOption.AllDirectories));

            var assesmblies = files.Select(AssemblyDefinition.ReadAssembly);
            var refAssemblies = (from d in assesmblies
                                 let a = d.MainModule
                                 where !Ignores.Any(x => d.Name.Name.StartsWith(x))
                                 select new ReferencedAssembly
                                 {
                                     Version = d.Name.Version.ToString(),
                                     FullName = d.FullName,
                                     IsGac = false,
                                     RuntimeVersion = a.RuntimeVersion,
                                     Location = a.FullyQualifiedName,
                                     Name = d.Name.Name
                                 }).ToList();


            return Json(refAssemblies.ToArray());
        }




    }
}