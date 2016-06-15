using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Humanizer;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class ChildListActionCode : ControllerAction
    {
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            if (table.PrimaryKeyCollection.Count != 1) return string.Empty;

            var code = new StringBuilder();
            var lines = from c in table.ChildRelationCollection
                        let t = c.GetTable(adapter)
                        where c.IsSelected && null != t
                        select this.GenerateChildListAction(table, adapter, t);
            lines.ToList().ForEach(l => code.AppendLine(l));

            Console.WriteLine($"ChildListActionCode for {table.Name} with {table.ChildRelationCollection.Count} child tables");

            return code.ToString();
        }


        private string GenerateChildListAction(TableDefinition table, Adapter adapter, TableDefinition child)
        {
            if (null == table.PrimaryKey) return string.Empty;

            var parentTable = adapter.TableDefinitionCollection.SingleOrDefault(x => x.Name == table.Name);
            var fk = parentTable?.ChildRelationCollection.SingleOrDefault(x => x.Table == child.Name);

            if (null == fk) return string.Empty;

            var code = new StringBuilder();
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var parameters = pks.Select(k => k.Name.ToCamelCase()).ToArray();
            var routes = pks.Select(k => k.Name.ToCamelCase() + this.GetRouteConstraint(k));
            var args = pks.Select(k => k.GenerateParameterCode());

            var delimiter = (new[] { "int", "double", "float", "short", "long", "byte", "single", "decimal" }).Contains(table.PrimaryKey.ClrType.ToCSharp()) ? "" : "'";
            var filter = $"[{fk.Column}] = {delimiter}{{{table.PrimaryKey.Name.ToCamelCase()}}}{delimiter}";


            var resources = child.Name.Pluralize().ToIdFormat();
            code.Append($"       [Route(\"{{{string.Join("/", routes)}}}/{resources}\")]");
            code.AppendLine();
            code.Append(
                $"       public async Task<IHttpActionResult> Get{child.Name.Pluralize()}By{table.Name}({string.Join(",", args)}, int page = 1, int size = 40, bool includeTotal = false)");
            code.AppendLine("       {");


            code.Append($@"
           if (size > 200)
                throw new ArgumentException(""Your are not allowed to do more than 200"", ""size"");

            var filter = $""WHERE {filter}"";
            var translator = new {adapter.OdataTranslator}<{child.Name}>(null,""{child.Name}"" ){{Schema = ""{table.Schema}""}};
            var sql = $""SELECT * FROM {child.Schema}.{child.Name} WHERE {filter}"";
            var count = 0;

            var context = new {child.Name}Adapter();
            var nextPageToken = string.Empty;
            var lo = await context.LoadAsync(sql, page, size);
            if (includeTotal || page > 1)
            {{
                var translator2 = new {adapter.OdataTranslator}<{child.Name}>(null, ""{child.Name}""){{Schema = ""{table.Schema}""}};
                var countSql = translator2.Count(filter);
                count = await context.ExecuteScalarAsync<int>(countSql);

                if (count >= lo.ItemCollection.Count())
                    nextPageToken = $""/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/{{{parameters.ToString(",")}}}/{resources}/?includeTotal=true&page={{page + 1}}&size={{size}}"";
            }}

            string previousPageToken = $""/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/{{{parameters.ToString(",")}}}/{resources}/?filer={{filter}}&includeTotal=true&page={{page - 1}}&size={{size}}"";
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
            return Json(json);
            ");


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }
    }
}