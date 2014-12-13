using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class EntityDefinitionDeletedSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName
        {
            get { return "ed_deleted"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityDefinition).Name + ".#.deleted" }; }
        }


        protected async override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            await RemoveSqlTablesAsync(item);
            await RemoveElasticSearchMappingsAsync(item);
            await RemoveOutputAsync(item);
        }

        private Task RemoveOutputAsync(EntityDefinition item)
        {
            this.WriteError(new Exception("Remove all the dll and pdbs for " + item));
            return Task.FromResult(0);
        }

        private Task RemoveElasticSearchMappingsAsync(EntityDefinition item)
        {
            this.WriteError(new Exception("Remove ElasticSearch type and mappings for " + item));
            return Task.FromResult(0);
        }

        private static async Task RemoveSqlTablesAsync(EntityDefinition item)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["sph"].ConnectionString;
            var applicationName = ConfigurationManager.ApplicationName;
            var existingTablesSql =
                string.Format(
                    "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{0}'  AND  TABLE_NAME LIKE '{1}_%'",
                    applicationName, item.Name);
            var dropTable = string.Format("DROP TABLE [{0}].[{1}]", applicationName, item.Name);
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(existingTablesSql, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            using (var dcmd = new SqlCommand(string.Format("{0}", reader.GetSqlString(0))))
                            {
                                await dcmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }


                using (var dcmd = new SqlCommand(dropTable))
                {
                    await dcmd.ExecuteNonQueryAsync();
                }
            }
        }


        public Task ProcessMessageAsync(EntityDefinition ed)
        {
            return this.ProcessMessage(ed, null);
        }
    }
}
