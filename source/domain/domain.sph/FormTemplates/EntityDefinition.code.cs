using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class EntityDefinition
    {
        [JsonIgnore]
        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.{this.Plural}.Domain";
        [JsonIgnore]
        public string SourceFile => $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\{Id}.json";
        [JsonIgnore]
        public string TypeName => $"{CodeNamespace}.{Name}";

        /// <summary>
        /// return CodeNamespace.Name, assemblyName
        /// </summary>
        [JsonIgnore]
        public string FullTypeName => $"{CodeNamespace}.{Name}, {ConfigurationManager.ApplicationName}.{Name}";

    }
}
