using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.SqlRepository.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.SqlRepository.Deployments
{
    [Export(typeof(IProjectDeployer))]
    public class SqlTableWithSourceDeployer : SqlTableTool, IProjectDeployer
    {
        public double Order => 1.1;
        public SqlTableWithSourceDeployer() : base(null)
        {
        }

        public SqlTableWithSourceDeployer(Action<string> writeMessage, Action<string> writeWarning = null,
            Action<Exception> writeError = null) : base(writeMessage, writeWarning, writeError)
        {
        }

        public async Task<bool> CheckForAsync(IProjectDefinition project)
        {
            await Task.Delay(100);
            if (!(project is EntityDefinition ed)) return false;
            return !ed.Transient && ed.TreatDataAsSource;
        }

        public async Task<RxCompilerResult> DeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int batchSize = 50)
        {
            if (!(project is EntityDefinition ed)) return null;

            var cr = new RxCompilerResult();
            var connectionString = ConfigurationManager.SqlConnectionString;

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                await DropTableAsync(conn, ed);
                await CreateTableAsync(conn, ed);
                Logger.WriteInfo($"Succesfully created table {ed.Name} ");

                var indicesErrors = await CreateIndicesAsync(conn, ed);
                cr.Errors.AddRange(indicesErrors);
                Logger.WriteInfo(indicesErrors.Length == 0
                    ? $"All indices has been succesfully created {ed.Name} "
                    : $"{indicesErrors.Length} errors produced when creating indices ");

                var insertErrors = await this.InsertSourceAsync(conn, project);
                cr.Errors.AddRange(insertErrors);
            }

            return cr;
        }

        private async Task<BuildError[]> InsertSourceAsync(SqlConnection conn, IProjectDefinition project)
        {
            var errors = new List<BuildError>();
            //TODO :var repos = ObjectBuilder.GetObject<ISourceRepository>();
            //TODO : user ISourceRepo var sources = await repos.LoadAsync();

            foreach (var src in Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\{project.Name}", "*.json"))
            {
                try
                {
                    Logger.WriteVerbose($"Reading {src} ....");
                    var json = File.ReadAllText(src);
                    dynamic item = JsonConvert.DeserializeObject(json,
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

                    using (var cmd = new SqlCommand($@"INSERT INTO [{ConfigurationManager.ApplicationName}].[{project.Name}]
                                                ([Id], [Json], [CreatedDate], [CreatedBy], [ChangedDate], [ChangedBy])
                                                VALUES(@Id, @Json,@CreatedDate, @CreatedBy, @ChangedDate, @ChangedBy)", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.Parameters.AddWithValue("@Json", json);
                        cmd.Parameters.AddWithValue("@CreatedDate", item.CreatedDate);
                        cmd.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                        cmd.Parameters.AddWithValue("@ChangedDate", item.ChangedDate);
                        cmd.Parameters.AddWithValue("@ChangedBy", item.ChangedBy);

                        await cmd.ExecuteNonQueryAsync();
                        Logger.WriteVerbose($"Inserted {item.Id}.");
                    }
                }
                catch (SqlException exception)
                {
                    errors.Add(exception.ToBuildError());
                }
            }


            return errors.ToArray();

        }

        public Task<RxCompilerResult> TestDeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int batchSize = 50)
        {
            return RxCompilerResult.TaskEmpty;
        }
    }
}