using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class ListActionCode : ControllerAction
    {
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            var code = new StringBuilder();
            code.AppendLine("       [Route]");
            code.AppendLinf(
                "       public async Task<System.Web.Mvc.ActionResult> Index(string filter = null, int page = 1, int size = 40, bool includeTotal = false)");
            code.AppendLine("       {");
            code.AppendFormat(@"
           if (size > 200)
                throw new ArgumentException(""Your are not allowed to do more than 200"", ""size"");

            var orderby = this.Request.QueryString[""$orderby""];
            var translator = new {0}<{1}>(null,""{1}"" ){{Schema = ""{3}""}};
            var sql = translator.Select(string.IsNullOrWhiteSpace(filter) ? ""{2} gt 0"" : filter, orderby);
            var count = 0;

            var context = new {1}Adapter();
            var nextPageToken = string.Empty;
            var lo = await context.LoadAsync(sql, page, size);
            if (includeTotal || page > 1)
            {{
                var translator2 = new {0}<{1}>(null, ""{1}""){{Schema = ""{3}""}};
                var countSql = translator2.Count(filter);
                count = await context.ExecuteScalarAsync<int>(countSql);

                if (count >= lo.ItemCollection.Count())
                    nextPageToken = string.Format(
                        ""/api/{5}/{4}/?filer={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page + 1, size);
            }}

            string previousPageToken = string.Format(""/api/{5}/{4}/?filer={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page - 1, size);
            if(page == 1)
                previousPageToken = null;
            var json = new
            {{
                count,
                page,
                nextPageToken,
                previousPageToken,
                size,
                results = lo.ItemCollection.ToArray()
            }};
            var setting = new JsonSerializerSettings();
            setting.Converters.Add(new StringEnumConverter());

            this.Response.ContentType = ""application/json"";
            return Content(JsonConvert.SerializeObject(json, Formatting.Indented, setting));
            ", adapter.OdataTranslator, table.Name, table.PrimaryKey, table.Schema, table.Name.ToLowerInvariant(), table.Schema.ToLowerInvariant());


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }
    }
}