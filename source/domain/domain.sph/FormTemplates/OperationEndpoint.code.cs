using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class OperationEndpoint
    {
        [JsonIgnore]
        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.IntegrationApis";

        [JsonIgnore]
        public string AssemblyName => $"{ConfigurationManager.ApplicationName}.OperationEndpoint.{Entity}.{Name}.dll";
        [JsonIgnore]
        public string PdbName => $"{ConfigurationManager.ApplicationName}.OperationEndpoint.{Entity}.{Name}.pdb";
        [JsonIgnore]
        public string TypeName => $"{Entity}{Name}OperationEndpointController";
        [JsonIgnore]
        public string TypeFullName => $"{CodeNamespace}.{TypeName}, {AssemblyName.Replace(".dll", "")}";


    }
}
