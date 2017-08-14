using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

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

        public TableSchemaBuilder(Action<string> writeMessage, Action<string> writeWarning = null, Action<Exception> writeError = null)
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
        public async Task BuildAsync(EntityDefinition ed)
        {

            if (ed.Transient) return;
            if (ed.StoreInDatabase.HasValue && ed.StoreInDatabase.Value == false) return;

            var connectionString = ConfigurationManager.SqlConnectionString;
            var applicationName = ConfigurationManager.ApplicationName;
            var tableExistSql =
                $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{applicationName}'  AND  TABLE_NAME = '{ed.Name}'";
            var createTable = await this.CreateTableSqlAsync(ed, applicationName);
            var createIndex = this.CreateIndexSql(ed, applicationName);
            var oldTable = $"{ed.Name}_{DateTime.Now:yyyyMMdd_HHmmss}";
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                int existingTableCount;
                using (var cmd = new SqlCommand(tableExistSql, conn))
                {
                    existingTableCount = (int)(await cmd.ExecuteScalarAsync());
                }
                if (existingTableCount > 0)
                {
                    // TODO : even if the SQL table has not changed, the data schema might have changed
                    // Verify that the table has changed
                    var changed = HasSchemaChanged(ed);
                    if (!changed) return;


                    // rename table for migration
                    using (var renameTableCommand = new SqlCommand("sp_rename", conn) { CommandType = CommandType.StoredProcedure })
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
                    // save to disk for source
                    var file = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(EntityDefinition)}\\{ed.Name}.sql";
                    System.IO.File.WriteAllText(file, createTable);
                }
                foreach (var s in createIndex)
                {
                    using (var createIndexCommand = new SqlCommand(s, conn))
                    {
                        await createIndexCommand.ExecuteNonQueryAsync();
                    }
                }
                if (existingTableCount == 0) return;

                this.WriteMessage("Migrating data for {0}", ed.Name);

                //TODO : migrate in batches #6223
                var readSql = $"SELECT [Id],[Json] FROM [{applicationName}].[{oldTable}]";
                int total;
                var row = 0;
                const int BATCH_SIZE = 20;
                using (var cmd = new SqlCommand("SELECT COUNT(*)  FROM [{applicationName}].[{oldTable}]"))
                {
                    total = (int)await cmd.ExecuteScalarAsync();
                }
                this.WriteMessage(readSql);

                var builder = new Builder { EntityDefinition = ed, Name = ed.Name };
                builder.Initialize();

                while (row <= total)
                {
                    this.WriteMessage($"Migrating batch of {row} of total {total}");
                    using (var cmd = new SqlCommand($"{readSql} ORDER BY [CreatedDateTime] OFFSET {row} ROWS FETCH NEXT {BATCH_SIZE} ROWS ONLY", conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                var id = reader.GetString(0);
                                var json = reader.GetString(1);
                                this.WriteMessage($"Sql migration from {oldTable} to {ed.Name}");
                                var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                                dynamic ent = JsonConvert.DeserializeObject(json, setting);
                                ent.Id = id;
                                //
                                await builder.InsertAsync(ent);
                            }
                        }
                    }
                    row += BATCH_SIZE;
                }
            }
        }


        private static string GetSqlType(string typeName)
        {
            switch (typeName)
            {
                case "System.String, mscorlib": return "VARCHAR(255)";
                case "System.Int32, mscorlib": return "INT";
                case "System.DateTime, mscorlib": return "SMALLDATETIME";
                case "System.Decimal, mscorlib": return "MONEY";
                case "System.Double, mscorlib": return "FLOAT";
                case "System.Boolean, mscorlib": return "BIT";
            }
            return "VARCHAR(255)";
        }

        public IEnumerable<Member> GetFilterableMembers(string parent, IList<Member> members)
        {
            var filterables = new ObjectCollection<Member>();
            var simples = members.OfType<SimpleMember>().Where(m => m.IsFilterable)
                .Where(m => m.Type != typeof(object))
                .Where(m => m.Type != typeof(Array))
                .ToList();
            var list = members.OfType<ComplexMember>()
                .Select(m => this.GetFilterableMembers(parent + m.Name + ".", m.MemberCollection)).ToList()
                .SelectMany(m =>
                {
                    var enumerable = m as Member[] ?? m.ToArray();
                    return enumerable;
                })
                .ToList();
            filterables.AddRange(simples);
            filterables.AddRange(list);

            filterables.Where(m => string.IsNullOrWhiteSpace(m.FullName) || !m.FullName.EndsWith(m.Name))
                .ToList().ForEach(m => m.FullName = parent + m.Name);

            return filterables;

        }

        private bool HasSchemaChanged(EntityDefinition ed)
        {
            var members = this.GetFilterableMembers("", ed.MemberCollection).ToList();
            var metadataProvider = ObjectBuilder.GetObject<ISqlServerMetadata>();
            var table = metadataProvider.GetTable(ed.Name);

            // compare the members againts column
            foreach (var mb in members.OfType<SimpleMember>())
            {
                var colType = GetSqlType(mb.TypeName).Replace("(255)", string.Empty);
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
                    && string.Equals(GetSqlType(m.TypeName).Replace("(255)", string.Empty), col1.SqlType, StringComparison.InvariantCultureIgnoreCase)
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

        private string[] CreateIndexSql(EntityDefinition item, string applicationName)
        {
            var members = this.GetFilterableMembers("", item.MemberCollection);
            var sql = from m in members.OfType<SimpleMember>()
                      let column = m.FullName ?? m.Name
                      select
                      $@"
CREATE NONCLUSTERED INDEX [{column}_index]
ON [{applicationName}].[{item.Name}] ([{column}]) ";

            return sql.ToArray();


        }

        private async Task<string> CreateTableSqlAsync(EntityDefinition item, string applicationName)
        {
            int? version;
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand("SELECT SERVERPROPERTY('ProductVersion')", conn))
            {
                await conn.OpenAsync();
                var pv = await cmd.ExecuteScalarAsync();
                version = Strings.RegexInt32Value($"{pv}", @"(?<version>[0-9]{1,2})..*", "version");
            }
            var sql = new StringBuilder();
            sql.Append($"CREATE TABLE [{applicationName}].[{item.Name}]");
            sql.AppendLine("(");
            sql.AppendLine("  [Id] VARCHAR(50) PRIMARY KEY NOT NULL");
            var members = this.GetFilterableMembers("", item.MemberCollection);
            foreach (var member in members.OfType<SimpleMember>())
            {
                // TODO : #4510 If SQL server version 13 and above is used,  Filtered member should be computed column
                Console.WriteLine($@"SQL Server version {version} ");
                sql.AppendLine($",[{member.FullName}] {GetSqlType(member.TypeName)} {(member.IsNullable ? "" : "NOT")} NULL");
            }
            sql.AppendLine(",[Json] VARCHAR(MAX)");
            sql.AppendLine(",[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[CreatedBy] VARCHAR(255) NULL");
            sql.AppendLine(",[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[ChangedBy] VARCHAR(255) NULL");
            sql.AppendLine(")");

            return sql.ToString();
        }


    }
}
