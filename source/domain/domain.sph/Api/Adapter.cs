using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    public abstract class Adapter
    {
        public async Task<object> CompileAsync()
        {
            var item = await this.GetSchemaDefinitionAsync();

            var options = new CompilerOptions();
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll"));
            

            var codes = item.GenerateCode();
            var sources = item.SaveSources(codes);

            var adapterCodes = await this.GenerateSourceCodeAsync(options, item.CodeNamespace);
            var adapterSources = item.SaveSources(adapterCodes);

            var result = item.Compile(options, sources.Concat(adapterSources).ToArray());

            result.Errors.ForEach(Console.WriteLine);
            Debug.WriteIf(!result.Result, result.ToJsonString(Formatting.Indented));

            var assembly = Assembly.LoadFrom(result.Output);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, item.EntityDefinitionId, item.Name);

            var edType = assembly.GetType(edTypeName);
            return edType;

        }

        //public Type GetAdapterType(string dll)
        //{

        //    var assembly = Assembly.LoadFrom(dll);
        //    var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, item.EntityDefinitionId, item.Name);

        //    var edType = assembly.GetType(edTypeName);
        //    return edType;
        //}

        //public Type GetEntityType(string dll)
        //{

        //    var assembly = Assembly.LoadFrom(dll);
        //    var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, item.EntityDefinitionId, item.Name);

        //    var edType = assembly.GetType(edTypeName);
        //    return edType;
        //}



        protected abstract Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces);
        protected abstract Task<EntityDefinition> GetSchemaDefinitionAsync();
    }
}
