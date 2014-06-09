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
        public string Table { get; set; }
        public string Schema { get; set; }
    
        public virtual string CodeNamespace
        {
            get { return string.Format("{0}.Adapters.{1}", ConfigurationManager.ApplicationName, this.Schema); }
        }
        private TableDefinition m_ed;
        public async Task<Type> CompileAsync()
        {
            m_ed = await this.GetSchemaDefinitionAsync();
            m_ed.CodeNamespace = this.CodeNamespace;
            var es = this.Name.Dehumanize() + "schema.json";
            File.WriteAllText(es, m_ed.ToJsonString(true));

            var options = new CompilerOptions();
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll"));
            options.AddReference(typeof(System.Data.UpdateStatus));
            options.AddReference(typeof(System.Configuration.ConfigurationManager));
            
            options.EmbeddedResourceCollection.Add(es);
            var codes = m_ed.GenerateCode();
            var sources = m_ed.SaveSources(codes);

            var adapterCodes = await this.GenerateSourceCodeAsync(options, m_ed.CodeNamespace);
            var adapterSources = m_ed.SaveSources(adapterCodes);

            var result = m_ed.Compile(options, sources.Concat(adapterSources).ToArray());

            if(!result.Result)
                throw new Exception(string.Join("\r\n", result.Errors.Select(e => e.ToString())));

            var assembly = Assembly.LoadFrom(result.Output);
            var edTypeName = string.Format("{0}.Adapters.{1}.{2}", ConfigurationManager.ApplicationName, this.Schema, m_ed.Name);

            var edType = assembly.GetType(edTypeName);
            return edType;

        }

        public Type GetAdapterType(string dll)
        {

            var assembly = Assembly.LoadFrom(dll);
            var edTypeName = string.Format("{0}.Adapters.{1}.{2}", ConfigurationManager.ApplicationName, this.Schema, m_ed.Name);

            var edType = assembly.GetType(edTypeName);
            return edType;
        }

        public Type GetEntityType(string dll)
        {

            var assembly = Assembly.LoadFrom(dll);
            var edTypeName = string.Format("{0}.Adapters.{1}.{2}", ConfigurationManager.ApplicationName, this.Schema, m_ed.Name);

            var edType = assembly.GetType(edTypeName);
            return edType;
        }

        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Description { get; set; }

        protected abstract Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces);
        protected abstract Task<TableDefinition> GetSchemaDefinitionAsync();
    }
}
