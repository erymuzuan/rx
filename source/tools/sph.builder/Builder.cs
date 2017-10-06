using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
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

        protected virtual void ReportBuildStatus(WorkflowCompilerResult result,
        [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            var logger = ObjectBuilder.GetObject<ILogger>();
            if (result.Errors.Any())
                logger.WriteError($" ================ {result.Errors.Count} errors , 1 failed ==================", filePath, memberName, lineNumber);
            else
                logger.WriteInfo(" ================ 0 errors , 1 succeeded ==================", filePath, memberName, lineNumber);
            result.Errors.ForEach(x => logger.WriteError(x.ToString(), filePath, memberName, lineNumber));
        }


    }
}