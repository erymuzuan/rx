using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.SourceBuilders
{
    public abstract class Builder<T> where T : Entity
    {
        protected abstract Task<WorkflowCompilerResult> CompileAssetAsync(T item);

        public virtual async Task RestoreAllAsync()
        {
            this.Initialize();
            var items = this.GetItems();
            foreach (var asset in items)
            {
                await this.RestoreAsync(asset);
            }
        }

        public async Task<WorkflowCompilerResult> RestoreAsync(T item)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            logger.WriteInfo($"Compiling {typeof(T).Name} : {item.Id} ......");
            var result = await CompileAssetAsync(item);

            if (result.Errors.Any())
                logger.WriteError($" ================ Compiling {typeof(T).Name}[{item.Id}] with {result.Errors.Count} errors , 1 failed ==================");
            else
                logger.WriteInfo($" ================ Compiling {typeof(T).Name}[{item.Id}] 0 errors , 1 succeeded ==================");
            result.Errors.ForEach(x => logger.WriteError(x.ToString()));


            return result;
        }

        public IEnumerable<T> GetItems()
        {
            var context = new SphDataContext();
            var folder = Path.Combine(ConfigurationManager.SphSourceDirectory, typeof(T).Name);

            if (!Directory.Exists(folder))
                return new List<T>();

            var list = context.LoadFromSources<T>().ToList();
            list.ForEach(x => ObjectBuilder.ComposeMefCatalog(x));
            return list;
        }



        public void Initialize()
        {
        }

        public void Clean()
        {
            var name = typeof(T).Name;
            using (var client = new HttpClient())
            {
                client.DeleteAsync(
                    $"{ConfigurationManager.ElasticSearchHost}/{ConfigurationManager.ElasticSearchIndex}/_mapping/{name.ToLowerInvariant()}")
                    .Wait(5000);
            }
        }




    }
}