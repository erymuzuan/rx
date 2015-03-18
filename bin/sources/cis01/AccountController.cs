using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Xml.Serialization;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace Dev.Adapters.dbo.cis01
{
   [RoutePrefix("api/dbo/account")]
   public partial class AccountController : ApiController
   {

       [Route("{accountId:int}")]
       [HttpDelete]
       public async Task<HttpResponseMessage> Remove(int accountId)
       {

            var context = new AccountAdapter();
            await context.DeleteAsync(accountId);

            var  response = Request.CreateResponse(HttpStatusCode.Accepted,new {success = true, status="OK"} );
            return response;
       }

       [Route("{accountId:int}")]
       [HttpGet]
       public async Task<object> Get(int accountId)
       {

            var context = new AccountAdapter();
            var item  =await context.LoadOneAsync(accountId);

            if(null == item)
                return new {success = false, status = "NotFound", statusCode=404, url="/api/docs/404", message ="item not found"};
            return new {success = true, status = "OK", item};


       }

       [Route("")]
       [HttpPost]
       public async Task<HttpResponseMessage> Insert([FromBody]Account item)
       {

            if(null == item) throw new ArgumentNullException("item");
            var context = new AccountAdapter();
            await context.InsertAsync(item);
            var  response = Request.CreateResponse(HttpStatusCode.Accepted,new {success = true, status="OK", item} );
            return response;
       }

       [Route("")]
       [HttpGet]
       public async Task<object> List(string filter = null, int page = 1, int size = 40, bool includeTotal = false, string orderby = null)
       {

           if (size > 200)
                throw new ArgumentException("Your are not allowed to do more than 200", "size");

            var translator = new OdataSqlTranslator<Account>(null,"Account" ){Schema = "dbo"};
            var sql = translator.Select(string.IsNullOrWhiteSpace(filter) ? "AccountId gt 0" : filter, orderby);
            var count = 0;

            var context = new AccountAdapter();
            var nextPageToken = string.Empty;
            var lo = await context.LoadAsync(sql, page, size);
            if (includeTotal || page > 1)
            {
                var translator2 = new OdataSqlTranslator<Account>(null, "Account"){Schema = "dbo"};
                var countSql = translator2.Count(filter);
                count = await context.ExecuteScalarAsync<int>(countSql);

                if (count >= lo.ItemCollection.Count())
                    nextPageToken = string.Format(
                        "/api/dbo/account/?filter={0}&includeTotal=true&page={1}&size={2}", filter, page + 1, size);
            }

            string previousPageToken = string.Format("/api/dbo/account/?filter={0}&includeTotal=true&page={1}&size={2}", filter, page - 1, size);
            if(page == 1)
                previousPageToken = null;
            var json = new
            {
                count,
                page,
                nextPageToken,
                previousPageToken,
                size,
                results = lo.ItemCollection.ToArray()
            };
            return json;
            
       }


       [Route]
       [HttpPut]
       public async Task<HttpResponseMessage> Save([FromBody]Account item)
       {

            if(null == item) throw new ArgumentNullException("item");
            var context = new AccountAdapter();
            await context.UpdateAsync(item);

            var  response = Request.CreateResponse(HttpStatusCode.Accepted,new {success = true, status="OK", item} );
            return response;
       }

   }
}
