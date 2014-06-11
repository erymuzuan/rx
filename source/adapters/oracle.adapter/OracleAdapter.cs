using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class OracleAdapter : Adapter
    {
        public string ConnectionString { get; set; }
        private ObjectCollection<TableDefinition> m_tableDefinitions = new ObjectCollection<TableDefinition>();

        private string GetOraclePrimaryKeySql(string table)
        {
            return "SELECT cols.column_name " +
                   "FROM all_constraints cons, all_cons_columns cols " +
                   "WHERE cols.table_name = '" + table
                   + "'AND cons.constraint_type = 'P' " +
                   "AND cons.constraint_name = cols.constraint_name " +
                   "AND cons.owner = cols.owner " +
                   "ORDER BY cols.table_name, cols.position";

        }
        private string GetOracleSchemaSql(string table)
        {

            return "SELECT cols.owner " +
                   "FROM all_constraints cons, all_cons_columns cols " +
                   "WHERE cols.table_name = '" + table
                   + "'AND cons.constraint_type = 'P' " +
                   "AND cons.constraint_name = cols.constraint_name " +
                   "AND cons.owner = cols.owner " +
                   "ORDER BY cols.table_name, cols.position";

        }
        private string GetTableSql(string table)
        {

            return @"select column_name, data_type, nullable, data_precision, data_scale from all_tab_columns where table_name = '" + table + "'";

        }


        public async Task OpenAsync(bool verbose = false)
        {
            foreach (var table in this.Tables)
            {
                var td = new TableDefinition { Name = table, CodeNamespace = this.CodeNamespace };
                m_columnCollection.Add(table, new ObjectCollection<Column>());
                m_tableDefinitions.Add(td);

                using (var conn = new OracleConnection(ConnectionString))
                using (var cmd = new OracleCommand(GetOracleSchemaSql(table), conn))
                {
                    await conn.OpenAsync();

                    var f = await cmd.ExecuteScalarAsync() as string;
                    this.Schema = f;

                }
                using (var conn = new OracleConnection(ConnectionString))
                using (var cmd = new OracleCommand(GetOraclePrimaryKeySql(table), conn))
                {
                    await conn.OpenAsync();

                    var f = await cmd.ExecuteScalarAsync() as string;
                    td.RecordName = f;
                }

                using (var conn = new OracleConnection(ConnectionString))
                using (var cmd = new OracleCommand(GetTableSql(table), conn))
                {
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync() as OracleDataReader)
                    {
                        if (null == reader)
                            throw new InvalidOperationException("Cannot get OracleDataReader");
                        var first = true;
                        while (await reader.ReadAsync())
                        {
                            if (verbose && first)
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
                            var col = new Column
                            {
                                Name = reader.GetOracleString(0).ToString(),
                                DataType = reader.GetOracleString(1).Value,
                                IsNullable = reader.GetOracleString(2) == "Y"
                            };
                            var precision = reader["data_precision"];
                            if (precision != DBNull.Value)
                                col.Precision = (decimal)precision;


                            var scale = reader["data_scale"];
                            if (scale != DBNull.Value)
                                col.Scale = (decimal?)scale;

                            if (null != col.GetClrType())
                                m_columnCollection[table].Add(col);

                        }
                    }
                }

                var members = from c in m_columnCollection[table]
                              select new Member
                              {
                                  Name = c.Name,
                                  IsNullable = c.IsNullable,
                                  IsFilterable = true,
                                  Type = c.GetClrType()
                              };
                td.MemberCollection.AddRange(members);



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
            header.AppendLine("using " + typeof(OracleConnection).Namespace + ";");
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

        private readonly Dictionary<string, ObjectCollection<Column>> m_columnCollection = new Dictionary<string, ObjectCollection<Column>>();


        protected override Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            options.AddReference(typeof(OracleConnection));
            var sources = new Dictionary<string, string>();
            foreach (var table in this.Tables)
            {
                var table1 = table;
                var td = m_tableDefinitions.Single(a => a.Name == table1);
                var name = table;
                var adapterName = table + "Adapter";

                var header = this.GetCodeHeader(namespaces);
                var code = new StringBuilder(header);

                code.AppendLine("   public class " + adapterName);
                code.AppendLine("   {");

                code.AppendLine(GenerateInsertMethod(name, table));
                code.AppendLine(GenerateUpdateMethod(name, table));
                code.AppendLine(GenerateConvertMethod(name));
                code.AppendLine(GenerateConnectionStringProperty());

                var record = td.MemberCollection.Single(m => m.Name == td.RecordName);
                code.AppendLine(GenerateSelectOne(name, record, table));
                code.AppendLine(GenerateSelectMethod(name, record, table));
                code.AppendLine(GenerateDeleteMethod(record, table));


                code.AppendLine("   }");// end class
                code.AppendLine("}");// end namespace


                sources.Add(adapterName + ".cs", code.ToString());
            }
            sources.Add("Column.cs", this.GenerateColumnClass());
            sources.Add("OracleHelpers.cs", this.GenerateHelperClass());

            return Task.FromResult(sources);

        }

        private string GenerateHelperClass()
        {

            var header = this.GetCodeHeader(typeof(ParameterDirection).Namespace);
            var code = new StringBuilder(header);

            code.AppendLine("   public static class OracleHelpers");
            code.AppendLine("   {");
            code.AppendLine(@" 
           public static IList<Column> AddWithValue(this IList<Column> columns, string name, object value)
        {
            var type = OracleDbType.Char;
            if (value is bool)
                type = OracleDbType.Char;
            if (value is int)
                type = OracleDbType.Int32;
            if (value is decimal)
                type = OracleDbType.Decimal;
            if (value is DateTime)
                type = OracleDbType.Date;

            var col = new Column
            {
                Name = name,
                Value = value,
                Type = type
            };
            columns.Add(col);
            return columns;
        }
        public static IList<Column> AddWithValue(this IList<Column> columns, string name, object value, OracleDbType type)
        {
            var col = new Column
            {
                Name = name,
                Value = value,
                Type = type
            };
            columns.Add(col);
            return columns;
        }

");


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            return code.ToString();
        }

        private string GenerateColumnClass()
        {

            var header = this.GetCodeHeader(typeof(ParameterDirection).Namespace);
            var code = new StringBuilder(header);

            code.AppendLine("   public class Column");
            code.AppendLine("   {");
            code.AppendLine(@" 
        public Column()
        {
            this.Direction = ParameterDirection.Input;
        }

        public string Name { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsComputed { get; set; }
        public bool IsIdentity { get; set; }
        public int Length { get; set; }
        public decimal? Scale { get; set; }
        public decimal? Precision { get; set; }
        public object Value { get; set; }
        public OracleDbType Type { get; set; }
        public ParameterDirection Direction { get; set; }");


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            return code.ToString();
        }

        private string GenerateDeleteMethod(Member record, string table)
        {
            var td = m_tableDefinitions.Single(t => t.Name == table);
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<int> DeleteAsync({0} id)", record.Type.ToCSharp());
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0}\", conn))", this.GetDeleteCommand(table));
            code.AppendLine("           {");

            code.AppendLinf("               cmd.Parameters.Add(\"{0}\", id);", td.RecordName);

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");
            return code.ToString();
        }

        private string GenerateUpdateMethod(string name, string table)
        {
            var code = new StringBuilder();
            var columns = m_columnCollection[table];
            code.AppendLinf("       public async Task<int> UpdateAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0}\", conn))", this.GetUpdateCommand(table));
            code.AppendLine("           {");
            foreach (var col in columns.Where(c => !c.IsIdentity).Where(c => !c.IsComputed))
            {
                var nullable = col.IsNullable ? ".ToDbNull()" : "";
                code.AppendLinf("               cmd.Parameters.Add(\"{0}\", item.{0}{1});", col.Name, nullable);
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");
            return code.ToString();
        }

        private string GenerateInsertMethod(string name, string table)
        {
            var code = new StringBuilder();
            var colums = m_columnCollection[table];
            code.AppendLinf("       public async Task<int> InsertAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLine("           var columns = new List<Column>()");
            foreach (var col in colums.Where(c => !c.IsIdentity).Where(c => !c.IsComputed))
            {
                var nullable = col.IsNullable ? ".ToDbNull()" : string.Empty;
                code.AppendLinf("               .AddWithValue(\"{0}\", item.{0}{1})", col.Name, nullable);
            }
            code.AppendLine("               ;");

            code.AppendFormat(@"            string sql = string.Format(""INSERT INTO {0}.{1}({{0}}) values ({{1}})"",
               string.Join("","", columns.Select(c => c.Name).ToArray()),
               string.Join("","", columns.Select(c => "":"" + c.Name).ToArray())
            );", this.Schema, table);
            code.AppendLine();
            code.AppendLine();
            code.AppendLine("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(sql, conn))");
            code.AppendLine("           {");
            code.AppendLine("               foreach (var c in columns)");
            code.AppendLine("               {");
            code.AppendLine("                   var p = cmd.Parameters.Add(c.Name, c.Type, c.Direction);");
            code.AppendLine("                   p.Value = c.Value;");
            code.AppendLine("               }");

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");
            return code.ToString();
        }

        private static string GenerateConvertMethod(string name)
        {
            var code = new StringBuilder();
            code.AppendLinf("       public T? Convert<T>(object val) where T : struct", name);
            code.AppendLine("       {");

            code.AppendLine("           if(val == null) return default(T?);");
            code.AppendLine("           System.Diagnostics.Debug.WriteLine(\"{0} -> {1}\", val.GetType(),val);");
            code.AppendLine("           if(val == System.DBNull.Value) return default(T?);");
            code.AppendLine("           return (T)val;");


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

        private string GenerateSelectOne(string name, Member record, string table)
        {
            var code = new StringBuilder();
            var td = m_tableDefinitions.Single(t => t.Name == table);
            code.AppendLinf("       public async Task<{0}> LoadOneAsync({1} id)", name, record.Type.ToCSharp());
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0}\", conn))", this.GetSelectOneCommand(table));
            code.AppendLine("           {");

            code.AppendLinf("               cmd.Parameters.Add(\"{0}\", id);", td.RecordName);

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               using(var reader = await cmd.ExecuteReaderAsync())");
            code.AppendLine("               {");
            code.AppendLine("                   while(await reader.ReadAsync())");
            code.AppendLine("                   {");
            code.AppendLinf("                       var item = new {0}();", name);
            foreach (var column in m_columnCollection[table])
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

        private string GenerateSelectMethod(string name, Member record, string table)
        {
            var code = new StringBuilder();
            //load async
            code.AppendLinf("       public async Task<IEnumerable<{0}>> LoadAsync(string filter)", name, record.Type.ToCSharp());
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0} \" + filter, conn))", this.GetSelectCommand(table));
            code.AppendLine("           {");

            code.AppendLinf("               var list = new List<{0}>();", name);

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               using(var reader = await cmd.ExecuteReaderAsync())");
            code.AppendLine("               {");
            code.AppendLine("                   while(await reader.ReadAsync())");
            code.AppendLine("                   {");
            code.AppendLinf("                       var item = new {0}();", name);
            foreach (var column in m_columnCollection[table])
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

        public string GetUpdateCommand(string table)
        {
            var td = m_tableDefinitions.Single(t => t.Name == table);
            var sql = new StringBuilder("UPDATE  ");
            sql.AppendFormat("{0}.{1} SET ", this.Schema, table);

            var cols = m_columnCollection[table]
                .Where(c => !c.IsIdentity)
                .Where(c => !c.IsComputed)
                .Select(c => c.Name)
                .ToArray();
            sql.AppendLine(string.Join(",", cols.Select(c => "" + c + " = :" + c).ToArray()));
            sql.AppendLinf(" WHERE {0} = :{0}", td.RecordName);

            return sql.ToString();

        }

        public string GetDeleteCommand(string table)
        {
            var td = m_tableDefinitions.Single(t => t.Name == table);
            var sql = new StringBuilder("DELETE FROM ");
            sql.AppendFormat("{0}.{1} ", this.Schema, table);


            sql.AppendFormat("WHERE {0} = :{0}", td.RecordName);

            return sql.ToString();
        }
        public string GetSelectOneCommand(string table)
        {
            var td = m_tableDefinitions.Single(t => t.Name == table);
            var sql = new StringBuilder("SELECT * FROM ");
            sql.AppendFormat("{0}.{1} ", this.Schema, table);


            sql.AppendFormat("WHERE {0} = :{0}", td.RecordName);

            return sql.ToString();
        }
        public string GetSelectCommand(string table)
        {
            return string.Format("SELECT * FROM {0}.{1} ", this.Schema, table);
        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            var td = m_tableDefinitions.Single(t => t.Name == table);
            return Task.FromResult(td);
        }
    }
}