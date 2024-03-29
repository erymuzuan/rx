﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;
using ParameterDirection = System.Data.ParameterDirection;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(Adapter))]
    [Export("AdapterDesigner", typeof(Adapter))]
    [DesignerMetadata(Name = "Oracle database", FontAwesomeIcon = "database", RouteTableProvider = typeof(OracleAdapterRoute), Route = "adapter.oracle/0")]
    public partial class OracleAdapter : Adapter
    {
        private readonly ObjectCollection<TableDefinition> m_tableDefinitions = new ObjectCollection<TableDefinition>();

        protected override Task<Class> GeneratePagingSourceCodeAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "Bespoke.Sph.Integrations.Adapters.OraclePagingTranslator.txt";

            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
            // ReSharper disable AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
            // ReSharper restore AssignNullToNotNullAttribute
            {
                var code = reader.ReadToEnd();
                code = code.Replace("__NAMESPACE__", this.CodeNamespace);
                var source = new Class(code) { FileName = "OraclePagingTranslator.cs" };
                return Task.FromResult(source);

            }
        }

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
                   + "' AND cons.constraint_type = 'P' " +
                   "AND cons.constraint_name = cols.constraint_name " +
                   "AND cons.owner = cols.owner " +
                   "ORDER BY cols.table_name, cols.position";

        }
        private string GetTableSql(string table)
        {

            return @"select column_name, data_type, nullable, data_precision, data_scale from all_tab_columns where table_name = '" + table + "'";

        }


        public override async Task OpenAsync(bool verbose = false)
        {
            foreach (var table in this.Tables)
            {
                var table1 = table;
                if (m_tableDefinitions.Contains(t => t.Name == table1.Name)) continue;

                var td = new TableDefinition { Schema = this.Schema, Name = table.Name, CodeNamespace = this.CodeNamespace };
                m_columnCollection.Add(table.Name, new ObjectCollection<Column>());
                m_tableDefinitions.Add(td);
                td.ChildTableCollection.ClearAndAddRange(from a in table.ChildRelationCollection
                                                         select new TableDefinition
                                                         {
                                                             Name = a.Table,
                                                             CodeNamespace = this.CodeNamespace,
                                                             Schema = this.Schema
                                                         });

                using (var conn = new OracleConnection(ConnectionString))
                using (var cmd = new OracleCommand(GetOracleSchemaSql(table.Name), conn))
                {
                    await conn.OpenAsync();

                    var f = await cmd.ExecuteScalarAsync() as string;
                    this.Schema = f;

                }
                using (var conn = new OracleConnection(ConnectionString))
                using (var cmd = new OracleCommand(GetOraclePrimaryKeySql(table.Name), conn))
                {
                    await conn.OpenAsync();
                    using (var reader = (OracleDataReader)await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            td.PrimaryKeyCollection.Add(reader.GetOracleString(0).Value);
                        }

                    }

                }

                using (var conn = new OracleConnection(ConnectionString))
                using (var cmd = new OracleCommand(GetTableSql(table.Name), conn))
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
                                    Console.Write(@"{0,-15}\t", reader.GetName(i));
                                }
                                Console.WriteLine();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write(@"{0,-15}\t", reader[i]);
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
                                m_columnCollection[table.Name].Add(col);

                        }
                    }
                }

                var members = from c in m_columnCollection[table.Name]
                              select new SimpleMember
                              {
                                  Name = c.Name,
                                  IsNullable = c.IsNullable,
                                  IsFilterable = true,
                                  Type = c.GetClrType()
                              };
                td.MemberCollection.AddRange(members);



            }

        }


        public static readonly string[] ImportDirectoves =
        {

            typeof(Entity).Namespace,
            typeof(int).Namespace ,
            typeof(Task<>).Namespace ,
            typeof(Enumerable).Namespace,
            typeof(IEnumerable<>).Namespace,
            typeof(OracleConnection).Namespace,
            typeof(XmlAttributeAttribute).Namespace ,
            "System.Web.Mvc",
            "Bespoke.Sph.Web.Helpers"

        };

        private readonly Dictionary<string, ObjectCollection<Column>> m_columnCollection = new Dictionary<string, ObjectCollection<Column>>();


        public override string OdataTranslator => "OdataOracleTranslator";

        protected override Task<IEnumerable<Class>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            options.AddReference(typeof(OracleConnection));
            var sources = new ObjectCollection<Class>();
            foreach (var table in this.Tables)
            {
                var table1 = table;
                var td = m_tableDefinitions.Single(a => a.Name == table1.Name);
                var name = table.Name;

                var code = new Class { Name = $"{name}Adapter", Namespace = CodeNamespace };
                code.ImportCollection.AddRange(ImportDirectoves);
                code.ImportCollection.AddRange(namespaces);

                sources.Add(code);


                code.AddMethod(GenerateExecuteScalarMethod());
                code.AddMethod(GenerateInsertMethod(td));
                code.AddMethod(GenerateUpdateMethod(td));
                code.AddMethod(GenerateConvertMethod(td));
                code.AddMethod(GenerateConnectionStringProperty());

                code.AddMethod(GenerateSelectOne(td));
                code.AddMethod(GenerateSelectMethod(td));
                code.AddMethod(GenerateDeleteMethod(td));



            }
            sources.Add(this.GenerateColumnClass());
            sources.Add(this.GenerateHelperClass());

            return Task.FromResult(sources.AsEnumerable());

        }

        protected override Task<Class> GenerateOdataTranslatorSourceCodeAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "Bespoke.Sph.Integrations.Adapters.OdataOracleTranslator.txt";

            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
            // ReSharper disable AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
            // ReSharper restore AssignNullToNotNullAttribute
            {
                var code = reader.ReadToEnd();
                code = code.Replace("__NAMESPACE__", this.CodeNamespace);
                var source = new Class(code) { FileName = "OdataOracleTranslator.cs" };
                return Task.FromResult(source);

            }
        }

        private Class GenerateHelperClass()
        {

            var code = new Class { Name = "OracleHelpers", IsStatic = true, Namespace = CodeNamespace };
            code.ImportCollection.AddRange(ImportDirectoves);
            code.ImportCollection.Add(typeof(ParameterDirection).Namespace);

            code.AddMethod(@" 
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
");

            code.AddMethod(@" 
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
        }");



            return code;
        }

        private Class GenerateColumnClass()
        {

            var code = new Class { Name = "Column", Namespace = CodeNamespace };
            code.ImportCollection.AddRange(ImportDirectoves);
            code.ImportCollection.Add(typeof(ParameterDirection).Namespace);

            code.CtorCollection.Add("public Column(){ this.Direction = ParameterDirection.Input; }");
            code.AddProperty("Name", typeof(string));
            code.AddProperty("IsPrimaryKey", typeof(bool));
            code.AddProperty("DataType", typeof(string));
            code.AddProperty("IsNullable", typeof(bool));
            code.AddProperty("IsComputed", typeof(bool));
            code.AddProperty("IsIdentity", typeof(bool));
            code.AddProperty("Length", typeof(int));
            code.AddProperty("Scale", typeof(decimal?));
            code.AddProperty("Precision", typeof(decimal?));
            code.AddProperty("Value", typeof(object));
            code.AddProperty("Type", typeof(OracleDbType));
            code.AddProperty("Direction", typeof(ParameterDirection));



            return code;
        }

        private string GenerateDeleteMethod(TableDefinition table)
        {
            var code = new StringBuilder();
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var arguements = pks.Select(k => k.GenerateParameterCode());
            code.AppendLinf("       public async Task<int> DeleteAsync({0})", string.Join(", ", arguements));
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0}\", conn))", this.GetDeleteCommand(table));
            code.AppendLine("           {");
            foreach (var pk in pks)
            {
                code.AppendLinf("               cmd.Parameters.Add(\"{0}\", {0});", pk.Name);
            }

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               return await cmd.ExecuteNonQueryAsync();");
            code.AppendLine("           }");

            code.AppendLine("       }");
            return code.ToString();
        }

        private string GenerateUpdateMethod(TableDefinition table)
        {
            var code = new StringBuilder();
            var columns = m_columnCollection[table.Name];
            code.AppendLinf("       public async Task<int> UpdateAsync({0} item)", table);
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

        private string GenerateInsertMethod(TableDefinition table)
        {
            var code = new StringBuilder();
            var colums = m_columnCollection[table.Name];
            code.AppendLinf("       public async Task<int> InsertAsync({0} item)", table.Name);
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

        private static string GenerateConvertMethod(TableDefinition table)
        {
            var code = new StringBuilder();
            code.AppendLinf("       public T? Convert<T>(object val) where T : struct", table.Name);
            code.AppendLine("       {");

            code.AppendLine("           if(val == null) return default(T?);");
            code.AppendLine("           System.Console.WriteLine(\"{0}:{1} ->{2}\", val.GetType(),val, typeof(T).FullName);");
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

        private string GenerateSelectOne(TableDefinition table)
        {
            var code = new StringBuilder();
            var name = table.Name;
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var arguements = pks.Select(k => k.GenerateParameterCode());
            code.AppendLinf("       public async Task<{0}> LoadOneAsync({1})", table.Name, string.Join(", ", arguements));
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new OracleCommand(@\"{0}\", conn))", this.GetSelectOneCommand(table));
            code.AppendLine("           {");

            foreach (var pk in pks)
            {
                code.AppendLinf("               cmd.Parameters.Add(\"{0}\", {0});", pk.Name);
            }

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               using(var reader = await cmd.ExecuteReaderAsync())");
            code.AppendLine("               {");
            code.AppendLine("                   while(await reader.ReadAsync())");
            code.AppendLine("                   {");
            code.AppendLinf("                       var item = new {0}();", name);
            foreach (var column in m_columnCollection[name])
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
        private string GenerateExecuteScalarMethod()
        {
            var code = new StringBuilder();
            code.AppendLine("       public async Task<T> ExecuteScalarAsync<T>(string sql)");
            code.AppendLine("       {");

            code.AppendLinf("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLine("           using(var cmd = new OracleCommand(sql, conn))");
            code.AppendLine("           {");

            code.AppendLine("               Console.WriteLine(sql);");
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               var dbval = await cmd.ExecuteScalarAsync();");
            code.AppendLine("               if(dbval == System.DBNull.Value)");
            code.AppendLine("                   return default(T);");

            code.AppendLine("               Console.WriteLine(\"{0}:{1}->{2}\", dbval.GetType().FullName,dbval,typeof(T).FullName);");
            code.AppendLine("               return (T)dbval;");
            code.AppendLine("           }");

            code.AppendLine("       }");

            return code.ToString();

        }


        private string GenerateSelectMethod(TableDefinition table)
        {
            var code = new StringBuilder();
            var name = table.Name;
            //load async
            code.AppendLinf("       public async Task<LoadOperation<{0}>> LoadAsync(string sql, int page = 1, int size = 40, bool includeTotal = false)", name);
            code.AppendLine("       {");

            code.AppendLine("           if (!sql.ToString().Contains(\"ORDER\"))");
            code.AppendLinf("               sql +=\"\\r\\nORDER BY {0}\";", table.PrimaryKeyCollection.FirstOrDefault() ?? table.MemberCollection.Select(m => m.Name).First());
            code.AppendLine("           var translator = new OraclePagingTranslator();");
            code.AppendLine("           sql = translator.Translate(sql, page, size);");
            code.AppendLine("           Console.WriteLine(sql);");

            code.AppendLine("           using(var conn = new OracleConnection(this.ConnectionString))");
            code.AppendLine("           using(var cmd = new OracleCommand(sql, conn))");
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
            foreach (var column in m_columnCollection[name])
            {
                if (column.IsNullable && column.GetClrType() != typeof(string))
                    code.AppendLinf("                       item.{0} = Convert<{1}>(reader[\"{0}\"]);", column.Name, column.GetCSharpType());
                else
                    code.AppendLinf("                       item.{0} = ({1})reader[\"{0}\"];", column.Name, column.GetCSharpType());
            }
            code.AppendLinf("                       lo.ItemCollection.Add(item);");
            code.AppendLine("                   }");
            code.AppendLine("               }");
            code.AppendLine("               return lo;");
            code.AppendLine("           }");

            code.AppendLine("       }");

            return code.ToString();
        }

        public string GetUpdateCommand(TableDefinition table)
        {
            var sql = new StringBuilder("UPDATE  ");
            sql.AppendFormat("{0}.{1} SET ", this.Schema, table);

            var cols = m_columnCollection[table.Name]
                .Where(c => !c.IsIdentity)
                .Where(c => !c.IsComputed)
                .Select(c => c.Name)
                .ToArray();
            sql.AppendLine(string.Join(",", cols.Select(c => "" + c + " = :" + c).ToArray()));
            sql.AppendLine(" WHERE ");

            var predicate = table.PrimaryKeyCollection.Select(k => k + " = :" + k);
            sql.AppendLine(string.Join(" AND ", predicate));

            return sql.ToString();

        }

        public string GetDeleteCommand(TableDefinition table)
        {
            var sql = new StringBuilder("DELETE FROM ");
            sql.AppendFormat("{0}.{1} ", this.Schema, table);
            sql.AppendLine("WHERE");

            var predicate = table.PrimaryKeyCollection.Select(k => k + " = :" + k);
            sql.AppendLine(string.Join(" AND ", predicate));



            return sql.ToString();
        }

        public string GetSelectOneCommand(TableDefinition table)
        {
            var sql = new StringBuilder("SELECT * FROM ");
            sql.Append($"{this.Schema}.{table} ");
            sql.AppendLine("WHERE");

            sql.JoinAndAppendLine(table.PrimaryKeyCollection, " AND ", x => $"{x} = :{x}");

            return sql.ToString();
        }
        public string GetSelectCommand(string table)
        {
            return $"SELECT * FROM {this.Schema}.{table} ";
        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            var td = m_tableDefinitions.Single(t => t.Name == table);
            return Task.FromResult(td);
        }



        public new Task<BuildValidationResult> ValidateAsync()
        {
            var result = new BuildValidationResult();
            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9_]*$");
            if (string.IsNullOrWhiteSpace(this.Name) || !validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must start with letter.You cannot use symbol or number as first character" });
            if (string.IsNullOrWhiteSpace(this.Schema))
                result.Errors.Add(new BuildError(this.WebId) { Message = "Please select a schema" });
            if (!this.Tables.Any())
                result.Errors.Add(new BuildError(this.WebId) { Message = "Please select at least one table" });


            result.Result = result.Errors.Count == 0;
            return Task.FromResult(result);
        }

    }
}