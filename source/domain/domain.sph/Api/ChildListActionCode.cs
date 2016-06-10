using System;
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
            if (table.PrimaryKeyCollection.Count != 1) return string.Empty;

            var code = new StringBuilder();
            var lines = from c in table.ChildTableCollection
                select this.GenerateChildListAction(table, adapter, c);
            lines.ToList().ForEach(l => code.AppendLine(l));

            Console.WriteLine($"ChildListActionCode for {table.Name} with {table.ChildTableCollection.Count} child tables");

            return code.ToString();
        }


        private string GenerateChildListAction(TableDefinition table, Adapter adapter, TableDefinition child)
        {
            var code = new StringBuilder();
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var parameters = pks.Select(k => k.Name.ToCamelCase());
            var routes = pks.Select(k => k.Name.ToCamelCase() + this.GetRouteConstraint(k));
            var args = pks.Select(k => k.GenerateParameterCode());
            var filter =string.Join(" AND ", pks.Select(k => k.Name + " = \" + " + k.Name.ToCamelCase() + "+\""));


            code.Append($"       [Route(\"{{{string.Join("/", routes)}}}/{child.Name}\")]");
            code.AppendLine();
            code.Append(
                $"       public async Task<IHttpActionResult> Get{child.Name}By{table.Name}({string.Join(",", args)}, int page = 1, int size = 40, bool includeTotal = false)");
            code.AppendLine("       {");


            code.Append($@"
           if (size > 200)
                throw new ArgumentException(""Your are not allowed to do more than 200"", ""size"");

            var filter = ""WHERE {filter}"";
            var translator = new {adapter.OdataTranslator}<{child.Name}>(null,""{child.Name}"" ){{Schema = ""{child.Schema}""}};
            var sql = ""SELECT * FROM {child.Schema}.{child.Name} WHERE {filter}"";
            var count = 0;

            var context = new {child.Name}Adapter();
            var nextPageToken = string.Empty;
            var lo = await context.LoadAsync(sql, page, size);
            if (includeTotal || page > 1)
            {{
                var translator2 = new {adapter.OdataTranslator}<{child.Name}>(null, ""{child.Name}""){{Schema = ""{child.Schema}""}};
                var countSql = translator2.Count(filter);
                count = await context.ExecuteScalarAsync<int>(countSql);

                if (count >= lo.ItemCollection.Count())
                    nextPageToken = string.Format(
                        ""/api/{table.Name}/{{0}}/{child.Name.ToLowerInvariant()}/?includeTotal=true&page={{1}}&size={{2}}"", {string.Join(",", parameters)}, page + 1, size);
            }}

            string previousPageToken = string.Format(""/api/{child.Schema.ToLowerInvariant()}/{child.Name.ToLowerInvariant()}/?filer={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page - 1, size);
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
            ");


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }
    }
}