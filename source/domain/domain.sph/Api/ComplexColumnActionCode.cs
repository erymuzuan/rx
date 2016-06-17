using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class ComplexColumnActionCode : ControllerAction
    {
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            if (table.PrimaryKeyCollection.Count != 1) return string.Empty;

            var code = new StringBuilder();
            var lines = from c in table.ColumnCollection
                        where c.IsComplex
                        select this.GenerateColumnAction(adapter, table, c);
            lines.ToList().ForEach(l => code.AppendLine(l));

            Console.WriteLine($"ChildListActionCode for {table.Name} with {table.ChildRelationCollection.Count} child tables");

            return code.ToString();
        }
        private string GenerateColumnAction(Adapter adapter, TableDefinition table, Column column)
        {
            if (null == table.PrimaryKey) return string.Empty;

            var code = new StringBuilder();
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var parameters = pks.Select(k => k.Name.ToCamelCase()).ToArray();
            var routes = pks.Select(k => k.Name.ToCamelCase() + this.GetRouteConstraint(k));
            var args = pks.Select(k => k.GenerateParameterCode());

            code.Append($"       [Route(\"{{{string.Join("/", routes)}}}/{column.Name}\")]");
            code.AppendLine();
            code.Append(
                $@"       public async Task<IHttpActionResult> Get{column.Name}([SourceEntity(""{adapter.Id}"")]Bespoke.Sph.Domain.Api.Adapter adapterDefinition, {string.Join(",", args)})");
            code.AppendLine("       {");


            var retVal = column.Name.ToCamelCase();
            var mime = $@"""{column.MimeType}""";
            var mimeStatement = "";
            if (column.MimeType.StartsWith("="))
            {
                mimeStatement = $"var item = await adapter.LoadOneAsync({parameters.ToString(", ")});";
                mime = $"item.{column.MimeType.Remove(0,1)}";
            }
            if (column.MimeType.StartsWith("{"))
            {
                mimeStatement = $@"
                var item = await adapter.LoadOneAsync({parameters.ToString(", ")});
                var scripting = ObjectBuilder.GetObject<IScriptEngine>();
                var table = adapterDefinition.TableDefinitionCollection.Single(x =>x.Name ==""{table.Name}"");
                var column = table.ColumnCollection.Single(x => x.Name == ""{column.Name}"");
                var code = column.MimeType;
                code = code.Remove(code.Length - 1, 1).Remove(0, 1);
                var mimeType = scripting.Evaluate<string, {table.Name}>(code, item);
";
                mime = "mimeType";
            }

            var nullGuard = column.IsNullable ? $@"
                  if(null == {retVal})
                    return NotFound($""Cannot find {column.Name} for {table.Name} {parameters.ToString("/", x => "{" + x + "}")}"");" : "";
            code.Append($@"
                var adapter = new {table.Name}Adapter();
                var {retVal} = await adapter.Get{column.Name}Async({parameters.ToString(", ")});
                {nullGuard}              
                {mimeStatement}
                return Ok({retVal}, {mime});
            ");


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }

        public override HypermediaLink[] GetHypermediaLinks(Adapter adapter, TableDefinition table)
        {
            if (null == table.PrimaryKey) return base.GetHypermediaLinks(adapter, table);

            var code = new StringBuilder();
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var parameters = pks.Select(k => k.Name.ToCamelCase()).ToArray();
            var links = from c in table.ColumnCollection
                where c.IsComplex
                        select new HypermediaLink
                        {
                            Rel = c.Name,
                            Method = "GET",
                            Href = $"{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/{parameters.ToString("/", x => $"{{{x}}}")}/{c.Name}",
                            Description = $"Get {table.Name}'s {c.Name}"
                        };
            return links.ToArray();
        }
    }
}