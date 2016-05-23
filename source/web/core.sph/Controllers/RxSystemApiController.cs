using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.Properties;
using Bespoke.Sph.WebApi;
using LinqToQuerystring;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("api/systems")]
    public class RxSystemApiController : BaseApiController
    {
        public static readonly string ConnectionString = ConfigurationManager.SqlConnectionString;
        [HttpGet]
        [Route("{type}/0")]
        public IHttpActionResult NotFound2(string type)
        {
            return NotFound($"Cannot find any item of type {type} with id 0");
        }
        [HttpGet]
        [Route("audittrail")]
        public async Task<IHttpActionResult> AuditTrail(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return await ExecuteAsync<AuditTrail>(filter, page, size, includeTotal, orderby);
        }

        [Route("adapter")]
        [HttpGet]
        public IHttpActionResult Adapter(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<Adapter>(filter, page, size, orderby);
        }

        [Route("DataTransferDefinition")]
        [HttpGet]
        public IHttpActionResult DataTransferDefinition(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<DataTransferDefinition>(filter, page, size, orderby: orderby);
        }
        [Route("Designation")]
        [HttpGet]
        public IHttpActionResult Designation(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<Designation>(filter, page, size, orderby: orderby);
        }

        [Route("DocumentTemplate")]
        [HttpGet]
        public IHttpActionResult DocumentTemplate(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<DocumentTemplate>(filter, page, size, orderby: orderby);
        }

        [Route("EmailTemplate")]
        [HttpGet]
        public IHttpActionResult EmailTemplate(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<EmailTemplate>(filter, page, size, orderby: orderby);
        }

        [Route("ValueObjectDefinition")]
        [HttpGet]
        public IHttpActionResult ValueObjectDefinition(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<ValueObjectDefinition>(filter, page, size, orderby: orderby);
        }

        [Route("EntityDefinition")]
        [HttpGet]
        public IHttpActionResult EntityDefinition(string filter = null, int page = 1, int size = 40,
            bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<EntityDefinition>(filter, page, size, orderby: orderby);
        }

        [Route("EntityChart")]
        [HttpGet]
        public IHttpActionResult EntityChart(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<EntityChart>(filter, orderby: orderby);
        }

        [Route("FormDialog")]
        [HttpGet]
        public IHttpActionResult FormDialog(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<FormDialog>(filter, page, size, orderby);
        }
        [Route("WorkflowForm")]
        [HttpGet]
        public IHttpActionResult WorkflowForm(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<WorkflowForm>(filter, page, size, orderby);
        }
        [Route("PartialView")]
        [HttpGet]
        public IHttpActionResult PartialView(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<PartialView>(filter, page, size, orderby);
        }

        [Route("EntityForm")]
        [HttpGet]
        public IHttpActionResult EntityForm(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<EntityForm>(filter, page, size, orderby);
        }

        [Route("EntityView")]
        [HttpGet]
        public IHttpActionResult EntityView(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<EntityView>(filter, page, size, orderby:orderby);
        }

        [Route("OperationEndpoint/{id}")]
        [HttpGet]
        public IHttpActionResult GetOneOperationEndpoint(string id)
        {
            return ReadFromSource<OperationEndpoint>($"Id eq '{id}'", 1, 1);
        }
        [Route("OperationEndpoint")]
        [HttpGet]
        public IHttpActionResult GetOperationEndpoint(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<OperationEndpoint>(filter, page, size, orderby: orderby);
        }

        [Route("QueryEndpoint/{id}")]
        [HttpGet]
        public IHttpActionResult GetOneQueryEndpoint(string id)
        {
            return ReadFromSource<QueryEndpoint>($"Id eq '{id}'", 1, 1);
        }
        [Route("QueryEndpoint")]
        [HttpGet]
        public IHttpActionResult QueryEndpoint(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<QueryEndpoint>(filter, page, size, orderby: orderby);
        }

        [Route("QuotaPolicy")]
        [HttpGet]
        public IHttpActionResult QuotaPolicy(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<QuotaPolicy>(filter, page, size, orderby: orderby);
        }

        [Route("Message")]
        [HttpGet]
        public async Task<IHttpActionResult> Message(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return await ExecuteAsync<Message>(filter, page, size, includeTotal);
        }

        [Route("ReportDelivery")]
        [HttpGet]
        public async Task<IHttpActionResult> ReportDelivery(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return await ExecuteAsync<ReportDelivery>(filter, page, size, includeTotal);
        }

        [Route("ReportDefinition")]
        [HttpGet]
        public IHttpActionResult ReportDefinition(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<ReportDefinition>(filter, page, size, orderby);
        }



        [Route("Setting")]
        [HttpGet]
        public async Task<IHttpActionResult> Setting(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return await ExecuteAsync<Setting>(filter, page, size, includeTotal);
        }


        [Route("Tracker")]
        [HttpGet]
        public async Task<IHttpActionResult> Tracker(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
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
            return await this.ExecuteAsync(filter, page, size, includeTotal, null, process);
        }


        [Route("Trigger")]
        [HttpGet]
        public IHttpActionResult Trigger(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<Trigger>(filter, page, size, orderby);
        }

        [Route("UserProfile")]
        [HttpGet]
        public async Task<IHttpActionResult> UserProfile(string filter = null, int page = 1, int size = 40,
            bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return await ExecuteAsync<UserProfile>(filter, page, size, includeTotal);
        }

        [Route("Watcher")]
        [HttpGet]
        public async Task<IHttpActionResult> Watcher(string filter = null, int page = 1, int size = 40,
            bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return await ExecuteAsync<Watcher>(filter, page, size, includeTotal);
        }

        [Route("TransformDefinition")]
        [HttpGet]
        public IHttpActionResult TransformDefinition(string filter = null, int page = 1, int size = 20, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<TransformDefinition>(filter, page, size, orderby);
        }


        [Route("WorkflowDefinition")]
        [HttpGet]
        public IHttpActionResult WorkflowDefinition(string filter = null, int page = 1, int size = 40,
            bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<WorkflowDefinition>(filter, page, size, orderby);
        }

        [Route("ViewTemplate")]
        [HttpGet]
        public IHttpActionResult ViewTemplate(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return ReadFromSource<ViewTemplate>(filter, page, size, orderby);
        }


        [Route("Workflow")]
        [HttpGet]
        public async Task<IHttpActionResult> Workflow(string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$orderby")]string orderby = null)
        {
            return await ExecuteAsync<Workflow>(filter, page, size, includeTotal);
        }



        private async Task<IHttpActionResult> ExecuteAsync<T>(
            [FromUri(Name = "filter")]string filter = null, int page = 1, int size = 40, bool includeTotal = false,
            [FromUri(Name = "$order")]string orderby = null,
            Action<IEnumerable<T>> processAction = null) where T : Entity
        {
            if (size > 200)
                throw new ArgumentException(Resources.ApiControllerYouAreNotAllowedToDoMoreThan200, nameof(size));

            var typeName = typeof(T).Name;

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
            var result = new
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

            return Json(JsonConvert.SerializeObject(result, setting));
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
            var sql2 = sql;
            if (!sql2.Contains("ORDER"))
            {
                sql2 += "\r\nORDER BY [Id]";
            }

            var paging = ObjectBuilder.GetObject<IOdataPagingProvider>();
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

        private IHttpActionResult ReadFromSource<T>(string filter, int page = 1, int size = 20, string orderby = null) where T : Entity
        {
            var list = new List<T>();
            var id = Strings.RegexSingleValue(filter, "^Id eq '(?<id>[0-9A-Za-z-_ ]{1,50})'", "id");
            string folder = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\";

            var rows = 0;
            if (Directory.Exists(folder))
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    var sources = IterateFromSource<T>(filter, page, size, folder, out rows, orderby).ToList();
                    list.AddRange(sources);
                }
                else
                {
                    var item = ReadOneFromSource<T>(id);
                    if (null != item) list.Add(item);
                }
            }

            var jsonList = list.Select(x => x.ToJsonString(true)).ToArray();
            var json = $@"
            {{
                ""results"" : [{  string.Join(",", jsonList)}],
                ""rows"" : {rows},
                ""page"" : 1,
                ""nextPageToken"" : """",
                ""previousPageToken"" : """",
                ""size"" : 20
            }}";

            return Json(json);
        }

        private static IList<T> IterateFromSource<T>(string filter, int page, int size, string folder, out int rows, string orderby = null)
            where T : Entity
        {
            List<T> list;
            var files = Directory.GetFiles(folder, "*.json");
            var sort = string.IsNullOrWhiteSpace(orderby) ? "" : "$orderby=" + orderby;
            if (string.IsNullOrWhiteSpace(filter) || filter == "Id ne '0'")
            {
                if (!string.IsNullOrWhiteSpace(sort))
                {
                    list = files
                        .Select(f => f.DeserializeFromJsonFile<T>())
                        .AsQueryable()
                        .LinqToQuerystring(sort)
                        .Skip((page - 1) * size)
                        .Take(size)
                        .ToList();
                }
                else
                {
                    // NOTE : if no sorting and no filter, DO NOT read every files
                    list = files
                        .Skip((page - 1) * size)
                        .Take(size)
                        .ToArray()
                        .Select(f => f.DeserializeFromJsonFile<T>())
                        .ToList();
                }
                rows = files.Length;
                return list;
            }

            // TODO : too many replace, creates unecessary string copies
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
                .Replace("[DataSource.EntityName]", "DataSource/EntityName")
                ;

            var filtered = files.Select(f => f.DeserializeFromJsonFile<T>())
                .AsQueryable()
                .LinqToQuerystring("?$filter=" + filter + sort)
                .ToList();
            list = filtered.Skip((page - 1) * size)
                .Take(size)
                .ToList();
            rows = filtered.Count;
            return list;
        }

        private T ReadOneFromSource<T>(string id) where T : Entity
        {
            string folder = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\";
            var file = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\{id}.json";
            if (System.IO.File.Exists(file))
            {
                return file.DeserializeFromJsonFile<T>();
            }

            var files = Directory.GetFiles(folder, "*.json");
            var item = files
                .Select(f => f.DeserializeFromJsonFile<T>())
                .FirstOrDefault(x => x.Id == id);
            return item;
        }
    }
}
