using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    public abstract class Adapter
    {
        private EntityDefinition m_ed;
        public async Task<object> CompileAsync()
        {
            m_ed = await this.GetSchemaDefinitionAsync();
            var es = this.Name.Dehumanize() + "schema.json";
            File.WriteAllText(es, m_ed.ToJsonString(true));

            var options = new CompilerOptions();
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll"));
            
            options.EmbeddedResourceCollection.Add(es);
            var codes = m_ed.GenerateCode();
            var sources = m_ed.SaveSources(codes);

            var adapterCodes = await this.GenerateSourceCodeAsync(options, m_ed.CodeNamespace);
            var adapterSources = m_ed.SaveSources(adapterCodes);

            var result = m_ed.Compile(options, sources.Concat(adapterSources).ToArray());

            result.Errors.ForEach(Console.WriteLine);
            Debug.WriteIf(!result.Result, result.ToJsonString(Formatting.Indented));

            var assembly = Assembly.LoadFrom(result.Output);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, m_ed.EntityDefinitionId, m_ed.Name);

            var edType = assembly.GetType(edTypeName);
            return edType;

        }

        public Type GetAdapterType(string dll)
        {

            var assembly = Assembly.LoadFrom(dll);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, m_ed.EntityDefinitionId, m_ed.Name);

            var edType = assembly.GetType(edTypeName);
            return edType;
        }

        public Type GetEntityType(string dll)
        {

            var assembly = Assembly.LoadFrom(dll);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, m_ed.EntityDefinitionId, m_ed.Name);

            var edType = assembly.GetType(edTypeName);
            return edType;
        }

        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Description { get; set; }

        protected abstract Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces);
        protected abstract Task<EntityDefinition> GetSchemaDefinitionAsync();
    }
}
