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
            search.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Search()");
            search.AppendLine("       {");
            search.AppendFormat(@"
            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = ""{1}/{0}/_search"";

            using(var client = new System.Net.Http.HttpClient())
            {{
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PostAsync(url, request);
                var content = response.Content as System.Net.Http.StreamContent;
                if (null == content) throw new Exception(""Cannot execute query on es "" + request);
                this.Response.ContentType = ""application/json; charset=utf-8"";
                return Content(await content.ReadAsStringAsync());
            }}
            ", ed.Name.ToLower(), ConfigurationManager.ApplicationName.ToLower());
            search.AppendLine();
            search.AppendLine("       }");
            search.AppendLine();
            return new Method { Code = search.ToString() };
        }


    }
}