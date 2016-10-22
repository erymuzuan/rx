using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using System;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{

    [Export(typeof(SqlAdapterDeveloperService))]
    public class SqlAdapterDeveloperService
    {
        private static bool m_initialized;

        public static void Init()
        {
            if (m_initialized) return;
            m_initialized = true;

            var ds = new DeveloperService();
            ObjectBuilder.ComposeMefCatalog(ds);
            ObjectBuilder.AddCacheList(ds);
        }

        [JsonIgnore]
        [ImportMany("SqlColumn", typeof(SqlColumn), AllowRecomposition = true)]
        public Lazy<SqlColumn, IColumnGeneratorMetadata>[] ColumnGenerators { get; set; }
    }

    public partial class SqlServerAdapter : Adapter
    {

        public override string OdataTranslator => "OdataSqlTranslator";

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var vr = (await base.ValidateAsync()).ToList();

            if (string.IsNullOrWhiteSpace(this.Database))
                vr.Add("Database", "Database cannot be empty");


            return vr.AsEnumerable();
        }

        public static readonly string[] ImportDirectives =
        {
               typeof(Entity).Namespace ,
               typeof(int).Namespace ,
               typeof(Task<>).Namespace ,
               typeof(Enumerable).Namespace ,
               typeof(IEnumerable<>).Namespace,
               typeof(StringBuilder).Namespace ,
               typeof(SqlConnection).Namespace ,
               typeof(CommandType).Namespace ,
               typeof(XmlAttributeAttribute).Namespace,
               "System.Web.Mvc",
               "Bespoke.Sph.Web.Helpers"
    };



        protected override Task<IEnumerable<Class>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            options.AddReference(typeof(Microsoft.CSharp.RuntimeBinder.Binder));
            var sources = new ObjectCollection<Class>();
            foreach (var at in this.TableDefinitionCollection)
            {
                var table = this.TableDefinitionCollection.Single(t => t.Name == at.Name);
                options.AddReference(typeof(SqlConnection));

                var code = new Class { Name = $"{table.ClrName}Adapter", Namespace = this.CodeNamespace };
                code.ImportCollection.AddRange(ImportDirectives);
                code.ImportCollection.AddRange(namespaces);


                code.AddMethod(GenerateExecuteScalarMethod());
                code.AddMethod(GenerateDeleteMethod(table));
                code.AddMethod(GenerateInsertMethod(table));
                code.AddMethod(GenerateUpdateMethod(table));
                code.AddMethod(GenerateSelectMethod(table));
                code.AddMethod(GenerateSelectOneMethod(table));
                code.AddMethod(GenerateConnectionStringProperty());


                var columns = from c in at.ColumnCollection
                              let read = c.GenerateReadAdapterCode(table, this)
                              where !string.IsNullOrWhiteSpace(read)
                              select read;
                columns.ToList().ForEach(x => code.AddMethod(x));

                sources.Add(code);


            }

            var adapterClass = new Class { Name = Name, Namespace = CodeNamespace };
            adapterClass.AddNamespaceImport<DateTime, DomainObject, SqlConnection, CommandType>();
            adapterClass.AddNamespaceImport<Task>();
            adapterClass.AddProperty(this.GenerateConnectionStringProperty());

            var webApi = new Class { Name = $"{Name}Controller", BaseClass = "BaseApiController", Namespace = CodeNamespace };
            webApi.AttributeCollection.Add($@"[RoutePrefix(""{RoutePrefix}"")]");
            webApi.AddNamespaceImport<DateTime, DomainObject, Task, BaseApiController>();
            webApi.AddNamespaceImport<System.Web.Http.IHttpActionResult, Polly.Policy>();

            webApi.AddMethod(this.GenerateDefaultAction());

            var addedActions = new List<string>();
            foreach (var op in this.OperationDefinitionCollection)
            {
                op.CodeNamespace = this.CodeNamespace;
                var methodName = op.MethodName;

                if (addedActions.Contains(methodName)) continue;
                addedActions.Add(methodName);
                webApi.AddMethod(op.GenerateApiCode(this));
                //
                adapterClass.AddMethod(op.GenerateActionCode(this));

                var requestSources = op.GenerateRequestCode();
                sources.AddRange(requestSources);

                var responseSources = op.GenerateResponseCode();
                sources.AddRange(responseSources);
            }
            sources.Add(adapterClass);
            sources.Add(webApi);

            return Task.FromResult(sources.AsEnumerable());
        }

        private string GenerateDefaultAction()
        {
            var code = new StringBuilder("");
            code.AppendLine($@"
            [Route("""")]
            [HttpGet]
            public IHttpActionResult Index()
            {{

                var links = new System.Collections.Generic.List<object>(); ");



            foreach (var table in this.TableDefinitionCollection)
            {
                var count = 0;
                foreach (var action in table.ControllerActionCollection.Where(x => x.IsEnabled))
                {
                    code.AppendLine($@"
                    var tl{table.ClrName}{++count} = new {{
                                    rel = ""{table.Name}"",
                                    href = $""{{ConfigurationManager.BaseUrl}}/api/{this.Id}/{table.Name.ToIdFormat()}"",                                    
                                    description = ""{action.Name}""
                                }};
                   links.Add(tl{table.ClrName}{count});
                ");

                }
            }
            foreach (var op in this.OperationDefinitionCollection)
            {
                code.AppendLine($@"
                    var tl{op.MethodName} = new {{
                                    rel = ""{op.Name}"",
                                    href = $""{{ConfigurationManager.BaseUrl}}/api/{this.Id}/{op.Name.ToIdFormat()}"",
                                    method = ""{(op.UseHttpGet ? "GET" : "POST")}""
                                }};
                   links.Add(tl{op.MethodName});
                ");

            }

            code.AppendLine($@"
                return Ok(new {{ _links = links}});

            }}");

            return code.ToString();
        }


        private string GenerateExecuteScalarMethod()
        {
            var code = new StringBuilder();
            code.AppendLine("       public async Task<T> ExecuteScalarAsync<T>(string sql)");
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLine("           using(var cmd = new SqlCommand(sql, conn))");
            code.AppendLine("           {");

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               var dbval = await cmd.ExecuteScalarAsync();");
            code.AppendLine("               if(dbval == System.DBNull.Value)");
            code.AppendLine("                   return default(T);");
            code.AppendLine("               return (T)dbval;");
            code.AppendLine("           }");

            code.AppendLine("       }");

            return code.ToString();

        }

        protected override Task<Class> GenerateOdataTranslatorSourceCodeAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "Bespoke.Sph.Integrations.Adapters.OdataSqlTranslator.cs";

            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
            // ReSharper disable AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
            // ReSharper restore AssignNullToNotNullAttribute
            {
                var code = reader.ReadToEnd();
                code = code.Replace("__NAMESPACE__", this.CodeNamespace);
                var source = new Class(code) { FileName = "OdataSqlTranslator.cs" };
                return Task.FromResult(source);

            }
        }

        public int Version { get; set; }
        public async Task<int> GetDatabaseVersionAsync()
        {
            var version = 2012;
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand("SELECT SERVERPROPERTY ('ProductVersion')", conn))
            {
                await conn.OpenAsync();
                var result = (await cmd.ExecuteScalarAsync()) as string;
                var number = Strings.RegexInt32Value(result, @"^(?<version>[0-9]{1,2})\..*?", "version");
                if (number < 11) version = 2008;
            }
            return version;
        }
        protected override Task<Class> GeneratePagingSourceCodeAsync()
        {

            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"Bespoke.Sph.Integrations.Adapters.Sql{Version}PagingTranslator.cs";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            // ReSharper disable AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
            // ReSharper restore AssignNullToNotNullAttribute
            {
                var code = reader.ReadToEnd();
                code = code.Replace("__NAMESPACE__", this.CodeNamespace);
                var source = new Class(code) { FileName = "SqlPagingTranslator.cs" };
                return Task.FromResult(source);

            }
        }

        private string GenerateUpdateMethod(TableDefinition table)
        {
            var columns = table.ColumnCollection;
            var code = new StringBuilder();
            code.AppendLine($"       public async Task<int> UpdateAsync({table.ClrName} item)");
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetUpdateCommand(table));
            code.AppendLine("           {");
            foreach (var col in columns.Where(c => c.CanWrite && !c.IsPrimaryKey))
            {
                var parameterCode = col.GenerateUpdateParameterValue();
                if (!string.IsNullOrWhiteSpace(parameterCode))
                    code.AppendLine(parameterCode);
            }

            // WHERE clause is in the primary key
            foreach (var pk in table.PrimaryKeyCollection)
            {
                code.AppendLine($"               cmd.Parameters.AddWithValue(\"@{pk}\", item.{pk.ToClrIdentifier(this.ClrNameStrategy)});");
            }

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");
            code.AppendLine("       }");
            return code.ToString();
        }


        private string GenerateConnectionStringProperty()
        {
            var code = new StringBuilder();
            code.AppendLinf("       public string ConnectionString");
            code.AppendLine("       {");

            code.AppendLine("           get ");
            code.AppendLine("           {");
            code.AppendLine($@"               var conn = ConfigurationManager.ConnectionStrings[""{Name}""];");
            code.AppendLine("               if(null != conn)return conn.ConnectionString;");
            code.AppendLine($@"             var conn2 = ConfigurationManager.GetEnvironmentVariable(""{Name}ConnectionString"");");
            code.AppendLine("               if(null != conn)return conn2;");
            code.AppendLinf("               return @\"{0}\";", this.ConnectionString);
            code.AppendLine("           }");
            code.AppendLine("       }");

            return code.ToString();
        }

        public string GetSelectOneCommand(TableDefinition table)
        {
            var columns = table.ColumnCollection.Where(c => !c.IsComplex).ToList();
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name));
            var sql = new StringBuilder();

            sql.AppendLine($"SELECT {columns.ToString(",\r\n ", x => $"t0.[{x.Name}]")} ");
            if (table.ColumnCollection.Any(c => c.LookupColumnTable.IsEnabled))
            {
                var lookups = table.ColumnCollection.Where(x => x.LookupColumnTable.IsEnabled)
                 .Select((x, i) => $"t{i + 1}.[{x.LookupColumnTable.ValueColumn}] as '{x.LookupClrName}'");
                sql.Append(",");
                sql.AppendLine(lookups.ToString(",\r\n"));


                sql.AppendLine($" FROM [{table.Schema}].[{table}] t0");
                var joins = table.ColumnCollection.Where(x => x.LookupColumnTable.IsEnabled)
                    .Select((x, i) => $" LEFT JOIN {x.LookupColumnTable.Table} t{i + 1} ON t0.[{x.Name}] = t{i + 1}.[{x.LookupColumnTable.KeyColumn}]");
                sql.AppendLine(joins.ToString("\r\n"));
            }
            else
            {
                sql.AppendLine($"FROM [{table.Schema}].[{table}] t0");
            }
            sql.AppendLine("WHERE ");
            var parameters = pks.Select(k => $"t0.[{k.Name}] = @{k.ClrName}");
            sql.AppendLine(string.Join(" AND ", parameters));

            return sql.ToString();
        }
        public string GetSelectCommand(TableDefinition table)
        {
            var columns = table.ColumnCollection.Where(c => !c.IsComplex).ToList();
            if (columns.All(c => !c.LookupColumnTable.IsEnabled))
                return $"SELECT {columns.ToString(", ", x => $"[{x.Name}]")} FROM [{table.Schema}].[{table}] ";

            var code = new StringBuilder($"SELECT {columns.ToString(", ", x => $"t0.[{ x.Name }]")}, ");
            var lookups = table.ColumnCollection.Where(x => x.LookupColumnTable.IsEnabled)
                .Select(x => x.LookupColumnTable)
                .Select((x, i) => $"t{i}.[{x.ValueColumn}] as '{x.Name}'");
            code.AppendLine(lookups.ToString(", "));
            code.AppendLine("FROM [{table.Schema}].[{table}] t0");
            var joins = table.ColumnCollection.Where(x => x.LookupColumnTable.IsEnabled)
                .Select((x, i) => $" LEFT JOIN {x.LookupColumnTable.Table} t{i} ON t0.[{x.Name}] = t1.[{x.LookupColumnTable.KeyColumn}]");
            code.AppendLine(joins.ToString("\r\n"));
            return code.ToString();
        }

        private string GenerateSelectOneMethod(TableDefinition table)
        {

            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var arguments = pks.Select(k => k.GenerateParameterCode());
            var code = new StringBuilder();
            code.AppendLine($"       public async Task<{table.ClrName}> LoadOneAsync({arguments.ToString(",")})");
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetSelectOneCommand(table));
            code.AppendLine("           {");
            foreach (var pk in pks)
            {

                code.AppendLinf("               cmd.Parameters.AddWithValue(\"@{0}\", {0});", pk.Name.ToCamelCase());
            }


            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               using(var reader = await cmd.ExecuteReaderAsync())");
            code.AppendLine("               {");
            code.AppendLine("                   while(await reader.ReadAsync())");
            code.AppendLine("                   {");
            code.AppendLine($"                       var item = new {table.ClrName}();");
            code.AppendLine(PopulateItemFromReader(table.Name));
            code.AppendLine("                       return item;");
            code.AppendLine("                   }");
            code.AppendLine("               }");
            code.AppendLine("               return null;");
            code.AppendLine("           }");

            code.AppendLine("       }");

            return code.ToString();
        }

        private string GenerateSelectMethod(TableDefinition table)
        {

            var columns = table.ColumnCollection.Where(c => !c.IsComplex).ToList();
            var select = $"SELECT {columns.ToString(",\r\n", x => $"t.[{x.Name}]")} FROM";
            var joins = new string[] { };
            if (columns.Any(x => x.LookupColumnTable.IsEnabled))
            {
                var fields = columns.Select(x => $"t.[{x.Name}]").ToList();
                var lookups = table.ColumnCollection.Where(x => x.LookupColumnTable.IsEnabled)
                                .Select((x, i) => $"t{i}.[{x.LookupColumnTable.ValueColumn}] as '{x.LookupClrName}'");

                joins = table.ColumnCollection.Where(x => x.LookupColumnTable.IsEnabled)
                    .Select((x, i) => $" LEFT JOIN {x.LookupColumnTable.Table} t{i} ON t.[{x.Name}] = t{i}.[{x.LookupColumnTable.KeyColumn}]")
                    .ToArray();

                select = $"SELECT {fields.Concat(lookups).ToString(",\r\n")} FROM";
            }
            var code = new StringBuilder();


            //load async
            code.Append($@"       public async Task<LoadOperation<{table.ClrName}>> LoadAsync(string sql, 
                                                                                    int page = 1, 
                                                                                    int size = 40, 
                                                                                    bool includeTotal = false)");
            code.AppendLine("       {");

            code.AppendLine("           if (!sql.ToString().Contains(\"ORDER\"))");
            code.AppendLinf("               sql +=\"\\r\\nORDER BY [{0}]\";", table.PrimaryKeyCollection.FirstOrDefault() ?? table.ColumnCollection.Select(m => m.Name).First());
            code.AppendLine("           var translator = new SqlPagingTranslator();");
            code.AppendLine("           sql = translator.Translate(sql, page, size);");
            code.AppendLine($@"           sql = sql.Replace(""SELECT * FROM"", @""{select}"")
                                                    .Replace(""[{table.Schema}].[{table.Name}]"", @""[{table.Schema}].[{table.Name}] t 
    {joins.ToString("\r\n")}"");");

            code.AppendLine();
            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand( sql, conn))", this.GetSelectCommand(table));
            code.AppendLine("           {");

            code.AppendLinf("               var lo = new LoadOperation<{0}>", table.ClrName);
            code.AppendLine("                            {");
            code.AppendLine("                               CurrentPage = page,");
            code.AppendLine("                               Filter = sql,");
            code.AppendLine("                               PageSize = size,");
            code.AppendLine("                            };");

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               using(var reader = await cmd.ExecuteReaderAsync())");
            code.AppendLine("               {");
            code.AppendLine("                   while(await reader.ReadAsync())");
            code.AppendLine("                   {");
            code.AppendLine($"                       var item = new {table.ClrName}();");
            code.AppendLine(PopulateItemFromReader(table.Name));
            code.AppendLinf("                       lo.ItemCollection.Add(item);");
            code.AppendLine("                   }");
            code.AppendLine("               }");
            code.AppendLine("               return lo;");
            code.AppendLine("           }");

            code.AppendLine("       }");

            return code.ToString();
        }

        private string PopulateItemFromReader(string name)
        {
            var columns = TableDefinitionCollection.Single(x => x.Name == name).ColumnCollection;
            var code = new StringBuilder();
            var readCodes = from c in columns
                            where !c.IsComplex
                            let dbVal = $@"reader[""{c.Name}""]"
                            let statement = c.GenerateValueStatementCode(dbVal)
                            let statementCode = string.IsNullOrWhiteSpace(statement) ? "" : statement + "\r\n"
                            select $"{statementCode}item.{c.ClrName} = {c.GenerateValueAssignmentCode(dbVal)};";
            code.JoinAndAppendLine(readCodes, "\r\n");

            // TODO : we should create a column from the lookup table and use the column Assignment code to do the read
            var readLookup = from c in columns
                             where c.LookupColumnTable.IsEnabled
                             let typeName = c.LookupColumnTable.Type.ToCSharp()
                             let read = typeName == "string" ? $@"reader[""{c.LookupClrName}""].ReadNullableString()" : $@" ({typeName})reader[""{c.LookupClrName}""]"
                             select $@"item.{c.LookupClrName} = {read};";
            code.JoinAndAppendLine(readLookup, "\r\n");

            return code.ToString();
        }

        public string GetDeleteCommand(TableDefinition table)
        {
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var parameters = pks.Select(k => string.Format("[{0}] = @{0}", k.Name));
            var sql = new StringBuilder("DELETE FROM ");
            sql.Append($"[{table.Schema}].[{table}] ");
            sql.AppendLine("WHERE");
            sql.AppendLine(string.Join(" AND ", parameters));


            return sql.ToString();
        }
        private string GenerateDeleteMethod(TableDefinition table)
        {
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var arguements = pks.Select(k => k.GenerateParameterCode());
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<int> DeleteAsync({0})", string.Join(", ", arguements));
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetDeleteCommand(table));
            code.AppendLine("           {");

            foreach (var pk in pks)
            {
                code.AppendLine();
                code.AppendFormat("               cmd.Parameters.AddWithValue(\"@{0}\", {1});", pk.Name, pk.Name.ToCamelCase());
            }

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");
            return code.ToString();
        }


        private string GenerateInsertMethod(TableDefinition table)
        {
            var code = new StringBuilder();
            var columns = table.ColumnCollection;
            var primaryKeyIdentityColumns = columns.Where(x => x.IsPrimaryKey && x.IsIdentity).ToList();
            var sql = this.GetInsertCommand(table);
            var hasSingleIdentityPrimaryKey = primaryKeyIdentityColumns.Count == 1;
            if (hasSingleIdentityPrimaryKey)
            {
                code.AppendLine($"       public async Task<{primaryKeyIdentityColumns[0].ClrType.ToCSharp()}> InsertAsync({table.ClrName} item)");
                sql += "; SELECT Scope_Identity()";
            }
            else
                code.AppendLine($"       public async Task<int> InsertAsync({table.ClrName} item)");

            code.AppendLine("       {");

            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");

            code.AppendLine($"           using(var cmd = new SqlCommand(@\"{sql}\", conn))");
            code.AppendLine("           {");
            code.AppendLine("               await conn.OpenAsync();");
            var lookupIndex = 0;
            foreach (var col in columns.Where(x => x.Ignore && x.LookupColumnTable.IsEnabled))
            {
                ++lookupIndex;
                var script = new StringBuilder();
                script.AppendLine($@"using(var cmd{lookupIndex} = new SqlCommand(@""SELECT [{col.LookupColumnTable.KeyColumn}] 
                                                                            FROM 
                                                                                {col.LookupColumnTable.Table} 
                                                                            WHERE
                                                                                [{col.LookupColumnTable.ValueColumn}] = @Value"", conn))");
                script.AppendLine("{");
                script.AppendLine($@"     cmd{lookupIndex}.Parameters.AddWithValue(""@Value"", item.{col.LookupClrName});");
                script.AppendLine($@"     var val{lookupIndex} = await cmd{lookupIndex}.ExecuteScalarAsync();");

                script.AppendLine($@"     item.{col.ClrName} = {col.GenerateValueAssignmentCode("val" + lookupIndex)};");
                script.AppendLine("}");
                code.AppendLine(script.ToString());
            }

            foreach (var col in columns)
            {
                var parameterCode = col.GenerateUpdateParameterValue();
                if (!string.IsNullOrWhiteSpace(parameterCode))
                    code.AppendLine(parameterCode);
            }



            if (hasSingleIdentityPrimaryKey)
                code.AppendLine($@"
                var scopeIdentity = await cmd.ExecuteScalarAsync();     
                item.{primaryKeyIdentityColumns[0].ClrName} = Convert.To{primaryKeyIdentityColumns[0].ClrType.Name}(scopeIdentity);
                return item.{primaryKeyIdentityColumns[0].ClrName};");
            else
                code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");

            code.AppendLine("           }");

            code.AppendLine("       }");


            return code.ToString();
        }

        public string GetUpdateCommand(TableDefinition table)
        {
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var primaryKeyNames = pks.Select(x => x.Name).ToArray();
            var columns = table.ColumnCollection.Where(x => !primaryKeyNames.Contains(x.Name));
            var sql = new StringBuilder("UPDATE  ");
            sql.Append($"[{table.Schema}].[{table}] SET ");

            var cols = columns
                .Where(c => c.CanWrite)
                .Where(c => !c.IsPrimaryKey)
                .ToArray();
            sql.JoinAndAppendLine(cols, ",\r\n", c => $"[{c.Name}] = @{c.ClrName}");
            sql.AppendLine(" WHERE ");

            sql.JoinAndAppendLine(pks, "\r\nAND\t\r\n", x => $"[{x.Name}] = @{x.ClrName}");

            return sql.ToString();

        }
        public string GetInsertCommand(TableDefinition table)
        {
            var sql = new StringBuilder("INSERT INTO ");
            sql.Append($"[{table.Schema}].[{table}] (");

            var cols = table.ColumnCollection
                .Where(c => c.CanWrite)
                .ToArray();
            sql.JoinAndAppendLine(cols, ",\r\n\t", c => $"[{c.Name}]");
            sql.AppendLine(")");
            sql.AppendLine("VALUES(");
            sql.JoinAndAppendLine(cols, ",\r\n\t", c => $"@{c.ClrName}");
            sql.Append(")");


            return sql.ToString();

        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            var td = this.TableDefinitionCollection.Last(t => t.Name == table);
            return Task.FromResult(td);
        }

    }

}
