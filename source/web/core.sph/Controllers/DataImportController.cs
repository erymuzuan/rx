using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.ViewModels;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "administrators,developers")]
    [RoutePrefix("api/data-imports")]
    public class DataImportController : BaseApiController
    {
        [HttpPost]
        [Route("")]
        public IHttpActionResult Save(ImportDataViewModel model)
        {
            var folder = ($"{ConfigurationManager.WebPath}/App_Data/data-imports/");
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);


            var setting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = JsonConvert.SerializeObject(model, Formatting.Indented, setting);
            System.IO.File.WriteAllText($"{ConfigurationManager.WebPath}/App_Data/data-imports/{model.Name}.json", json);
            return Json(new { });
        }

        [HttpDelete]
        [Route("{name}")]
        public IHttpActionResult Remove(string name)
        {
            var file = ($"{ConfigurationManager.WebPath}/App_Data/data-imports/{name}.json");
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);

            return Json(new { });
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult List()
        {
            var folder = $"{ConfigurationManager.WebPath}/App_Data/data-imports/";
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            var files = from f in System.IO.Directory.GetFiles(folder, "*.json")
                        select System.IO.File.ReadAllText(f);

            return Json("[" + string.Join(",", files.ToArray()) + "]");
        }

        [HttpPost]
        [Route("preview")]
        public async Task<IHttpActionResult> Preview([FromBody]ImportDataViewModel model)
        {
            var context = new SphDataContext();
            var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.Adapter);

            dynamic tableAdapter = GetTableAdapterInstance(model, adapter);
            if (null == tableAdapter) return NotFound($"{adapter.AssemblyName}.dll does not exist, you may have to build your adapter before it could be used");

            var lo = await tableAdapter.LoadAsync(model.Sql);
            return Json(lo);

        }

        [HttpPost]
        [Route("{name}/execute")]
        public async Task<IHttpActionResult> Execute(string name,[FromBody]ImportDataViewModel model)
        {
            var context = new SphDataContext();
            var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.Adapter);
            var mapping = await context.LoadOneAsync<TransformDefinition>(x => x.Id == model.Map);

            dynamic tableAdapter = GetTableAdapterInstance(model, adapter);
            if (null == tableAdapter) return NotFound($"{adapter.AssemblyName}.dll does not exist, you may have to build your adapter before it could be used");

            dynamic map = GetMapInstance(mapping);
            if (null == map) return NotFound($"{mapping.AssemblyName}.dll in web\\bin does not exist, you may have to build your TransformDefinition before it could be used");

            var rows = 0;
            var lo = await tableAdapter.LoadAsync(model.Sql, 1, model.BatchSize, false);
            while (lo.ItemCollection.Count > 0)
            {
                using (var session = context.OpenSession())
                {
                    foreach (var source in lo.ItemCollection)
                    {
                        rows++;
                        var item = await map.TransformAsync(source);
                        Console.WriteLine("ENT_ID:" + item.Id);
                        session.Attach(item);
                    }

                    await session.SubmitChanges("Import");
                }

                lo = await tableAdapter.LoadAsync(model.Sql, lo.CurrentPage + 1, model.BatchSize, false);
            }

            return Json(new { success = true, rows, message = $"successfully imported {rows}", status = "OK" });
        }

        private object GetTableAdapterInstance(ImportDataViewModel model, Adapter adapter)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var dll = assemblies.SingleOrDefault(x => x.GetName().Name == adapter.AssemblyName);
            if (null == dll) return null;
            var adapterTypeName = $"{adapter.CodeNamespace}.{model.Table}Adapter";
            var adapterType = dll.GetType(adapterTypeName);
            return Activator.CreateInstance(adapterType);
        }

        private static object GetMapInstance(TransformDefinition mapping)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var dll = assemblies.SingleOrDefault(x => x.GetName().Name == mapping.AssemblyName);
            if (null == dll) return null;

            var type = dll.GetType(mapping.FullTypeName);
            return Activator.CreateInstance(type);
        }
    }
}