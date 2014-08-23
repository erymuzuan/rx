using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain.Api;
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


        [HttpPost]
        [Route("children/{table}")]
        public async Task<HttpResponseMessage> GetChildTablesAsync([FromBody]SqlServerAdapter adapter, string table)
        {
            const string SQL = @"Select
                    object_name(rkeyid) Parent_Table,
                    object_name(fkeyid) Child_Table,
                    object_name(constid) FKey_Name,
                    c1.name FKey_Col,
                    c2.name Ref_KeyCol
                    From
                    sys.sysforeignkeys s
                    Inner join sys.syscolumns c1
                    on ( s.fkeyid = c1.id And s.fkey = c1.colid )
                    Inner join syscolumns c2
                    on ( s.rkeyid = c2.id And s.rkey = c2.colid )
                    WHERE object_name(rkeyid) = @table";
            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand(SQL, conn))
            {
                cmd.Parameters.AddWithValue("@table", table);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var childTables = new List<TableRelation>();
                    while (await reader.ReadAsync())
                    {
                        var ct = new TableRelation
                        {
                            Table = reader.GetString(1),
                            Constraint = reader.GetString(2),
                            Column = reader.GetString(3),
                            ForeignColumn = reader.GetString(4)
                        };
                        childTables.Add(ct);
                    }
                    var json = JsonConvert.SerializeObject(new { children = childTables.ToArray(), success = true, status = "OK" });
                    var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
                    return response;
                }


            }
        }


        [HttpPost]
        [Route("generate")]
        public async Task<HttpResponseMessage> GenerateAsync([FromBody]SqlServerAdapter adapter)
        {
            if (null == adapter.Tables || 0 == adapter.Tables.Length)
            {

                var json = JsonConvert.SerializeObject(new { message = "No tables is specified", success = false, status = "Fail" });
                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
                return response;
            }
            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync();

            var json2 = JsonConvert.SerializeObject(new { message = "Success fully compiled", success = cr.Result, status = "OK" });
            var response2 = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json2) };
            return response2;
        }




    }
}
