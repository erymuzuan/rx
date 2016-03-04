using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.WebApi;
using LinqToQuerystring;

namespace Bespoke.Sph.Web.Api
{
    [RoutePrefix("api/list")]
    public class ListController : BaseApiController
    {
        public static readonly string ConnectionString = ConfigurationManager.SqlConnectionString;
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Index([FromUri(Name = "column")]string column,
            [FromUri(Name = "table")]string table,
            [FromUri(Name = "filter")]string filter = null)
        {
            // TODO : should get all the one with SaveAsSourceAttribute instead
            var type = table.ToLowerInvariant();
            switch (type)
            {
                case "designation": return SelectSystemObjectProperty<Designation>(column, filter);
                case "emailtemplate": return SelectSystemObjectProperty<EmailTemplate>(column, filter);
                case "documenttemplate": return SelectSystemObjectProperty<DocumentTemplate>(column, filter);
                case "workflowdefinition": return SelectSystemObjectProperty<WorkflowDefinition>(column, filter);
                case "entitydefinition": return SelectSystemObjectProperty<EntityDefinition>(column, filter);
                case "valueobjectdefinition": return SelectSystemObjectProperty<ValueObjectDefinition>(column, filter);
                case "transformdefinition": return SelectSystemObjectProperty<TransformDefinition>(column, filter);
                case "entityview": return SelectSystemObjectProperty<EntityView>(column, filter);
                case "entityform": return SelectSystemObjectProperty<EntityForm>(column, filter);
                case "formdialog": return SelectSystemObjectProperty<FormDialog>(column, filter);
                case "partialview": return SelectSystemObjectProperty<PartialView>(column, filter);
                case "entitychart": return SelectSystemObjectProperty<EntityChart>(column, filter);
                case "operationendpoint": return SelectSystemObjectProperty<OperationEndpoint>(column, filter);
                case "queryendpoint": return SelectSystemObjectProperty<QueryEndpoint>(column, filter);
                case "trigger": return SelectSystemObjectProperty<Trigger>(column, filter);
                case "adapter": return SelectSystemObjectProperty<Adapter>(column, filter);
                case "viewtemplate": return SelectSystemObjectProperty<ViewTemplate>(column, filter);
            }
            var translator = new OdataSqlTranslator(column, table);
            var sql = translator.Scalar(filter);
            return await ExecuteListAsync(sql);
        }


        [Route("distinct")]
        [HttpGet]
        public async Task<IHttpActionResult> Distinct([FromUri(Name = "column")]string column,
            [FromUri(Name = "table")]string table,
            [FromUri(Name = "filter")]string filter = null)
        {
            // TODO : should get all the one with SaveAsSourceAttribute instead
            var type = table.ToLowerInvariant();
            switch (type)
            {
                case "adapter": return SelectDistinctSystemObjectProperty<Adapter>(column, filter);
                case "designation": return SelectDistinctSystemObjectProperty<Designation>(column, filter);
                case "documenttemplate": return SelectDistinctSystemObjectProperty<DocumentTemplate>(column, filter);
                case "emailtemplate": return SelectDistinctSystemObjectProperty<EmailTemplate>(column, filter);
                case "entitychart": return SelectDistinctSystemObjectProperty<EntityChart>(column, filter);
                case "entitydefinition": return SelectDistinctSystemObjectProperty<EntityDefinition>(column, filter);
                case "entityview": return SelectDistinctSystemObjectProperty<EntityView>(column, filter);
                case "entityform": return SelectDistinctSystemObjectProperty<EntityForm>(column, filter);
                case "trigger": return SelectDistinctSystemObjectProperty<Trigger>(column, filter);
                case "transformdefinition": return SelectDistinctSystemObjectProperty<TransformDefinition>(column, filter);
                case "viewtemplate": return SelectDistinctSystemObjectProperty<ViewTemplate>(column, filter);
                case "workflowdefinition": return SelectDistinctSystemObjectProperty<WorkflowDefinition>(column, filter);
            }
            var translator = new OdataSqlTranslator(column, table);
            var sql = translator.Distinct(filter);
            return await ExecuteListAsync(sql);
        }

