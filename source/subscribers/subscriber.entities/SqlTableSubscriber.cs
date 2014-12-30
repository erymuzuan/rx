using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
        public override string QueueName
        {
            get { return "ed_sql_table"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityDefinition).Name + ".changed.Publish" }; }
        }

        private string GetSqlType(string typeName)
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
                string.Format(
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{0}'  AND  TABLE_NAME = '{1}'",
                    applicationName, item.Name);
            var createTable = this.CreateTableSql(item, applicationName);
            var oldTable = string.Format("{0}_{1:yyyyMMdd_HHmmss}", item.Name, DateTime.Now);
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
                        renameTableCommand.Parameters.AddWithValue("@objname", string.Format("[{0}].[{1}]", applicationName, item.Name));
                        renameTableCommand.Parameters.AddWithValue("@newname", oldTable);
                        //renameTableCommand.Parameters.AddWithValue("@objtype", "OBJECT");

                        await renameTableCommand.ExecuteNonQueryAsync();
                    }

                }

                WriteCreateTableSqlFile(item, createTable);
                using (var createTableCommand = new SqlCommand(createTable, conn))
                {
                    await createTableCommand.ExecuteNonQueryAsync();
                }
                if (existingTableCount == 0) return;

                this.WriteMessage("Migrating data for {0}", item.Name);

                //migrate
                var readSql = string.Format("SELECT [Id],[Json] FROM [{0}].[{1}]", applicationName, oldTable);
                this.WriteMessage(readSql);

                var builder = new Builder(this.NotificicationService) { EntityDefinition = item, Name = item.Name };
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

        private static void WriteCreateTableSqlFile(EntityDefinition item, string createTable)
        {
            var createTableFile = Path.Combine(ConfigurationManager.UserSourceDirectory,
                "EntityDefinition\\" + item.Name + ".sql");
            var createTableFileContent = string.Format(@"
USE [{0}]
GO
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = '{0}' 
                 AND  TABLE_NAME = '{1}'))
BEGIN
    exec sp_rename '[{0}.{1}]',[{0}.{1}_Original]
END
GO
{2}", ConfigurationManager.ApplicationName, item.Name, createTable);

            File.WriteAllText(createTableFile, createTableFileContent);
        }

        private bool CheckForNewColumns(EntityDefinition item)
        {
            var members = this.GetFilterableMembers("", item.MemberCollection);
            var metadataProvider = ObjectBuilder.GetObject<ISqlServerMetadata>();
            var table = metadataProvider.GetTable(item.Name);
            foreach (var mb in members)
            {
                var colType = this.GetSqlType(mb.TypeName).Replace("(255)", string.Empty);
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
                sql.AppendFormat(",[{0}] {1} {2} NULL", member.FullName, this.GetSqlType(member.TypeName), member.IsNullable ? "" : "NOT");
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
