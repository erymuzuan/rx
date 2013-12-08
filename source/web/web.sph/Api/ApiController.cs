﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Api
{
    public class ApiController : Controller
    {
        public static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;

        public async Task<ActionResult> ContractTemplate(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<ContractTemplate>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> ApplicationTemplate(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<ApplicationTemplate>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> Contract(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Contract>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> Designation(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Designation>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> Complaint(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Complaint>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> AuditTrail(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<AuditTrail>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> RentalApplication(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<RentalApplication>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> Page(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Page>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> ReportDelivery(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<ReportDelivery>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> ReportDefinition(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<ReportDefinition>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> Land(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Land>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> Building(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Building>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> BuildingTemplate(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<BuildingTemplate>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> Space(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Space>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> SpaceTemplate(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<SpaceTemplate>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> ComplaintTemplate(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<ComplaintTemplate>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> Deposit(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Deposit>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> Invoice(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Invoice>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> Inventory(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Inventory>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> Maintenance(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Maintenance>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> Message(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Message>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> MaintenanceTemplate(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<MaintenanceTemplate>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> Rent(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Rent>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> Rebate(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Rebate>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> Setting(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Setting>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> Payment(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Payment>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> UserProfile(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<UserProfile>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> Tenant(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Tenant>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> Trigger(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Trigger>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> Watcher(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Watcher>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> WorkOrder(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<WorkOrder>(filter, page, size, includeTotal);
        }
        public async Task<ActionResult> WorkflowDefinition(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<WorkflowDefinition>(filter, page, size, includeTotal);
        } 
        public async Task<ActionResult> Workflow(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Workflow>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> Tracker(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Tracker>(filter, page, size, includeTotal);
        }

        public async Task<ActionResult> ExecuteAsync<T>(string filter = null, int page = 1, int size = 40, bool includeTotal = false) where T : Entity
        {
            if (size > 200)
                throw new ArgumentException("Your are not allowed to do more than 200", "size");

            var typeName = typeof(T).Name;

            var orderby = this.Request.QueryString["$orderby"];
            var translator = new OdataSqlTranslator<T>(null, typeName);
            var sql = translator.Select(filter, orderby);
            var rows = 0;
            var nextPageToken = "";
            var list = await this.ExecuteListTupleAsync<T>(sql, page, size);

            if (includeTotal || page > 1)
            {
                var translator2 = new OdataSqlTranslator<T>(typeName + "Id", typeName);
                var sumSql = translator2.Count(filter);
                rows = await ExecuteScalarAsync(sumSql);

                if (rows >= list.Count)
                    nextPageToken = string.Format(
                        "/Api/{3}/?filer={0}&includeTotal=true&page={1}&size={2}", filter, page + 1, size, typeName);
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
            var setting = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                };

            this.Response.ContentType = "application/json";
            return Content(JsonConvert.SerializeObject(json, Formatting.None, setting));
        }

        private async Task<int> ExecuteScalarAsync(string sql)
        {
            using (var conn = new SqlConnection(ConnectionString))
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
            var type = typeof(IRepository<T>);
            dynamic repos = ObjectBuilder.GetObject(type);

            var sql2 = sql;
            if (!sql2.Contains("ORDER"))
            {
                sql2 += string.Format("\r\nORDER BY [{0}Id]", typeof(T).Name);
            }

            var paging = ObjectBuilder.GetObject<IPagingTranslator2>();
            sql2 = paging.Tranlate(sql2, page, size);
            using (var conn = new SqlConnection(ConnectionString))
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
                        if (repos.IsJson)
                        {
                            dynamic t1 = reader.GetString(1).DeserializeFromJson<T>();
                            prop.SetValue(t1, id, null);
                            result.Add(t1);
                            continue;
                        }
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
