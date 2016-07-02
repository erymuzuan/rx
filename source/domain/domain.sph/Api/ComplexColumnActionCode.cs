using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class ComplexColumnActionCode : ControllerAction
    {
        public override bool Applicable(TableDefinition table)
        {
            return null != table?.PrimaryKey;
        }

        public override string Name => "Complex column link action";

        public CachingSetting CachingSetting { get; set; } = new CachingSetting
        {
            CacheControl = "Public",
            NoStore = true,
            Expires = 600
        };

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
            code.Append($@"       public async Task<IHttpActionResult> Get{column.Name}Async(
                        [SourceEntity(""{adapter.Id}"")]Bespoke.Sph.Domain.Api.Adapter adapterDefinition, 
                        {args.ToString(",\r\n")})");
            code.AppendLine("       {");




            code.AppendLine($@"           CacheMetadata cache = null;
                                          var adapter = new {table.ClrName}Adapter();");

            Func<Column, string> generateFilter = (c) =>
            {
                var name = c.Name;
                var identifier = name.ToCamelCase();
                if (typeof(DateTime) == c.Type)
                    return $"{name} eq DateTime'{identifier}'";
                if (typeof(string) == c.Type)
                    return $"{name} eq '{identifier}'";
                return $"{name} eq {identifier}";
            };


            var filter = pks.Select(generateFilter);
            var cachingCode = $@"
            var cacheSetting = adapterDefinition
                                .TableDefinitionCollection.Single(x => x.Name == ""{table.Name}"" && x.Schema == ""{table.Schema}"")
                                .ControllerActionCollection.OfType<{typeof(ChildListActionCode).FullName}>().Single()
                                .CachingSetting;
            var scalarTranslator = new {adapter.OdataTranslator}<{table.ClrName}>(""{table.ModifiedDateColumn}"",""{table.Name}""){{Schema = ""{table.Schema}""}};
            var getModifiedDateSql = scalarTranslator.Scalar($""{filter.ToString(" and ")}"");
            var modifiedDate = (await adapter.ExecuteScalarAsync<DateTime?>(getModifiedDateSql)) ?? System.DateTime.Now;
            cache = new CacheMetadata(null, modifiedDate, cacheSetting);
            
            if(modifiedSince.IsMatch(modifiedDate))
            {{
                return NotModified(cache);   
            }}";

            if (modifiedDate || version)
                code.AppendLine(cachingCode);

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
                var mimeType = scripting.Evaluate<string, {table.ClrName}>(code, item);
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

        public override string[] GetActionNames(TableDefinition table, Adapter adapter)
        {
            var actions = from c in table.ColumnCollection
                        where c.IsComplex
                        select $"Get{c.Name}Async";

            return actions.ToArray();
        }

        public override HypermediaLink[] GetHypermediaLinks(Adapter adapter, TableDefinition table)
        {
            if (null == table.PrimaryKey) return base.GetHypermediaLinks(adapter, table);

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