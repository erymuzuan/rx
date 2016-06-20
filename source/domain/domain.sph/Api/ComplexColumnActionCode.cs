using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class ComplexColumnActionCode : ControllerAction
    {
        public override string Name => "Complex column link action";
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
            var args = pks.Select(k => k.GenerateParameterCode()).ToList();

            var version = !string.IsNullOrWhiteSpace(table.VersionColumn);
            var modifiedDate = !string.IsNullOrWhiteSpace(table.ModifiedDateColumn);
            if (version)
                args.Add("[IfNoneMatch]ETag etag");
            if (modifiedDate)
                args.Add("[ModifiedSince]ModifiedSinceHeader modifiedSince");

            var shouldRetrieveItem = version || modifiedDate || column.MimeType.StartsWith("=") || column.MimeType.StartsWith("{");

            code.Append($"       [Route(\"{{{string.Join("/", routes)}}}/{column.Name}\")]");
            code.AppendLine();
            code.Append($@"       public async Task<IHttpActionResult> Get{column.Name}(
                        [SourceEntity(""{adapter.Id}"")]Bespoke.Sph.Domain.Api.Adapter adapterDefinition, 
                        {args.ToString(",\r\n")})");
            code.AppendLine("       {");

            code.AppendLine($@"           CacheMetadata cache = null;
                                          var adapter = new {table.Name}Adapter();");
            if (shouldRetrieveItem)
                code.AppendLine($"var item = await adapter.LoadOneAsync({parameters.ToString(", ")});");
            if (version || modifiedDate)
                code.AppendLine(this.GenerateCachingCode(table, version, modifiedDate));


            var retVal = column.Name.ToCamelCase();
            var mime = $@"""{column.MimeType}""";
            var mimeStatement = "";
            if (column.MimeType.StartsWith("="))
            {
                mime = $"item.{column.MimeType.Remove(0, 1)}";
            }
            if (column.MimeType.StartsWith("{"))
            {
                mimeStatement = $@"
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
                var {retVal} = await adapter.Get{column.Name}Async({parameters.ToString(", ")});
                {nullGuard}              
                {mimeStatement}
                return Ok({retVal}, {mime}, cache);
            ");


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }

        private string GenerateCachingCode(TableDefinition table, bool version, bool modifiedDate)
        {
            var code = new StringBuilder();
            if (version)
                code.AppendLine($"var version = item.{table.VersionColumn}.TimeStampToString();");


            if (version && modifiedDate)
                code.Append(
                    $@"
 cache = new CacheMetadata(version, item.{table.ModifiedDateColumn});
           ");
            if (version && !modifiedDate)
                code.Append(
                    @"
 cache = new CacheMetadata(version, null);
           ");

            if (!version && modifiedDate)
                code.Append(
                    $@"
 cache = new CacheMetadata(null, item.{table.ModifiedDateColumn});
           ");


            if (modifiedDate)
                code.Append(
                    $@"
            if(modifiedSince.IsMatch(item.{table.ModifiedDateColumn}))
            {{
                return NotModified(cache);   
            }}");
            if (version)
                code.Append(
                    @"
            if(etag.IsMatch(version))
            {
                return NotModified(cache);   
            }");

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