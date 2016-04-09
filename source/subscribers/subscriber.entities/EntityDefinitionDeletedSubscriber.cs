using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class EntityDefinitionDeletedSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName => "ed_deleted";

        public override string[] RoutingKeys => new[] { "EntityDefinition.deleted.#" };

        protected override async Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            await RemoveSqlTablesAsync(item);
            await RemoveElasticSearchMappingsAsync(item);
            var task2 = RemoveSourceAsync(item);
            var task3 = RemoveOutputAsync(item);
            await Task.WhenAll(task2, task3);
        }

        private Task RemoveSourceAsync(EntityDefinition item)
        {
            var sourceDirectory = ConfigurationManager.SphSourceDirectory;
            var web = ConfigurationManager.WebPath;

            Action<string> delete = file =>
            {
                try
                {
                    this.WriteMessage("Deleting " + file);
                    if (File.Exists(file))
                        File.Delete(file);
                }
                catch (IOException e)
                {
                    this.WriteMessage("Failed to delete " + file + "\r\n" + e.Message);
                }
            };

            delete(Path.Combine($"{sourceDirectory}\\EntityDefinition\\{item.Id}.json"));
            delete(Path.Combine($"{sourceDirectory}\\EntityDefinition\\{item.Name}.mapping"));
            delete(Path.Combine($"{web}\\SphApp\\viewmodels\\{item.Name}.js"));
            delete(Path.Combine($"{web}\\SphApp\\views\\{item.Name}.html"));

            //TODO: the forms, views and triggers - these should include all the views.html and viewmodels.js and partialjs for Form and View


            var csharpSourceFolder = Path.Combine(sourceDirectory, "\\" + item.Name);
            if (Directory.Exists(csharpSourceFolder))
                Directory.Delete(csharpSourceFolder, true);


            return Task.FromResult(0);

        }
        private Task RemoveOutputAsync(EntityDefinition item)
        {
            var output = ConfigurationManager.CompilerOutputPath;
            var web = ConfigurationManager.WebPath + "\\bin";
            var subs = ConfigurationManager.SubscriberPath;
            var app = ConfigurationManager.ApplicationName;
            var pdb = $"{app}.{item.Name}.pdb";
            var dll = $"{app}.{item.Name}.dll";

            Action<string> delete = folder =>
            {
                var pdbFile = Path.Combine(folder, pdb);
                var dllFile = Path.Combine(folder, dll);

                try
                {
                    this.WriteMessage("Deleting " + pdb);
                    if (File.Exists(pdbFile))
                        File.Delete(pdbFile);
                }
                catch (IOException e)
                {
                    this.WriteMessage("Failed to delete " + pdb + "\r\n" + e.Message);
                }
                try
                {
                    this.WriteMessage("Deleting " + dll);
                    if (File.Exists(dllFile))
                        File.Delete(dllFile);
                }
                catch (IOException e)
                {
                    this.WriteMessage("Failed to delete " + pdb + "\r\n" + e.Message);
                }
            };

            delete(output);
            delete(web);
            delete(subs);
            return Task.FromResult(0);

        }

        private async Task RemoveElasticSearchMappingsAsync(EntityDefinition item)
        {
            var url = $"{ConfigurationManager.ElasticSearchIndex}/_mapping/{item.Name}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                try
                {
                    var response = await client.DeleteAsync(url);
                    this.WriteMessage("Success ", response.StatusCode);
                }
                catch (HttpRequestException e)
                {
                    this.WriteMessage("Error in deleting ElasticSearch mapping for {0}", item.Name);
                    this.WriteError(e);
                    throw;
                }

            }

        }

        private static async Task RemoveSqlTablesAsync(EntityDefinition item)
        {
            var connectionString = ConfigurationManager.SqlConnectionString;
            var applicationName = ConfigurationManager.ApplicationName;


            var dropTable = $"DROP TABLE [{applicationName}].[{item.Name}]";
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(dropTable, conn))
            {
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            var existingTablesSql =
                string.Format(
                    "SELECT [TABLE_NAME] FROM [INFORMATION_SCHEMA].[TABLES] WHERE [TABLE_SCHEMA] = '{0}'  AND  [TABLE_NAME] LIKE '{1}_%'",
                    applicationName, item.Name);
            var tables = new List<string>();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(existingTablesSql, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }

            }

            using (var conn = new SqlConnection(connectionString))
            {
                foreach (var table in tables)
                {
                    var cmdText = string.Format("DROP TABLE [{0}].[{1}]", applicationName, table);
                    using (var cmd = new SqlCommand(cmdText,conn))
                    {
                        if (conn.State != ConnectionState.Open) await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }


        public Task ProcessMessageAsync(EntityDefinition ed)
        {
            return this.ProcessMessage(ed, null);
        }
    }
}
