using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
using LinqToQuerystring;

namespace Bespoke.Sph.Web.Api
{
    [NoCache]
    [RoutePrefix("aggregate")]
    public class AggregateController : Controller
    {
        public static readonly string ConnectionString =
            ConfigurationManager.SqlConnectionString;

        [Route("scalar")]
        public async Task<ActionResult> Scalar(string column, string table, string filter)
        {
            // TODO : should get all the one with SaveAsSourceAttribute instead
            var type = table.ToLowerInvariant();
            switch (type)
            {
                case "workflowdefinition": return GetSystemObjectScalar<WorkflowDefinition>(column, filter);
                case "designation": return GetSystemObjectScalar<Designation>(column, filter);
                case "entitydefinition": return GetSystemObjectScalar<EntityDefinition>(column, filter);
                case "valueobjectdefinition": return GetSystemObjectScalar<ValueObjectDefinition>(column, filter);
                case "transformdefinition": return GetSystemObjectScalar<TransformDefinition>(column, filter);
                case "entityview": return GetSystemObjectScalar<EntityView>(column, filter);
                case "entityform": return GetSystemObjectScalar<EntityForm>(column, filter);
                case "formdialog": return GetSystemObjectScalar<FormDialog>(column, filter);
                case "entitychart": return GetSystemObjectScalar<EntityChart>(column, filter);
                case "trigger": return GetSystemObjectScalar<Trigger>(column, filter);
                case "adapter": return GetSystemObjectScalar<Adapter>(column, filter);
            }
            var translator = new OdataSqlTranslator(column, table);
            var sql = translator.Scalar(filter);
            return await ExecuteScalarAsync(sql);
        }

        private ActionResult GetSystemObjectScalar<T>(string column, string filter) where T : Entity
        {
            var context = new SphDataContext();
            var val = context.LoadFromSources<T>()
                .AsQueryable()
                .LinqToQuerystring($"?$filter={filter}")
                .AsQueryable()
                .LinqToQuerystring<T, IQueryable<Dictionary<string, object>>>($"?$select={column}")
                .FirstOrDefault();
            if (null == val)
                return Json("", JsonRequestBehavior.AllowGet);
            if (!val.ContainsKey(column))
                return Json("", JsonRequestBehavior.AllowGet);

            return Json(val[column].ToString(), JsonRequestBehavior.AllowGet);
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
            var sql = translator.Min(filter);
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
