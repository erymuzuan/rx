using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.Properties;
using LinqToQuerystring;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Api
{
    [NoCache]
    [SessionState(SessionStateBehavior.Disabled)]
    [RoutePrefix("api")]
    public class ApiController : Controller
    {
        public static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;

        [Route("audittrail")]
        public async Task<ActionResult> AuditTrail(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<AuditTrail>(filter, page, size, includeTotal);
        }
        [Route("adapter")]
        public async Task<ActionResult> Adapter(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<Adapter>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<Adapter>(filter, page, size, includeTotal);
        }

        [Route("Designation")]
        public async Task<ActionResult> Designation(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<Designation>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<Designation>(filter, page, size, includeTotal);
        }

        [Route("DocumentTemplate")]
        public async Task<ActionResult> DocumentTemplate(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<DocumentTemplate>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<DocumentTemplate>(filter, page, size, includeTotal);
        }
        [Route("EmailTemplate")]
        public async Task<ActionResult> EmailTemplate(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<EmailTemplate>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<EmailTemplate>(filter, page, size, includeTotal);
        }
        [Route("EntityDefinition")]
        public async Task<ActionResult> EntityDefinition(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<EntityDefinition>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<EntityDefinition>(filter, page, size, includeTotal);
        }

        [Route("EntityChart")]
        public async Task<ActionResult> EntityChart(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<EntityChart>(filter);
            if (null != source) return source;
            return await ExecuteAsync<EntityChart>(filter, page, size, includeTotal);
        }
        [Route("EntityForm")]
        public async Task<ActionResult> EntityForm(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<EntityForm>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<EntityForm>(filter, page, size, includeTotal);
        }

        [Route("EntityView")]
        public async Task<ActionResult> EntityView(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<EntityView>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<EntityView>(filter, page, size, includeTotal);
        }

        [Route("Message")]
        public async Task<ActionResult> Message(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Message>(filter, page, size, includeTotal);
        }

        [Route("Page")]
        public async Task<ActionResult> Page(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Page>(filter, page, size, includeTotal);
        }

        [Route("ReportDelivery")]
        public async Task<ActionResult> ReportDelivery(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<ReportDelivery>(filter, page, size, includeTotal);
        }

        [Route("ReportDefinition")]
        public async Task<ActionResult> ReportDefinition(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<ReportDefinition>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<ReportDefinition>(filter, page, size, includeTotal);
        }


        [Route("SearchDefinition")]
        public async Task<ActionResult> SearchDefinition(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<SearchDefinition>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<SearchDefinition>(filter, page, size, includeTotal);
        }

        [Route("Setting")]
        public async Task<ActionResult> Setting(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<Setting>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<Setting>(filter, page, size, includeTotal);
        }


        [Route("Tracker")]
        public async Task<ActionResult> Tracker(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            Action<IEnumerable<Tracker>> process = list =>
            {
                foreach (var tracker in list)
                {
                    var sl = tracker.ExecutedActivityCollection.ToList();
                    sl.Sort(new ExecutedAcitivityComparer());
                    tracker.ExecutedActivityCollection.ClearAndAddRange(sl);
                }
            };
            return await ExecuteAsync(filter, page, size, includeTotal, process);
        }


        [Route("Trigger")]
        public async Task<ActionResult> Trigger(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<Trigger>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<Trigger>(filter, page, size, includeTotal);
        }

        [Route("UserProfile")]
        public async Task<ActionResult> UserProfile(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<UserProfile>(filter, page, size, includeTotal);
        }

        [Route("Watcher")]
        public async Task<ActionResult> Watcher(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Watcher>(filter, page, size, includeTotal);
        }

        [Route("TransformDefinition")]
        public async Task<ActionResult> TransformDefinition(string filter = null, int page = 1, int size = 20, bool includeTotal = false)
        {
            var source = ReadFromSource<TransformDefinition>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<TransformDefinition>(filter, page, size, includeTotal);
        }


        [Route("WorkflowDefinition")]
        public async Task<ActionResult> WorkflowDefinition(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            var source = ReadFromSource<WorkflowDefinition>(filter, page, size);
            if (null != source) return source;
            return await ExecuteAsync<WorkflowDefinition>(filter, page, size, includeTotal);
        }


        [Route("Workflow")]
        public async Task<ActionResult> Workflow(string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            return await ExecuteAsync<Workflow>(filter, page, size, includeTotal);
        }

        [Route("index/{typeName}")]
        public async Task<ActionResult> Index(string typeName, string filter = null, int page = 1, int size = 40, bool includeTotal = false)
        {
            if (size > 200)
                throw new ArgumentException(Resources.ApiControllerYouAreNotAllowedToDoMoreThan200, nameof(size));


            var orderby = this.Request.QueryString["$orderby"];
            var translator = new CustomEntityRestSqlTranslator(null, typeName);
            var sql = translator.Select(filter, orderby);
            var rows = 0;
            var nextPageToken = "";
            var list = await this.ExecuteCustomEntityListAsync(typeName, sql, page, size);

            if (includeTotal || page > 1)
            {
                var translator2 = new CustomEntityRestSqlTranslator("Id", typeName);
                var sumSql = translator2.Count(filter);
                rows = await ExecuteScalarAsync(sumSql);

                if (rows >= list.Count)
                    nextPageToken = string.Format("/Api/{3}/?filer={0}&includeTotal=true&page={1}&size={2}", filter, page + 1, size, typeName);
                //

            }

            var previousPageToken = DateTime.Now.ToShortTimeString();
            var json = new StringBuilder("{");
            json.AppendLinf("   \"results\":[{0}],", string.Join(",\r\n", list));
            json.AppendLinf("   \"rows\":{0},", rows);
            json.AppendLinf("   \"page\":{0},", page);
            json.AppendLinf("   \"nextPageToken\":\"{0}\",", nextPageToken);
            json.AppendLinf("   \"previousPageToken\":\"{0}\",", previousPageToken);
            json.AppendLinf("   \"size\":{0}", size);

            json.AppendLine("}");

            this.Response.ContentType = "application/json";
            return Content(json.ToString());
        }

        private async Task<ActionResult> ExecuteAsync<T>(string filter = null, int page = 1, int size = 40, bool includeTotal = false, Action<IEnumerable<T>> processAction = null) where T : Entity
        {
            if (size > 200)
                throw new ArgumentException(Resources.ApiControllerYouAreNotAllowedToDoMoreThan200, nameof(size));

            var typeName = typeof(T).Name;

            var orderby = this.Request.QueryString["$orderby"];
            var translator = new OdataSqlTranslator(null, typeName);
            var sql = translator.Select(string.IsNullOrWhiteSpace(filter) ? "Id ne ''" : filter, orderby);
            var rows = 0;
            var nextPageToken = "";
            var list = await this.ExecuteListTupleAsync<T>(sql, page, size);
            processAction?.Invoke(list);

            if (includeTotal || page > 1)
            {
                var translator2 = new OdataSqlTranslator("Id", typeName);
                var sumSql = translator2.Count(filter);
                rows = await ExecuteScalarAsync(sumSql);

                if (rows >= list.Count)
                    nextPageToken = string.Format(
                        "/api/{3}/?filer={0}&includeTotal=true&page={1}&size={2}", filter, page + 1, size, typeName);
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

        private async Task<List<string>> ExecuteCustomEntityListAsync(string type, string sql, int page, int size)
        {
            var sql2 = sql;
            if (!sql2.Contains("ORDER"))
            {
                sql2 += "\r\nORDER BY [Id]";
            }

            var paging = ObjectBuilder.GetObject<IPagingTranslator2>();
            sql2 = paging.Tranlate(sql2, page, size);
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql2, conn))
            {
                await conn.OpenAsync();

                var result = new List<string>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetString(0);
                        var json = reader.GetString(1)
                            .Replace("Id\":0", type + "Id\":\"" + id + "\"");
                        result.Add(json);
                    }
                }

                return result;
            }
        }

        private async Task<List<T>> ExecuteListTupleAsync<T>(string sql, int page, int size) where T : Entity
        {
            var sql2 = sql;
            if (!sql2.Contains("ORDER"))
            {
                sql2 += "\r\nORDER BY [Id]";
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
                        var id = reader.GetString(0);
                        dynamic t1 = reader.GetString(1).DeserializeFromJson<T>();
                        t1.Id = id;
                        result.Add(t1);

                    }
                }

                return result;
            }
        }

        private ActionResult ReadFromSource<T>(string filter, int page = 1, int size = 20) where T : Entity
        {
            var list = new List<T>();
            var rows = 0;
            var id = Strings.RegexSingleValue(filter, "^Id eq '(?<id>[0-9A-Za-z-_ ]{1,50})'", "id");
            string folder = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\";
            if (string.IsNullOrWhiteSpace(id))
            {
                if (!Directory.Exists(folder)) return null;
                var files = System.IO.Directory.GetFiles(folder, "*.json");
                if (string.IsNullOrWhiteSpace(filter) || filter == "Id ne '0'")
                {

                    list = files
                       .Skip((page - 1) * size)
                       .Take(size)
                       .Select(f => System.IO.File.ReadAllText(f).DeserializeFromJson<T>())
                       .ToList();
                    rows = files.Length;
                }
                else
                {
                    filter = filter
                        .Replace("IsPublished eq 1", "IsPublished eq true")
                        .Replace("IsPublished eq 0", "IsPublished eq false")
                        .Replace("IsAllowedNewItem eq 1", "IsAllowedNewItem eq true")
                        .Replace("IsAllowedNewItem eq 0", "IsAllowedNewItem eq false")
                        .Replace("IsDashboardItem eq 1", "IsDashboardItem eq true")
                        .Replace("IsDashboardItem eq 0", "IsDashboardItem eq false")
                        .Replace("IsDefault eq 1", "IsDefault eq true")
                        .Replace("IsDefault eq 0", "IsDefault eq false")
                        .Replace("IsActive eq 1", "IsActive eq true")
                        .Replace("IsActive eq 0", "IsActive eq false")
                        .Replace("IsPrivate eq 1", "IsPrivate eq true")
                        .Replace("IsPrivate eq 0", "IsPrivate eq false")
                        .Replace(" OR ", " or ")
                        ;
                    var filtered = files.Select(f => System.IO.File.ReadAllText(f).DeserializeFromJson<T>())
                        .AsQueryable()
                        .LinqToQuerystring("?$filter=" + filter)
                        .ToList();
                    list = filtered.Skip((page - 1) * size)
                        .Take(size)
                        .ToList();
                    rows = filtered.Count;

                }

            }
            else
            {
                var file = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\{id}.json";
                if (System.IO.File.Exists(file))
                {
                    using (var stream = System.IO.File.Open(file, FileMode.Open, FileAccess.Read))
                    {
                        var item = stream.DeserializeJson<T>();
                        list.Add(item);
                    }
                    rows = 1;
                }
                else
                {
                    var files = System.IO.Directory.GetFiles(folder, "*.json");
                    var item = files
                        .Select(f => System.IO.File.ReadAllText(f).DeserializeFromJson<T>())
                        .FirstOrDefault(x => x.Id == id);
                    if (null != item)
                        list.Add(item);
                }
            }
            var json = new
            {
                results = list,
                rows,
                page = 1,
                nextPageToken = "",
                previousPageToken = "",
                size = 20
            };
            var setting = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };

            this.Response.ContentType = "application/json";
            return Content(JsonConvert.SerializeObject(json, Formatting.None, setting), "application/json", Encoding.UTF8);
        }
    }
}
