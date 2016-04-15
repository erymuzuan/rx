using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Integrations.Adapters.Properties;
using MySql.Data.MySqlClient;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(Adapter))]
    [Export("AdapterDesigner", typeof(Adapter))]
    [DesignerMetadata(Name = "MySql database", PngIcon = "/images/mysql-24-black.png", RouteTableProvider = typeof(MySqlServerRouteProvider), Route = "adapter.mysql/0")]
    public class MySqlAdapter : Adapter
    {
        public MySqlAdapter()
        {
            
        }

        public MySqlAdapter( string database,string server = "localhost", string userId = "root",  string password = null)
        {
            this.Database = database;
            this.Schema = database;
            this.UserId = userId;
            this.Password = password;
            this.Server = server;
        }
        public override async Task OpenAsync(bool verbose = false)
        {
            this.TableDefinitionCollection.Clear();
            foreach (var table in this.Tables)
            {

                var td = new TableDefinition(table) { Schema = this.Schema };
                var columns = new ObjectCollection<SqlColumn>();

                var keys = await GetPrimaryKeysAsync(table);
                td.PrimaryKeyCollection.ClearAndAddRange(keys);

                using (var conn = new MySqlConnection(ConnectionString))
                using (var cmd = new MySqlCommand("describe " + table.Name, conn))
                {
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var first = true;
                        while (await reader.ReadAsync())
                        {
                            if (first && verbose)
                            {
                                WriteResultHeader(reader);
                            }
                            first = false;
                            var col = new SqlColumn(reader);
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
                m_tableColumns.Add(table.Name, columns);
                this.TableDefinitionCollection.Add(td);


            }
        }

        private async Task<string[]> GetPrimaryKeysAsync(AdapterTable table)
        {
            var keys = new List<string>();
            using (var conn = new MySqlConnection(ConnectionString))
            using (var cmd = new MySqlCommand("describe " + table.Name, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (reader.GetString(3) == "PRI")
                            keys.Add(reader.GetString(0));
                    }
                }
            }

            return keys.ToArray();
        }

        private static void WriteResultHeader(IDataRecord reader)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(Resources.Tab15, reader.GetName(i));
            }
            Console.WriteLine();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(Resources.Tab15, reader[i]);
            }
        }


        public static readonly string[] ImportDirectives =
        {

   typeof(Entity).Namespace,
   typeof(Int32).Namespace,
   typeof(Task<>).Namespace ,
   typeof(Enumerable).Namespace ,
   typeof(IEnumerable<>).Namespace,
   typeof(StringBuilder).Namespace ,
   typeof(MySqlConnection).Namespace,
   typeof(CommandType).Namespace ,
   typeof(XmlAttributeAttribute).Namespace ,
   "System.Web.Mvc",
  "Bespoke.Sph.Web.Helpers"

        };

        protected override Task<IEnumerable<Class>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            options.AddReference(typeof(Microsoft.CSharp.RuntimeBinder.Binder));
            options.AddReference(typeof(MySqlConnection));
            var sources = new ObjectCollection<Class>();
            foreach (var at in this.Tables)
            {
                var table = this.TableDefinitionCollection.Single(t => t.Name == at.Name);
                var code = new Class { Name = $"{table}Adapter", Namespace = CodeNamespace };
                code.ImportCollection.AddRange(namespaces);
                code.ImportCollection.AddRange(ImportDirectives);
                sources.Add(code);

                code.AddMethod(GenerateExecuteScalarMethod());
                code.AddMethod(GenerateDeleteMethod(table));
                code.AddMethod(GenerateInsertMethod(table));
                code.AddMethod(GenerateUpdateMethod(table));
                code.AddMethod(GenerateSelectMethod(table));
                code.AddMethod(GenerateSelectOneMethod(table));
                code.AddMethod(GenerateConnectionStringProperty());

            }

            var code2 = new Class { Name = Name, Namespace = CodeNamespace, FileName = $"{Name}.sproc.cs" };
            code2.AddProperty("ConnectionString", typeof(string));

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
            sql.Append($"`{Schema}`.`{table}` ");
            sql.AppendLine("WHERE ");
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name));
            var parameters = pks.Select(k => $"`{k.Name}` = @{k.Name}");


            sql.AppendLine(string.Join(" AND ", parameters));

            return sql.ToString();
        }

        private string GenerateSelectOneMethod(TableDefinition table)
        {

            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var arguments = pks.Select(k => k.GenerateParameterCode());
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<{0}> LoadOneAsync({1})", table.Name, string.Join(", ", arguments));
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new MySqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new MySqlCommand(@\"{0}\", conn))", this.GetSelectOneCommand(table));
            code.AppendLine("           {");
            foreach (var pk in pks)
            {
                code.AppendLine($"               cmd.Parameters.AddWithValue(\"@{pk.Name}\", {pk.Name.ToCamelCase()});");
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
            code.AppendLine($"       public async Task<LoadOperation<{table.Name}>> LoadAsync(string sql, int page = 1, int size = 40, bool includeTotal = false)");
            code.AppendLine("       {");

            code.AppendLine("           if (!sql.ToString().Contains(\"ORDER\"))");
            var pk = table.PrimaryKeyCollection.FirstOrDefault() ?? table.MemberCollection.Select(m => m.Name).First();
            code.AppendLine($"               sql +=\"\\r\\nORDER BY `{pk}`\";");
            code.AppendLine("           var translator = new MySqlPagingTranslator();");
            code.AppendLine("           sql = translator.Translate(sql, page, size);");
            code.AppendLine();
            code.AppendLinf("           using(var conn = new MySqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new MySqlCommand( sql, conn))", this.GetSelectCommand(table));
            code.AppendLine("           {");

            code.AppendLine($"               var lo = new LoadOperation<{table.Name}>");
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
            code.AppendLine($"                       var item = new {table.Name}();");
            code.AppendLine(PopulateItemFromReader(table.Name));
            code.AppendLinf("                       lo.ItemCollection.Add(item);");
            code.AppendLine("                   }");
            code.AppendLine("               }");
            code.AppendLine("               return lo;");
            code.AppendLine("           }");

            code.AppendLine("       }");

            return code.ToString();
        }

        public string GetSelectCommand(TableDefinition table)
        {
            return $"SELECT * FROM `{this.Schema}`.`{table}` ";
        }

        private string PopulateItemFromReader(string name)
        {
            var columns = m_tableColumns[name];
            var code = new StringBuilder();
            var count = -1;
            foreach (var column in columns)
            {
                count++;
                if (column.IsNullable && column.GetClrType() != typeof(string))
                {
                    code.AppendLinf("                       item.{0} = reader[\"{0}\"].ReadNullable<{1}>();", column.Name,
                        column.GetCSharpType());
                    continue;
                }
                if (column.IsNullable && column.GetClrType() == typeof(string))
                {
                    code.AppendLinf("                       item.{0} = reader[\"{0}\"].ReadNullableString();", column.Name);
                    continue;
                }
                if (column.GetClrType() == typeof(DateTime) && column.IsNullable)
                {
                    code.AppendLine($"                       var __temp{count} = reader[\"{column.Name}\"];");
                    code.AppendLine($"                       if( __temp{count} != DBNull.Value)");
                    code.AppendLine("                       {");
                    code.AppendLine($"                          var __val{count} = (MySql.Data.Types.MySqlDateTime)__temp{count};");
                    code.AppendLine($"                          if(__val{count}.IsValidDateTime ) item.{column.Name} = __val{count}.GetDateTime();");
                    code.AppendLine("                       }");
                    continue;
                }
                if (column.GetClrType() == typeof(DateTime))
                {
                    code.AppendLine($"                       var __type{count} = reader.GetFieldType({count});");
                    code.AppendLine($"                       if(__type{count} == typeof(DateTime))");
                    code.AppendLine("                       {");
                    code.AppendLine($"                          item.{column.Name} = reader.GetDateTime({count});");
                    code.AppendLine("                       }");
                    code.AppendLine("                       else");
                    code.AppendLine("                       {");
                    code.AppendLine($"                          var __temp{count} = (MySql.Data.Types.MySqlDateTime)reader[\"{column.Name}\"];");
                    code.AppendLine($"                          if(__temp{count}.IsValidDateTime ) item.{column.Name} = __temp{count}.GetDateTime();");
                    code.AppendLine("                       }");
                    continue;
                }
                code.AppendLinf("                       item.{0} = ({1})reader[\"{0}\"];", column.Name, column.GetCSharpType());
            }

            return code.ToString();
        }

        private string GenerateUpdateMethod(TableDefinition table)
        {
            var name = table.Name;
            var columns = m_tableColumns[name];
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<object> UpdateAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new MySqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new MySqlCommand(@\"{0}\", conn))", this.GetUpdateCommand(table));
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

        private string GenerateExecuteScalarMethod()
        {
            var code = new StringBuilder();
            code.AppendLine("       public async Task<T> ExecuteScalarAsync<T>(string sql)");
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new MySqlConnection(this.ConnectionString))");
            code.AppendLine("           using(var cmd = new MySqlCommand(sql, conn))");
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

        public string GetDeleteCommand(TableDefinition table)
        {
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var parameters = pks.Select(k => string.Format("`{0}` = @{0}", k.Name));
            var sql = new StringBuilder("DELETE FROM ");
            sql.AppendFormat("{0}.{1} ", this.Schema, table);
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

            code.AppendLinf("           using(var conn = new MySqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new MySqlCommand(@\"{0}\", conn))", this.GetDeleteCommand(table));
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
        private readonly Dictionary<string, ObjectCollection<SqlColumn>> m_tableColumns = new Dictionary<string, ObjectCollection<SqlColumn>>();


        private string GenerateInsertMethod(TableDefinition table)
        {
            var name = table.Name;
            var code = new StringBuilder();
            var columns = m_tableColumns[name];
            code.AppendLinf("       public async Task<object> InsertAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLine("           using(var conn = new MySqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new MySqlCommand(@\"{0}\", conn))", this.GetInsertCommand(table));
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
            var parameters = pks.Select(m => string.Format("`{0}` = @{0}", m.Name));
            var columns = m_tableColumns[table.Name];
            var sql = new StringBuilder("UPDATE  ");
            sql.AppendFormat("`{0}`.`{1}` SET ", this.Schema, table);

            var cols = columns
                .Where(c => !c.IsIdentity)
                .Where(c => !c.IsComputed)
                .Select(c => c.Name)
                .ToArray();
            sql.AppendLine(string.Join(",\r\n", cols.Select(c => string.Format("`{0}` = @{0}", c)).ToArray()));
            sql.AppendLine(" WHERE ");

            sql.AppendLine(string.Join(", ", parameters));

            return sql.ToString();

        }
        public string GetInsertCommand(TableDefinition table)
        {

            var sql = new StringBuilder("INSERT INTO ");
            sql.AppendFormat("`{0}`.`{1}` (", this.Schema, table);


            var cols = m_tableColumns[table.Name]
                .Where(c => !c.IsIdentity)
                .Where(c => !c.IsComputed)
                .Select(c => c.Name)
                .ToArray();
            sql.AppendLine(string.Join(",\r\n", cols.Select(c => "`" + c + "`").ToArray()));
            sql.AppendLine(")");
            sql.AppendLine("VALUES(");
            sql.AppendLine(string.Join(",\r\n", cols.Select(c => "@" + c).ToArray()));
            sql.AppendLine(")");

            return sql.ToString();

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

        protected override Task<Class> GeneratePagingSourceCodeAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "Bespoke.Sph.Integrations.Adapters.MySqlPagingTranslator.cs";

            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
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

        protected override async Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            using (var conn = new MySqlConnection(this.ConnectionString))
            using (var cmd = new MySqlCommand("describe " + table, conn))
            {

                await conn.OpenAsync();
                var td = new TableDefinition
                {
                    Name = table,
                    Schema = this.Database
                };
                var jtb = this.Tables.Single(t => t.Name == table);
                td.ChildTableCollection.ClearAndAddRange(from a in jtb.ChildRelationCollection
                                                         select new TableDefinition
                                                         {
                                                             Name = a.Table,
                                                             CodeNamespace = this.CodeNamespace,
                                                             Schema = this.Schema
                                                         });
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var col = new SimpleMember
                        {
                            Name = reader.GetString(0),
                            Type = reader.GetString(1).GetClrDataType(),
                            IsNullable = reader.GetString(2) != "NO"

                        };
                        if (reader.GetString(3) == "PRI")
                        {
                            td.PrimaryKeyCollection.Add(reader.GetString(0));
                        }
                        td.MemberCollection.Add(col);
                    }
                }


                return td;

            }
        }


        public override string OdataTranslator => "OdataSqlTranslator";

        public string ConnectionString
        {
            get
            {
                return $"Server={this.Server};Database={this.Database};Uid={this.UserId};Pwd={this.Password};Allow User Variables=true;Allow Zero Datetime=true;";
            }
        }

        public string Password { get; set; }
        public string UserId { get; set; }
        public string Server { get; set; }

        public string Database { get; set; }

        public new ObjectCollection<OperationDefinition> OperationDefinitionCollection { get; } = new ObjectCollection<OperationDefinition>();
    }
}
