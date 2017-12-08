using System.Linq;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class QueryEndpoint
    {
        public override string ToString()
        {
            return this.Name;
        }

        [JsonIgnore]
        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.Api";
        [JsonIgnore]
        public string AssemblyName => $"{ConfigurationManager.ApplicationName}.QueryEndpoint.{Entity}.{Id}";
        [JsonIgnore]
        public string TypeName => $"{ControllerName}Controller";
        public string ControllerName => $"{Entity}{Name.ToPascalCase()}QueryEndpoint";
        [JsonIgnore]
        public string TypeFullName => $"{CodeNamespace}.{TypeName}, {AssemblyName}";



        public QueryDsl QueryDsl
        {
            get
            {
                var query = new QueryDsl(this.FilterCollection.ToArray(), this.SortCollection.ToArray());
                if (this.MemberCollection.Any())
                    query.Fields.AddRange(this.MemberCollection);
                return query;
            }
        }




    }
}