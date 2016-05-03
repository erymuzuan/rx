using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.ViewModels;
using Microsoft.AspNet.SignalR;

namespace Bespoke.Sph.Web.Hubs
{
    [Authorize(Roles = "developers,administrators")]
    public class DataImportHub : Hub
    {
        public async Task<object> Execute(IProgress<int> progress, string name, ImportDataViewModel model)
        {
            var context = new SphDataContext();
            var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.Adapter);
            var mapping = await context.LoadOneAsync<TransformDefinition>(x => x.Id == model.Map);

            dynamic tableAdapter = GetTableAdapterInstance(model, adapter);
            if (null == tableAdapter) return new
            {
                statusCode = 404,
                message = $"{adapter.AssemblyName}.dll does not exist, you may have to build your adapter before it could be used"
            };

            dynamic map = GetMapInstance(mapping);
            if (null == map) return new
            {
                statusCode = 404,
                message = $"{mapping.AssemblyName}.dll in web\\bin does not exist, you may have to build your TransformDefinition before it could be used"
            };

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
                    progress.Report(rows);
                }

                lo = await tableAdapter.LoadAsync(model.Sql, lo.CurrentPage + 1, model.BatchSize, false);
            }

            return new { success = true, rows, message = $"successfully imported {rows}", status = "OK" };
        }

        public async Task<object> Preview(ImportDataViewModel model)
        {
            var context = new SphDataContext();
            var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.Adapter);

            dynamic tableAdapter = GetTableAdapterInstance(model, adapter);
            if (null == tableAdapter) return new
            {
                statusCode = 404,
                message = $"{adapter.AssemblyName}.dll does not exist, you may have to build your adapter before it could be used"
            };

            var lo = await tableAdapter.LoadAsync(model.Sql);
            return lo;

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
        public void Dispose()
        {
            //
        }
    }
}