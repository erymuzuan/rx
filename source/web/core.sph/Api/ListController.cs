using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Api
{
    [NoCache]
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

        public async Task<ActionResult> Distinct(string column, string table, string filter)
        {
            var translator = new OdataSqlTranslator(column, table);
            var sql = translator.Distinct(filter);
            return await ExecuteListAsync(sql);
        }

        public async Task<ActionResult> Tuple(string table, string filter, string column, string column2, string column3 = "", string column5 = "", string column4 = "")
        {
            var translator = new OdataSqlTranslator("", table);
            if (!string.IsNullOrWhiteSpace(column5))
            {
                var sql4 = translator.Scalar(filter).Replace("SELECT []", $"SELECT [{column}],[{column2}],[{column3}],[{column4}],[{column5}]");
                return await ExecuteListTuple5Async(sql4);
            }
            if (!string.IsNullOrWhiteSpace(column4))
            {
                var sql4 = translator.Scalar(filter).Replace("SELECT []", $"SELECT [{column}],[{column2}],[{column3}],[{column4}]");
                return await ExecuteListTuple4Async(sql4);
            }
            if (!string.IsNullOrWhiteSpace(column3))
            {
                var sql3 = translator.Scalar(filter).Replace("SELECT []", $"SELECT [{column}],[{column2}],[{column3}]");
                return await ExecuteListTuple3Async(sql3);
            }
            var sql = translator.Scalar(filter).Replace("SELECT []", $"SELECT [{column}],[{column2}]");
            return await ExecuteListTupleAsync(sql);
        }

        private async Task<ActionResult> ExecuteListTuple4Async(string sql)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();

                var result = new List<Tuple<object, object, object,object>>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var item = new Tuple<object, object,object,object>(reader[0], reader[1], reader[2], reader[3]);
                        result.Add(item);
                    }
                }

                return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<ActionResult> ExecuteListTuple5Async(string sql)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();

                var result = new List<Tuple<object, object, object,object,object>>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var item = new Tuple<object, object,object,object,object>(reader[0], reader[1], reader[2], reader[3], reader[4]);
                        result.Add(item);
                    }
                }

                return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            }
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
        private async Task<ActionResult> ExecuteListTuple3Async(string sql)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();

                var result = new List<Tuple<object, object, object>>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var item = new Tuple<object, object, object>(reader[0], reader[1], reader[2]);
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