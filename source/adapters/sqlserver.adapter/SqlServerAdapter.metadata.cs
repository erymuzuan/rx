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
        public async Task<IEnumerable<Change>> LoadDatabaseObjectAsync(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch (SqlException e) when (e.Number == -1)
                {
                    return Array.Empty<Change>();
                }
                var deletedTables = new ConcurrentBag<TableDefinition>();
                var deletedOperations = new ConcurrentBag<OperationDefinition>();

                var tasks = from t in this.TableDefinitionCollection
                            select RefreshTableMetadataAsync(t, deletedTables);
                var operations = from t in this.OperationDefinitionCollection
                                 select RefreshOperationMetadaAsync(t, deletedOperations);
                var changes = (await Task.WhenAll(tasks.Concat(operations))).SelectMany(x => x);

                foreach (var dt in deletedTables)
                {
                    this.TableDefinitionCollection.Remove(dt);
                }
                foreach (var dt in deletedOperations)
                {
                    this.OperationDefinitionCollection.Remove(dt);
                }

                return changes;

            }


        }

        private async Task<IEnumerable<Change>> RefreshOperationMetadaAsync(OperationDefinition operation, ConcurrentBag<OperationDefinition> deletedOperations)
        {
            //TODO : find out if the object is still in the database .. create async will always return an object
            try
            {
                await operation.RefreshMetadataAsync(this);
            }
            catch (SqlException e) when (e.Number == 2812 || e.Message.Contains("Could not find"))
            {
                deletedOperations.Add(operation);
            }

            return Array.Empty<Change>();
        }

        private async Task<IEnumerable<Change>> RefreshTableMetadataAsync(TableDefinition table, ConcurrentBag<TableDefinition> deletedTables)
        {
            var db = await this.GetTableOptionDetailsAsync(table.Schema, table.Name);
            if (null == db)
            {
                deletedTables.Add(table);
                return new[] { new Change { WebId = table.Name, Action = "Deleted" } };
            }
            var changes = new List<Change>();
            foreach (var col in db.ColumnCollection)
            {
                var oc = table.ColumnCollection.SingleOrDefault(x => x.WebId == col.WebId);
                if (null != oc)
                {
                    var list = col.Merge(oc, table);
                    changes.AddRange(list);
                }
                else
                {
                    changes.Add(new ColumnChange
                    {
                        PropertyName = "Column",
                        Table = table.Name,
                        Name = col.Name,
                        WebId = col.WebId,
                        Action = "Added",
                        NewValue = col.Name
                    });
                }
            }
            var deletedColumns = from c in table.ColumnCollection.Except(db.ColumnCollection)
                                 select new ColumnChange { Table = table.Name, Name = c.Name, PropertyName = "Column", Action = "Deleted", OldValue = c.Name, WebId = c.WebId };
            changes.AddRange(deletedColumns);
            // now refresh the table column with the one read from db, but with user's metadata intact
            table.ColumnCollection.ClearAndAddRange(db.ColumnCollection.OrderBy(c => c.Order));

            //TODO : we should merge action generators with new ones, and remove the old one
            if (table.ControllerActionCollection.Count == 0)
                table.ControllerActionCollection.AddRange(ObjectBuilder.GetObject<IDeveloperService>().ActionCodeGenerators);

            changes.OfType<ColumnChange>().ToList().ForEach(x =>
            {
                x.WebId = $"column-{table.Name}-{x.WebId}";
                x.Table = table.Name;
            });

            return changes;
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
                try
                {
                    await conn.OpenAsync();
                }
                catch (SqlException e) when (e.Number == -1)
                {
                    return new TableDefinition[] { };
                }
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
                            Type = "V"
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
                        try
                        {
                            var sp = this.CreateMetadata(reader.GetString(2), reader.GetString(0), reader.GetString(1));
                            functions.AddOrReplace(sp, x => x.Name == sp.Name && x.Schema == sp.Schema);
                        }
                        catch (Exception e)
                        {
                            // ignored
                            await ObjectBuilder.GetObject<ILogger>().LogAsync(new LogEntry(e));
                        }
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
            using (var columnCommand = new SqlCommand(Resources.SelectColumnsSql, conn))
            {
                using (var reader = await columnCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var table = tables.SingleOrDefault(x => x.Name == (string)reader["Table"] && (string)reader["Schema"] == x.Schema);
                        if (null == table) continue;
                        var mt = ColumnMetadata.Read(reader);
                        try
                        {
                            var col = await reader.ReadColumnAsync(this, table);
                            var existingColumn = table.ColumnCollection.SingleOrDefault(x => x.WebId == col.WebId);
                            col.IsSelected = existingColumn?.IsSelected ?? false;

                            table.ColumnCollection.AddOrReplace(col, x => x.WebId == col.WebId);
                        }
                        catch (Exception e)
                        {
                            var oc = table.ColumnCollection.SingleOrDefault(x => x.WebId == mt.WebId);
                            if (null != oc) oc.Unsupported = true;
                            var exc = new NotSupportedException($"Fail to initilize column [{table.Schema}].[{table.Name}].{mt}", e) { Data = { { "col", mt.ToJson() } } };
                            await ObjectBuilder.GetObject<ILogger>().LogAsync(new LogEntry(exc));
                        }
                    }
                }
            }
        }

        private async Task ReadViewColumnsAsync(SqlConnection conn, TableDefinition view)
        {
            const string SQL = @"

SELECT 
         /*o.name as 'Table'
		,s.name as 'Schema'
        ,*/c.name as 'Column'
        ,t.name as 'Type' 		
        ,c.max_length as 'Length'
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
        o.type = 'V'
        AND t.name <> N'sysname'
        AND t.is_user_defined= 0
		AND o.name = @Name
		AND s.name = @Schema
    ORDER 
        BY s.name, o.name";
            using (var columnCommand = new SqlCommand(SQL, conn))
            {
                columnCommand.Parameters.AddWithValue("@Schema", view.Schema);
                columnCommand.Parameters.AddWithValue("@Name", view.Name);
                using (var reader = await columnCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var col = await reader.ReadColumnAsync(this, view);
                        var existingColumn = view.ColumnCollection.SingleOrDefault(x => x.Name == col.Name);
                        col.IsSelected = existingColumn?.IsSelected ?? false;

                        view.ColumnCollection.AddOrReplace(col, x => x.Name == col.Name);
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
                try
                {
                    await conn.OpenAsync();
                }
                catch (SqlException e) when (e.Number == -1)
                {
                    return Array.Empty<TableDefinition>();
                }
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
                            Type = "U"
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

            const string SQL = @"
SELECT  
    o.type 
FROM 
    sys.all_objects o
INNER JOIN 
    sys.schemas s ON o.schema_id = s.schema_id
WHERE 
    o.[type] IN( 'U', 'V')
AND 
    s.name NOT IN ('sys')
AND 
    s.name = @Schema 
AND 
    o.name = @Name ";
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(SQL, conn))
            {
                cmd.Parameters.AddWithValue("@Schema", schema);
                cmd.Parameters.AddWithValue("@Name", name);

                await conn.OpenAsync();

                var dbVal = await cmd.ExecuteScalarAsync();
                if (dbVal == DBNull.Value) return null;
                var type = ((string)dbVal).Trim();
                if (type == "U")
                {
                    var table = new TableDefinition
                    {
                        Name = name,
                        Schema = schema,
                        AllowDelete = true,
                        AllowInsert = true,
                        AllowRead = true,
                        AllowUpdate = true,
                        IsSelected = false,
                        Type = "U"
                    };

                    await ReadColumnsAsync(conn, table);
                    var primaryKeyTask = ReadPrimaryKeysAsync(conn, table);
                    var childTableTask = GetChildTablesAsync(conn, table);
                    await Task.WhenAll(primaryKeyTask, childTableTask);
                    return table;
                }
                if (type == "V")
                {
                    var view = new TableDefinition
                    {
                        Name = name,
                        Schema = schema,
                        AllowDelete = false,
                        AllowInsert = false,
                        AllowRead = true,
                        AllowUpdate = false,
                        IsSelected = false,
                        Type = "V"
                    };

                    await ReadViewColumnsAsync(conn, view);
                    return view;
                }
            }
            return null;
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
