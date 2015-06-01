using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("data-import")]
    public class DataImportController : Controller
    {
        [HttpPost]
        [Route("preview")]
        public async Task<ActionResult> Preview(ImportDataViewModel model)
        {
            var context = new SphDataContext();
            var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.Adapter);
            var map = await context.LoadOneAsync<TransformDefinition>(x => x.Id == model.Map);

            var path = $"{ConfigurationManager.WebPath}\\bin\\{ConfigurationManager.ApplicationName}.{adapter.Name}.dll";
            if (!System.IO.File.Exists(path))
            {
                return
                    Json(
                        new
                        {
                            success = false,
                            status = "404",
                            message = $"{path} does not exist, you may have to build you adapter"
                        });
            }
            var dll = Assembly.LoadFile(path);
            var typeName = $"{ConfigurationManager.ApplicationName}.Adapters.{adapter.Schema}.{adapter.Name}.{model.Table}Adapter";
            var type = dll.GetType(typeName);
            dynamic tableAdapter = Activator.CreateInstance(type);
            var lo = await tableAdapter.LoadAsync(model.Sql);
            return Content(JsonConvert.SerializeObject(lo), "application/json", Encoding.UTF8);



        }
        [HttpPost]
        [Route("execute")]
        public async Task<ActionResult> Execute(ImportDataViewModel model)
        {
            var context = new SphDataContext();
            var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.Adapter);
            var map = await context.LoadOneAsync<TransformDefinition>(x => x.Id == model.Map);

            var adapterDllPath = $"{ConfigurationManager.WebPath}\\bin\\{ConfigurationManager.ApplicationName}.{adapter.Name}.dll";
            if (!System.IO.File.Exists(adapterDllPath))
            {
                return HttpNotFound($"{adapterDllPath} does not exist, you may have to build you adapter");
            }
            var adapterDll = Assembly.LoadFile(adapterDllPath);
            var adapterTypeName = $"{ConfigurationManager.ApplicationName}.Adapters.{adapter.Schema}.{adapter.Name}.{model.Table}Adapter";
            var type = adapterDll.GetType(adapterTypeName);
            dynamic tableAdapter = Activator.CreateInstance(type);
            var lo = await tableAdapter.LoadAsync(model.Sql);

            return Json(new { model, adapter, map });
        }

    }
}