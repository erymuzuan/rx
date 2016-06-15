using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters.Properties;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class SqlServerAdapter
    {


        public async Task<bool> LoadDatabaseObjectAsync(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
                var tableTask = ReadTablesAsync(conn);
                var viewsTask = ReadViewsAsync(conn);
                await Task.WhenAll(tableTask, viewsTask);


                // Used for performance testing, since it's not easy to use profiler
                //await GetChildTablesAsync(conn);
                //await ReadColumnsAsync(conn);
                //await ReadPrimaryKeysAsync(conn);
                //await ReadStoreProceduresAsync(conn);
                //await ReadFunctionsAsync(conn);

                var childTablesTask = GetChildTablesAsync(conn);
                var columnsTask = ReadColumnsAsync(conn);
                var primariKeyTask = ReadPrimaryKeysAsync(conn);
                var sprocTask = ReadStoreProceduresAsync(conn);
                var funcTask = ReadFunctionsAsync(conn);

                await Task.WhenAll(columnsTask, primariKeyTask, sprocTask, funcTask, childTablesTask);
            }


            return true;
        }

        private async Task GetChildTablesAsync(SqlConnection conn)
        {
            var tasks = this.TableDefinitionCollection.Select(x => this.GetChildTablesAsync(x, conn));
            await Task.WhenAll(tasks);
        }
        private async Task GetChildTablesAsync(TableDefinition table, SqlConnection conn)
        {
            const string SQL = @"Select
                    object_name(rkeyid) Parent_Table,
                    object_name(fkeyid) Child_Table,
                    object_name(constid) FKey_Name,
                    c1.name FKey_Col,
                    c2.name Ref_KeyCol
                    From
                    sys.sysforeignkeys s
                    Inner join sys.syscolumns c1
                    on ( s.fkeyid = c1.id And s.fkey = c1.colid )
                    Inner join syscolumns c2
                    on ( s.rkeyid = c2.id And s.rkey = c2.colid )
                    WHERE object_name(rkeyid) = @table";

            using (var cmd = new SqlCommand(SQL, conn))
            {
                cmd.Parameters.AddWithValue("@table", table.Name);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var childTables = new List<TableRelation>();
                    while (await reader.ReadAsync())
                    {
                        var ct = new TableRelation
                        {
                            Table = reader.GetString(1),
                            Constraint = reader.GetString(2),
                            Column = reader.GetString(3),
                            ForeignColumn = reader.GetString(4)
                        };
                        childTables.Add(ct);
                    }

                    table.ChildRelationCollection.ClearAndAddRange(childTables);
                }

            }


        }

        private async Task ReadViewsAsync(SqlConnection conn)
        {
            using (var tableCommand = new SqlCommand(Resources.SelectTablesSql.Replace("'U'", "'V'"), conn))
            {
                using (var reader = await tableCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var table = new TableDefinition
                        {
                            Name = reader.GetString(0),
                            Schema = reader.GetString(1),
                            AllowDelete = false,
                            AllowInsert = false,
                            AllowRead = true,
                            AllowUpdate = false,
                            IsSelected = false

                        };
                        if (this.TableDefinitionCollection.All(x => x.Name != table.Name))
                            this.TableDefinitionCollection.Add(table);
                    }
                }
            }
        }

        private async Task ReadFunctionsAsync(SqlConnection conn)
        {
            // get the sprocs
            var selectSprocSql =
                $@"
SELECT s.name as 'schema', o.name as 'sproc' FROM sys.all_objects o
INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
WHERE [type] = 'FN'
AND s.name NOT IN ('sys')";
            using (var spocCommand = new SqlCommand(selectSprocSql, conn))
            {
                using (var reader = await spocCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var sp = await this.GetFunctionAsync(reader.GetString(0), reader.GetString(1));
                        this.OperationDefinitionCollection.AddOrReplace(sp, x => x.Name == sp.Name && x.Schema == sp.Schema);
                    }
                }
            }
        }

        private async Task ReadStoreProceduresAsync(SqlConnection conn)
        {
            // get the sprocs
            var excludeNames = new[] { "SqlQueryNotificationStoredProcedure", "aspnet_" };
            var selectSprocSql =
                $@"
SELECT s.name as 'schema', o.name as 'sproc' FROM sys.all_objects o
INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
WHERE [type] = 'P'
AND s.name NOT IN ('sys')
AND
o.[name] NOT LIKE {excludeNames.ToString("\r\nAND\r\n o.[name] NOT LIKE", x => $"'{x}%'")}";

            var list = new List<Tuple<string, string>>();

            using (var spocCommand = new SqlCommand(selectSprocSql, conn))
            {
                using (var reader = await spocCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Tuple<string, string>(reader.GetString(0), reader.GetString(1)));
                    }
                }
            }

            var tasks = from s in list
                        select this.GetStoreProcedureAsync(s.Item1, s.Item2);
            var sprocs = await Task.WhenAll(tasks);
            foreach (var sp in sprocs)
            {
                this.OperationDefinitionCollection.AddOrReplace(sp, x => x.Name == sp.Name && x.Schema == sp.Schema);
            }
        }

        private async Task<SprocOperationDefinition> GetStoreProcedureAsync(string schema, string name)
        {

            const string SQL = @"
select * from information_schema.PARAMETERS
where SPECIFIC_NAME = @name
order by ORDINAL_POSITION";

            var uuid = Guid.NewGuid().ToString();
            var od = new SprocOperationDefinition
            {
                Name = name,
                MethodName = name.ToCsharpIdentitfier(),
                Uuid = uuid,
                Schema = schema,
                CodeNamespace = this.CodeNamespace,
                WebId = uuid,
            };
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(SQL, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@name", name);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var dt = (string)reader["DATA_TYPE"];
                        var cml = reader["CHARACTER_MAXIMUM_LENGTH"].ReadNullable<int>();
                        var mode = (string)reader["PARAMETER_MODE"];
                        var pname = (string)reader["PARAMETER_NAME"];
                        var position = reader["ORDINAL_POSITION"].ReadNullable<int>();

                        var member = new SprocParameter
                        {
                            Name = pname,
                            FullName = pname,
                            SqlType = dt,
                            Type = dt.GetClrType(),
                            IsNullable = cml == 0,
                            MaxLength = cml,
                            Mode = mode == "IN" ? ParameterMode.In : ParameterMode.Out,
                            Position = position ?? 0,
                            WebId = Guid.NewGuid().ToString()
                        };
                        if (mode == "IN" || mode == "INOUT")
                            od.RequestMemberCollection.Add(member);
                        if (mode == "OUT" || mode == "INOUT")
                        {
                            SqlDbType t;
                            Enum.TryParse(dt, true, out t);
                            var rm = new SprocResultMember
                            {
                                Name = pname,
                                SqlDbType = t,
                                Type = dt.GetClrType()
                            };
                            od.ResponseMemberCollection.Add(rm);
                        }
                    }
                }

            }


            var retVal = new SprocResultMember
            {
                Name = "@return_value",
                Type = typeof(int),
                SqlDbType = SqlDbType.Int
            };
            od.ResponseMemberCollection.Add(retVal);
            return od;
        }

        private async Task<FuncOperationDefinition> GetFunctionAsync(string schema, string name)
        {

            const string SQL = @"
SELECT 
    SCHEMA_NAME(SCHEMA_ID) AS [Schema], 
    SO.name AS [ObjectName],
    SO.Type_Desc AS [ObjectType (UDF/SP)],
    P.parameter_id AS [ParameterID],
    P.name AS [ParameterName],
    TYPE_NAME(P.user_type_id) AS [ParameterDataType],
    P.max_length AS [ParameterMaxBytes],
    P.is_output AS [IsOutPutParameter]
FROM 
    sys.objects AS SO
INNER JOIN 
    sys.parameters AS P 
ON SO.OBJECT_ID = P.OBJECT_ID
WHERE 
    SO.OBJECT_ID IN ( SELECT OBJECT_ID FROM sys.objects WHERE TYPE IN ('FN'))
AND 
    SO.name = @name
ORDER 
    BY [Schema], SO.name, P.parameter_id
";

            var uuid = Guid.NewGuid().ToString();
            var od = new FuncOperationDefinition
            {
                Name = name,
                MethodName = name.ToCsharpIdentitfier(),
                Uuid = uuid,
                Schema = schema,
                CodeNamespace = this.CodeNamespace,
                WebId = uuid,
            };
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(SQL, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@name", name);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var dt = (string)reader["ParameterDataType"];
                        var cml = reader["ParameterMaxBytes"].ReadNullable<short>();
                        var mode = ((bool)reader["IsOutPutParameter"]) ? "OUT" : "IN";
                        var pname = (string)reader["ParameterName"];
                        var position = reader["ParameterID"].ReadNullable<int>();

                        var member = new SprocParameter
                        {
                            Name = pname,
                            FullName = pname,
                            SqlType = dt,
                            Type = dt.GetClrType(),
                            IsNullable = cml == 0,
                            MaxLength = cml,
                            Mode = mode == "IN" ? ParameterMode.In : ParameterMode.Out,
                            Position = position ?? 0,
                            WebId = Guid.NewGuid().ToString()
                        };
                        if (mode == "IN" || mode == "INOUT")
                            od.RequestMemberCollection.Add(member);
                        if (mode == "OUT" || mode == "INOUT")
                        {
                            SqlDbType t;
                            Enum.TryParse(dt, true, out t);
                            var rm = new SprocResultMember
                            {
                                Name = string.IsNullOrWhiteSpace(pname) ? "Result" : pname,
                                SqlDbType = t,
                                Type = dt.GetClrType()
                            };
                            od.ResponseMemberCollection.Add(rm);
                        }
                    }
                }

            }

            return od;
        }

        private async Task ReadPrimaryKeysAsync(SqlConnection conn)
        {
            var tasks = from t in this.TableDefinitionCollection
                        select ReadPrimaryKeysAsync2(t, conn);
            await Task.WhenAll(tasks);
        }
        private async Task ReadPrimaryKeysAsync2(TableDefinition table, SqlConnection conn)
        {

            using (var primaryKeyCommand = new SqlCommand(Resources.SelectTablePrimaryKeysSql, conn))
            {
                primaryKeyCommand.Parameters.AddWithValue("@Table", table.Name);
                primaryKeyCommand.Parameters.AddWithValue("@Schema", table.Schema);
                using (var reader = await primaryKeyCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var name = reader.GetString(0);
                        var col = table.ColumnCollection.SingleOrDefault(x => x.Name == name);
                        if (null != col)
                            col.IsPrimaryKey = true;

                        table.PrimaryKeyCollection.AddOrReplace(name, x => x == name);
                    }
                }
            }

        }

        private async Task ReadColumnsAsync(SqlConnection conn)
        {
            using (var columnCommand = new SqlCommand(Resources.SelectColumnsSql, conn))
            {
                using (var reader = await columnCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var table =
                            this.TableDefinitionCollection.Single(
                                x => x.Name == (string)reader["Table"] && (string)reader["Schema"] == x.Schema);
                        var mt = ColumnMetadata.Read(reader, table);

                        if (null == this.ColumnGenerators)
                            ObjectBuilder.ComposeMefCatalog(this);
                        var scores = (from g in this.ColumnGenerators
                                      let s = g.Metadata.GetScore(mt)
                                      where s >= 0
                                      orderby s descending
                                      select g).ToList();
                        var generator = scores.FirstOrDefault();
                        if (null == generator)
                            throw new InvalidOperationException($"Cannot find column generator for {mt}");
                        var col = generator.Value.Initialize(mt, table);
                        var existingColumn = table.ColumnCollection.SingleOrDefault(x => x.Name == col.Name);
                        col.IsSelected = existingColumn?.IsSelected ?? false;

                        table.ColumnCollection.AddOrReplace(col, x => x.Name == col.Name);
                    }
                }
            }
        }

        private async Task ReadTablesAsync(SqlConnection conn)
        {
            using (var tableCommand = new SqlCommand(Resources.SelectTablesSql, conn))
            {
                using (var reader = await tableCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var table = new TableDefinition
                        {
                            Name = reader.GetString(0),
                            Schema = reader.GetString(1),
                            AllowDelete = true,
                            AllowInsert = true,
                            AllowRead = true,
                            AllowUpdate = true,
                            IsSelected = false
                        };
                        if (this.TableDefinitionCollection.All(x => x.Name != table.Name))
                            this.TableDefinitionCollection.Add(table);
                    }
                }
            }
        }
    }
}
