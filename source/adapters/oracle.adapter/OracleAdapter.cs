using System;
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
                return @"select column_name, data_type, nullable from all_tab_columns where table_name = '" + this.Table + "'";
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
        }
        public async Task OpenAsync()
        {
            m_ed = new EntityDefinition { Name = this.Schema + "_" + this.Table };
            m_columnCollection = new ObjectCollection<Column>();
            var insertCommand = new StringBuilder("INSERT INTO ");
            insertCommand.AppendFormat("[{0}].[{1}] VALUES(", this.Schema, this.Table);

            var updateCommand = new StringBuilder("UPDATE ");
            updateCommand.AppendFormat("[{0}].[{1}] SET", this.Schema, this.Table);

            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(OracleSchemaSql, conn))
            {
                Console.WriteLine(OraclePrimaryKeySql);
                await conn.OpenAsync();

                var f = await cmd.ExecuteScalarAsync() as string;
                this.Schema = f;

            }
            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(OraclePrimaryKeySql, conn))
            {
                Console.WriteLine(OraclePrimaryKeySql);
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
                        var col = new Column
                        {
                            Name = reader.GetOracleString(0).ToString(),
                            DataType = reader.GetOracleString(1).Value,
                            IsNullable = reader.GetOracleString(2) == "Y"
                        };
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
            var cols = m_columnCollection.Where(c => !c.IsIdentity).Where(c => !c.IsComputed).Select(c => c.Name).ToArray();
            insertCommand.AppendLine(string.Join(",\r\n", cols.Select(c => "[" + c + "]").ToArray()));
            insertCommand.AppendLine(")");
            insertCommand.AppendLine("VALUES(");
            insertCommand.AppendLine(string.Join(",\r\n", cols.Select(c => "@" + c).ToArray()));
            insertCommand.AppendLine(")");


            this.InsertCommand = insertCommand.ToString();
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

            code.AppendLinf("           using(var conn = new OracleConnection(@\"{0}\"))", this.ConnectionString);
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0}\", conn))", this.InsertCommand);
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

        public string InsertCommand { get; set; }

        protected override Task<EntityDefinition> GetSchemaDefinitionAsync()
        {
            return Task.FromResult(m_ed);
        }
    }
}