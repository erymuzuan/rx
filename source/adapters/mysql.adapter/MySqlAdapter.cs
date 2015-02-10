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
    [DesignerMetadata(Name = "MySql database", PngIcon = "~/images/mysql-24-black.png", RouteTableProvider = typeof(MySqlServerRouteProvider), Route = "adapter.mysql/0")]
    public class MySqlAdapter : Adapter
    {
        private string m_connectionString;

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
                              select new Member
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


        private string GetCodeHeader(params string[] namespaces)
        {

            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(Int32).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine("using " + typeof(IEnumerable<>).Namespace + ";");
            header.AppendLine("using " + typeof(StringBuilder).Namespace + ";");
            header.AppendLine("using " + typeof(MySqlConnection).Namespace + ";");
            header.AppendLine("using " + typeof(CommandType).Namespace + ";");
            header.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");
            header.AppendLine("using System.Web.Mvc;");
            header.AppendLine("using Bespoke.Sph.Web.Helpers;");
            foreach (var ns in namespaces)
            {
                header.AppendLinf("using {0};", ns);
            }
            header.AppendLine();

            header.AppendLine("namespace " + this.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }

        protected override Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            //options.AddReference(typeof(Microsoft.CSharp.RuntimeBinder.Binder));
            //options.AddReference(typeof(MySqlConnection));
            var sources = new Dictionary<string, string>();
            var header = this.GetCodeHeader(namespaces);
            foreach (var at in this.Tables)
            {
                var table = this.TableDefinitionCollection.Single(t => t.Name == at.Name);
                var adapterName = table + "Adapter";

                var code = new StringBuilder(header);

                code.AppendLine("   public class " + adapterName);
                code.AppendLine("   {");

                code.AppendLine(GenerateExecuteScalarMethod());
                code.AppendLine(GenerateDeleteMethod(table));
                code.AppendLine(GenerateInsertMethod(table));
                code.AppendLine(GenerateUpdateMethod(table));
                code.AppendLine(GenerateSelectMethod(table));
                code.AppendLine(GenerateSelectOneMethod(table));
                code.AppendLine(GenerateConnectionStringProperty());


                code.AppendLine("   }");// end class
                code.AppendLine("}");// end namespace


                sources.Add(adapterName + ".cs", code.ToString());


            }

            var code2 = new StringBuilder(header);

            code2.AppendLine("   public partial class " + this.Name + "");
            code2.AppendLine("   {");

            code2.AppendLine("      public string ConnectionString{set;get;}");

            var addedActions = new List<string>();
            foreach (var op in this.OperationDefinitionCollection.OfType<SprocOperationDefinition>())
            {
                op.CodeNamespace = this.CodeNamespace;
                var methodName = op.MethodName;

                if (addedActions.Contains(methodName)) continue;
                addedActions.Add(methodName);

                //
                code2.AppendLine(op.GenerateActionCode(this, methodName));

                var requestSources = op.GenerateRequestCode();
                AddSources(requestSources, sources);

                var responseSources = op.GenerateResponseCode();
                AddSources(responseSources, sources);
            }



            code2.AppendLine("   }");// end class
            code2.AppendLine("}");// end namespace
            sources.Add(this.Name + ".sproc.cs", code2.ToString());

            return Task.FromResult(sources);
        }


        private static void AddSources(Dictionary<string, string> classes, Dictionary<string, string> sources)
        {
            foreach (var cs in classes.Keys)
            {
                if (!sources.ContainsKey(cs))
                {
                    sources.Add(cs, classes[cs]);
                    continue;
                }
                if (sources[cs] != classes[cs])
                    throw new InvalidOperationException("You are generating 2 different sources for " + cs);
            }
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
            sql.AppendFormat("`{0}`.`{1}` ", this.Schema, table);
            sql.AppendLine("WHERE ");
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name));
            var parameters = pks.Select(k => string.Format("`{0}` = @{0}", k.Name));


            sql.AppendLine(string.Join(" AND ", parameters));

            return sql.ToString();
        }

        private string GenerateSelectOneMethod(TableDefinition table)
        {

            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var arguments = pks.Select(k => string.Format("{0} {1}", k.Type.ToCSharp(), k.Name));
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<{0}> LoadOneAsync({1})", table.Name, string.Join(", ", arguments));
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new MySqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new MySqlCommand(@\"{0}\", conn))", this.GetSelectOneCommand(table));
            code.AppendLine("           {");
            foreach (var pk in pks)
            {

                code.AppendLinf("               cmd.Parameters.AddWithValue(\"@{0}\", {0});", pk.Name);
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
            code.AppendLinf("               sql +=\"\\r\\nORDER BY `{0}`\";", table.PrimaryKeyCollection.FirstOrDefault() ?? table.MemberCollection.Select(m => m.Name).First());
            code.AppendLine("           var translator = new MySqlPagingTranslator();");
            code.AppendLine("           sql = translator.Translate(sql, page, size);");
            code.AppendLine();
            code.AppendLinf("           using(var conn = new MySqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new MySqlCommand( sql, conn))", this.GetSelectCommand(table));
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

        public string GetSelectCommand(TableDefinition table)
        {
            return string.Format("SELECT * FROM `{0}`.`{1}` ", this.Schema, table);
        }

        private string PopulateItemFromReader(string name)
        {
            var columns = m_tableColumns[name];
            var code = new StringBuilder();
            foreach (var column in columns)
            {
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
            var arguements = pks.Select(k => string.Format("{0} {1}", k.Type.ToCSharp(), k.Name.ToCamelCase()));
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

        protected override Task<Tuple<string, string>> GenerateOdataTranslatorSourceCodeAsync()
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
                var source = new Tuple<string, string>("OdataSqlTranslator.cs", code);
                return Task.FromResult(source);

            }
        }

        protected override Task<Tuple<string, string>> GeneratePagingSourceCodeAsync()
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
                var source = new Tuple<string, string>("SqlPagingTranslator.cs", code);
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
                        var col = new Member
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


        public override string OdataTranslator
        {
            get { return "OdataSqlTranslator"; }
        }

        public string ConnectionString
        {
            get
            {

                if (string.IsNullOrWhiteSpace(m_connectionString))
                    m_connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};Allow User Variables=true;", this.Server, this.Database, this.UserId, this.Password);
                return m_connectionString;
            }
            set { m_connectionString = value; }
        }

        public string Password { get; set; }
        public string UserId { get; set; }
        public string Server { get; set; }

        public string Database { get; set; }

        private readonly ObjectCollection<OperationDefinition> m_operationDefinitionCollection = new ObjectCollection<OperationDefinition>();

        public new ObjectCollection<OperationDefinition> OperationDefinitionCollection
        {
            get
            {
                return m_operationDefinitionCollection;
            }
        }

        public override string DefaultNamespace
        {
            get { throw new NotImplementedException(); }
        }

        public override Task<IEnumerable<Class>> GenerateCodeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
