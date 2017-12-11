using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.SqlRepository.Deployments
{
    internal class SourceTableBuilder
    {
        private readonly Action<string> m_writeMessage;
        private readonly Action<string> m_writeWarning;
        private readonly Action<Exception> m_writeError;

        public async Task CleanAndBuildAsync(EntityDefinition ed)
        {
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand($"TRUNCATE TABLE [{ConfigurationManager.ApplicationName}].[{ed.Name}]", conn))
            {
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                this.m_writeWarning(
                    $"Table [{ConfigurationManager.ApplicationName}].[{ed.Name}] has been truncated successfully");

                // TODO : what we do here
                var builder = new Builder { Name = ed.Name, EntityDefinition = ed };
                builder.Initialize();

                var files = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\{ed.Name}\", "*.json");
                foreach (var f in files)
                {
                    var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    dynamic ent = JsonConvert.DeserializeObject(File.ReadAllText(f), setting);
                    ent.Id = Path.GetFileNameWithoutExtension(f);
                    await builder.InsertAsync(ent);

                }
            }

        }
        public async Task BuildAsync(EntityDefinition ed)
        {

            this.m_writeWarning(
                $"Table [{ConfigurationManager.ApplicationName}].[{ed.Name}] has been truncated successfully");

            //
            var builder = new Builder { Name = ed.Name, EntityDefinition = ed };
            builder.Initialize();

            var sourcesDirectory = $@"{ConfigurationManager.SphSourceDirectory}\{ed.Name}\";
            if (!Directory.Exists(sourcesDirectory))
            {
                m_writeWarning($"The source folder for {ed.Name} does not exist");
                return;
            }
            var files = Directory.GetFiles(sourcesDirectory, "*.json");
            foreach (var f in files)
            {
                var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                dynamic ent = JsonConvert.DeserializeObject(File.ReadAllText(f), setting);
                ent.Id = Path.GetFileNameWithoutExtension(f);

                using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
                using (var cmd = new SqlCommand($"SELECT COUNT(*) FROM [{ConfigurationManager.ApplicationName}].[{ed.Name}] WHERE [Id] = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", ent.Id.ToString());
                    await conn.OpenAsync();

                    var row = (int)(await cmd.ExecuteScalarAsync());
                    if (row == 0)
                        await builder.InsertAsync(ent);

                }
            }
        }

        public SourceTableBuilder(Action<string> writeMessage, Action<string> writeWarning = null, Action<Exception> writeError = null)
        {
            m_writeMessage = writeMessage;
            m_writeWarning = writeWarning;
            m_writeError = writeError;
        }
    }
}