using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    public abstract partial class Adapter : Entity
    {
        [JsonIgnore]
        public string RoutePrefix => $"api/{Id}";
        [JsonIgnore]
        public virtual string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.Adapters.{this.Name.ToPascalCase()}";
        [JsonIgnore]
        public virtual string AssemblyName => $"{ConfigurationManager.ApplicationName}.{Name}";
        public abstract string OdataTranslator { get; }


    }
}
