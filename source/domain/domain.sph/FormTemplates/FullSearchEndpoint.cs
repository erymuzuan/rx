using System.Text;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class FullSearchEndpoint : DomainObject
    {
        public Method GenerateSearchAction(EntityDefinition ed)
        {
            var search = new StringBuilder();

            search.AppendLinf("       [HttpPost]");
            search.AppendLinf("       [Route(\"search\")]");
            search.AppendLinf("       public async Task<IHttpActionResult> Search([RawBody]string query)");
            search.AppendLine("       {");
            search.Append($@"
            var repos = ObjectBuilder.GetObject<IReadonlyRepository<{ed.Name}>>();
            var response = await repos.SearchAsync(query);
            return Json(response);
            ");
            search.AppendLine();
            search.AppendLine("       }");
            search.AppendLine();
            return new Method { Code = search.ToString() };
        }


    }
}