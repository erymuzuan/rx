using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{
    [RoutePrefix("mysql-adapter")]
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
                    var json = JsonConvert.SerializeObject(new { schema = list.ToArray(), success = true, status = "OK" });
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


        [HttpPost]
        [Route("objects")]
        public async Task<HttpResponseMessage> GetObjectsAsync([FromBody]MySqlAdapter adapter)
        {
            var connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", adapter.Server, "information_schema", adapter.UserId, adapter.Password);

            using (var conn = new MySqlConnection(connectionString))
            using (var pcmd = new MySqlCommand("SELECT SPECIFIC_NAME, ROUTINE_DEFINITION FROM ROUTINES" +
                                               " WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_SCHEMA = @schema", conn))
            using (var tcmd = new MySqlCommand("SELECT TABLE_NAME FROM TABLES WHERE TABLE_SCHEMA = @schema", conn))
            {
                pcmd.Parameters.AddWithValue("@schema", adapter.Schema);
                tcmd.Parameters.AddWithValue("@schema", adapter.Schema);
                await conn.OpenAsync();
                var procs = new List<object>();
                using (var reader = await pcmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        procs.Add(new
                        {
                            Name = reader.GetString(0),
                            Text = reader.GetString(1)
                        });
                    }
                }
                var tables = new List<string>();
                using (var reader = await tcmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tables.Add(reader.GetString(0) );
                    }
                }
                var json = JsonConvert.SerializeObject(new { tables = tables.ToArray(), sprocs = procs.ToArray(), success = true, status = "OK" });
                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(json) };
                return response;

            }
        }


        [HttpPost]
        [Route("children/{table}")]
        public async Task<HttpResponseMessage> GetChildTablesAsync([FromBody]MySqlAdapter adapter, string table)
        {
            const string SQL = @"SELECT *
FROM
  KEY_COLUMN_USAGE
WHERE
  REFERENCED_TABLE_NAME = '{0}'
  AND TABLE_SCHEMA = '{1}';";

            var connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", adapter.Server, "information_schema", adapter.UserId, adapter.Password);
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(string.Format(SQL, table, adapter.Database), conn))
            {


                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var childTables = new List<TableRelation>();
                    while (await reader.ReadAsync())
                    {
                        var ct = new TableRelation
                        {
                            Table = (string)reader["TABLE_NAME"],
                            Constraint = (string)reader["CONSTRAINT_NAME"],
                            Column = (string)reader["REFERENCED_COLUMN_NAME"],
                            ForeignColumn = (string)reader["COLUMN_NAME"]
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
        public async Task<HttpResponseMessage> GenerateAsync([FromBody]MySqlAdapter adapter)
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
