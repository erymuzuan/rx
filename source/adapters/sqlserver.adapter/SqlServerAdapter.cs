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
                              select new Member
                              {
                                  Name = c.Name,
                                  IsNullable = c.IsNullable,
                                  IsFilterable = true,
                                  Type = c.GetClrType()
                              };
                td.MemberCollection.AddRange(members);

                if (m_tableColumns.ContainsKey(table.Name))
                    m_tableColumns[table.Name] = columns;
                else
                    m_tableColumns.Add(table.Name, columns);

                this.TableDefinitionCollection.Add(td);


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
            header.AppendLine("using " + typeof(SqlConnection).Namespace + ";");
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

        private readonly Dictionary<string, ObjectCollection<SqlColumn>> m_tableColumns = new Dictionary<string, ObjectCollection<SqlColumn>>();


        protected override Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            options.AddReference(typeof(Microsoft.CSharp.RuntimeBinder.Binder));
            var sources = new Dictionary<string, string>();
            var header = this.GetCodeHeader(namespaces);
            foreach (var at in this.Tables)
            {
                var table = this.TableDefinitionCollection.Single(t => t.Name == at.Name);
                options.AddReference(typeof(SqlConnection));
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


                sources.Add(adapterName + ".cs", code.FormatCode());


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
            sources.Add(this.Name + ".sproc.cs", code2.FormatCode());

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
            const string RESOURCE_NAME = "Bespoke.Sph.Integrations.Adapters.Sql2012PagingTranslator.cs";

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

        private string GenerateUpdateMethod(TableDefinition table)
        {
            var name = table.Name;
            var columns = m_tableColumns[name];
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<object> UpdateAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetUpdateCommand(table));
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
            return string.Format("SELECT * FROM {0}.{1} ", this.Schema, table);
        }

        private string GenerateSelectOneMethod(TableDefinition table)
        {

            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var arguments = pks.Select(k => string.Format("{0} {1}", k.Type.ToCSharp(), k.Name));
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<{0}> LoadOneAsync({1})", table.Name, string.Join(", ", arguments));
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetSelectOneCommand(table));
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
            var arguements = pks.Select(k => string.Format("{0} {1}", k.Type.ToCSharp(), k.Name.ToCamelCase()));
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
            var columns = m_tableColumns[name];
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
            var columns = m_tableColumns[table.Name];
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


            var cols = m_tableColumns[table.Name]
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
