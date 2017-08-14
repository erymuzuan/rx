using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;
using Bespokse.Sph.ElasticsearchRepository;
using Console = Colorful.Console;

namespace Bespoke.Sph.Mangements
{
    public class DeploymentMetadata
    {
        private readonly EntityDefinition m_entityDefinition;
        public DateTime DateTime { get; set; }
        public string Tag { get; set; }
        public string Revision { get; set; }

        public DeploymentMetadata(EntityDefinition entityDefinition)
        {
            m_entityDefinition = entityDefinition;
        }


        public async Task<IList<DeploymentHistory>> QueryAsync()
        {
            var list = new List<DeploymentHistory>();
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand(@"SELECT [Name], [Tag],[Revision], MAX([datetime]) as 'DateTime'
            FROM[Sph].[DeploymentMetadata]
            GROUP BY [Name], [Tag], [Revision]
            ORDER BY MAX([DateTime]) DESC", conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var row = new DeploymentHistory();
                        row.Name = reader.GetString(0);
                        row.Tag = reader.GetString(1);
                        row.Revision = reader.GetString(2);
                        row.DateTime = reader.GetDateTime(3);

                        list.Add(row);
                    }

                }

            }

            return list;
        }

        public async Task BuildAsync(bool truncate, bool nes)
        {
            var ed = m_entityDefinition;

            var lastDeployedDate = await this.GetLastDeployedDateTimeAsyc();
            var hasChanges = lastDeployedDate < m_entityDefinition.ChangedDate;

            var tableBuilder = new TableSchemaBuilder(WriteMessage);
            if (hasChanges)
                await tableBuilder.BuildAsync(ed);


            if (ed.TreatDataAsSource)
            {
                var sourceMigrator = new SourceTableBuilder(WriteMessage, WriteWarning, WriteError);
                if (truncate)
                    await sourceMigrator.CleanAndBuildAsync(ed);
                else
                    await sourceMigrator.BuildAsync(ed);

            }

            using (var mappingBuilder = new MappingBuilder(WriteMessage, WriteWarning, WriteError))
            {
                if (!nes)
                {
                    await mappingBuilder.DeleteMappingAsync(ed);
                    await mappingBuilder.BuildAllAsync(ed);
                }

            }

            await InsertDeploymentMetadataAsync();

            if (!hasChanges)
                Console.WriteLine($"\"{m_entityDefinition.Name}\" was last deployed on {lastDeployedDate} and the source has not changed since");

            Console.WriteLine($@"{ed.Name} was succesfully deployed ");
        }

        public async Task<DateTime?> GetLastDeployedDateTimeAsyc()
        {
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand($"SELECT MAX(DateTime) FROM [Sph].[DeploymentMetadata] WHERE [Name] = '{m_entityDefinition.Name}'", conn))
            {
                await conn.OpenAsync();
                var val = await cmd.ExecuteScalarAsync();
                if (val == DBNull.Value) return default(DateTime);
                return (DateTime)val;
            }
        }

        private async Task InsertDeploymentMetadataAsync()
        {
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand(@"
INSERT INTO [Sph].[DeploymentMetadata]( Name, EdId, Tag, Revision, [Source])
VALUES ( @Name, @EdId, @Tag, @Revision, @Source)
", conn))
            {
                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = m_entityDefinition.Name;
                cmd.Parameters.Add("@EdId", SqlDbType.VarChar, 255).Value = m_entityDefinition.Id;
                cmd.Parameters.Add("@Tag", SqlDbType.VarChar, 255).Value = this.GetGitHeadComment() ;
                cmd.Parameters.Add("@Revision", SqlDbType.VarChar, 255).Value = this.GetGitHeadCommitId();
                cmd.Parameters.Add("@Source", SqlDbType.VarChar, -1).Value = m_entityDefinition.ToJsonString();
                await conn.OpenAsync();


                await cmd.ExecuteNonQueryAsync();
            }
        }

        private string GetGitHeadCommitId()
        {
            //git rev-parse --short HEAD 
            var git = new ProcessStartInfo("git.exe", "rev-parse --short HEAD")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var p = Process.Start(git);
            var output = p?.StandardOutput.ReadToEnd();
            p?.WaitForExit();
            return output;
        }

        private string GetGitHeadComment()
        {
            //git log -1
            var git = new ProcessStartInfo("git.exe", "log -1")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            // Redirect the output stream of the child process.
            var p = Process.Start(git);
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            var output = p?.StandardOutput.ReadToEnd();
            p?.WaitForExit();
            return output;
        }

        private static void WriteMessage(string m)
        {
            Console.WriteLine($@"{DateTime.Now:T} : {m}", Color.Cyan);
        }

        private static void WriteWarning(string m)
        {
            Console.WriteLine($@"{DateTime.Now:T} : {m}", Color.Yellow);
        }

        private static void WriteError(Exception m)
        {
            Console.WriteLine($@"{DateTime.Now:T} : {m}", Color.OrangeRed);
        }


        public static async Task InitializeAsync()
        {
            const string SQL = @"
CREATE TABLE [Sph].[DeploymentMetadata](
     [Id] INT PRIMARY KEY NOT NULL IDENTITY(1,1)
    ,[Name] VARCHAR(255)  NULL
    ,[EdId] VARCHAR(255) NOT NULL
    ,[Tag] VARCHAR(255) NOT NULL
    ,[Revision] VARCHAR(255)  NULL
    ,[DateTime] SMALLDATETIME NOT NULL DEFAULT GETDATE()
    ,[Source] VARCHAR(MAX)
)

                ";
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var checkTableCommand = new SqlCommand(@"SELECT COUNT(*)
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Sph' 
                 AND  TABLE_NAME = 'DeploymentMetadata'", conn))
            {

                await conn.OpenAsync();
                var exist = (int)await checkTableCommand.ExecuteScalarAsync() == 1;
                if (!exist)
                {
                    using (var createTableCommand = new SqlCommand(SQL, conn))
                    {
                        await createTableCommand.ExecuteNonQueryAsync();
                    }
                }


            }
        }

    }
}