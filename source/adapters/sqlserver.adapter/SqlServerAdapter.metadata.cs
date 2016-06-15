using System;
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
                await ReadTablesAsync(conn);
                await ReadColumnsAsync(conn);
                await ReadPrimaryKeysAsync(conn);
                await ReadStoreProceduresAsync(conn);
            }


            return true;
        }

        private static async Task ReadStoreProceduresAsync(SqlConnection conn)
        {
            // get the sprocs
            var excludeNames = new[] { "SqlQueryNotificationStoredProcedure", "aspnet_" };
            var selectSprocSql =
                $@"SELECT * FROM sys.all_objects 
                                            WHERE
                                                [name] NOT LIKE {excludeNames
                    .ToString("\r\nAND\r\n [name] NOT LIKE", x => $"'{x}%'")}";
            using (var spocCommand = new SqlCommand(selectSprocSql, conn))
            {
                using (var reader = await spocCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        //var sp = await this.GetStoreProcedureAsync(adapter, reader.GetString(0));
                        //vm.Add(sp);
                    }
                }
            }
        }

        private async Task ReadPrimaryKeysAsync(SqlConnection conn)
        {
            foreach (var table in this.TableDefinitionCollection)
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
