using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.Integrations.Adapters
{
    [RoutePrefix("api/sqlserveradapter")]
    public class SqlServerAdapterController : ApiController
    {
        [HttpGet]
        [Route("databases")]
        public async Task<IHttpActionResult> GetDatabasesAsync([FromUri]string server, [FromUri]bool trustedConnection, [FromUri]string userId = null, [FromUri]string password = null)
        {
            var adapter = new SqlServerAdapter
            {
                Server = server,
                TrustedConnection = trustedConnection,
                UserId = userId,
                Password = password
            };

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
                    return Ok(list);
                }

            }
        }
        [HttpGet]
        [Route("schema/{database}")]
        public async Task<IHttpActionResult> GetSchemaAsync(string database, [FromUri]string server, [FromUri]bool trustedConnection, [FromUri]string userId = null, [FromUri]string password = null)
        {
            var adapter = new SqlServerAdapter
            {
                Server = server,
                TrustedConnection = trustedConnection,
                UserId = userId,
                Password = password,
                Database = database
            };


            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand("SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE CATALOG_NAME = @Database", conn))
            {
                cmd.Parameters.AddWithValue("@Database", database);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var list = new List<string>();
                    while (await reader.ReadAsync())
                    {
                        list.Add(reader.GetString(0));
                    }
                    return Ok(list);
                }

            }
        }
    }
}
