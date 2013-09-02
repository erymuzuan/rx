using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;

namespace Bespoke.Sph.Commerspace.Web.Api
{
    public class ListController : Controller
    {
        public static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;

        public async Task<ActionResult> Index(string column, string table, string filter)
        {
            var translator = new OdataSqlTranslator(column, table);
            var sql = translator.Scalar(filter);
            return await ExecuteListAsync(sql);
        }

        public async Task<ActionResult> Tuple(string column, string column2, string table, string filter)
        {
            var translator = new OdataSqlTranslator("", table);
            var sql = translator.Scalar(filter).Replace("SELECT []", string.Format("SELECT [{0}],[{1}]", column, column2));
            return await ExecuteListTupleAsync(sql);
        }

        private async Task<ActionResult> ExecuteListTupleAsync(string sql)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();

                var result = new List<Tuple<object, object>>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var item = new Tuple<object, object>(reader[0], reader[1]);
                        result.Add(item);
                    }
                }

                return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<ActionResult> ExecuteListAsync(string sql)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();

                var result = new List<object>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result.Add(reader[0]);
                    }
                }

                return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}