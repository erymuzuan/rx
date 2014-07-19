using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    [RoutePrefix("httpadapter")]
    public class HttpAdapterController : ApiController
    {
        [Route("operations/{id}")]
        public async Task<IHttpActionResult> GetOperations(string id)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var json = await store.GetContentAsync(id);
            var temp = Path.GetTempFileName();
            File.WriteAllBytes(temp, json.Content);
            var ha = new HttpAdapter { Har = temp };
            await ha.OpenAsync();


            var tt2 = Path.Combine(Path.GetTempPath(), id + ".json");
            File.WriteAllText(tt2, ha.ToJsonString(true));

            return Ok(ha.OperationDefinitionCollection);
        }

        [Route("operation/{id}/{uuid}")]
        public IHttpActionResult GetOperation(string id, string uuid)
        {

            var tt2 = Path.Combine(Path.GetTempPath(), id + ".json");
            var ha = File.ReadAllText(tt2).DeserializeFromJson<HttpAdapter>();

            return Ok(ha.OperationDefinitionCollection.SingleOrDefault(o => o.Uuid == uuid));
        }
    }
}
