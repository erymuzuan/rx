﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.CommercialSpace.Domain;
using Bespoke.Station.Web.Helpers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Api
{
    public class JsonDataServiceController : Controller
    {
        private static readonly string m_connectionString =
            ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        //public async Task<ActionResult> DailySummary(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        //{
        //    return await ExecuteAsync<DailySummary>(filter, page, size, includeTotal);
        //}


        public async Task<ActionResult> ExecuteAsync<T>(string filter = null, int page = 1, int size = 40, bool includeTotal = false) where T : Entity
        {
            if (size > 200)
                throw new ArgumentException("Your are not allowed to do more than 200", "size");

            var typeName = typeof(T).Name;

            var translator = new OdataSqlTranslator(null, typeName);
            var sql = translator.Select(filter);
            var rows = 0;
            var nextPageToken = "";
            var list = await this.ExecuteListTupleAsync<T>(sql, page, size);

            if (includeTotal || page > 1)
            {
                var translator2 = new OdataSqlTranslator(typeName + "Id", typeName);
                var sumSql = translator2.Count(filter);
                rows = await ExecuteScalarAsync(sumSql);

                if (rows >= list.Count)
                    nextPageToken = string.Format(
                        "/JsonDataService/{3}/?filer={0}&includeTotal=true&page={1}&size={2}", filter, page + 1, size, typeName);
            }

            string previousPageToken = DateTime.Now.ToShortTimeString();
            var json = new
            {
                results = list.ToArray(),
                rows,
                page,
                nextPageToken,
                previousPageToken,
                size
            };

            this.Response.ContentType = "application/json";
            return Content(JsonConvert.SerializeObject(json));
        }

        private async Task<int> ExecuteScalarAsync(string sql)
        {
            using (var conn = new SqlConnection(m_connectionString))
            using (var command = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                if (result == DBNull.Value)
                    return 0;
                return (int)result;
            }
        }

        private async Task<List<T>> ExecuteListTupleAsync<T>(string sql, int page, int size) where T : Entity
        {
            var sql2 = sql;
            if (!sql2.Contains("ORDER"))
            {
                sql2 += string.Format("\r\nORDER BY [{0}Id]", typeof(T).Name);
            }

            var paging = ObjectBuilder.GetObject<IPagingTranslator2>();
            sql2 = paging.Tranlate(sql2, page, size);
            Console.WriteLine("*************************");
            Console.WriteLine(sql);
            Console.WriteLine("*************************");
            Console.WriteLine(sql2);
            Console.WriteLine("*************************");
            using (var conn = new SqlConnection(m_connectionString))
            using (var command = new SqlCommand(sql2, conn))
            {
                await conn.OpenAsync();

                var result = new List<T>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var prop = typeof(T).GetProperty(typeof(T).Name + "Id");
                        var item = XmlSerializerService.DeserializeFromXml<T>(reader.GetString(1));
                        prop.SetValue(item, id);
                        result.Add(item);
                    }
                }

                return result;
            }
        }
    }
}
