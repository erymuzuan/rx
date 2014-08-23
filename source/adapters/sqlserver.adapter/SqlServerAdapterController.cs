using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{
    [RoutePrefix("sqlserver-adapter")]
    public class SqlServerAdapterController : ApiController
    {
        [HttpPost]
        [Route("databases")]
        public async Task<HttpResponseMessage> GetDatabasesAsync([FromBody]SqlServerAdapter adapter)
        {
            using (var conn = new SqlConnection(adapter.ConnectionString))

            using (var cmd = new SqlCommand("select name from sysdatabases", conn))
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
                    var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
                    return response;
                }

            }
        }
        [HttpPost]
        [Route("schema")]
        public async Task<HttpResponseMessage> GetSchemaAsync([FromBody]SqlServerAdapter adapter)
        {
            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand("SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE CATALOG_NAME = @Database", conn))
            {
                cmd.Parameters.AddWithValue("@Database", adapter.Database);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var list = new List<string>();
                    while (await reader.ReadAsync())
                    {
                        list.Add(reader.GetString(0));
                    }
                    var json = JsonConvert.SerializeObject(new { schema = list.ToArray(), success = true, status = "OK" });
                    var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
                    return response;
                }

            }
        }

        [HttpPost]
        [Route("objects")]
        public async Task<HttpResponseMessage> GetDatabaseOjectsAsync([FromBody]SqlServerAdapter adapter)
        {
            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand("SELECT schema_id FROM sys.schemas WHERE [name] = @Schema", conn))
            {
                cmd.Parameters.AddWithValue("@Schema", adapter.Schema);
                await conn.OpenAsync();
                var schemaId = (int)(await cmd.ExecuteScalarAsync());

                var tables = new List<string>();
                using (var tableCommand = new SqlCommand("SELECT * FROM sys.all_objects WHERE [schema_id] = @schema_id AND [type] = 'U'", conn))
                {
                    tableCommand.Parameters.AddWithValue("@schema_id", schemaId);
                    using (var reader = await tableCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }

                using (var spocCommand = new SqlCommand("SELECT * FROM sys.all_objects WHERE [schema_id] = @schema_id AND [type] = 'P'", conn))
                {
                    spocCommand.Parameters.AddWithValue("@schema_id", schemaId);
                    using (var reader = await spocCommand.ExecuteReaderAsync())
                    {
                        var sprocs = new List<string>();
                        while (await reader.ReadAsync())
                        {
                            sprocs.Add(reader.GetString(0));
                        }
                        var json = JsonConvert.SerializeObject(new { sprocs = sprocs.ToArray(), tables = tables.ToArray(), success = true, status = "OK" });
                        var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
                        return response;
                    }
                }

            }
        }


    }
}
