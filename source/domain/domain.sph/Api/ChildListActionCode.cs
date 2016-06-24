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
        public override string Name => "Related table list action";
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
            var args = pks.Select(k => k.GenerateParameterCode()).ToList();


            var version = !string.IsNullOrWhiteSpace(child.VersionColumn);
            var modifiedDate = !string.IsNullOrWhiteSpace(child.ModifiedDateColumn);
            if (version || modifiedDate)
                args.Add($@"[SourceEntity(""{adapter.Id}"")]Bespoke.Sph.Domain.Api.Adapter adapterDefinition");
            if (version)
                args.Add("[IfNoneMatch]ETag etag");

            if (modifiedDate)
                args.Add("[ModifiedSince]ModifiedSinceHeader modifiedSince");


            var delimiter = (new[] { "int", "double", "float", "short", "long", "byte", "single", "decimal" }).Contains(table.PrimaryKey.ClrType.ToCSharp()) ? "" : "'";
            var filter = $"{fk.Column} eq {delimiter}{{{table.PrimaryKey.Name.ToCamelCase()}}}{delimiter}";

            var childPks = child.ColumnCollection.Where(m => child.PrimaryKeyCollection.Contains(m.Name))
              .Select(x =>x.Type == typeof(DateTime)? $"{{item.{x.Name}:yyyy-MM-dd}}" : $"{{item.{x.Name}}}")
              .ToString("/");
            var objectLinks = "";
            if (!string.IsNullOrWhiteSpace(childPks))
                objectLinks = $@"        
                JArray links = JArray.Parse($@""[
		                                {{{{
	                                """"method"""": """"GET"""",
	                                """"rel"""": """"self"""",
	                                """"href"""": """"{{ConfigurationManager.BaseUrl}}/api/{adapter.Id}/{child.Name.ToIdFormat()}/{childPks}"""",
	                                """"desc"""": """"Issue a GET request""""
	                                }}}}
		                                ]"");
		        var link = new JProperty(""_links"", links);
		        r.Last.AddAfterSelf(link);";

            var cachingCode = $@"
            var cacheSetting = adapterDefinition
                                .TableDefinitionCollection.Single(x => x.Name == ""{table.Name}"" && x.Schema == ""{table.Schema}"")
                                .ControllerActionCollection.OfType<{typeof(ChildListActionCode).FullName}>().Single()
                                .CachingSetting;
            var maxTranslator = new {adapter.OdataTranslator}<{child.ClrName}>(""{child.ModifiedDateColumn}"",""{child.Name}""){{Schema = ""{child.Schema}""}};
            var getMaxModifiedDateSql = maxTranslator.Max($""{filter}"");
            var modifiedDate = (await context.ExecuteScalarAsync<DateTime?>(getMaxModifiedDateSql)) ?? System.DateTime.Now;
            var cache = new CacheMetadata(null, modifiedDate, cacheSetting);
            
            if(modifiedSince.IsMatch(modifiedDate))
            {{
                return NotModified(cache);   
            }}";


            var resources = child.Name.Pluralize().ToIdFormat();
            code.Append($"       [Route(\"{{{string.Join("/", routes)}}}/{resources}\")]");
            code.AppendLine();
            code.Append(
                $"       public async Task<IHttpActionResult> Get{child.Name.Pluralize()}By{table.Name}({args.ToString(",\r\n\t\t\t\t")}, int page = 1, int size = 40, bool includeTotal = false)");
            code.AppendLine("       {");


            code.Append($@"
           if (size > 200)
                throw new ArgumentException(""Your are not allowed to do more than 200"", ""size"");

            var translator = new {adapter.OdataTranslator}<{child.ClrName}>(null,""{child.Name}"" ){{Schema = ""{child.Schema}""}};
            var sql = translator.Select($""{filter}"", null);
            var count = 0;
            var nextPageToken = string.Empty;

            var context = new {child.ClrName}Adapter();
            {(modifiedDate ? cachingCode : "")}

            var lo = await context.LoadAsync(sql, page, size);
            if (includeTotal || page > 1)
            {{
                var countSql = translator.Count($""{filter}"");
                count = await context.ExecuteScalarAsync<int>(countSql);

                if (page * size < count)
                    nextPageToken = $""/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/{{{parameters.ToString(",")}}}/{resources}/?includeTotal=true&page={{page + 1}}&size={{size}}"";
                else
                    nextPageToken = null;
            }}

            string previousPageToken = $""/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/{{{parameters.ToString(",")}}}/{resources}/?filer={filter}&includeTotal=true&page={{page - 1}}&size={{size}}"";
            if(page == 1)
                previousPageToken = null;
            
            var pageLinks = new System.Collections.Generic.List<object>();
            if(null != previousPageToken)
                pageLinks.Add(new {{ method = ""GET"", rel= ""previous"", href= previousPageToken}});
            if(null != nextPageToken)
                pageLinks.Add(new {{ method = ""GET"", rel= ""next"", href= nextPageToken}});


            var json = JObject.Parse( JsonConvert.SerializeObject( new
            {{
                count,
                page,
                size,
                _links = pageLinks
            }}));

            JArray results = JArray.Parse(""[]"");
	        foreach (var item in lo.ItemCollection)
	        {{
		        var r = JObject.Parse(JsonConvert.SerializeObject(item));
		        r.Remove(""WebId"");
		        {objectLinks}

		        results.Add(r);
	        }}
	        json.Last.AddBeforeSelf(new JProperty(""results"", results));

            return Json(json.ToString(){((modifiedDate) ? ", cache" : "")});
            ");


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }

        public override HypermediaLink[] GetHypermediaLinks(Adapter adapter, TableDefinition table)
        {
            if (table.PrimaryKeyCollection.Count == 0) return null;
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var parameters = pks.Select(m => m.Name.ToCamelCase()).ToArray();
            
            var links = from r in table.ChildRelationCollection
                where r.IsSelected
                let child = adapter.TableDefinitionCollection.SingleOrDefault(x => x.Name == r.Table)
                where null != child
                select new HypermediaLink
                {
                    Rel = $"{r.Table}",
                    Method = "GET",
                    Href =$"{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/{parameters.ToString("/", x => $"{{{x}}}")}/{child.Name.Pluralize().ToIdFormat()}",
                    Description = $"Issue a GET command to your {child.Name.Pluralize()}"
                };




            return links.ToArray();

        }
    }
}