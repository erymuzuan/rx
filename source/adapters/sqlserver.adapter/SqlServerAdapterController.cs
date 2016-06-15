﻿using System;
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
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Integrations.Adapters
{
    [RoutePrefix("sqlserver-adapter")]
    public class SqlServerAdapterController : BaseApiController
    {
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


        [HttpGet]
        [GetRoute("sproc/{id}/{schema}/{name}")]
        public async Task<HttpResponseMessage> GetSprocAsync(string id, string schema, string name)
        {

            var context = new SphDataContext();
            var adapter = (await context.LoadOneAsync<Adapter>(a => a.Id == id)) as SqlServerAdapter;
            if (null == adapter)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var sproc = adapter.OperationDefinitionCollection.OfType<SprocOperationDefinition>()
                    .SingleOrDefault(x => x.Name == name);
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

            var sproc =
                adapter.OperationDefinitionCollection.OfType<SprocOperationDefinition>()
                    .SingleOrDefault(x => x.Name == name);
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
                return Ok(sb.ToString());

            }
        }




        [HttpPatch]
        [Route("sproc/{id}")]
        public async Task<IHttpActionResult> UpdateSprocDefinitionAsync(string id, [JsonBody]SprocOperationDefinition operation)
        {
            var context = new SphDataContext();
            var sa = (await context.LoadOneAsync<Adapter>(x => x.Id == id)) as SqlServerAdapter;
            if (null == sa)
                return NotFound();

            var op = sa.OperationDefinitionCollection.OfType<SprocOperationDefinition>().SingleOrDefault(o => o.Uuid == operation.Uuid);
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
            var noSprocs = 0 == adapter.OperationDefinitionCollection.Count;

            if (noTables && noSprocs)
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




    }

    public class DatabaseObjectsViewModel
    {
        public void Add(SprocOperationDefinition sproc)
        {
            this.Sprocs.Add(sproc);
        }
        public void Add(TableObjectViewModel table)
        {
            this.Tables.Add(table);
        }
        public IList<SprocOperationDefinition> Sprocs { get; set; } = new List<SprocOperationDefinition>();
        public IList<TableObjectViewModel> Tables { get; set; } = new List<TableObjectViewModel>();
        public class TableObjectViewModel
        {
            public string Name { get; set; }
            public IList<string> RowVersionColumnOptions { get; set; } = new List<string>();
            public IList<string> ModifiedDateColumnOptions { get; set; } = new List<string>();

            public void AddColumn(string name, string type)
            {
                switch (type)
                {
                    case "datetime":
                    case "smalldatetime":
                    case "datetime2":
                        this.ModifiedDateColumnOptions.Add(name);
                        break;
                    case "rowversion":
                    case "timestamp":
                        this.RowVersionColumnOptions.Add(name);
                        break;
                    default: throw new Exception($"Column {name} with type {type} is not supported");
                }

            }
        }
    }
}
