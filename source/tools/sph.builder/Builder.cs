using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Console = Colorful.Console;

namespace Bespoke.Sph.SourceBuilders
{
    public class Builder<T> where T : Entity
    {
        public virtual Task RestoreAllAsync()
        {
            return Task.FromResult(0);
        }

        public virtual Task RestoreAsync(T item)
        {
            Console.WriteLine($"Compiling {typeof(T).Namespace} : {item.Id} ......");
            return Task.FromResult(0);
        }

        public IEnumerable<T> GetItems()
        {
            var folder = Path.Combine(ConfigurationManager.SphSourceDirectory, typeof(T).Name);

            if (!Directory.Exists(folder))
                return new List<T>();

            var list = Directory.GetFiles(folder, "*.json").Select(f => f.DeserializeFromJsonFile<T>()).ToList();
            //list.ForEach(x => ObjectBuilder.ComposeMefCatalog(x));
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

        protected virtual void ReportBuildStatus(WorkflowCompilerResult result)
        {


            Console.WriteLine(result.Errors.Count > 0
                ? $" ================ {result.Errors.Count} errors , 1 failed =================="
                : " ================ 0 errors , 1 succeeded ==================", Color.Cyan);
            result.Errors.ForEach(x => Console.WriteLine(x, Color.Red));
        }


    }
}