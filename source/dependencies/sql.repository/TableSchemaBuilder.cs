using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.SqlRepository
{
    public class TableSchemaBuilder
    {
        private readonly Action<string> m_writeMessage;
        private readonly Action<string> m_writeWarning;
        private readonly Action<Exception> m_writeError;

        public TableSchemaBuilder()
        {
        }

        public TableSchemaBuilder(Action<string> writeMessage, Action<string> writeWarning = null,
            Action<Exception> writeError = null)
        {
            m_writeMessage = writeMessage;
            m_writeWarning = writeWarning;
            m_writeError = writeError;
        }

        private void WriteMessage(string message, params object[] args)
        {
            m_writeMessage?.Invoke(string.Format(message, args));
        }

        private void WriteWarning(string message, params object[] args)
        {
            m_writeWarning?.Invoke(string.Format(message, args));
        }

        private void WriteError(Exception exception)
        {
            m_writeError?.Invoke(exception);
        }


        public async Task BuildAsync(EntityDefinition ed, Action<JObject, dynamic> migration, int sqlBatchSize = 50,
            bool deploy = false)
        {
            if (ed.Transient) return;
            if (ed.StoreInDatabase.HasValue && ed.StoreInDatabase.Value == false) return;

            var connectionString = ConfigurationManager.SqlConnectionString;
            var applicationName = ConfigurationManager.ApplicationName;
            var tableExistSql =
                $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{applicationName}'  AND  TABLE_NAME = '{ed.Name}'";

            string createTable;
            var tableSql = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(EntityDefinition)}\\{ed.Name}.sql";
            if (deploy && System.IO.File.Exists(tableSql))
                createTable = System.IO.File.ReadAllText(tableSql);
            else
                createTable = await this.CreateTableSqlAsync(ed, applicationName);

            var version = await GetSqlServerProductVersionAsync();
            var createIndex = ed.CreateIndexSql(version);

            var oldTable = $"{ed.Name}_{DateTime.Now:yyyyMMdd_HHmmss}";
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var existingTableCount = conn.GetDatabaseScalarValue<int>(tableExistSql);

                if (existingTableCount > 0)
                {
                    // TODO : even if the SQL table has not changed, the data schema might have changed
                    // Verify that the table has changed
                    var changed = HasSchemaChanged(ed);
                    if (!changed) return;


                    // rename table for migration
                    using (var renameTableCommand =
                        new SqlCommand("sp_rename", conn) { CommandType = CommandType.StoredProcedure })
                    {
                        renameTableCommand.Parameters.AddWithValue("@objname", $"[{applicationName}].[{ed.Name}]");
                        renameTableCommand.Parameters.AddWithValue("@newname", oldTable);
                        //renameTableCommand.Parameters.AddWithValue("@objtype", "OBJECT");

                        await renameTableCommand.ExecuteNonQueryAsync();
                    }
                }

                using (var createTableCommand = new SqlCommand(createTable, conn))
                {
                    await createTableCommand.ExecuteNonQueryAsync();
                }
                foreach (var s in createIndex)
                {
                    using (var createIndexCommand = new SqlCommand(s, conn))
                    {
                        await createIndexCommand.ExecuteNonQueryAsync();
                    }
                }
                if (existingTableCount == 0) return;

                await MigrateDataAsync(ed, sqlBatchSize, oldTable, migration, true);
            }
        }

        public async Task MigrateDataAsync(EntityDefinition ed, int sqlBatchSize, string oldTable,
            Action<JObject, dynamic> migration, bool insert = false)
        {
            this.WriteMessage("Migrating data for {0}", ed.Name);
            var row = 0;

            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            {
                await conn.OpenAsync();

                var builder = new Builder { EntityDefinition = ed, Name = ed.Name };
                builder.Initialize();

                var total = await conn.GetScalarValueAsync<int>(
                    $"SELECT COUNT(*)  FROM [{ConfigurationManager.ApplicationName}].[{oldTable}]");

                while (row <= total)
                {
                    this.WriteMessage($"Migrating batch of {row} of total {total}");
                    var sql =
                        $"SELECT [Id],[Json] FROM [{ConfigurationManager.ApplicationName}].[{oldTable}] ORDER BY [Id] OFFSET {row} ROWS FETCH NEXT {sqlBatchSize} ROWS ONLY";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var id = reader.GetString(0);
                                var json = reader.GetString(1);
                                this.WriteMessage($"Sql migration from {oldTable} to {ed.Name}");
                                var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                                dynamic ent = JsonConvert.DeserializeObject(json, setting);
                                ent.Id = id;
                                //
                                migration?.Invoke(JObject.Parse(json), ent);
                                if (insert)
                                    await builder.InsertAsync(ent);
                            }
                        }
                    }
                    row += sqlBatchSize;
                }
            }
        }


        private bool HasSchemaChanged(EntityDefinition ed)
        {
            var members = ed.GetFilterableMembers().ToList();
            var metadataProvider = ObjectBuilder.GetObject<ISqlServerMetadata>();
            var table = metadataProvider.GetTable(ed.Name);

            // compare the members againts column
            foreach (var mb in members.OfType<SimpleMember>())
            {
                var colType = mb.GetSqlType().Replace("(255)", string.Empty);
                var mb1 = mb;
                var col = table.Columns.SingleOrDefault(c =>
                    c.Name.Equals(mb1.FullName, StringComparison.InvariantCultureIgnoreCase) &&
                    c.SqlType.Equals(colType, StringComparison.InvariantCultureIgnoreCase)
                    && c.IsNullable == mb1.IsNullable);
                if (null == col)
                {
                    this.WriteMessage("[COLUMN-COMPARE] - > Cannot find column {0} as {1}", mb1.FullName, colType);
                    return true;
                }
            }
            // compare the columns againts column
            foreach (var col in table.Columns)
            {
                if (col.Name == "Id") continue;
                if (col.Name == "CreatedDate") continue;
                if (col.Name == "CreatedBy") continue;
                if (col.Name == "ChangedDate") continue;
                if (col.Name == "ChangedBy") continue;
                if (col.Name == "Json") continue;

                var col1 = col;
                var member = members.OfType<SimpleMember>().SingleOrDefault(m =>
                    m.Name.Equals(col1.Name, StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(m.GetSqlType().Replace("(255)", string.Empty), col1.SqlType,
                        StringComparison.InvariantCultureIgnoreCase)
                    && col.IsNullable == m.IsNullable);
                if (null == member)
                {
                    this.WriteMessage("[Member-COMPARE] - > Cannot find member {0} as {1}", col.Name, col.SqlType);
                    return true;
                }
            }
            this.WriteMessage($"No schema changes detected in [{ConfigurationManager.ApplicationName}].[{ed.Name}]");
            return false;
        }



        private async Task<string> CreateTableSqlAsync(EntityDefinition item, string applicationName)
        {
            var version = await GetSqlServerProductVersionAsync();
            var sql = new StringBuilder();
            sql.Append($"CREATE TABLE [{applicationName}].[{item.Name}]");
            sql.AppendLine("(");
            sql.AppendLine("  [Id] VARCHAR(50) PRIMARY KEY NOT NULL");
            var members = item.GetFilterableMembers();
            foreach (var member in members.OfType<SimpleMember>())
            {
                // TODO : #4510 If SQL server version 13 and above is used,  Filtered member should be computed column
                Console.WriteLine($@"SQL Server version {version} ");
                sql.AppendLine("," + member.GenerateColumnExpression(version));
            }
            sql.AppendLine(",[Json] VARCHAR(MAX)");
            sql.AppendLine(",[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[CreatedBy] VARCHAR(255) NULL");
            sql.AppendLine(",[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[ChangedBy] VARCHAR(255) NULL");
            sql.AppendLine(")");


            // save to disk for source
            var file = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(EntityDefinition)}\\{item.Name}.sql";
            System.IO.File.WriteAllText(file, sql.ToString());

            return sql.ToString();
        }

        private static async Task<int?> GetSqlServerProductVersionAsync()
        {
            int? version;
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand("SELECT SERVERPROPERTY('ProductVersion')", conn))
            {
                await conn.OpenAsync();
                var pv = await cmd.ExecuteScalarAsync();
                version = Strings.RegexInt32Value($"{pv}", @"(?<version>[0-9]{1,2})..*", "version");
            }
            return version;
        }
    }
}