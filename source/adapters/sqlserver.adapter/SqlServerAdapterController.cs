using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{
    [RoutePrefix("sqlserver-adapter")]
    public class SqlServerAdapterController : BaseApiController
    {
        public SqlServerAdapterController()
        {
            var developerService = ObjectBuilder.GetObject<SqlAdapterDeveloperService>();
            if (null == developerService)
            {
                developerService = new SqlAdapterDeveloperService();
                ObjectBuilder.AddCacheList(developerService);
            }
            if (null == developerService.ColumnGenerators)
                ObjectBuilder.ComposeMefCatalog(developerService);
        }
        [HttpGet]
        [Route("resource/{resource}")]
        public HttpResponseMessage GetEmbeddedResource(string resource)
        {
            HttpContent content = new StringContent(Properties.Resources._ko_adapter_sqlserver);
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(SqlServerAdapter).Namespace}.{resource}");
            if (null != stream)
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    var bytes = memoryStream.ToArray();
                    content = new ByteArrayContent(bytes);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
        }


        [HttpGet]
        [Route("databases")]
        public async Task<HttpResponseMessage> GetDatabasesAsync([FromUri(Name = "server")]string server = ".",
            [FromUri(Name = "userid")]string userid = "",
            [FromUri(Name = "password")]string password = "",
            [FromUri(Name = "trusted")]bool trusted = true)
        {
            var adapter = new SqlServerAdapter
            {
                Server = server,
                TrustedConnection = trusted,
                UserId = userid,
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
                    var json = JsonConvert.SerializeObject(new { databases = list.ToArray(), success = true, status = "OK" });
                    var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(json) };
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
                    var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(json) };
                    return response;
                }

            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetAdapterAsync(string id,
            [FromUri(Name = "server")]string server,
            [FromUri(Name = "database")]string database,
            [FromUri(Name = "trusted")]bool? trusted,
            [FromUri(Name = "userid")]string user,
            [FromUri(Name = "password")]string password)
        {
            var context = new SphDataContext();
            var adapter = context.LoadOneFromSources<Adapter>(x => x.Id == id) as SqlServerAdapter;
            if (null == adapter)
                return NotFound($"Cannot find SQL server adapter with id {id}");

            if (!string.IsNullOrWhiteSpace(database))
                adapter.Database = database;
            if (!string.IsNullOrWhiteSpace(server))
                adapter.Server = server;
            if (!string.IsNullOrWhiteSpace(user))
                adapter.UserId = user;
            if (!string.IsNullOrWhiteSpace(password))
                adapter.Password = password;
            if (trusted.HasValue)
                adapter.TrustedConnection = trusted.Value;
            var connected = await adapter.LoadDatabaseObjectAsync(adapter.ConnectionString);

            return Json(adapter.ToJsonString(), new CacheMetadata
            {
                Etag = connected ? "connected" : "not connected"
            });
        }



        [HttpGet]
        [GetRoute("sproc/{id}/{schema}/{name}")]
        public async Task<HttpResponseMessage> GetSprocAsync(string id, string schema, string name)
        {

            var context = new SphDataContext();
            var adapter = (await context.LoadOneAsync<Adapter>(a => a.Id == id)) as SqlServerAdapter;
            if (null == adapter)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var sproc = adapter.OperationDefinitionCollection.SingleOrDefault(x => x.Name == name);
            if (null == sproc)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            return new JsonResponseMessage(sproc.ToJsonString(true));
        }

        [HttpGet]
        [Route("sproc-text/{id}/{schema}/{name}")]
        public async Task<IHttpActionResult> GetSprocTextAsync(string id, string schema, string name)
        {

            var context = new SphDataContext();
            var adapter = (await context.LoadOneAsync<Adapter>(a => a.Id == id)) as SqlServerAdapter;
            if (null == adapter)
                return NotFound($"Cannot find any adapter with Id {id}");

            var sproc = adapter.OperationDefinitionCollection.SingleOrDefault(x => x.Name == name);
            if (null == sproc)
                return NotFound($"Cannot find any sproc with name {name}");

            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand("sp_helptext", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@objname", schema + "." + name);
                await conn.OpenAsync();

                var sb = new StringBuilder();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        sb.Append(reader.GetString(0));
                    }
                }
                return Ok(sb.ToString(), "text/plain");

            }
        }


        [HttpPatch]
        [Route("sproc/{id}")]
        public async Task<IHttpActionResult> UpdateSprocDefinitionAsync(string id, [JsonBody]OperationDefinition operation)
        {
            var context = new SphDataContext();
            var sa = (await context.LoadOneAsync<Adapter>(x => x.Id == id)) as SqlServerAdapter;
            if (null == sa)
                return NotFound();

            var op = sa.OperationDefinitionCollection.SingleOrDefault(o => o.Uuid == operation.Uuid);
            if (null == op)
                sa.OperationDefinitionCollection.Add(operation);
            else
                sa.OperationDefinitionCollection.Replace(op, operation);

            using (var session = context.OpenSession())
            {
                session.Attach(sa);
                await session.SubmitChanges();
            }


            return Ok(new { success = true, status = "OK", uuid = operation.Uuid });
        }

        [HttpPost]
        [Route("generate")]
        public async Task<IHttpActionResult> GenerateAsync([JsonBody]SqlServerAdapter adapter)
        {
            var noTables = adapter.TableDefinitionCollection.Count == 0;
            var noOps = 0 == adapter.OperationDefinitionCollection.Count;

            if (noTables && noOps)
            {
                return Ok(new { message = "No tables is specified", success = false, status = "Fail" });
            }
            await adapter.OpenAsync(true);
            var result = await adapter.CompileAsync();

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(adapter);
                await session.SubmitChanges("Publish");
            }

            return Json(new
            {
                message = result.Result ? "Successfully compiled" : "There are errors in your adapter",
                errors = result.Errors.ToArray(),
                success = result.Result,
                status = "OK"
            });
        }



        [HttpGet]
        [Route("table-options")]
        public async Task<IHttpActionResult> GetTableOptionsAsync(
            [FromUri(Name = "server")] string server,
            [FromUri(Name = "database")] string database,
            [FromUri(Name = "trusted")] bool trusted = true,
            [FromUri(Name = "userid")] string user = "",
            [FromUri(Name = "password")] string password = "")
        {
            var adapter = new SqlServerAdapter
            {
                Server = server,
                Database = database,
                TrustedConnection = trusted,
                UserId = user,
                Password = password
            };

            var tables = await adapter.GetTableOptionsAsync(true);
            var views = await adapter.GetViewOptionsAsync();
            var options = tables.Concat(views);
            var json = "[" + options.ToString(",\r\n", x => x.ToJsonString(false)) + "]";
            return Json(json);
        }


        [HttpGet]
        [Route("table-options/{schema}/{name}")]
        public async Task<IHttpActionResult> GetTableOptionDetailAsync(
            string schema,
            string name,
            [FromUri(Name = "server")] string server,
            [FromUri(Name = "database")] string database,
            [FromUri(Name = "trusted")] bool trusted = true,
            [FromUri(Name = "strategy")] string strategy = null,
            [FromUri(Name = "userid")] string user = "",
            [FromUri(Name = "password")] string password = "")
        {
            var adapter = new SqlServerAdapter
            {
                Server = server,
                Database = database,
                TrustedConnection = trusted,
                UserId = user,
                Password = password,
                ColumnDisplayNameStrategy = strategy
            };

            var table = await adapter.GetTableOptionDetailsAsync(schema, name);
            if (null == table) return NotFound($"No object with name {schema}.{name} found in {server}.{database}");

            var ds = ObjectBuilder.GetObject<IDeveloperService>();
            table.ControllerActionCollection.ClearAndAddRange(ds.ActionCodeGenerators.Where(x => x.Applicable(table)));

            return Json(table.ToJsonString());
        }

        [HttpGet]
        [Route("operation-options")]
        public async Task<IHttpActionResult> GetOperationOptionsAsync(
            [FromUri(Name = "server")] string server,
            [FromUri(Name = "database")] string database,
            [FromUri(Name = "trusted")] bool trusted = true,
            [FromUri(Name = "userid")] string user = "",
            [FromUri(Name = "password")] string password = "")
        {
            var adapter = new SqlServerAdapter
            {
                Server = server,
                Database = database,
                TrustedConnection = trusted,
                UserId = user,
                Password = password
            };


            var tables = await adapter.GetOperationOptionsAsync();
            var json = "[" + tables.ToString(",\r\n", x => x.ToJsonString(false)) + "]";
            return Json(json);
        }

        [HttpGet]
        [Route("operation-options/{type}/{schema}/{name}")]
        public async Task<IHttpActionResult> GetSprocDetailsAsync(string type, string schema, string name,
            [FromUri(Name = "server")] string server,
            [FromUri(Name = "database")] string database,
            [FromUri(Name = "trusted")] bool trusted = true,
            [FromUri(Name = "strategy")] string strategy = null,
            [FromUri(Name = "userid")] string user = "",
            [FromUri(Name = "password")] string password = "")
        {
            var adapter = new SqlServerAdapter
            {
                Server = server,
                Database = database,
                TrustedConnection = trusted,
                UserId = user,
                Password = password,
                ColumnDisplayNameStrategy = strategy
            };

            var operation = await adapter.CreateAsync(type, schema, name);
            return Json(operation.ToJsonString());
        }



        [Route("action-generators-designers")]
        public IHttpActionResult GetActionGeneratorDesigners()
        {
            var generators = ObjectBuilder.GetObject<IDeveloperService>().ActionCodeGenerators;
            var code = new StringBuilder();
            foreach (var ga in generators)
            {
                code.AppendLine($@"         
            <!-- ko if : ko.unwrap($type) === ""{ga.GetType().GetShortAssemblyQualifiedName()}"" -->
            <h3>{ga.Name}</h3>
            {ga.GetDesignerHtmlView()}
            <!-- /ko -->");
            }

            return Ok(code.ToString(), "text/html");

        }

    }

}
