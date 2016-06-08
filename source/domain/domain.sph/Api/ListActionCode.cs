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
            code.AppendLine("       [Route(\"\")]");
            code.AppendLine("       [HttpGet]");
            code.AppendLinf(
                "       public async Task<object> List(string filter = null, int page = 1, int size = 40, bool includeTotal = false, string orderby = null)");
            code.AppendLine("       {");
            var pk = table.PrimaryKey == null ? "" : table.PrimaryKey.Name;
            code.Append($@"
           if (size > 200)
                throw new ArgumentException(""Your are not allowed to do more than 200"", ""size"");

            var translator = new {adapter.OdataTranslator}<{table.Name}>(null,""{table.Name}"" ){{Schema = ""{table.Schema}""}};
            var sql = translator.Select(string.IsNullOrWhiteSpace(filter) ? ""{pk} gt 0"" : filter, orderby);
            var count = 0;

            var context = new {table.Name}Adapter();
            var nextPageToken = string.Empty;
            var lo = await context.LoadAsync(sql, page, size);
            if (includeTotal || page > 1)
            {{
                var translator2 = new {adapter.OdataTranslator}<{table.Name}>(null, ""{table.Name}""){{Schema = ""{table.Schema}""}};
                var countSql = translator2.Count(filter);
                count = await context.ExecuteScalarAsync<int>(countSql);

                if (count >= lo.ItemCollection.Count())
                    nextPageToken = $""{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/?filter={{filter}}&includeTotal=true&page={{page + 1}}&size={{size}}"";
            }}

            var previousPageToken = $""{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/?filter={{filter}}&includeTotal=true&page={{page - 1}}&size={{size}}"";
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
            return Ok(json);
            ");


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }
    }
}