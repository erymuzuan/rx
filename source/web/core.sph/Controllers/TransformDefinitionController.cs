using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "developers")]
    [RoutePrefix("api/transform-definitions")]
    public class TransformDefinitionController : BaseApiController
    {

        [HttpPost]
        [Route("validate")]
        public async Task<IHttpActionResult> Validate([JsonBody] TransformDefinition map)
        {
            var erros = await map.ValidateBuildAsync();
            if (!erros.Result)
                return Json(erros);

            return Json(new { success = true, status = "OK", message = "Your map has been successfully validated" });

        }

        [HttpPost]
        [Route("validate-fix")]
        public async Task<IHttpActionResult> ValidateFix([JsonBody] TransformDefinition map)
        {
            if (string.IsNullOrWhiteSpace(map.Id))
                return await Validate(map);

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(map);
                await session.SubmitChanges("Save");
            }
            return await Validate(map);
        }

        [HttpPost]
        [Route("{id}/publish")]
        public async Task<IHttpActionResult> Publish([JsonBody] TransformDefinition map, string id)
        {
            var erros = await map.ValidateBuildAsync();
            if (!erros.Result)
                return Json(erros);


            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.GeneratedSourceDirectory
            };
            options.AddReference<ApiController>();
            options.AddReference<BaseApiController>();
            options.AddReference<TransformDefinitionController>();
            options.AddReference<Newtonsoft.Json.JsonConverter>();

            var codes = map.GenerateCode();
            var sources = map.SaveSources(codes);
            var result = await map.CompileAsync(options, sources);

            result.Errors.ForEach(Console.WriteLine);
            if (!result.Result)
                return Json(result);



            map.IsPublished = true;
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(map);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your map has been successfully published", id = map.Id });

        }

        [HttpPost]
        [Route("{id}/generate-partial")]
        public IHttpActionResult GeneratePartial(string id, [JsonBody] TransformDefinition map)
        {
            string file;
            var partial = map.GeneratePartialCode(out file);
            return Json(new { success = partial, status = "OK", message = $"Your partial code is successfuly generated {file}", id = map.Id });

        }

        [HttpGet]
        [Route("functoids")]
        public IHttpActionResult GetFunctoids()
        {

            var list = from f in this.DeveloperService.Functoids
                       let v = f.Value
                       let g = v.Initialize()
                       orderby f.Metadata.Category
                       select new { designer = f.Metadata, functoid = v };
            return Json(list.ToJsonString(true));
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> SaveAsync([JsonBody]TransformDefinition map)
        {
            var context = new SphDataContext();
            if (string.IsNullOrWhiteSpace(map.Name))
                return Json(new { success = false, status = "Not OK", message = "You will need a valid name for your mapping" });

            var baru = map.IsNewItem;
            if (baru)
            {
                map.Id = map.Name.ToIdFormat();
            }

            map.FunctoidCollection.RemoveAll(f => null == f);
            using (var session = context.OpenSession())
            {
                session.Attach(map);
                await session.SubmitChanges("Save");
            }
            var content = new { success = true, status = "OK", message = "Your mapping has been successfully saved ", id = map.Id };

            if (baru)
                return Created("/api/transform-definitions/" + map.Id, content);
            return Json(content);


        }

        [HttpPost]
        [Route("json-schema")]
        public IHttpActionResult Schema([JsonBody]TransformDefinition map)
        {
            if (!string.IsNullOrWhiteSpace(map.InputTypeName))
            {
                var type = Strings.GetType(map.InputTypeName);
                var schema = JsonSerializerService.GetJsonSchemaFromObject(type);
                return Json(schema);
            }

            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("     \"types\":[\"object\", null],");
            sb.AppendLine("     \"properties\": {");
            var schemes = from t in map.InputCollection
                          let sc = JsonSerializerService.GetJsonSchemaFromObject(t.Type)
                          select $@"""{t.Name}"" : {sc}";
            sb.AppendLine(string.Join(", ", schemes));
            sb.AppendLine("     }");
            sb.AppendLine("}");
            return Json(sb.ToString());
        }


        [HttpGet]
        [Route("functoid/js")]
        public IHttpActionResult GetFunctoidDesignerViewModel([FromUri]string type)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            if (null == ds.Functoids) throw new InvalidOperationException("Cannot compose MEF");

            var functoid = ds.Functoids.Single(x => x.Value.GetType().GetShortAssemblyQualifiedName()
                .ToLowerInvariant() == type).Value;

            var js = functoid.GetEditorViewModel();
            return Javascript(js);

        }
        [HttpGet]
        [Route("functoid/html")]
        public IHttpActionResult GetFunctoidDesignerView([FromUri]string type)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            if (null == ds.Functoids) throw new InvalidOperationException("Cannot compose MEF");

            var functoid = ds.Functoids.Single(x => x.Value.GetType().GetShortAssemblyQualifiedName()
                .ToLowerInvariant() == type).Value;

            var html = functoid.GetEditorView();
            return Html(html);

        }

        [HttpGet]
        [Route("{id}/designer")]
        public IHttpActionResult GetDesigner(string id)
        {
            var source = $"{ConfigurationManager.SphSourceDirectory}\\TransformDefinition\\{id}.designer";
            if (!System.IO.File.Exists(source)) return Json("[]");
            var json = System.IO.File.ReadAllText(source);
            return Json(json);
        }
        [HttpPost]
        [Route("{id}/designer")]
        public IHttpActionResult SaveDesigner(string id, [RawBody]string json)
        {
            var source = $"{ConfigurationManager.SphSourceDirectory}\\TransformDefinition\\{id}.designer";
            System.IO.File.WriteAllText(source, json, Encoding.UTF8);
            return Json(new { success = true });
        }
    }
}