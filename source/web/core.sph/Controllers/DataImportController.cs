using System;
using System.Linq;

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

            dynamic tableAdapter = GetTableAdapterInstance(model, adapter);
            if (null == tableAdapter) return HttpNotFound($"{ConfigurationManager.ApplicationName}.{adapter.Name}.dll does not exist, you may have to build your adapter before it could be used");

            var lo = await tableAdapter.LoadAsync(model.Sql);
            return Content(JsonConvert.SerializeObject(lo), "application/json", Encoding.UTF8);

        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Execute(ImportDataViewModel model)
        {
            var context = new SphDataContext();
            var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.Adapter);
            var transformDefinition = await context.LoadOneAsync<TransformDefinition>(x => x.Id == model.Map);

            dynamic tableAdapter = GetTableAdapterInstance(model, adapter);
            if (null == tableAdapter) return HttpNotFound($"{ConfigurationManager.ApplicationName}.{adapter.Name}.dll does not exist, you may have to build your adapter before it could be used");

            dynamic map = GetMapInstance(transformDefinition);
            if (null == map) return HttpNotFound($"{ConfigurationManager.ApplicationName}.{transformDefinition.Name}.dll in web\\bin does not exist, you may have to build your TransformDefinition before it could be used");

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

            return Content(JsonConvert.SerializeObject(new { success = true, rows, message = $"successfully imported {rows}", status = "OK" }), "application/json", Encoding.UTF8);
        }

        private object  GetTableAdapterInstance(ImportDataViewModel model, Adapter adapter)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var dllName = $"{ConfigurationManager.ApplicationName}.{adapter.Name}";

            var adapterDll = assemblies.SingleOrDefault(x => x.GetName().Name == dllName);
            if (null == adapterDll) return null;
            var adapterTypeName =
                $"{ConfigurationManager.ApplicationName}.Adapters.{adapter.Schema}.{adapter.Name}.{model.Table}Adapter";
            var adapterType = adapterDll.GetType(adapterTypeName);
            return Activator.CreateInstance(adapterType);
        }

        private static object GetMapInstance(TransformDefinition transformDefinition)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var dllName = $"{ConfigurationManager.ApplicationName}.{transformDefinition.Name}";

            var mapDll = assemblies.SingleOrDefault(x => x.GetName().Name == dllName);
            if (null == mapDll) return null;

            var mapTypeName = $"{ConfigurationManager.ApplicationName}.Integrations.Transforms.{transformDefinition.Name}";
            var mapType = mapDll.GetType(mapTypeName);

            return Activator.CreateInstance(mapType);
        }
    }
}