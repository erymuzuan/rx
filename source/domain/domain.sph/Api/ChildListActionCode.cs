using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class ChildListActionCode : ControllerAction
    {
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            var code = new StringBuilder();

            foreach (var child in table.ChildTableCollection)
            {
                code.AppendLine(GenerateChildListAction(table, adapter, child));
            }

            return code.ToString();
        }


        private string GenerateChildListAction(TableDefinition table, Adapter adapter, TableDefinition child)
        {
            var code = new StringBuilder();
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var routes = pks.Select(k => k.Name.ToCamelCase() + this.GetRouteConstraint(k.Type));
            var args = pks.Select(k => string.Format("{0} {1}", k.Type.ToCSharp(), k.Name.ToCamelCase()));
            var filter =string.Join(" AND ", pks.Select(k => k.Name + " = \" + " + k.Name.ToCamelCase() + "+\""));



            code.AppendFormat("       [Route(\"{{{0}}}/{1}\")]", string.Join("/", routes), child.Name);
            code.AppendLine();
            code.AppendFormat(
                "       public async Task<System.Web.Mvc.ActionResult> Get{0}By{1}({2}, int page = 1, int size = 40, bool includeTotal = false)",
                child.Name, table.Name, string.Join(",",args));
            code.AppendLine("       {");


            code.AppendFormat(@"
           if (size > 200)
                throw new ArgumentException(""Your are not allowed to do more than 200"", ""size"");

            var filter = ""WHERE {5}"";
            var orderby = this.Request.QueryString[""$orderby""];
            var translator = new {0}<{1}>(null,""{1}"" ){{Schema = ""{2}""}};
            var sql = ""SELECT * FROM {2}.{1} WHERE {5}"";
            var count = 0;

            var context = new {1}Adapter();
            var nextPageToken = string.Empty;
            var lo = await context.LoadAsync(sql, page, size);
            if (includeTotal || page > 1)
            {{
                var translator2 = new {0}<{1}>(null, ""{1}""){{Schema = ""{2}""}};
                var countSql = translator2.Count(filter);
                count = await context.ExecuteScalarAsync<int>(countSql);

                if (count >= lo.ItemCollection.Count())
                    nextPageToken = string.Format(
                        ""/api/{4}/{3}/?filer={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page + 1, size);
            }}

            string previousPageToken = string.Format(""/api/{4}/{3}/?filer={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page - 1, size);
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
            ", adapter.OdataTranslator, child.Name, child.Schema, child.Name.ToLowerInvariant(),
             child.Schema.ToLowerInvariant(),
             filter);


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }
    }
}