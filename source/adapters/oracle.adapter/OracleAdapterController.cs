using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class OracleAdapterController : Controller
    {
        public async Task<ActionResult> Schemas(string cs)
        {
            using (var conn = new OracleConnection(cs))
            using (var cmd = new OracleCommand("select USERNAME from SYS.ALL_USERS order by USERNAME", conn))
            {
                await conn.OpenAsync();
                using (var reader = (OracleDataReader)await cmd.ExecuteReaderAsync())
                {
                    var list = new List<string>();
                    while (await reader.ReadAsync())
                    {
                        list.Add(reader.GetOracleString(0).Value);
                    }
                    return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
                }

            }
        }
        public async Task<ActionResult> Tables(string id, string cs)
        {
            var schema = id;
            var sql =
                string.Format(
                    "select TABLE_NAME from SYS.ALL_TABLES WHERE OWNER = '{0}' order by TABLE_NAME",
                    schema);
            using (var conn = new OracleConnection(cs))
            using (var cmd = new OracleCommand(sql, conn))
            {
                await conn.OpenAsync();
                using (var reader = (OracleDataReader)await cmd.ExecuteReaderAsync())
                {
                    var list = new List<string>();
                    while (await reader.ReadAsync())
                    {
                        list.Add(reader.GetOracleString(0).Value);
                    }
                    return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
                }

            }
        }


        public async Task<ActionResult> Relationships()
        {
            var json = this.GetRequestBody();
            var o = JObject.Parse(json);
            var table = o.SelectToken("$.table").Value<string>();
            var schema = o.SelectToken("$.schema").Value<string>();
            var cs = o.SelectToken("$.connectionString").Value<string>();
            var sql =
                string.Format(
                    @"
SELECT FK.TABLE_NAME AS CHILD_TABLE
     , SRC.TABLE_NAME AS PARENT_TABLE
     , FK.CONSTRAINT_NAME AS FK_CONSTRAINT
     , SRC.CONSTRAINT_NAME AS REFERENCED_CONSTRAINT
FROM ALL_CONSTRAINTS FK
JOIN ALL_CONSTRAINTS SRC ON FK.R_CONSTRAINT_NAME = SRC.CONSTRAINT_NAME
WHERE FK.CONSTRAINT_TYPE = 'R'
  AND SRC.OWNER = '{1}'
  AND (SRC.TABLE_NAME = '{0}' OR FK.TABLE_NAME = '{0}')",
                    table, schema);
            using (var conn = new OracleConnection(cs))
            using (var cmd = new OracleCommand(sql, conn))
            {
                await conn.OpenAsync();
                using (var reader = (OracleDataReader)await cmd.ExecuteReaderAsync())
                {
                    var list = new List<object>();
                    while (await reader.ReadAsync())
                    {
                        var r = new
                        {
                            src_table_name = reader.GetOracleString(0).Value,
                            fk_table_name = reader.GetOracleString(1).Value,
                            src_constraint_name = reader.GetOracleString(2).Value,
                            fk_constraint_name = reader.GetOracleString(3).Value,
                        };
                        list.Add(r);
                    }
                    return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
                }

            }
        }

        public string GetRequestBody()
        {
            using (var reader = new StreamReader(this.Request.InputStream))
            {
                string text = reader.ReadToEnd();
                return text;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Generate()
        {
            var json = this.GetRequestBody();
            var o = JObject.Parse(json);

            var tables = o.SelectToken("$.tables").Values<string>();
            var list = new List<string>();
            foreach (var table in tables)
            {
                var ora = new OracleAdapter
                {
                    ConnectionString = o.SelectToken("$.connectionString").Value<string>(),
                    Table = table,
                    Name = o.SelectToken("$.name").Value<string>(),
                    Description = o.SelectToken("$.description").Value<string>(),
                    Schema = o.SelectToken("$.schema").Value<string>()
                };
                await ora.OpenAsync();
                var type = await ora.CompileAsync();
                list.Add(type.ToString());

            }
            this.Response.ContentType = "application/json";
            return Content(JsonConvert.SerializeObject(list.ToArray()));
        }
    }
}