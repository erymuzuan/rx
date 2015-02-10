using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Humanizer;
using Oracle.ManagedDataAccess.Client;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class OracleAdapterController : Controller
    {
        public async Task<ActionResult> Schemas([RequestBody]OracleAdapter ora)
        {
            try
            {
                using (var conn = new OracleConnection(ora.ConnectionString))
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
                        return Json(new { success = true, status = "OK", schemas = list.ToArray() });
                    }

                }
            }
            catch (OracleException e)
            {
                return Json(new { success = false, status = false, message = e.Message });
            }
        }
        public async Task<ActionResult> Tables([RequestBody]OracleAdapter ora)
        {
            var sql = string.Format("select TABLE_NAME from SYS.ALL_TABLES WHERE OWNER = '{0}' order by TABLE_NAME", ora.Schema);
            const string RELSQL = @"
SELECT FK.TABLE_NAME AS CHILD_TABLE
     , SRC.TABLE_NAME AS PARENT_TABLE
     , FK.CONSTRAINT_NAME AS FK_CONSTRAINT
     , SRC.CONSTRAINT_NAME AS REFERENCED_CONSTRAINT
FROM ALL_CONSTRAINTS FK
JOIN ALL_CONSTRAINTS SRC ON FK.R_CONSTRAINT_NAME = SRC.CONSTRAINT_NAME
WHERE FK.CONSTRAINT_TYPE = 'R'
  AND SRC.OWNER = '{1}'
  AND (SRC.TABLE_NAME = '{0}' OR FK.TABLE_NAME = '{0}')";

            using (var conn = new OracleConnection(ora.ConnectionString))
            using (var tableCommand = new OracleCommand(sql, conn))
            {
                var tableNames = new List<string>();
                await conn.OpenAsync();
                using (var tableReader = (OracleDataReader)await tableCommand.ExecuteReaderAsync())
                {
                    while (await tableReader.ReadAsync())
                    {
                        tableNames.Add(tableReader.GetOracleString(0).Value);
                    }
                }
                var tables = new List<object>();
                foreach (var t in tableNames)
                {

                    using (var cmd = new OracleCommand(string.Format(RELSQL, t, ora.Schema), conn))
                    {
                        using (var reader = (OracleDataReader)await cmd.ExecuteReaderAsync())
                        {
                            var list = new List<dynamic>();
                            while (await reader.ReadAsync())
                            {
                                dynamic r = new
                                {
                                    src_table_name = reader.GetOracleString(0).Value,
                                    fk_table_name = reader.GetOracleString(1).Value,
                                    parent = InflectorExtensions.Singularize(reader.GetOracleString(1).Value),
                                    src_constraint_name = reader.GetOracleString(2).Value,
                                    fk_constraint_name = reader.GetOracleString(3).Value,
                                };
                                list.Add(r);
                            }
                            tables.Add(new
                            {
                                name = t,
                                parentOptions = list.Where(a => a.src_table_name.ToLowerInvariant() == t.ToLowerInvariant()).ToArray(),
                                childrenOptions = list.Where(a => a.src_table_name.ToLowerInvariant() != t.ToLowerInvariant()).ToArray()
                            });
                        }
                    }
                }

                return Json(new { success = true, status = "OK", tables = tables.ToArray() });

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
        public async Task<ActionResult> Generate([RequestBody]OracleAdapter ora)
        {
            var valid = await ora.ValidateAsync();
            if (!valid.Result)
                return Json(valid);

            await ora.OpenAsync();
            var result = await ora.CompileAsync(new CompilerOptions());
            this.Response.ContentType = "application/json";
            return Json(new
            {
                success = true,
                status = "OK",
                result,
                message = "Your entity has been successfully published",
                id = ora.Name
            });
        }


    }
}