using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.SqlRepository.Compilers;
using Bespoke.Sph.SqlRepository.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.SqlRepository.Deployments
{
    [Export(typeof(IProjectDeployer))]
    public class SqlTableDeployer : SqlTableTool, IProjectDeployer
    {

        [ImportMany(typeof(IProjectBuilder))]
        public IProjectBuilder[] ProjectBuilders { get; set; }

        public SqlTableDeployer() : base(null)
        {
        }

        public SqlTableDeployer(Action<string> writeMessage, Action<string> writeWarning = null,
            Action<Exception> writeError = null) : base(writeMessage, writeWarning, writeError)
        {
        }

        // applica
        public async Task<bool> CheckForAsync(IProjectDefinition project)
        {
            await Task.Delay(100);
            if (!(project is EntityDefinition ed)) return false;
            if (ed.TreatDataAsSource) return false; // for sources use SqlTableWithSourceDeployer
            if (ed.Transient) return false; 
            return ed.GetPersistenceOption().IsSqlDatabase;
        }

        public async Task<RxCompilerResult> DeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int sqlBatchSize = 50)
        {
            if (!(project is EntityDefinition ed)) return null;

            var cr = new RxCompilerResult();
            var connectionString = ConfigurationManager.SqlConnectionString;

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var oldTable = await RenameExistingTable(conn, ed);
                await CreateTableAsync(conn, ed);
                await CreateIndicesAsync(conn, ed);
                if (string.IsNullOrWhiteSpace(oldTable))
                    return cr;

                await MigrateDataAsync(ed, sqlBatchSize, oldTable, migration, true);
            }

            return cr;
        }

        public Task<RxCompilerResult> TestDeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int batchSize = 50)
        {
            throw new NotImplementedException();
        }


        public async Task TestMigrationAsync(string migrationPlan, string outputFolder)
        {
            await Task.Delay(500);
            /*
            var builder = new SqlTableDeployer(WriteMessage, WriteWarning, WriteError);
            var plan = MigrationPlan.ParseFile(migrationPlan);

            void Migration(JObject json, dynamic item)
            {
                foreach (var change in plan.ChangeCollection)
                {
                    change.Migrate(item, json);
                }

                File.WriteAllText($"{outputFolder}\\{item.Id}.json", JsonSerializerService.ToJson(item));
            }

            await builder.MigrateDataAsync(m_entityDefinition, 20, m_entityDefinition.Name, Migration);
            */
        }

        private async Task CreateIndicesAsync(SqlConnection conn, EntityDefinition ed)
        {
            var createIndex = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\", $"{ed.Name}.Index.*.sql")
                .Select(File.ReadAllText);
            foreach (var s in createIndex)
            {
                using (var createIndexCommand = new SqlCommand(s, conn))
                {
                    await createIndexCommand.ExecuteNonQueryAsync();
                }
            }
        }

        private static async Task CreateTableAsync(SqlConnection conn, EntityDefinition ed)
        {
            var source = $@"{ConfigurationManager.SphSourceDirectory}\{nameof(EntityDefinition)}\{ed.Name}.sql";
            if (!File.Exists(source))
                throw new InvalidOperationException("Please build for sql source");
            var createTable = File.ReadAllText(source);
            using (var createTableCommand = new SqlCommand(createTable, conn))
            {
                await createTableCommand.ExecuteNonQueryAsync();
            }
        }

        private async Task<string> RenameExistingTable(SqlConnection conn, EntityDefinition ed)
        {
            var oldTable = $"{ed.Name}_{DateTime.Now:yyyyMMdd_HHmmss}";
            var tableExistSql =
                $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{ConfigurationManager.ApplicationName}'  AND  TABLE_NAME = '{ed.Name}'";
            var existingTableCount = conn.GetDatabaseScalarValue<int>(tableExistSql);

            if (existingTableCount <= 0) return null;

            // TODO : even if the SQL table has not changed, the data schema might have changed
            // Verify that the table has changed
            var changed = HasSchemaChanged(ed);
            if (!changed) return oldTable;


            // rename table for migration
            using (var renameTableCommand =
                new SqlCommand("sp_rename", conn) { CommandType = CommandType.StoredProcedure })
            {
                renameTableCommand.Parameters.AddWithValue("@objname", $"[{ConfigurationManager.ApplicationName}].[{ed.Name}]");
                renameTableCommand.Parameters.AddWithValue("@newname", oldTable);
                //renameTableCommand.Parameters.AddWithValue("@objtype", "OBJECT");

                await renameTableCommand.ExecuteNonQueryAsync();
            }

            return oldTable;
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
            var compiler = this.ProjectBuilders.OfType<SqlTableBuilder>().Single();
            var members = ed.GetFilterableMembers(compiler).ToList();
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

    }
}