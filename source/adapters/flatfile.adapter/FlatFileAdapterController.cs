using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{
    [RoutePrefix("flatfile-adapter")]
    public class FlatFileAdapterController : ApiController
    {

        [HttpGet]
        [Route("resource/{resource}")]
        public HttpResponseMessage GetEmbeddedResource(string resource)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };
        }
    
        [HttpPost]
        [Route("generate")]
        public async Task<HttpResponseMessage> GenerateAsync([FromBody]FlatFileAdapter adapter)
        {
            if (null == adapter.Tables || 0 == adapter.Tables.Length)
            {

                var json = JsonConvert.SerializeObject(new { message = "No tables is specified", success = false, status = "Fail" });
                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
                return response;
            }
            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(adapter);
                await session.SubmitChanges("Publish");
            }

            var json2 = JsonConvert.SerializeObject(new { message = "Successfully compiled", success = cr.Result, status = "OK" });
            var response2 = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json2) };
            return response2;
        }


    }
}