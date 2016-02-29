using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

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
            return Json(new { success = true, status = "OK", url =$"/binarystore/{zd.Id}"});
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
            var result =await wd.CompileAsync();
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
            

            var result =await wd.CompileAsync();
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
            using (var session = context.OpenSession())
            {
                session.Delete(wd);
                await session.SubmitChanges();
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

            var list = wd.VariableDefinitionCollection.Select(v => v.Name).ToList();
            var schema = wd.GetCustomSchema();
            if (null != schema)
            {
                var xsd = new XsdMetadata(schema);
                foreach (var v in wd.VariableDefinitionCollection.OfType<ComplexVariable>())
                {
                    list.AddRange(xsd.GetMembersPath(v.TypeName).Select(x => v.Name + "." + x));
                }
            }

            foreach (var v in wd.VariableDefinitionCollection.OfType<ClrTypeVariable>())
            {
                var v1 = v;
                var entity = await context.LoadOneAsync<EntityDefinition>(e => e.Name == v1.Type.Name);
                if (null != entity)
                {
                    list.AddRange(entity.GetMembersPath().Select(x => v.Name + "." + x));
                }
                else
                {
                    var properties = v.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    list.AddRange(properties.Select(x => v.Name + "." + x.Name));
                }
            }

            return Json(list.Select(d => new { Path = d }).ToArray());
        }

        

        private async Task<string> Save(string operation, WorkflowDefinition wd, params Entity[] entities)
        {
            var context = new SphDataContext();
            if (null == wd) throw new ArgumentNullException(nameof(wd));

            if(string.IsNullOrWhiteSpace(wd.Name))
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
            "App_Web"
        };

        [HttpGet]
        [Route("assemblies")]
        public IHttpActionResult GetLoadedAssemblies()
        {
            var assesmblies = AppDomain.CurrentDomain.GetAssemblies();
            var refAssemblies = (from a in assesmblies
                                 where a.IsDynamic == false
                                 let name = a.GetName()
                                 where !Ignores.Any(x => name.Name.StartsWith(x))
                                 let web = ConfigurationManager.WebPath + "\\bin\\" + Path.GetFileName(a.Location)
                                 let path = System.IO.File.Exists(web) ? web : a.Location
                                 select new ReferencedAssembly
                                 {
                                     Version = name.Version.ToString(),
                                     FullName = name.FullName,
                                     IsGac = a.GlobalAssemblyCache,
                                     RuntimeVersion = a.ImageRuntimeVersion,
                                     Location = path,
                                     Name = name.Name
                                 }).ToList();
            var outputs = from f in Directory.GetFiles(ConfigurationManager.CompilerOutputPath, "*.dll")
                          let fn = Path.GetFileNameWithoutExtension(f)
                          where !fn.StartsWith("ff") && !fn.StartsWith("subscriber")
                          && refAssemblies.All(x => x.Name != fn)
                          let dll = Assembly.ReflectionOnlyLoadFrom(f)
                          let name = dll.GetName()
                          select new ReferencedAssembly
                          {
                              Version = name.Version.ToString(),
                              FullName = name.FullName,
                              IsGac = false,
                              RuntimeVersion = dll.ImageRuntimeVersion,
                              Location = f,
                              Name = fn
                          };

            refAssemblies.AddRange(outputs);

            return Json(refAssemblies.ToArray());
        }




    }
}