using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize]
    [RoutePrefix("workflow-designer")]
    public class WorkflowDefinitionController : ApiController
    {

        [HttpPost]
        [Route("export")]
        public async Task<HttpResponseMessage> Export([FromBody]WorkflowDefinition wd)
        {
            var package = new WorkflowDefinitionPackage();
            var zd = await package.PackAsync(wd);
            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, status = "OK", url = "/binary-store/" + zd.Id });
        }



        [HttpGet]
        [Route("xsd-elements/{id}")]
        public async Task<HttpResponseMessage> GetXsdElementName(string id)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = await store.GetContentAsync(id);
            using (var stream = new MemoryStream(content.Content))
            {
                var xsd = XElement.Load(stream);

                XNamespace x = "http://www.w3.org/2001/XMLSchema";
                var elements = xsd.Elements(x + "element").Select(e => e.Attribute("name").Value).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, elements);

            }
        }

        [HttpPost]
        [Route("build")]
        public async Task<HttpResponseMessage> Compile([FromBody]WorkflowDefinition wd)
        {
            var buildValidation = wd.ValidateBuild();

            await this.Save("Compile", wd);

            if (!buildValidation.Result)
                return Request.CreateResponse(HttpStatusCode.OK, buildValidation);


            var options = new CompilerOptions();
            var result = await wd.CompileAsync(options);
            if (!result.Result)
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, version = wd.Version, status = "ERROR", result.Errors });


            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, status = "OK", message = "Your workflow has been successfully built " });

        }

        [HttpPost]
        [Route("publish/{dll}")]
        public async Task<HttpResponseMessage> Publish([FromBody]WorkflowDefinition wd, string dll = "")
        {
            var buildValidation = wd.ValidateBuild();

            if (!buildValidation.Result)
                return Request.CreateResponse(HttpStatusCode.OK, buildValidation);


            using (var stream = new FileStream(dll, FileMode.Create))
            {
                // compile , then save
                var options = new CompilerOptions
                {
                    Emit = true,
                    Stream = stream
                };

                var result = await wd.CompileAsync(options);
                if (!result.Result || !File.Exists(dll))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = false, version = wd.Version, status = "ERROR", result.Errors });
                }
            }

            // save
            //archive the WD
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var archived = new BinaryStore
            {
                Id = string.Format("wd.{0}.{1}", wd.Id, wd.Version),
                Content = Encoding.Unicode.GetBytes(wd.ToJsonString(true)),
                Extension = ".xml",
                FileName = string.Format("wd.{0}.{1}.xml", wd.Id, wd.Version)

            };
            await store.DeleteAsync(archived.Id);
            await store.AddAsync(archived);
            await this.Save("Publish", wd);

            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, version = wd.Version, status = "OK", message = "Your workflow has been successfully compiled and published : " + dll });
        }

        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Save([FromBody]WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(wd.Name))
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, status = "Not OK", message = "Name cannot be empty" });
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
                    Content = File.ReadAllBytes(ConfigurationManager.WebPath + @"/App_Data/empty.xsd")
                };
                await store.AddAsync(xsd);
                wd.SchemaStoreId = xsd.Id;

            }
            var id = await this.Save(string.IsNullOrWhiteSpace(wd.Id) ? "Add" : "Update", wd);
            return Request.CreateResponse(HttpStatusCode.OK, new { success = !string.IsNullOrWhiteSpace(wd.Id), id, status = "OK" });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Remove(string id)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(x => x.Id == id);
            if (null == wd) return Request.CreateResponse(HttpStatusCode.NotFound);

            using (var session = context.OpenSession())
            {
                session.Delete(wd);
                await session.SubmitChanges();
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, id = wd.Id, status = "OK" });
        }

        [HttpGet]
        [Route("variables/{id}")]
        public async Task<HttpResponseMessage> GetVariablePath(string id)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.Id == id);
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

            return Request.CreateResponse(HttpStatusCode.OK, list.Select(d => new { Path = d }).ToArray());
        }

        private async Task<string> Save(string operation, WorkflowDefinition wd, params Entity[] entities)
        {
            var context = new SphDataContext();
            if (null == wd) throw new ArgumentNullException("wd");


            using (var session = context.OpenSession())
            {
                if (entities.Any())
                    session.Attach(entities);

                session.Attach(wd);
                await session.SubmitChanges(operation);
            }
            return wd.Id;
        }

        [HttpGet]
        [Route("assemblies")]
        public HttpResponseMessage GetLoadedAssemblies()
        {
            var assesmblies = AppDomain.CurrentDomain.GetAssemblies();
            var refAssemblies = from a in assesmblies
                                where a.IsDynamic == false
                                let name = a.GetName()
                                select new ReferencedAssembly
                                {
                                    Version = name.Version.ToString(),
                                    FullName = name.FullName,
                                    Location = a.Location,
                                    Name = name.Name
                                };

            return Request.CreateResponse(HttpStatusCode.OK, refAssemblies.ToArray());
        }




    }
}