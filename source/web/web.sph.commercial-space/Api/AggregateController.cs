using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;

namespace Bespoke.Sph.Commerspace.Web.Api
{
   
    public class AggregateController : Controller
    {
        public static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;

        public async Task<ActionResult> Scalar(string column, string table, string filter)
        {
            var translator = new OdataSqlTranslator(column, table);
            var sql = translator.Scalar(filter);
            return await ExecuteScalarAsync(sql);
        }

        public async Task<ActionResult> Sum(string column, string table, string filter)
        {
            var translator = new OdataSqlTranslator(column, table);
            var sql = translator.Sum(filter);
            return await ExecuteScalarAsync(sql);
        }

        public async Task<ActionResult> Count(string column, string table, string filter)
        {
            var translator = new OdataSqlTranslator(column, table);
            var sql = translator.Count(filter);
            return await ExecuteScalarAsync(sql);
        }
        public async Task<ActionResult> Average(string column, string table, string filter)
        {
            var translator = new OdataSqlTranslator(column, table);
            var sql = translator.Average(filter);
            return await ExecuteScalarAsync(sql);
        }
        public async Task<ActionResult> Max(string column, string table, string filter)
        {
            var translator = new OdataSqlTranslator(column, table);
            var sql = translator.Max(filter);
            return await ExecuteScalarAsync(sql);
        }

        public async Task<ActionResult> Min(string column, string table, string filter)
        {
            var translator = new OdataSqlTranslator(column, table);
            var sql = translator.Count(filter);
            return await ExecuteScalarAsync(sql);
        }

        private async Task<ActionResult> ExecuteScalarAsync(string sql)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                if (result == DBNull.Value)
                    return Json(0, JsonRequestBehavior.AllowGet);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
