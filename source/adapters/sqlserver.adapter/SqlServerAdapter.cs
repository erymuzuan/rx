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
    public partial class SqlServerAdapter : Adapter
    {
        [JsonIgnore]
        [ImportMany("SqlColumn", typeof(SqlColumn), AllowRecomposition = true)]
        public Lazy<SqlColumn, IColumnGeneratorMetadata>[] ColumnGenerators { get; set; }


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

                var code = new Class { Name = $"{table}Adapter", Namespace = this.CodeNamespace };
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



            var addedActions = new List<string>();
            foreach (var op in this.OperationDefinitionCollection.OfType<SprocOperationDefinition>())
            {
                op.CodeNamespace = this.CodeNamespace;
                var methodName = op.MethodName;

                if (addedActions.Contains(methodName)) continue;
                addedActions.Add(methodName);
                webApi.AddMethod(op.GenerateApiCode(this));
                //
                adapterClass.AddMethod(op.GenerateActionCode(this, methodName));

                var requestSources = op.GenerateRequestCode();
                sources.AddRange(requestSources);

                var responseSources = op.GenerateResponseCode();
                sources.AddRange(responseSources);
            }
            sources.Add(adapterClass);
            sources.Add(webApi);

            return Task.FromResult(sources.AsEnumerable());
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
        protected override async Task<Class> GeneratePagingSourceCodeAsync()
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

            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"Bespoke.Sph.Integrations.Adapters.Sql{version}PagingTranslator.cs";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            // ReSharper disable AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
            // ReSharper restore AssignNullToNotNullAttribute
            {
                var code = reader.ReadToEnd();
                code = code.Replace("__NAMESPACE__", this.CodeNamespace);
                var source = new Class(code) { FileName = "SqlPagingTranslator.cs" };
                return source;

            }
        }

        private string GenerateUpdateMethod(TableDefinition table)
        {
            var name = table.Name;
            var columns = table.ColumnCollection;
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<int> UpdateAsync({0} item)", name);
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
                code.AppendLine($"               cmd.Parameters.AddWithValue(\"@{pk}\", item.{pk});");
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
            code.AppendLinf("               var conn = ConfigurationManager.ConnectionStrings[\"{0}\"];", this.Name);
            code.AppendLine("               if(null != conn)return conn.ConnectionString;");
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
                 .Select(x => x.LookupColumnTable)
                 .Select((x, i) => $"t{i + 1}.[{x.ValueColumn}] as '{x.Name}'");
                sql.Append(",");
                sql.AppendLine(lookups.ToString(", "));


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
            var parameters = pks.Select(k => string.Format("t0.[{0}] = @{0}", k.Name));
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
                .Select((x, i) => $" INNER JOIN {x.LookupColumnTable.Table} t{i} ON t0.[{x.Name}] = t1.[{x.LookupColumnTable.KeyColumn}]");
            code.AppendLine(joins.ToString("\r\n"));
            return code.ToString();
        }

        private string GenerateSelectOneMethod(TableDefinition table)
        {

            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var arguments = pks.Select(k => k.GenerateParameterCode());
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<{0}> LoadOneAsync({1})", table.Name, string.Join(", ", arguments));
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
            code.AppendLinf("                       var item = new {0}();", table.Name);
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

            var columns = from c in table.ColumnCollection
                          where !c.IsComplex
                          select c;
            var select = $"SELECT {columns.ToString(",\r\n", x => $"[{x.Name}]")} FROM";
            var code = new StringBuilder();


            //load async
            code.AppendFormat("       public async Task<LoadOperation<{0}>> LoadAsync(string sql, int page = 1, int size = 40, bool includeTotal = false)", table.Name);
            code.AppendLine("       {");

            code.AppendLine("           if (!sql.ToString().Contains(\"ORDER\"))");
            code.AppendLinf("               sql +=\"\\r\\nORDER BY [{0}]\";", table.PrimaryKeyCollection.FirstOrDefault() ?? table.ColumnCollection.Select(m => m.Name).First());
            code.AppendLine("           var translator = new SqlPagingTranslator();");
            code.AppendLine("           sql = translator.Translate(sql, page, size);");
            code.AppendLine($@"           sql = sql.Replace(""SELECT * FROM"", @""{select}"");");

            code.AppendLine();
            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand( sql, conn))", this.GetSelectCommand(table));
            code.AppendLine("           {");

            code.AppendLinf("               var lo = new LoadOperation<{0}>", table.Name);
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
            code.AppendLinf("                       var item = new {0}();", table.Name);
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
                            select $"item.{c.Name} = {c.GenerateValueAssignmentCode($@"reader[""{c.Name}""]")};";
            code.JoinAndAppendLine(readCodes, "\r\n");

            var readLookup = from c in columns
                             where c.LookupColumnTable.IsEnabled
                             select $@"item.{c.LookupColumnTable.Name} = ({c.LookupColumnTable.Type.ToCSharp()})reader[""{c.LookupColumnTable.Name}""];";
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
            var name = table.Name;
            var code = new StringBuilder();
            var columns = table.ColumnCollection;
            var primaryKeyIdentityColumns = columns.Where(x => x.IsPrimaryKey && x.IsIdentity).ToList();
            var sql = this.GetInsertCommand(table);
            var hasSingleIdentityPrimaryKey = primaryKeyIdentityColumns.Count == 1;
            if (hasSingleIdentityPrimaryKey)
            {
                code.AppendLine($"       public async Task<{primaryKeyIdentityColumns[0].ClrType.ToCSharp()}> InsertAsync({name} item)");
                sql += "; SELECT Scope_Identity()";
            }
            else
                code.AppendLinf("       public async Task<int> InsertAsync({0} item)", name);

            code.AppendLine("       {");

            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");

            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", sql);
            code.AppendLine("           {");
            foreach (var col in columns)
            {
                var parameterCode = col.GenerateUpdateParameterValue();
                if (!string.IsNullOrWhiteSpace(parameterCode))
                    code.AppendLine(parameterCode);
            }
            code.AppendLine("               await conn.OpenAsync();");
            if (hasSingleIdentityPrimaryKey)
                code.AppendLine($@"
                var scopeIdentity = await cmd.ExecuteScalarAsync();     
                item.{primaryKeyIdentityColumns[0].Name} = Convert.To{primaryKeyIdentityColumns[0].ClrType.Name}(scopeIdentity);
                return item.{primaryKeyIdentityColumns[0].Name};");
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
                .Select(c => c.Name)
                .ToArray();
            sql.JoinAndAppendLine(cols, ",\r\n", c => $"[{c}] = @{c}");
            sql.AppendLine(" WHERE ");

            sql.JoinAndAppendLine(pks, "\r\nAND\t\r\n", x => $"[{x.Name}] = @{x.Name}");

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
            sql.JoinAndAppendLine(cols, ",\r\n\t", c => $"@{c.Name}");
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
