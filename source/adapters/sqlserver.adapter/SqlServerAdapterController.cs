﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public HttpResponseMessage GetEmbeddedResource()
        {

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(Properties.Resources._ko_adapter_sqlserver)
            };
        }


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

        [HttpPost]
        [Route("objects")]
        public async Task<IHttpActionResult> GetDatabaseOjectsAsync([FromBody]SqlServerAdapter adapter)
        {
            var vm = new DatabaseObjectsViewModel();
            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand("SELECT schema_id FROM sys.schemas WHERE [name] = @Schema", conn))
            {
                cmd.Parameters.AddWithValue("@Schema", adapter.Schema);
                await conn.OpenAsync();
                var schemaId = (int)(await cmd.ExecuteScalarAsync());

                using (var tableCommand = new SqlCommand("SELECT * FROM sys.all_objects WHERE [schema_id] = @schema_id AND [type] = 'U'", conn))
                {
                    tableCommand.Parameters.AddWithValue("@schema_id", schemaId);
                    using (var reader = await tableCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var table = new DatabaseObjectsViewModel.TableObjectViewModel { Name = reader.GetString(0) };
                            vm.Add(table);
                        }
                    }
                }
                using (var columnCommand = new SqlCommand(@"
SELECT 
         o.name as 'Table'
        ,c.name as 'Column'
        ,t.name as 'Type' 
    FROM 
        sys.objects o INNER JOIN sys.all_columns c
        ON c.object_id = o.object_id
        INNER JOIN sys.types t 
        ON c.system_type_id = t.system_type_id
        INNER JOIN sys.schemas s
        ON s.schema_id = o.schema_id
    WHERE 
        o.type = 'U'
        AND s.name = @schema
        AND t.name <> N'sysname'
        AND t.is_user_defined= 0
		AND c.is_nullable = 0
		AND t.name IN ('smalldatetime', 'datetime','datetime2', 'rowversion', 'timestamp')
    ORDER 
        BY o.type", conn))
                {
                    columnCommand.Parameters.AddWithValue("@schema", adapter.Schema);
                    using (var reader = await columnCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var table = vm.Tables.SingleOrDefault(x => x.Name == (string)reader["Table"]);
                            table?.AddColumn((string)reader["Column"], (string)reader["Type"]);
                        }
                    }
                }

                using (var spocCommand = new SqlCommand("SELECT * FROM sys.all_objects WHERE [schema_id] = @schema_id AND [type] = 'P'", conn))
                {
                    spocCommand.Parameters.AddWithValue("@schema_id", schemaId);
                    using (var reader = await spocCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var sp = await this.GetStoreProcedureAsync(adapter, reader.GetString(0));
                            vm.Add(sp);
                        }
                    }
                }
                return Json(vm, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

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

        private async Task<SprocOperationDefinition> GetStoreProcedureAsync(SqlServerAdapter adapter, string name)
        {

            const string SQL = @"
select * from information_schema.PARAMETERS
where SPECIFIC_NAME = @name
and SPECIFIC_SCHEMA = @schema
order by ORDINAL_POSITION";

            var uuid = Guid.NewGuid().ToString();
            var od = new SprocOperationDefinition
            {
                Name = name,
                MethodName = name.ToCsharpIdentitfier(),
                Uuid = uuid,
                CodeNamespace = adapter.CodeNamespace,
                WebId = uuid,
            };
            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand(SQL, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@schema", adapter.Schema);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var dt = (string)reader["DATA_TYPE"];
                        var cml = reader["CHARACTER_MAXIMUM_LENGTH"].ReadNullable<int>();
                        var mode = (string)reader["PARAMETER_MODE"];
                        var pname = (string)reader["PARAMETER_NAME"];
                        var position = reader["ORDINAL_POSITION"].ReadNullable<int>();

                        var member = new SprocParameter
                        {
                            Name = pname,
                            FullName = pname,
                            SqlType = dt,
                            Type = dt.GetClrType(),
                            IsNullable = cml == 0,
                            MaxLength = cml,
                            Mode = mode == "IN" ? ParameterMode.In : ParameterMode.Out,
                            Position = position ?? 0,
                            WebId = Guid.NewGuid().ToString()
                        };
                        if (mode == "IN" || mode == "INOUT")
                            od.RequestMemberCollection.Add(member);
                        if (mode == "OUT" || mode == "INOUT")
                        {
                            SqlDbType t;
                            Enum.TryParse(dt, true, out t);
                            var rm = new SprocResultMember
                            {
                                Name = pname,
                                SqlDbType = t,
                                Type = dt.GetClrType()
                            };
                            od.ResponseMemberCollection.Add(rm);
                        }
                    }
                }

            }


            var retVal = new SprocResultMember
            {
                Name = "@return_value",
                Type = typeof(int),
                SqlDbType = SqlDbType.Int
            };
            od.ResponseMemberCollection.Add(retVal);
            return od;
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
        public async Task<HttpResponseMessage> GenerateAsync([JsonBody]SqlServerAdapter adapter)
        {
            if (null == adapter.Tables || 0 == adapter.Tables.Length)
            {

                var json = JsonConvert.SerializeObject(new { message = "No tables is specified", success = false, status = "Fail" });
                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
                return response;
            }
            await adapter.OpenAsync(true);
            var result = await adapter.CompileAsync();

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(adapter);
                await session.SubmitChanges("Publish");
            }

            var json2 = JsonConvert.SerializeObject(new
            {
                message = result.Result ? "Successfully compiled" : "There are errors in your adapter",
                errors = result.Errors.ToArray(),
                success = result.Result,
                status = "OK"
            });
            var response2 = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json2) };
            return response2;
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
