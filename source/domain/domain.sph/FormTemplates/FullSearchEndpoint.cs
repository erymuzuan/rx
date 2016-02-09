using System.Text;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class FullSearchEndpoint : DomainObject
    {
        public Method GenerateSearchAction(EntityDefinition ed)
        {
            var search = new StringBuilder();

            search.AppendLinf("       [Route(\"search\")]");
            search.AppendLinf("       public async Task<IHttpActionResult> Search([RawBody]string json)");
            search.AppendLine("       {");
            search.Append($@"
            var request = new System.Net.Http.StringContent(json);
            var url = ""{ConfigurationManager.ApplicationName.ToLowerInvariant()}/{ed.Name.ToLowerInvariant()}/_search"";

            using(var client = new System.Net.Http.HttpClient())
            {{
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PostAsync(url, request);
                var content = response.Content as System.Net.Http.StreamContent;
                if (null == content) throw new Exception(""Cannot execute query on es "" + request);
                return Json(await content.ReadAsStringAsync());
            }}
            ");
            search.AppendLine();
            search.AppendLine("       }");
            search.AppendLine();
            return new Method { Code = search.ToString() };
        }


    }
}