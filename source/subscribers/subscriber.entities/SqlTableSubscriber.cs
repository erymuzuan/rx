using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;

namespace subscriber.entities
{
    public class SqlTableSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName => "ed_sql_table";

        public override string[] RoutingKeys => new[] { typeof(EntityDefinition).Name + ".changed.Publish" };

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
            var filterables = members.Where(m => m.IsFilterable)
                .Where(m => m.Type != typeof(object))
                .Where(m => m.Type != typeof(Array))
                .ToList();
            var list = members.Where(m => m.Type == typeof(object))
                .Select(m => this.GetFilterableMembers(parent + m.Name + ".", m.MemberCollection)).ToList()
                .SelectMany(m =>
                {
                    var enumerable = m as Member[] ?? m.ToArray();
                    return enumerable;
                })
                .ToList();
            filterables.AddRange(list);

            filterables.Where(m => string.IsNullOrWhiteSpace(m.FullName))
                .ToList().ForEach(m => m.FullName = parent + m.Name);

            return filterables;

        }

        protected async override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["sph"].ConnectionString;
            var applicationName = ConfigurationManager.ApplicationName;
            var tableExistSql =
                $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{applicationName}'  AND  TABLE_NAME = '{item.Name}'";
            var createTable = this.CreateTableSql(item, applicationName);
            var oldTable = $"{item.Name}_{DateTime.Now:yyyyMMdd_HHmmss}";
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
                    // Verify that the table has changed
                    var ok = CheckForNewColumns(item);
                    if (ok) return;


                    // rename table for migration
                    using (var renameTableCommand = new SqlCommand("sp_rename", conn) { CommandType = CommandType.StoredProcedure })
                    {
                        renameTableCommand.Parameters.AddWithValue("@objname", $"[{applicationName}].[{item.Name}]");
                        renameTableCommand.Parameters.AddWithValue("@newname", oldTable);
                        //renameTableCommand.Parameters.AddWithValue("@objtype", "OBJECT");

                        await renameTableCommand.ExecuteNonQueryAsync();
                    }

                }

                using (var createTableCommand = new SqlCommand(createTable, conn))
                {
                    await createTableCommand.ExecuteNonQueryAsync();
                }
                if (existingTableCount == 0) return;

                this.WriteMessage("Migrating data for {0}", item.Name);

                //migrate
                var readSql = $"SELECT [Id],[Json] FROM [{applicationName}].[{oldTable}]";
                this.WriteMessage(readSql);

                var builder = new Builder { EntityDefinition = item, Name = item.Name };
                builder.Initialize();

                using (var cmd = new SqlCommand(readSql, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetString(0);
                            var json = reader.GetString(1);
                            this.WriteMessage("Migrating {0} : {1}", item.Name, id);
                            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                            dynamic ent = JsonConvert.DeserializeObject(json, setting);
                            ent.Id = id;
                            //
                            await builder.InsertAsync(ent);
                        }

                    }
                }



            }

        }

        private bool CheckForNewColumns(EntityDefinition item)
        {
            var members = this.GetFilterableMembers("", item.MemberCollection).ToList();
            var metadataProvider = ObjectBuilder.GetObject<ISqlServerMetadata>();
            var table = metadataProvider.GetTable(item.Name);

            // compare the members againts column
            foreach (var mb in members)
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
                    return false;
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
                var member = members.SingleOrDefault(m =>
                            m.Name.Equals(col1.Name, StringComparison.InvariantCultureIgnoreCase)
                            && string.Equals(GetSqlType(m.TypeName).Replace("(255)", string.Empty), col1.SqlType, StringComparison.InvariantCultureIgnoreCase)
                            && col.IsNullable == m.IsNullable);
                if (null == member)
                {
                    this.WriteMessage("[Member-COMPARE] - > Cannot find member {0} as {1}", col.Name, col.SqlType);
                    return false;
                }
            }
            this.WriteMessage($"No schema changes detected in [{ConfigurationManager.ApplicationName}].[{item.Name}]");
            return true;
        }


        private string CreateTableSql(EntityDefinition item, string applicationName)
        {

            var sql = new StringBuilder();
            sql.AppendFormat("CREATE TABLE [{0}].[{1}]", applicationName, item.Name);
            sql.AppendLine("(");
            sql.AppendLinf("  [Id] VARCHAR(50) PRIMARY KEY NOT NULL", item.Name);
            var members = this.GetFilterableMembers("", item.MemberCollection);
            foreach (var member in members)
            {
                sql.AppendFormat(",[{0}] {1} {2} NULL", member.FullName, GetSqlType(member.TypeName), member.IsNullable ? "" : "NOT");
                sql.AppendLine("");
            }
            sql.AppendLine(",[Json] VARCHAR(MAX)");
            sql.AppendLine(",[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[CreatedBy] VARCHAR(255) NULL");
            sql.AppendLine(",[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[ChangedBy] VARCHAR(255) NULL");
            sql.AppendLine(")");

            return sql.ToString();
        }

        public Task ProcessMessageAsync(EntityDefinition ed)
        {
            return this.ProcessMessage(ed, null);
        }
    }
}
