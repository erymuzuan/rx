using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Integrations.Adapters.Properties;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class SqlServerAdapter : Adapter
    {

        public override string OdataTranslator => "OdataSqlTranslator";

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var vr = (await base.ValidateAsync()).ToList();

            if (string.IsNullOrWhiteSpace(this.Schema))
                vr.Add("Schema", "Schema cannot be empty");

            if (string.IsNullOrWhiteSpace(this.Database))
                vr.Add("Database", "Database cannot be empty");


            return vr.AsEnumerable();
        }


        private const string PkSql = @"
SELECT  
    B.COLUMN_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS A, 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE B
WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND A.CONSTRAINT_NAME = B.CONSTRAINT_NAME
    AND A.TABLE_NAME = @Table
    AND A.CONSTRAINT_SCHEMA = @Schema";

        private const string Sql = @"SELECT 
        '[' + s.name + '].[' + o.name + ']' as 'Table'
        ,c.name as 'Column'
        ,t.name as 'Type' 
        ,c.max_length as 'length'
        ,c.is_nullable as 'IsNullable'    
	    ,c.is_identity as 'IsIdentity'
        ,c.is_computed as 'IsComputed'
    FROM 
        sys.objects o INNER JOIN sys.all_columns c
        ON c.object_id = o.object_id
        INNER JOIN sys.types t 
        ON c.system_type_id = t.system_type_id
        INNER JOIN sys.schemas s
        ON s.schema_id = o.schema_id
    WHERE 
        o.type = 'U'
        AND s.name = @Schema
        AND o.Name = @Table
        AND t.name <> N'sysname'
    ORDER 
        BY o.type";

        public override async Task OpenAsync(bool verbose = false)
        {
            this.TableDefinitionCollection.Clear();
            TableColumns.Clear();

            foreach (var table in this.Tables)
            {

                var td = new TableDefinition { Name = table.Name, Schema = this.Schema };
                var columns = new ObjectCollection<SqlColumn>();

                var updateCommand = new StringBuilder("UPDATE ");
                updateCommand.AppendFormat("[{0}].[{1}] SET", this.Schema, table.Name);

                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(PkSql, conn))
                {
                    cmd.Parameters.AddWithValue("@Schema", this.Schema);
                    cmd.Parameters.AddWithValue("@Table", table.Name);
                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            td.PrimaryKeyCollection.Add(reader.GetString(0));
                        }
                    }
                }
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(Sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Schema", this.Schema);
                    cmd.Parameters.AddWithValue("@Table", table.Name);
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var first = true;
                        while (await reader.ReadAsync())
                        {
                            if (first && verbose)
                            {
                                WriteTableHeader(reader);
                            }
                            first = false;
                            var col = new SqlColumn
                            {
                                Name = reader.GetString(1),
                                DataType = reader.GetString(2),
                                Length = reader.GetInt16(3),
                                IsNullable = reader.GetBoolean(4),
                                IsIdentity = reader.GetBoolean(5),
                                IsComputed = reader.GetBoolean(6)
                            };
                            col.IsPrimaryKey = td.PrimaryKeyCollection.Contains(col.Name);
                            if (null != col.GetClrType())
                                columns.Add(col);

                        }
                    }
                }

                var members = from c in columns
                              select new SimpleMember
                              {
                                  Name = c.Name,
                                  IsNullable = c.IsNullable,
                                  IsFilterable = true,
                                  Type = c.GetClrType()
                              };
                td.MemberCollection.AddRange(members);

                if (TableColumns.Any(x => x.Name == table.Name))
                    TableColumns.Single(x => x.Name == table.Name).ColumnCollection.ClearAndAddRange(columns);
                else
                {
                    var sqlTable = new SqlTable { Name = table.Name };
                    sqlTable.ColumnCollection.AddRange(columns);
                    TableColumns.Add(sqlTable);
                }

                this.TableDefinitionCollection.Add(td);

            }
        }

        private static void WriteTableHeader(SqlDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(Resources.Format15Tab, reader.GetName(i));
            }
            Console.WriteLine();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(Resources.Format15Tab, reader[i]);
            }
        }


        public static readonly string[] ImportDirectives =
        {
               typeof(Entity).Namespace ,
               typeof(Int32).Namespace ,
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
            foreach (var at in this.Tables)
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

                sources.Add(code);


            }

            var code2 = new Class { Name = Name, Namespace = CodeNamespace };
            code2.AddProperty("      public string ConnectionString{set;get;}");

            var addedActions = new List<string>();
            foreach (var op in this.OperationDefinitionCollection.OfType<SprocOperationDefinition>())
            {
                op.CodeNamespace = this.CodeNamespace;
                var methodName = op.MethodName;

                if (addedActions.Contains(methodName)) continue;
                addedActions.Add(methodName);

                //
                code2.AddMethod(op.GenerateActionCode(this, methodName));

                var requestSources = op.GenerateRequestCode();
                sources.AddRange(requestSources);

                var responseSources = op.GenerateResponseCode();
                sources.AddRange(responseSources);
            }




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
            var columns = TableColumns.Single(x => x.Name == name).ColumnCollection;
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<object> UpdateAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetUpdateCommand(table));
            code.AppendLine("           {");
            foreach (var col in columns.Where(c => !c.IsIdentity).Where(c => !c.IsComputed))
            {
                var nullable = col.IsNullable ? ".ToDbNull()" : "";
                code.AppendLine($"               cmd.Parameters.AddWithValue(\"@{col.Name}\", item.{col.Name}{nullable});");
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
            var sql = new StringBuilder("SELECT * FROM ");
            sql.AppendFormat("{0}.{1} ", this.Schema, table);
            sql.AppendLine("WHERE ");
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name));
            var parameters = pks.Select(k => string.Format("[{0}] = @{0}", k.Name));


            sql.AppendLine(string.Join(" AND ", parameters));

            return sql.ToString();
        }
        public string GetSelectCommand(TableDefinition table)
        {
            return $"SELECT * FROM {this.Schema}.{table} ";
        }

        private string GenerateSelectOneMethod(TableDefinition table)
        {

            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
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
            var code = new StringBuilder();


            //load async
            code.AppendFormat("       public async Task<LoadOperation<{0}>> LoadAsync(string sql, int page = 1, int size = 40, bool includeTotal = false)", table.Name);
            code.AppendLine("       {");

            code.AppendLine("           if (!sql.ToString().Contains(\"ORDER\"))");
            code.AppendLinf("               sql +=\"\\r\\nORDER BY [{0}]\";", table.PrimaryKeyCollection.FirstOrDefault() ?? table.MemberCollection.Select(m => m.Name).First());
            code.AppendLine("           var translator = new SqlPagingTranslator();");
            code.AppendLine("           sql = translator.Translate(sql, page, size);");
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
            var columns = TableColumns.Single(x => x.Name == name).ColumnCollection;
            var code = new StringBuilder();
            foreach (var column in columns)
            {

                if (column.IsNullable)
                {
                    var type = column.GetClrType();
                    if (type == typeof(string))
                    {
                        code.AppendLinf("                       item.{0} = reader[\"{0}\"].ReadNullableString();", column.Name);
                    }
                    else if (type == typeof(byte[]))
                    {
                        code.AppendLinf("                       item.{0} = reader[\"{0}\"].ReadNullableByteArray();", column.Name);
                    }
                    else
                    {
                        code.AppendLinf("                       item.{0} = reader[\"{0}\"].ReadNullable<{1}>();", column.Name, column.GetCSharpType());
                    }
                    continue;
                }

                code.AppendLinf("                       item.{0} = ({1})reader[\"{0}\"];", column.Name, column.GetCSharpType());
            }

            return code.ToString();
        }

        public string GetDeleteCommand(TableDefinition table)
        {
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var parameters = pks.Select(k => string.Format("[{0}] = @{0}", k.Name));
            var sql = new StringBuilder("DELETE FROM ");
            sql.AppendFormat("[{0}].[{1}] ", this.Schema, table);
            sql.AppendLine("WHERE");
            sql.AppendLine(string.Join(" AND ", parameters));


            return sql.ToString();
        }
        private string GenerateDeleteMethod(TableDefinition table)
        {
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
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
            var columns = TableColumns.Single(x => x.Name == name).ColumnCollection;
            code.AppendLinf("       public async Task<object> InsertAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetInsertCommand(table));
            code.AppendLine("           {");
            foreach (var col in columns.Where(c => !c.IsIdentity).Where(c => !c.IsComputed))
            {
                var nullable = col.IsNullable ? ".ToDbNull()" : "";
                code.AppendLinf("               cmd.Parameters.AddWithValue(\"@{0}\", item.{0}{1});", col.Name, nullable);
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");
            return code.ToString();
        }

        public string GetUpdateCommand(TableDefinition table)
        {
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name));
            var parameters = pks.Select(m => string.Format("[{0}] = @{0}", m.Name));
            var columns = TableColumns.Single(x => x.Name == table.Name).ColumnCollection;
            var sql = new StringBuilder("UPDATE  ");
            sql.AppendFormat("[{0}].[{1}] SET ", this.Schema, table);

            var cols = columns
                .Where(c => !c.IsIdentity)
                .Where(c => !c.IsComputed)
                .Select(c => c.Name)
                .ToArray();
            sql.AppendLine(string.Join(",\r\n", cols.Select(c => string.Format("[{0}] = @{0}", c)).ToArray()));
            sql.AppendLine(" WHERE ");

            sql.AppendLine(string.Join(", ", parameters));

            return sql.ToString();

        }
        public string GetInsertCommand(TableDefinition table)
        {
            var sql = new StringBuilder("INSERT INTO ");
            sql.AppendFormat("[{0}].[{1}] (", this.Schema, table);


            var cols = TableColumns.Single(x => x.Name == table.Name).ColumnCollection
                .Where(c => !c.IsIdentity)
                .Where(c => !c.IsComputed)
                .Select(c => c.Name)
                .ToArray();
            sql.AppendLine(string.Join(",\r\n", cols.Select(c => "[" + c + "]").ToArray()));
            sql.AppendLine(")");
            sql.AppendLine("VALUES(");
            sql.AppendLine(string.Join(",\r\n", cols.Select(c => "@" + c).ToArray()));
            sql.AppendLine(")");

            return sql.ToString();

        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            var td = this.TableDefinitionCollection.Last(t => t.Name == table);
            return Task.FromResult(td);
        }


    }

}
