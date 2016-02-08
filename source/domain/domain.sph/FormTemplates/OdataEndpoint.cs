using System.Text;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class OdataEndpoint : DomainObject
    {

        public Method GenerateOdataActionCode(EntityDefinition ed)
        {
            var code = new StringBuilder();
            code.Append($@"        
        [Route("""")]
        [HttpGet]
        public async Task<IHttpActionResult> OdataApi([FromUri(Name = ""$orderby"")]string orderby = null, 
                                                [FromUri(Name = ""$filter"")]string filter = null,
                                                [FromUri(Name = ""$top"")]int? top = null, 
                                                [FromUri(Name = ""$skip"")]int? skip = null, 
                                                [FromUri(Name = ""page"")]int page = 1, 
                                                [FromUri(Name = ""size"")]int size = 40, 
                                                [FromUri(Name = ""$inlinecount"")]string inlinecount = ""none"")
        {{
            const string typeName = ""{ed.Name}"";
            if (size > 200)
                throw new ArgumentException(""You cannot fetch more than 200 records at once, use page parameter instead"", nameof(size));

            var translator = new CustomEntityRestSqlTranslator(null, typeName);
            var sql = translator.Select(filter, orderby);
            var rows = 0;
            var nextPageToken = """";
            var list = await this.ExecuteCustomEntityListAsync(typeName, sql, page, size);

            if (inlinecount == ""allpages"" || page > 1)
            {{
                var translator2 = new CustomEntityRestSqlTranslator(""Id"", typeName);
                var sumSql = translator2.Count(filter);
                rows = await ExecuteScalarAsync(sumSql);

                if (rows >= list.Count)
                    nextPageToken = $""/api/{ed.Plural.ToLowerInvariant()}/?$filter={{filter}}&$inlinecount={{inlinecount}}&page={{page + 1}}&size={{size}}"";

            }}

            var previousPageToken = $""/api/{ed.Plural.ToLowerInvariant()}/?$filter={{filter}}&$inlinecount={{inlinecount}}&page={{page - 1}}&size={{size}}"";
            if(page == 1) previousPageToken = null;
            var json = new StringBuilder(""{{"");
            json.AppendLinf(""   \""results\"":[{{0}}],"", string.Join("",\r\n"", list));
            json.AppendLinf(""   \""count\"":{{0}},"", rows);
            json.AppendLinf(""   \""page\"":{{0}},"", page);
            json.AppendLinf(""   \""next\"":\""{{0}}\"","", nextPageToken);
            json.AppendLinf(""   \""previous\"":\""{{0}}\"","", previousPageToken);
            json.AppendLinf(""   \""size\"":{{0}}"", size);

            json.AppendLine(""}}"");

            return Json(json.ToString());
        }}");

            return new Method {Code = code.ToString()};
        }

        public Method GetOtherMethods()
        {
            var code = $@"
        private async Task<int> ExecuteScalarAsync(string sql)
        {{
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var command = new SqlCommand(sql, conn))
            {{
                await conn.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                if (result == DBNull.Value)
                    return 0;
                return (int)result;
            }}
        }}
        
        private async Task<List<string>> ExecuteCustomEntityListAsync(string type, string sql, int page, int size)
        {{
            var sql2 = sql;
            if (!sql2.Contains(""ORDER""))
            {{
                sql2 += ""\r\nORDER BY [Id]"";
            }}

            var paging = ObjectBuilder.GetObject<IOdataPagingProvider>();
            sql2 = paging.Tranlate(sql2, page, size);
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var command = new SqlCommand(sql2, conn))
            {{
                await conn.OpenAsync();

                var result = new List<string>();
                using (var reader = await command.ExecuteReaderAsync())
                {{
                    while (reader.Read())
                    {{
                        var id = reader.GetString(0);
                        var json = reader.GetString(1)
                            .Replace(""Id\"":0"", type + ""Id\"":\"""" + id + ""\"""");
                        result.Add(json);
                    }}
                }}

                return result;
            }}
        }}
";
            return new Method {Code = code};
        }
    }
}