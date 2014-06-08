using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain.Api
{
    public class SqlServerAdapter : Adapter
    {
        public string ConnectionString { get; set; }
        private Type GetClrType(string typeName)
        {
            switch (typeName)
            {
                case "xml":
                case "char":
                case "nchar":
                case "ntext":
                case "text":
                case "uniqueidentifier":
                case "nvarchar":
                case "varchar": return typeof(string);
                case "bigint": return typeof(long);
                case "tinyint": return typeof(short);
                case "int": return typeof(int);
                case "datetimeoffset":
                case "time":
                case "datetime":
                case "datetime2":
                case "smalldatetime": return typeof(DateTime);
                case "bit": return typeof(bool);
                case "numeric":
                case "smallmoney":
                case "money": return typeof(decimal);
                case "real":
                case "float": return typeof(double);
            }
            return null;
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
        private EntityDefinition m_ed;

        class SqlColumn
        {
            public string Name { get; set; }
            public bool IsPrimaryKey { get; set; }
            public string DataType { get; set; }
            public bool IsNullable { get; set; }
            public bool IsComputed { get; set; }
            public bool IsIdentity { get; set; }
            public int Length { get; set; }
        }
        public async Task OpenAsync()
        {
            m_ed = new EntityDefinition { Name = this.Schema + "_" + this.Table };
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
                        if (first)
                        {
                            first = false;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write("{0,-15}\t", reader.GetName(i));
                            }

                        }
                        Console.WriteLine();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write("{0,-15}\t", reader[i]);
                        }
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
                        if (null != GetClrType(col.DataType))
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
                            Type = GetClrType(c.DataType)
                        };
            m_ed.MemberCollection.AddRange(members);


        }



        public string Table { get; set; }
        public string Schema { get; set; }
        public string CodeNamespace
        {
            get { return string.Format("{0}.Adapters.{1}", ConfigurationManager.ApplicationName, this.Schema); }
        }

        private string GetCodeHeader(params string[] namespaces)
        {

            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(Int32).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
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
            var name = this.Schema + "_" + this.Table;
            var adapterName = this.Schema + "_" + this.Table + "Adapter";
            var sources = new Dictionary<string, string>();

            var header = this.GetCodeHeader(namespaces);
            var code = new StringBuilder(header);

            code.AppendLine("   public class " + adapterName);
            code.AppendLine("   {");

            code.AppendLinf("       public async Task<object> InsertAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(@\"{0}\"))", this.ConnectionString);
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetInsertCommand());
            code.AppendLine("           {");
            foreach (var col in m_columnCollection.Where(c => !c.IsIdentity).Where(c => !c.IsComputed))
            {
                code.AppendLinf("               cmd.Parameters.AddWithValue(\"@{0}\", item.{0});", col.Name);
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");

            code.AppendLinf("       public async Task<object> UpdateAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new SqlConnection(@\"{0}\"))", this.ConnectionString);
            code.AppendLinf("           using(var cmd = new SqlCommand(@\"{0}\", conn))", this.GetUpdateCommand());
            code.AppendLine("           {");
            foreach (var col in m_columnCollection.Where(c => !c.IsIdentity).Where(c => !c.IsComputed))
            {
                code.AppendLinf("               cmd.Parameters.AddWithValue(\"@{0}\", item.{0});", col.Name);
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace


            sources.Add(adapterName + ".cs", code.ToString());

            return Task.FromResult(sources);

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
            sql.AppendFormat("[{0}].[{1}] VALUES(", this.Schema, this.Table);


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

        protected override Task<EntityDefinition> GetSchemaDefinitionAsync()
        {
            return Task.FromResult(m_ed);
        }
    }
}