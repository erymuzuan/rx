using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json;
using Polly;
using static System.IO.File;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class MappingBuilder : IDisposable
    {
        private readonly HttpClient m_elasticsearchHttpClient = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };
        private readonly Action<string> m_writeMessage;
        private readonly Action<string> m_writeWarning;
        private readonly Action<Exception> m_writeError;

        public MappingBuilder(Action<string> writeMessage, Action<string> writeWarning = null, Action<Exception> writeError = null)
        {
            m_writeMessage = writeMessage;
            m_writeWarning = writeWarning;
            m_writeError = writeError;
        }

        public async Task BuildAllAsync(EntityDefinition ed)
        {
            if (ed.StoreInElasticsearch.HasValue && ed.StoreInElasticsearch.Value == false) return;

            this.WriteMessage("There are differences from the existing ElasticSearch mapping");
            var result = await PutMappingAsync(ed);
            if (result)
            {
                // verify that the SQL is enabled for this EntityDefinition
                if (ed.Transient) return;
                if (ed.StoreInDatabase.HasValue && ed.StoreInDatabase.Value == false) return;

                await MigrateDataAsync(ed);

            }
        }

        public async Task ReBuildAsync(EntityDefinition ed)
        {
            if (ed.StoreInElasticsearch.HasValue && ed.StoreInElasticsearch.Value == false) return;

            // compare
            var map = ed.GetElasticsearchMapping();
            if (this.Compare(ed, map)) return;

            this.WriteMessage("There are differences from the existing ElasticSearch mapping");
            var result = await PutMappingAsync(ed);
            if (result)
            {
                this.SaveMap(ed, map);

                // verify that the SQL is enabled for this EntityDefinition
                if (ed.Transient) return;
                if (ed.StoreInDatabase.HasValue && ed.StoreInDatabase.Value == false) return;

                this.SaveMigrationMarker(ed);
            }
        }

        public async Task DeleteMappingAsync(EntityDefinition item)
        {
            var type = item.Name.ToLowerInvariant();
            var index = ConfigurationManager.ApplicationName.ToLowerInvariant();
            var url = $"{index}/_mapping/{type}";
            await m_elasticsearchHttpClient.DeleteAsync(url);
        }
        public async Task<bool> PutMappingAsync(EntityDefinition item)
        {
            var type = item.Name.ToLowerInvariant();
            var index = ConfigurationManager.ApplicationName.ToLowerInvariant();
            var url = $"{index}/_mapping/{type}";

            var map = item.GetElasticsearchMapping();
            var content = new StringContent(map);

            var response = await m_elasticsearchHttpClient.PutAsync(url, content);
            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    this.WriteMessage("creates the index for the 1st time for {0}", item.Name);
                    await m_elasticsearchHttpClient.PutAsync(index, new StringContent(""));
                    return await this.PutMappingAsync(item);
                case HttpStatusCode.BadRequest:
                    var rc = response.Content as StreamContent;
                    var text = string.Empty;
                    if (null != rc)
                        text = await rc.ReadAsStringAsync();

                    if (text.Contains("MergeMappingException") && text.Contains("of different type, current_type"))
                    {
                        this.WriteMessage("Deleting current mapping because there's different in data type and schema");
                        await m_elasticsearchHttpClient.DeleteAsync(url);
                        return await PutMappingAsync(item);

                    }
                    this.WriteError(new Exception($" Error creating Elasticsearch map for [{item.Name}]\r\n{text}"));
                    break;

                case HttpStatusCode.OK:
                    this.WriteMessage("No changes detected in your mapping");
                    break;
                default:
                    this.WriteWarning($"Unknow response type {response.StatusCode}");
                    break;
            }

            return response.StatusCode == HttpStatusCode.OK;

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



        public async Task MigrateDataAsync(EntityDefinition ed)
        {
            if (ed.Transient) return;
            var name = ed.Name;
            this.WriteWarning(name);
            this.WriteMessage("Starting data migration for " + name);
            var connectionString = ConfigurationManager.SqlConnectionString;
            var applicationName = ConfigurationManager.ApplicationName;
            int total;
            var row = 0;
            const int BATCH_SIZE = 20;

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand($"SELECT COUNT(*) FROM [{applicationName}].[{name}]", conn))
            {
                await conn.OpenAsync();
                try
                {
                    total = (int)await cmd.ExecuteScalarAsync();
                    this.WriteMessage($@"There are {total} rows in [{applicationName}].[{name}]");
                }
                catch (SqlException)
                {
                    return;
                }
            }

            var taskBuckets = new List<Task>();

            using (var conn = new SqlConnection(connectionString))
            {

                await conn.OpenAsync();//migrate
                while (row <= total)
                {
                    var readSql = $"SELECT [Id],[Json] FROM [{applicationName}].[{name}] ORDER BY [Id]  OFFSET {row} ROWS FETCH NEXT {BATCH_SIZE} ROWS ONLY";
                    this.WriteMessage(readSql);


                    using (var cmd = new SqlCommand(readSql, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                var id = reader.GetString(0);
                                var json = reader.GetString(1)
                                    .Replace($"Bespoke.{ConfigurationManager.ApplicationName}_{ed.Id}.Domain", ed.CodeNamespace)
                                    .Replace($"{ed.CodeNamespace}.HomeAddress", $"{ed.CodeNamespace}.CustomHomeAddress");
                                this.WriteMessage($"ES dumping {name} : {id}");
                                var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                                dynamic ent = JsonConvert.DeserializeObject(json, setting);
                                ent.Id = id;

                                var task = IndexItemToElasticSearchAsync(ent);
                                taskBuckets.Add(task);
                                if (taskBuckets.Count > 10)
                                {
                                    await Task.WhenAll(taskBuckets);
                                    taskBuckets.Clear();
                                }
                            }

                        }
                    }
                    row += BATCH_SIZE;
                }
            }
            if (taskBuckets.Any())
                await Task.WhenAll(taskBuckets);


        }

        private async Task IndexItemToElasticSearchAsync(Entity item)
        {
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(item, setting);

            var content = new StringContent(json);
            var id = item.Id;
            if (item.GetType().Namespace == typeof(Entity).Namespace) return;// just custom entity


            var name = item.GetType().Name.ToLowerInvariant();
            var index = ConfigurationManager.ApplicationName.ToLowerInvariant();
            var url = $"{index}/{name}/{id}";

            var c = m_elasticsearchHttpClient;
            var pr = await Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(Math.Pow(2, x) * 500))
                .ExecuteAndCaptureAsync(async () => await c.PostAsync(url, content));
            if (null != pr.FinalException)
                pr.Result.EnsureSuccessStatusCode();



        }


        private bool Compare(EntityDefinition item, string map)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = typeof(EntityDefinition);
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, item.Id + ".mapping");
            if (!Exists(file)) return false;
            return ReadAllText(file) == map;

        }

        private void SaveMap(EntityDefinition item, string map)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, item.Id + ".mapping");
            WriteAllText(file, map);


        }

        private void SaveMigrationMarker(EntityDefinition ed)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = ed.GetType();
            var folder = Path.Combine(wc, type.Name);

            var marker = Path.Combine(folder, ed.Id + ".marker");
            WriteAllText(marker, DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        public void Dispose()
        {
            m_elasticsearchHttpClient?.Dispose();
        }
    }
}
