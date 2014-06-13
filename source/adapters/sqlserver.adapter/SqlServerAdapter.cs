using System.Collections.Generic;
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

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class SqlServerAdapter : Adapter
    {
        public override string CodeNamespace
        {
            get { return string.Format("{0}.Adapters.{1}", ConfigurationManager.ApplicationName, this.Schema); }
        }

        public override string OdataTranslator
        {
            get { return "OdataSqlTranslator"; }
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
        private readonly ObjectCollection<TableDefinition> m_tableDefinitionCollection = new ObjectCollection<TableDefinition>();

        public async Task OpenAsync(bool verbose = false)
        {
            foreach (var table in this.Tables)
            {

                var td = new TableDefinition { Name = table.Name , Schema = this.Schema};
                var columns = new ObjectCollection<SqlColumn>();

                var updateCommand = new StringBuilder("UPDATE ");
                updateCommand.AppendFormat("[{0}].[{1}] SET", this.Schema, table.Name);

                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(PkSql, conn))
                {
                    cmd.Parameters.AddWithValue("@Schema", this.Schema);
                    cmd.Parameters.AddWithValue("@Table", table.Name);
                    await conn.OpenAsync();

                    var f = await cmd.ExecuteScalarAsync() as string;
                    td.RecordName = f;
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
                                    Console.Write("{0,-15}\t", reader.GetName(i));
                                }
                                Console.WriteLine();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write("{0,-15}\t", reader[i]);
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
                            col.IsPrimaryKey = col.Name == td.RecordName;
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
                m_tableDefinitionCollection.Add(td);


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
            foreach (var table in this.Tables)
            {
                var td = m_tableDefinitionCollection.Single(t => t.Name == table.Name);
                options.AddReference(typeof(SqlConnection));
                var name = table.Name;
                var adapterName = table + "Adapter";

                var header = this.GetCodeHeader(namespaces);
                var code = new StringBuilder(header);
                var record = td.MemberCollection.Single(m => m.Name == td.RecordName);

                code.AppendLine("   public class " + adapterName);
                code.AppendLine("   {");

                code.AppendLine(GenerateExecuteScalarMethod());
                code.AppendLine(GenerateDeleteMethod(record, name));
                code.AppendLine(GenerateInsertMethod(name));
                code.AppendLine(GenerateUpdateMethod(name));
                code.AppendLine(GenerateSelectMethod(name, record));
                code.AppendLine(GenerateSelectOneMethod(name, record));
                code.AppendLine(GenerateConnectionStringProperty());


                code.AppendLine("   }");// end class
                code.AppendLine("}");// end namespace


                sources.Add(adapterName + ".cs", code.ToString());


            }
            return Task.FromResult(sources);
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
            using (var reader = new StreamReader(stream))
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
            using (var reader = new StreamReader(stream))
            {
                var code = reader.ReadToEnd();
                code = code.Replace("__NAMESPACE__", this.CodeNamespace);
                var source = new Tuple<string, string>("SqlPagingTranslator.cs", code);
                return Task.FromResult(source);

            }
        }

        private string GenerateUpdateMethod(string name)
        {
            var columns = m_tableColumns[name];
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<object> UpdateAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetUpdateCommand(name));
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

        public string GetSelectOneCommand(string table)
        {
            var td = m_tableDefinitionCollection.Single(t => t.Name == table);
            var sql = new StringBuilder("SELECT * FROM ");
            sql.AppendFormat("{0}.{1} ", this.Schema, table);


            sql.AppendFormat("WHERE {0} = @{0}", td.RecordName);

            return sql.ToString();
        }
        public string GetSelectCommand(string table)
        {
            return string.Format("SELECT * FROM {0}.{1} ", this.Schema, table);
        }
        private string GenerateSelectOneMethod(string name, Member record)
        {
            var td = m_tableDefinitionCollection.Single(t => t.Name == name);
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<{0}> LoadOneAsync({1} id)", name, record.Type.ToCSharp());
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetSelectOneCommand(name));
            code.AppendLine("           {");

            code.AppendLinf("               cmd.Parameters.AddWithValue(\"@{0}\", id);", td.RecordName);

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               using(var reader = await cmd.ExecuteReaderAsync())");
            code.AppendLine("               {");
            code.AppendLine("                   while(await reader.ReadAsync())");
            code.AppendLine("                   {");
            code.AppendLinf("                       var item = new {0}();", name);
            code.AppendLine(PopulateItemFromReader(name));
            code.AppendLinf("                       return item;", name);
            code.AppendLine("                   }");
            code.AppendLine("               }");
            code.AppendLine("               return null;");
            code.AppendLine("           }");

            code.AppendLine("       }");

            return code.ToString();
        }

        private string GenerateSelectMethod(string name, Member record)
        {
            var code = new StringBuilder();
            //load async
            code.AppendLinf("       public async Task<LoadOperation<{0}>> LoadAsync(string sql, int page = 1, int size = 40, bool includeTotal = false)", name, record.Type.ToCSharp());
            code.AppendLine("       {");

            code.AppendLine("           if (!sql.ToString().Contains(\"ORDER\"))");
            code.AppendLinf("               sql +=\"\\r\\nORDER BY [{0}]\";", record.Name);
            code.AppendLine("           var translator = new SqlPagingTranslator();");
            code.AppendLine("           sql = translator.Translate(sql, page, size);");
            code.AppendLine();
            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand( sql, conn))", this.GetSelectCommand(name));
            code.AppendLine("           {");

            code.AppendLinf("               var lo = new LoadOperation<{0}>", name);
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
            code.AppendLinf("                       var item = new {0}();", name);
            code.AppendLine(PopulateItemFromReader(name));
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

        public string GetDeleteCommand(string table)
        {
            var td = m_tableDefinitionCollection.Single(t => t.Name == table);
            var sql = new StringBuilder("DELETE FROM ");
            sql.AppendFormat("[{0}].[{1}] ", this.Schema, table);


            sql.AppendFormat("WHERE {0} = @{0}", td.RecordName);

            return sql.ToString();
        }
        private string GenerateDeleteMethod(Member record, string table)
        {
            var td = m_tableDefinitionCollection.Single(t => t.Name == table);
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<int> DeleteAsync({0} id)", record.Type.ToCSharp());
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetDeleteCommand(table));
            code.AppendLine("           {");

            code.AppendLinf("               cmd.Parameters.AddWithValue(\"@{0}\", id);", td.RecordName);

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");
            return code.ToString();
        }


        private string GenerateInsertMethod(string name)
        {
            var code = new StringBuilder();
            var columns = m_tableColumns[name];
            code.AppendLinf("       public async Task<object> InsertAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetInsertCommand(name));
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

        public string GetUpdateCommand(string table)
        {
            var columns = m_tableColumns[table];
            var td = m_tableDefinitionCollection.Single(t => t.Name == table);
            var sql = new StringBuilder("UPDATE  ");
            sql.AppendFormat("[{0}].[{1}] SET ", this.Schema, table);

            var cols = columns
                .Where(c => !c.IsIdentity)
                .Where(c => !c.IsComputed)
                .Select(c => c.Name)
                .ToArray();
            sql.AppendLine(string.Join(",\r\n", cols.Select(c => "[" + c + "] = @" + c).ToArray()));
            sql.AppendLinf(" WHERE [{0}] = @{0}", td.RecordName);

            return sql.ToString();

        }
        public string GetInsertCommand(string table)
        {
            var sql = new StringBuilder("INSERT INTO ");
            sql.AppendFormat("[{0}].[{1}] (", this.Schema, table);


            var cols = m_tableColumns[table]
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
            var td = m_tableDefinitionCollection.Single(t => t.Name == table);
            return Task.FromResult(td);
        }
    }

}
