using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
                var deletedTables = new ConcurrentBag<TableDefinition>();

                var tasks = from t in this.TableDefinitionCollection
                    select RefreshTableMetadataAsync(t, deletedTables);
                await Task.WhenAll(tasks);

                foreach (var dt in deletedTables)
                {
                    this.TableDefinitionCollection.Remove(dt);
                }

            }


            return true;
        }

        private async Task RefreshTableMetadataAsync(TableDefinition table, ConcurrentBag<TableDefinition> deletedTables)
        {
            var db = await this.GetTableOptionDetailsAsync(table.Schema, table.Name);
            if (null == db)
            {
                deletedTables.Add(table);
                return;
            }

            foreach (var col in db.ColumnCollection)
            {
                var oc = table.ColumnCollection.SingleOrDefault(x => x.Name == col.Name);
                if (null != oc)
                {
                    col.Merge(oc, table);
                }
            }
            // now refresh the table column with the one read from db, but with user's metada intact
            table.ColumnCollection.ClearAndAddRange(db.ColumnCollection.OrderBy(c => c.Order));

            //TODO : we should merge action generators with new ones, and remove the old one
            if (table.ControllerActionCollection.Count == 0)
                table.ControllerActionCollection.AddRange(ObjectBuilder.GetObject<IDeveloperService>().ActionCodeGenerators);
        }

        private async Task GetChildTablesAsync(SqlConnection conn, params TableDefinition[] tables)
        {
            var tasks = tables.Select(x => this.GetChildTablesAsync(x, conn));
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

        public async Task<IEnumerable<TableDefinition>> GetViewOptionsAsync(bool ommitDetails = true)
        {
            var list = new List<TableDefinition>();
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var tableCommand = new SqlCommand(Resources.SelectTablesSql.Replace("'U'", "'V'"), conn))
            {
                await conn.OpenAsync();
                using (var reader = await tableCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var view = new TableDefinition
                        {
                            Name = reader.GetString(0),
                            Schema = reader.GetString(1),
                            AllowDelete = false,
                            AllowInsert = false,
                            AllowRead = true,
                            AllowUpdate = false,
                            IsSelected = false,
                            Type = "View"
                        };
                        list.AddOrReplace(view, x => x.Name == view.Name && x.Schema == view.Schema);
                    }
                }
            }

            return list;
        }

        private async Task<IEnumerable<SqlOperationDefinition>> ReadOperationsAsync(SqlConnection conn)
        {
            // get the sprocs      

            var excludeNames = new[] { "SqlQueryNotificationStoredProcedure", "aspnet_" };

            var selectSprocSql =
                $@"
SELECT 
    s.name as 'schema', o.name as 'name', o.type as 'type' 
FROM 
    sys.all_objects o
INNER JOIN 
    sys.schemas s ON o.schema_id = s.schema_id
WHERE 
    [type] IN ( 'TF', 'P', 'FN', 'IF')
AND 
    s.name NOT IN ('sys')
AND
    o.[name] NOT LIKE {excludeNames.ToString("\r\nAND\r\n o.[name] NOT LIKE", x => $"'{x}%'")}";

            var functions = new List<SqlOperationDefinition>();
            using (var spocCommand = new SqlCommand(selectSprocSql, conn))
            {
                using (var reader = await spocCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var sp = await this.CreateAsync(reader.GetString(2), reader.GetString(0), reader.GetString(1));
                        functions.AddOrReplace(sp, x => x.Name == sp.Name && x.Schema == sp.Schema);
                    }
                }
            }

            return functions;
        }



        private async Task ReadPrimaryKeysAsync(SqlConnection conn, params TableDefinition[] tables)
        {
            var tasks = from t in tables
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

        private async Task ReadColumnsAsync(SqlConnection conn, params TableDefinition[] tables)
        {
            var developerService = ObjectBuilder.GetObject<SqlAdapterDeveloperService>();
            using (var columnCommand = new SqlCommand(Resources.SelectColumnsSql, conn))
            {
                using (var reader = await columnCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var table = tables.SingleOrDefault(x => x.Name == (string)reader["Table"] && (string)reader["Schema"] == x.Schema);
                        if (null == table) continue;
                        var mt = ColumnMetadata.Read(reader, table);

                        var scores = (from g in developerService.ColumnGenerators
                                      let s = g.Metadata.GetScore(mt)
                                      where s >= 0
                                      orderby s descending
                                      select g).ToList();
                        var generator = scores.FirstOrDefault();
                        if (null == generator)
                            throw new InvalidOperationException($"Cannot find column generator for {mt}");
                        var col = generator.Value.Initialize(this, table, mt);
                        var existingColumn = table.ColumnCollection.SingleOrDefault(x => x.Name == col.Name);
                        col.IsSelected = existingColumn?.IsSelected ?? false;

                        table.ColumnCollection.AddOrReplace(col, x => x.Name == col.Name);
                    }
                }
            }
        }

        public async Task<IEnumerable<TableDefinition>> GetTableOptionsAsync(bool ommitDetails = false)
        {
            var list = new ObjectCollection<TableDefinition>();
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var tableCommand = new SqlCommand(Resources.SelectTablesSql, conn))
            {
                await conn.OpenAsync();
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
                            IsSelected = false,
                            Type = "Table"
                        };
                        list.AddOrReplace(table, x => x.Name == table.Name && x.Schema == table.Schema);
                    }
                }
                if (ommitDetails) return list;
                await ReadColumnsAsync(conn, list.ToArray());
                var primaryKeyTask = ReadPrimaryKeysAsync(conn, list.ToArray());
                var childTableTask = GetChildTablesAsync(conn, list.ToArray());
                await Task.WhenAll(primaryKeyTask, childTableTask);
            }

            return list;
        }
        public async Task<TableDefinition> GetTableOptionDetailsAsync(string schema, string name)
        {
            var table = new TableDefinition
            {
                Name = name,
                Schema = schema,
                AllowDelete = true,
                AllowInsert = true,
                AllowRead = true,
                AllowUpdate = true,
                IsSelected = false
            };
            using (var conn = new SqlConnection(this.ConnectionString))
            {
                await conn.OpenAsync();
                await ReadColumnsAsync(conn, table);
                var primaryKeyTask = ReadPrimaryKeysAsync(conn, table);
                var childTableTask = GetChildTablesAsync(conn, table);
                await Task.WhenAll(primaryKeyTask, childTableTask);
            }

            return table;
        }

        public async Task<IEnumerable<OperationDefinition>> GetOperationOptionsAsync()
        {
            var list = new ObjectCollection<OperationDefinition>();
            using (var conn = new SqlConnection(this.ConnectionString))
            {
                await conn.OpenAsync();
                var operations = await ReadOperationsAsync(conn);
                list.AddRange((operations));
            }
            return list;
        }

    }
}
