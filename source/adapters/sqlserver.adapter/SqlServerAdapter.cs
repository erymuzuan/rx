using System.Collections.Generic;
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
    public class SqlServerAdapter : Adapter
    {
        public string ConnectionString { get; set; }

        public override string CodeNamespace
        {
            get { return string.Format("{0}.Adapters.{1}", ConfigurationManager.ApplicationName, this.Schema); }
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
        private TableDefinition m_ed;

        public async Task OpenAsync(bool verbose = false)
        {
            m_ed = new TableDefinition { Name = this.Table };
            m_columnCollection = new ObjectCollection<SqlColumn>();

            var updateCommand = new StringBuilder("UPDATE ");
            updateCommand.AppendFormat("[{0}].[{1}] SET", this.Schema, this.Table);

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(PkSql, conn))
            {
                cmd.Parameters.AddWithValue("@Schema", this.Schema);
                cmd.Parameters.AddWithValue("@Table", this.Table);
                await conn.OpenAsync();

                var f = await cmd.ExecuteScalarAsync() as string;
                m_ed.RecordName = f;
            }
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(Sql, conn))
            {
                cmd.Parameters.AddWithValue("@Schema", this.Schema);
                cmd.Parameters.AddWithValue("@Table", this.Table);
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
                        col.IsPrimaryKey = col.Name == m_ed.RecordName;
                        if (null != col.GetClrType())
                            m_columnCollection.Add(col);

                    }
                }
            }

            var members = from c in m_columnCollection
                          select new Member
                          {
                              Name = c.Name,
                              IsNullable = c.IsNullable,
                              IsFilterable = true,
                              Type = c.GetClrType()
                          };
            m_ed.MemberCollection.AddRange(members);


        }




        private string GetCodeHeader(params string[] namespaces)
        {

            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(Int32).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine("using " + typeof(IEnumerable<>).Namespace + ";");
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

        private ObjectCollection<SqlColumn> m_columnCollection;


        protected override Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            options.AddReference(typeof(SqlConnection));
            var name = this.Table;
            var adapterName = this.Table + "Adapter";
            var sources = new Dictionary<string, string>();

            var header = this.GetCodeHeader(namespaces);
            var code = new StringBuilder(header);
            var record = m_ed.MemberCollection.Single(m => m.Name == m_ed.RecordName);

            code.AppendLine("   public class " + adapterName);
            code.AppendLine("   {");

            code.AppendLine(GenerateDeleteMethod(record));
            code.AppendLine(GenerateInsertMethod(name));
            code.AppendLine(GenerateUpdateMethod(name));
            code.AppendLine(GenerateSelectMethod(name, record));
            code.AppendLine(GenerateSelectOne(name, record));
            code.AppendLine(GenerateConnectionStringProperty());


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace


            sources.Add(adapterName + ".cs", code.ToString());

            return Task.FromResult(sources);

        }

        private string GenerateUpdateMethod(string name)
        {
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<object> UpdateAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetUpdateCommand());
            code.AppendLine("           {");
            foreach (var col in m_columnCollection.Where(c => !c.IsIdentity).Where(c => !c.IsComputed))
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
        public string GetSelectOneCommand()
        {
            var sql = new StringBuilder("SELECT * FROM ");
            sql.AppendFormat("{0}.{1} ", this.Schema, this.Table);


            sql.AppendFormat("WHERE {0} = :{0}", m_ed.RecordName);

            return sql.ToString();
        }
        public string GetSelectCommand()
        {
            return string.Format("SELECT * FROM {0}.{1} ", this.Schema, this.Table);
        }
        private string GenerateSelectOne(string name, Member record)
        {
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<{0}> LoadOneAsync({1} id)", name, record.Type.ToCSharp());
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetSelectOneCommand());
            code.AppendLine("           {");

            code.AppendLinf("               cmd.Parameters.Add(\"{0}\", id);", m_ed.RecordName);

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               using(var reader = await cmd.ExecuteReaderAsync())");
            code.AppendLine("               {");
            code.AppendLine("                   while(await reader.ReadAsync())");
            code.AppendLine("                   {");
            code.AppendLinf("                       var item = new {0}();", name);
            foreach (var column in m_columnCollection)
            {
                if (column.IsNullable && column.GetClrType() != typeof(string))
                    code.AppendLinf("                       item.{0} = Convert<{1}>(reader[\"{0}\"]);", column.Name, column.GetCSharpType());
                else
                    code.AppendLinf("                       item.{0} = ({1})reader[\"{0}\"];", column.Name, column.GetCSharpType());
            }
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
            code.AppendLinf("       public async Task<IEnumerable<{0}>> LoadAsync(string filter)", name, record.Type.ToCSharp());
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0} \" + filter, conn))", this.GetSelectCommand());
            code.AppendLine("           {");

            code.AppendLinf("               var list = new List<{0}>();", name);

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               using(var reader = await cmd.ExecuteReaderAsync())");
            code.AppendLine("               {");
            code.AppendLine("                   while(await reader.ReadAsync())");
            code.AppendLine("                   {");
            code.AppendLinf("                       var item = new {0}();", name);
            foreach (var column in m_columnCollection)
            {
                if (column.IsNullable && column.GetClrType() != typeof(string))
                    code.AppendLinf("                       item.{0} = Convert<{1}>(reader[\"{0}\"]);", column.Name, column.GetCSharpType());
                else
                    code.AppendLinf("                       item.{0} = ({1})reader[\"{0}\"];", column.Name, column.GetCSharpType());
            }
            code.AppendLinf("                       list.Add(item);");
            code.AppendLine("                   }");
            code.AppendLine("               }");
            code.AppendLine("               return list;");
            code.AppendLine("           }");

            code.AppendLine("       }");

            return code.ToString();
        }

        public string GetDeleteCommand()
        {
            var sql = new StringBuilder("DELETE FROM ");
            sql.AppendFormat("[{0}].[{1}] ", this.Schema, this.Table);


            sql.AppendFormat("WHERE {0} = @{0}", m_ed.RecordName);

            return sql.ToString();
        }
        private string GenerateDeleteMethod(Member record)
        {
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<int> DeleteAsync({0} id)", record.Type.ToCSharp());
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetDeleteCommand());
            code.AppendLine("           {");

            code.AppendLinf("               cmd.Parameters.AddWithValue(\"@{0}\", id);", m_ed.RecordName);

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");
            return code.ToString();
        }


        private string GenerateInsertMethod(string name)
        {
            var code = new StringBuilder();

            code.AppendLinf("       public async Task<object> InsertAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetInsertCommand());
            code.AppendLine("           {");
            foreach (var col in m_columnCollection.Where(c => !c.IsIdentity).Where(c => !c.IsComputed))
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

        public string GetUpdateCommand()
        {
            var sql = new StringBuilder("UPDATE  ");
            sql.AppendFormat("[{0}].[{1}] SET ", this.Schema, this.Table);

            var cols = m_columnCollection
                .Where(c => !c.IsIdentity)
                .Where(c => !c.IsComputed)
                .Select(c => c.Name)
                .ToArray();
            sql.AppendLine(string.Join(",\r\n", cols.Select(c => "[" + c + "] = @" + c).ToArray()));
            sql.AppendLinf(" WHERE [{0}] = @{0}", m_ed.RecordName);

            return sql.ToString();

        }
        public string GetInsertCommand()
        {
            var sql = new StringBuilder("INSERT INTO ");
            sql.AppendFormat("[{0}].[{1}] (", this.Schema, this.Table);


            var cols = m_columnCollection
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

        protected override Task<TableDefinition> GetSchemaDefinitionAsync()
        {
            return Task.FromResult(m_ed);
        }
    }

}
