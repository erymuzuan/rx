using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class MySqlAdapterController : ApiController
    {
        [HttpPost]
        [Route("databases")]
        public async Task<HttpResponseMessage> GetDatabasesAsync([FromBody]MySqlAdapter adapter)
        {
            using (var conn = new MySqlConnection(adapter.ConnectionString))

            using (var cmd = new MySqlCommand("show databases", conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var list = new List<string>();
                    while (await reader.ReadAsync())
                    {
                        list.Add(reader.GetString(0));
                    }
                    var json = JsonConvert.SerializeObject(new { databases = list.ToArray(), success = true, status = "OK" });
                    var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(json) };
                    return response;
                }

            }
        }

        [HttpPost]
        [Route("schemas")]
        public async Task<HttpResponseMessage> GetSchemasAsync([FromBody]MySqlAdapter adapter)
        {
            using (var conn = new MySqlConnection(adapter.ConnectionString))

            using (var cmd = new MySqlCommand("show databases", conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var list = new List<string>();
                    while (await reader.ReadAsync())
                    {
                        list.Add(reader.GetString(0));
                    }
                    var json = JsonConvert.SerializeObject(new { schemas = list.ToArray(), success = true, status = "OK" });
                    var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(json) };
                    return response;
                }

            }
        }

        [HttpPost]
        [Route("tables")]
        public async Task<HttpResponseMessage> GetTablesAsync([FromBody]MySqlAdapter adapter)
        {
            using (var conn = new MySqlConnection(adapter.ConnectionString))

            using (var cmd = new MySqlCommand("show tables", conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var list = new List<string>();
                    while (await reader.ReadAsync())
                    {
                        list.Add(reader.GetString(0));
                    }
                    var json = JsonConvert.SerializeObject(new { tables = list.ToArray(), success = true, status = "OK" });
                    var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(json) };
                    return response;
                }

            }
        }
        
        
        
        //http://stackoverflow.com/questions/733349/list-of-stored-procedures-functions-mysql-command-line
        [HttpPost]
        [Route("procedures")]
        public async Task<HttpResponseMessage> GetProceduresAsync([FromBody]MySqlAdapter adapter)
        {
            using (var conn = new MySqlConnection(adapter.ConnectionString))

            using (var cmd = new MySqlCommand("show procedure status", conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var list = new List<string>();
                    while (await reader.ReadAsync())
                    {
                        list.Add(reader.GetString(0));
                    }
                    var json = JsonConvert.SerializeObject(new { tables = list.ToArray(), success = true, status = "OK" });
                    var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(json) };
                    return response;
                }

            }
        }


    }
}