        [Route("tuple")]
        [HttpGet]
        public async Task<IHttpActionResult> Tuple([FromUri(Name = "table")]string table,
            [FromUri(Name = "column")]string column,
            [FromUri(Name = "column2")]string column2,
            [FromUri(Name = "filter")]string filter = null,
            [FromUri(Name = "column3")]string column3 = "",
            [FromUri(Name = "column4")]string column4 = "",
            [FromUri(Name = "column5")]string column5 = "")
        {
            var type = table.ToLowerInvariant();
            switch (type)
            {
                case "adapter": return SelectTupleFromSource<Adapter>(filter, column, column2, column3, column4, column5);
                case "designation": return SelectTupleFromSource<Designation>(filter, column, column2, column3, column4, column5);
                case "documenttemplate": return SelectTupleFromSource<DocumentTemplate>(filter, column, column2, column3, column4, column5);
                case "emailtemplate": return SelectTupleFromSource<EmailTemplate>(filter, column, column2, column3, column4, column5);
                case "entitychart": return SelectTupleFromSource<EntityChart>(filter, column, column2, column3, column4, column5);
                case "entitydefinition": return SelectTupleFromSource<EntityDefinition>(filter, column, column2, column3, column4, column5);
                case "entityform": return SelectTupleFromSource<EntityForm>(filter, column, column2, column3, column4, column5);
                case "formdialog": return SelectTupleFromSource<FormDialog>(filter, column, column2, column3, column4, column5);
                case "partialview": return SelectTupleFromSource<PartialView>(filter, column, column2, column3, column4, column5);
                case "entityview": return SelectTupleFromSource<EntityView>(filter, column, column2, column3, column4, column5);
                case "transformdefinition": return SelectTupleFromSource<TransformDefinition>(filter, column, column2, column3, column4, column5);
                case "trigger": return SelectTupleFromSource<Trigger>(filter, column, column2, column3, column4, column5);
                case "viewtemplate": return SelectTupleFromSource<ViewTemplate>(filter, column, column2, column3, column4, column5);
                case "valueobjectdefinition": return SelectTupleFromSource<ValueObjectDefinition>(filter, column, column2, column3, column4, column5);
                case "workflowdefinition": return SelectTupleFromSource<WorkflowDefinition>(filter, column, column2, column3, column4, column5);
                case "workflowform": return SelectTupleFromSource<WorkflowForm>(filter, column, column2, column3, column4, column5);
            }

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

        [Route("{table}")]
        [HttpGet]
        public async Task<IHttpActionResult> Select(string table,
            [FromUri(Name = "$select")]string columns,
            [FromUri(Name = "$filter")]string filter = null)
        {
            var cols = columns.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            string column1 = cols.First(), column2 = "", column3 = "", column4 = "", column5 = "";
            if (cols.Length > 1)
                column2 = cols[1];
            if (cols.Length > 2)
                column3 = cols[2];
            if (cols.Length > 3)
                column4 = cols[3];
            if (cols.Length > 4)
                column5 = cols[4];

            var type = table.ToLowerInvariant();
            switch (type)
            {
                case "adapter": return SelectTupleFromSource<Adapter>(filter, column1, column2, column3, column4, column5);
                case "designation": return SelectTupleFromSource<Designation>(filter, column1, column2, column3, column4, column5);
                case "documenttemplate": return SelectTupleFromSource<DocumentTemplate>(filter, column1, column2, column3, column4, column5);
                case "emailtemplate": return SelectTupleFromSource<EmailTemplate>(filter, column1, column2, column3, column4, column5);
                case "entitychart": return SelectTupleFromSource<EntityChart>(filter, column1, column2, column3, column4, column5);
                case "entitydefinition": return SelectTupleFromSource<EntityDefinition>(filter, column1, column2, column3, column4, column5);
                case "entityform": return SelectTupleFromSource<EntityForm>(filter, column1, column2, column3, column4, column5);
                case "entityview": return SelectTupleFromSource<EntityView>(filter, column1, column2, column3, column4, column5);
                case "formdialog": return SelectTupleFromSource<FormDialog>(filter, column1, column2, column3, column4, column5);
                case "partialview": return SelectTupleFromSource<PartialView>(filter, column1, column2, column3, column4, column5);
                case "transformdefinition": return SelectTupleFromSource<TransformDefinition>(filter, column1, column2, column3, column4, column5);
                case "trigger": return SelectTupleFromSource<Trigger>(filter, column1, column2, column3, column4, column5);
                case "viewtemplate": return SelectTupleFromSource<ViewTemplate>(filter, column1, column2, column3, column4, column5);
                case "workflowdefinition": return SelectTupleFromSource<WorkflowDefinition>(filter, column1, column2, column3, column4, column5);
                case "workflowform": return SelectTupleFromSource<WorkflowForm>(filter, column1, column2, column3, column4, column5);
                case "valueobjectdefinition": return SelectTupleFromSource<ValueObjectDefinition>(filter, column1, column2, column3, column4, column5);
            }

            var translator = new OdataSqlTranslator("", table);
            if (!string.IsNullOrWhiteSpace(column5))
            {
                var sql4 = translator.Scalar(filter).Replace("SELECT []", $"SELECT [{column1}],[{column2}],[{column3}],[{column4}],[{column5}]");
                return await ExecuteListTuple5Async(sql4);
            }
            if (!string.IsNullOrWhiteSpace(column4))
            {
                var sql4 = translator.Scalar(filter).Replace("SELECT []", $"SELECT [{column1}],[{column2}],[{column3}],[{column4}]");
                return await ExecuteListTuple4Async(sql4);
            }
            if (!string.IsNullOrWhiteSpace(column3))
            {
                var sql3 = translator.Scalar(filter).Replace("SELECT []", $"SELECT [{column1}],[{column2}],[{column3}]");
                return await ExecuteListTuple3Async(sql3);
            }
            var sql = translator.Scalar(filter).Replace("SELECT []", $"SELECT [{column1}],[{column2}]");
            return await ExecuteListTupleAsync(sql);
        }

        private IHttpActionResult SelectSystemObjectProperty<T>(string column, string filter) where T : Entity
        {
            var context = new SphDataContext();
            if (string.IsNullOrWhiteSpace(filter))
            {
                var val2 = context.LoadFromSources<T>()
                    .AsQueryable()
                    .LinqToQuerystring<T, IQueryable<Dictionary<string, object>>>($"?$select={column}")
                    .SelectMany(d => d.Values)
                    .Where(v => null != v)
                    .ToList();

                return Json(val2);

            }
            var val = context.LoadFromSources<T>()
                .AsQueryable()
                .LinqToQuerystring($"?$filter={filter}")
                .AsQueryable()
                .LinqToQuerystring<T, IQueryable<Dictionary<string, object>>>($"?$select={column}")
                .SelectMany(d => d.Values);

            return Json(val);
        }

        private IHttpActionResult SelectDistinctSystemObjectProperty<T>(string column, string filter) where T : Entity
        {
            var context = new SphDataContext();
            if (string.IsNullOrWhiteSpace(filter))
            {
                var val2 = context.LoadFromSources<T>()
                    .AsQueryable()
                    .LinqToQuerystring<T, IQueryable<Dictionary<string, object>>>($"?$select={column}")
                    .SelectMany(d => d.Values)
                    .Where(v => null != v)
                    .Distinct()
                    .ToList();

                return Json(val2);

            }
            var val = context.LoadFromSources<T>()
                .AsQueryable()
                .LinqToQuerystring($"?$filter={filter}")
                .AsQueryable()
                .LinqToQuerystring<T, IQueryable<Dictionary<string, object>>>($"?$select={column}")
                .SelectMany(d => d.Values)
                .Distinct();

            return Json(val);
        }
        private IHttpActionResult SelectTupleFromSource<T>(string filter, string column, string column2, string column3 = "", string column4 = "", string column5 = "") where T : Entity
        {
            if (string.IsNullOrWhiteSpace(filter)) filter = "Id ne '0'";

            var context = new SphDataContext();
            var list = context.LoadFromSources<T>()
                .AsQueryable()
                .LinqToQuerystring($"?$filter={filter}")
                .AsQueryable();

            var select = $"?$select={column},{column2}";
            if (!string.IsNullOrWhiteSpace(column3))
                select = $"?$select={column},{column2},{column3}";
            if (!string.IsNullOrWhiteSpace(column4))
                select = $"?$select={column},{column2},{column3},{column4}";
            if (!string.IsNullOrWhiteSpace(column5))
                select = $"?$select={column},{column2},{column3},{column4},{column5}";

            var tuples = list.LinqToQuerystring<T, IQueryable<Dictionary<string, object>>>(select)
                .ToList();
            //var results = new ArrayList();
            //foreach (var t in tuples)
            //{
            //    var q = new ArrayList();
            //    foreach (var k in t.Keys)
            //    {

            //    }
            //}
            return Json(tuples);
        }

        private async Task<IHttpActionResult> ExecuteListTuple4Async(string sql)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();

                var result = new List<Tuple<object, object, object, object>>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var item = new Tuple<object, object, object, object>(reader[0], reader[1], reader[2], reader[3]);
                        result.Add(item);
                    }
                }

                return Json(result.ToArray());
            }
        }

        private async Task<IHttpActionResult> ExecuteListTuple5Async(string sql)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();

                var result = new List<Tuple<object, object, object, object, object>>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var item = new Tuple<object, object, object, object, object>(reader[0], reader[1], reader[2], reader[3], reader[4]);
                        result.Add(item);
                    }
                }

                return Json(result.ToArray());
            }
        }


        private async Task<IHttpActionResult> ExecuteListTupleAsync(string sql)
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

                return Json(result.ToArray());
            }
        }
        private async Task<IHttpActionResult> ExecuteListTuple3Async(string sql)
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

                return Json(result.ToArray());
            }
        }

        private async Task<IHttpActionResult> ExecuteListAsync(string sql)
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

                return Json(result.ToArray());
            }
        }
    }
}