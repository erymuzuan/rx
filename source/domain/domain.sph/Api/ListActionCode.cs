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
            code.AppendFormat(@"
           if (size > 200)
                throw new ArgumentException(""Your are not allowed to do more than 200"", ""size"");

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
                        ""/api/{5}/{4}/?filter={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page + 1, size);
            }}

            string previousPageToken = string.Format(""/api/{5}/{4}/?filter={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page - 1, size);
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
            return json;
            ", adapter.OdataTranslator, table.Name, pk, table.Schema, table.Name.ToLowerInvariant(), table.Schema.ToLowerInvariant());


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }
    }
}