﻿using System;
using System.Collections.Generic;
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
        private Type GetClrType(Column column)
        {

            var lowered = column.DataType.ToLowerInvariant();

            if (lowered == "number" && column.Scale == 2
                && column.Precision == 8)
                return typeof(double);
            if (lowered == "number" && column.Scale == 0
                && column.Precision == 4)
                return typeof(short);
            if (lowered == "number" && column.Scale > 0)
                return typeof(decimal);

            switch (lowered)
            {
                case "xml":
                case "char":
                case "char2":
                case "varchar2":
                case "nchar":
                case "ntext":
                case "text":
                case "uniqueidentifier":
                case "nvarchar":
                case "varchar": return typeof(string);
                case "bigint": return typeof(long);
                case "tinyint": return typeof(short);
                case "number":
                case "int": return typeof(int);
                case "datetimeoffset":
                case "time":
                case "date":
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

        private string OraclePrimaryKeySql
        {
            get
            {
                return "SELECT cols.column_name " +
                       "FROM all_constraints cons, all_cons_columns cols " +
                       "WHERE cols.table_name = '" + this.Table
                       + "'AND cons.constraint_type = 'P' " +
                       "AND cons.constraint_name = cols.constraint_name " +
                       "AND cons.owner = cols.owner " +
                       "ORDER BY cols.table_name, cols.position";
            }
        }
        private string OracleSchemaSql
        {
            get
            {
                return "SELECT cols.owner " +
                       "FROM all_constraints cons, all_cons_columns cols " +
                       "WHERE cols.table_name = '" + this.Table
                       + "'AND cons.constraint_type = 'P' " +
                       "AND cons.constraint_name = cols.constraint_name " +
                       "AND cons.owner = cols.owner " +
                       "ORDER BY cols.table_name, cols.position";
            }
        }
        private string TableSql
        {
            get
            {
                return @"select column_name, data_type, nullable, data_precision, data_scale from all_tab_columns where table_name = '" + this.Table + "'";
            }
        }
        private EntityDefinition m_ed;

        class Column
        {
            public string Name { get; set; }
            public bool IsPrimaryKey { get; set; }
            public string DataType { get; set; }
            public bool IsNullable { get; set; }
            public bool IsComputed { get; set; }
            public bool IsIdentity { get; set; }
            public int Length { get; set; }
            public decimal? Scale { get; set; }
            public decimal? Precision { get; set; }
        }


        public async Task OpenAsync(bool verbose = false)
        {
            m_ed = new EntityDefinition { Name = this.Schema + "_" + this.Table };
            m_columnCollection = new ObjectCollection<Column>();

            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(OracleSchemaSql, conn))
            {
                await conn.OpenAsync();

                var f = await cmd.ExecuteScalarAsync() as string;
                this.Schema = f;

            }
            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(OraclePrimaryKeySql, conn))
            {
                await conn.OpenAsync();

                var f = await cmd.ExecuteScalarAsync() as string;
                m_ed.RecordName = f;
            }

            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(TableSql, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync() as OracleDataReader)
                {
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
                        if (precision != System.DBNull.Value)
                            col.Precision = (decimal)precision;


                        var scale = reader["data_scale"];
                        if (scale != System.DBNull.Value)
                            col.Scale = (decimal?)scale;

                        if (null != GetClrType(col))
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
                              Type = GetClrType(c)
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

        private ObjectCollection<Column> m_columnCollection;


        protected override Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            options.AddReference(typeof(OracleConnection));
            var name = this.Schema + "_" + this.Table;
            var adapterName = this.Schema + "_" + this.Table + "Adapter";
            var sources = new Dictionary<string, string>();

            var header = this.GetCodeHeader(namespaces);
            var code = new StringBuilder(header);

            code.AppendLine("   public class " + adapterName);
            code.AppendLine("   {");

            code.AppendLinf("       public async Task<object> InsertAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0}\", conn))", this.GetInsertCommand());
            code.AppendLine("           {");
            foreach (var col in m_columnCollection.Where(c => !c.IsIdentity).Where(c => !c.IsComputed))
            {
                var nullable = col.IsNullable ? ".ToDbNull()" : string.Empty;
                code.AppendLinf("               cmd.Parameters.Add(\"{0}\", item.{0}{1});", col.Name, nullable);
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");

            code.AppendLinf("       public async Task<object> UpdateAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0}\", conn))", this.GetUpdateCommand());
            code.AppendLine("           {");
            foreach (var col in m_columnCollection.Where(c => !c.IsIdentity).Where(c => !c.IsComputed))
            {
                code.AppendLinf("               cmd.Parameters.Add(\"{0}\", item.{0});", col.Name);
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");

            //Delete
            code.AppendLinf("       public async Task<object> DeleteAsync({0} item)", name);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0}\", conn))", this.GetDeleteCommand());
            code.AppendLine("           {");

            code.AppendLinf("               cmd.Parameters.Add(\"{0}\", item.{0});", m_ed.RecordName);

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");

            //Delete
            code.AppendLinf("       public T? Convert<T>(object val) where T : struct", name);
            code.AppendLine("       {");

            code.AppendLine("           if(val == null) return default(T);");
            code.AppendLine("           Console.WriteLine(\"{0} -> {1}\", val.GetType(),val);");
            code.AppendLine("           if(val == System.DBNull.Value) return default(T);");
            code.AppendLine("           return (T)val;");


            code.AppendLine("       }");
            //Delete
            code.AppendLinf("       public string ConnectionString");
            code.AppendLine("       {");

            code.AppendLine("           get ");
            code.AppendLine("           {");
            code.AppendLinf("               var conn = ConfigurationManager.ConnectionStrings[\"{0}\"];", this.Name);
            code.AppendLine("               if(null != conn)return conn.ConnectionString;");
            code.AppendLinf("               return @\"{0}\";", this.ConnectionString);
            code.AppendLine("           }");
            code.AppendLine("       }");

            var record = m_ed.MemberCollection.Single(m => m.Name == m_ed.RecordName);
            //load one
            code.AppendLinf("       public async Task<{0}> LoadOneAsync({1} id)", name, record.Type.FullName);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0}\", conn))", this.GetSelectOneCommand());
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
                if (column.IsNullable && GetClrType(column) != typeof(string))
                    code.AppendLinf("                       item.{0} = Convert<{1}>(reader[\"{0}\"]);", column.Name, GetClrType(column));
                else
                    code.AppendLinf("                       item.{0} = ({1})reader[\"{0}\"];", column.Name, GetClrType(column));

            }
            code.AppendLinf("                       return item;", name);
            code.AppendLine("                   }");
            code.AppendLine("               }");
            code.AppendLine("               return null;");
            code.AppendLine("           }");

            code.AppendLine("       }");
            //load async
            code.AppendLinf("       public async Task<IEnumerable<{0}>> LoadAsync({1} id)", name, record.Type.FullName);
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0}\", conn))", this.GetSelectOneCommand());
            code.AppendLine("           {");

            code.AppendLinf("               var list = new List<{0}>();", name);
            code.AppendLinf("               cmd.Parameters.Add(\"{0}\", id);", m_ed.RecordName);

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               using(var reader = await cmd.ExecuteReaderAsync())");
            code.AppendLine("               {");
            code.AppendLine("                   while(await reader.ReadAsync())");
            code.AppendLine("                   {");
            code.AppendLinf("                       var item = new {0}();", name);
            foreach (var column in m_columnCollection)
            {
                if (column.IsNullable && GetClrType(column) != typeof(string))
                    code.AppendLinf("                       item.{0} = Convert<{1}>(reader[\"{0}\"]);", column.Name, GetClrType(column));
                else
                    code.AppendLinf("                       item.{0} = ({1})reader[\"{0}\"];", column.Name, GetClrType(column));

            }
            code.AppendLinf("                       list.Add(item);");
            code.AppendLine("                   }");
            code.AppendLine("               }");
            code.AppendLine("               return list;");
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
            sql.AppendLine(string.Join(",", cols.Select(c => "" + c + " = :" + c).ToArray()));
            sql.AppendLinf(" WHERE [{0}] = @{0}", m_ed.RecordName);

            return sql.ToString();

        }
        public string GetInsertCommand()
        {
            var sql = new StringBuilder("INSERT INTO ");
            sql.AppendFormat("{0}.{1} (", this.Schema, this.Table);


            var cols = m_columnCollection
                .Where(c => !c.IsIdentity)
                .Where(c => !c.IsComputed)
                .Select(c => c.Name)
                .ToArray();
            sql.AppendLine(string.Join(",", cols.ToArray()));
            sql.AppendLine(")");
            sql.AppendLine("VALUES(");
            sql.AppendLine(string.Join(",", cols.Select(c => ":" + c).ToArray()));
            sql.AppendLine(")");

            return sql.ToString();
        }

        public string GetDeleteCommand()
        {
            var sql = new StringBuilder("DELETE FROM ");
            sql.AppendFormat("{0}.{1} ", this.Schema, this.Table);


            sql.AppendFormat("WHERE {0} = :{0}", m_ed.RecordName);

            return sql.ToString();
        }
        public string GetSelectOneCommand()
        {
            var sql = new StringBuilder("SELECT * FROM ");
            sql.AppendFormat("{0}.{1} ", this.Schema, this.Table);


            sql.AppendFormat("WHERE {0} = :{0}", m_ed.RecordName);

            return sql.ToString();
        }

        protected override Task<EntityDefinition> GetSchemaDefinitionAsync()
        {
            return Task.FromResult(m_ed);
        }
    }
}