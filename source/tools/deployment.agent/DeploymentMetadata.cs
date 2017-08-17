using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;
using Bespokse.Sph.ElasticsearchRepository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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




        public async Task<MigrationPlan> GetChangesAsync()
        {
            var cvs = ObjectBuilder.GetObject("CvsProvider");
            var sourceJson = $@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\{m_entityDefinition.Id}.json";

            var plan = new MigrationPlan
            {
                CurrentCommitId = cvs.GetCommitId(sourceJson)
            };
            var jsonSql = $"SELECT TOP 1 Source FROM [Sph].[DeploymentMetadata] WHERE [Name] = '{m_entityDefinition.Name}' ORDER BY [DateTime] DESC ";
            var previousCommitIdSql = $"SELECT TOP 1 [Revision] FROM [Sph].[DeploymentMetadata] WHERE [Name] = '{m_entityDefinition.Name}' ORDER BY [DateTime] DESC ";
            var deployedSql = $"SELECT TOP 1 [DateTime] FROM [Sph].[DeploymentMetadata] WHERE [Name] = '{m_entityDefinition.Name}' ORDER BY [DateTime] DESC ";
            string json;

            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            {
                await conn.OpenAsync();
                plan.PreviousCommitId = (await conn.GetScalarValueAsync(previousCommitIdSql)) as string;
                plan.DeployedDateTime = await conn.GetNullableScalarValueAsync<DateTime>(deployedSql);
                json = (await conn.GetScalarValueAsync(jsonSql)) as string;
            }

            if (string.IsNullOrWhiteSpace(json)) return plan;


            var old = json.DeserializeFromJson<EntityDefinition>();
            var changes = GetChanges(m_entityDefinition.MemberCollection, old.MemberCollection, "$", "$");
            plan.ChangeCollection.ClearAndAddRange(changes);
            return plan;


        }

        IEnumerable<MemberChange> GetChanges(IEnumerable<Member> members, IReadOnlyCollection<Member> oldMembers, string parent, string oldParent)
        {
            var changes = new List<MemberChange>();
            foreach (var mbr in members)
            {
                var existingMbr = oldMembers.FirstOrDefault(x => x.WebId == mbr.WebId);
                if (mbr.MemberCollection.Count > 0 && null != existingMbr)
                {
                    GetChanges(mbr.MemberCollection, existingMbr.MemberCollection, $"{parent}.{mbr.Name}", $"{oldParent}.{existingMbr.Name}");
                }
                var change = new MemberChange(mbr, existingMbr, parent, oldParent);
                if (!change.IsEmpty && mbr.MemberCollection.Count > 0 && null != existingMbr)
                {
                    // Complex type name change, so we got to include all the children
                    var complexChanges = from cm in mbr.MemberCollection
                                         let em = existingMbr.MemberCollection.FirstOrDefault(x => x.WebId == cm.WebId)
                                         where null != em
                                         select new MemberChange
                                         {
                                             Action = "ParentChanged",
                                             Name = mbr.Name,
                                             WebId = mbr.WebId,
                                             OldPath = $"{oldParent}.{existingMbr.Name}.{em.Name}",
                                             NewPath = $"{parent}.{mbr.Name}.{cm.Name}",
                                             OldType = em.GetMemberTypeName(),
                                             NewType = em.GetMemberTypeName(),
                                             MigrationStrategy = MemberMigrationStrategies.Direct
                                         };
                    changes.AddRange(complexChanges);
                }
                changes.Add(change);
            }

            return changes;
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
#pragma warning disable IDE0017 // Simplify object initialization
                        var row = new DeploymentHistory();
                        row.Name = reader.GetString(0);
                        row.Tag = reader.GetString(1);
                        row.Revision = reader.GetString(2);
                        row.DateTime = reader.GetDateTime(3);
#pragma warning restore IDE0017 // Simplify object initialization

                        list.Add(row);
                    }

                }

            }

            return list;
        }

        public async Task BuildAsync(bool truncate, bool nes, int sqlBatchSize)
        {
            var ed = m_entityDefinition;

            var lastDeployedDate = await this.GetLastDeployedDateTimeAsyc();
            var hasChanges = lastDeployedDate < m_entityDefinition.ChangedDate;

            var tableBuilder = new TableSchemaBuilder(WriteMessage);
            if (hasChanges)
                await tableBuilder.BuildAsync(ed, sqlBatchSize);


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
            var cvs = ObjectBuilder.GetObject("CvsProvider");
            var sourceJson = $@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\{m_entityDefinition.Id}.json";
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand(@"
INSERT INTO [Sph].[DeploymentMetadata]( Name, EdId, Tag, Revision, [Source])
VALUES ( @Name, @EdId, @Tag, @Revision, @Source)
", conn))
            {
                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = m_entityDefinition.Name;
                cmd.Parameters.Add("@EdId", SqlDbType.VarChar, 255).Value = m_entityDefinition.Id;
                cmd.Parameters.Add("@Tag", SqlDbType.VarChar, 255).Value = cvs.GetCommitComment(sourceJson);
                cmd.Parameters.Add("@Revision", SqlDbType.VarChar, 255).Value = cvs.GetCommitId(sourceJson);
                cmd.Parameters.Add("@Source", SqlDbType.VarChar, -1).Value = m_entityDefinition.ToJsonString();
                await conn.OpenAsync();


                await cmd.ExecuteNonQueryAsync();
            }
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

        public async Task TestMigrationAsync(string migrationPlan, string outputFolder)
        {
            var builder = new TableSchemaBuilder(WriteMessage, WriteWarning, WriteError);


            var jsonSource = File.ReadAllText(migrationPlan);
            var setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var plan = JsonConvert.DeserializeObject<MigrationPlan>(jsonSource, setting);
            

            void Migration(JObject json, dynamic item)
            {
                foreach (var change in plan.ChangeCollection)
                {
                    change.Migrate(item, json);
                }

                File.WriteAllText($"{outputFolder}\\{item.Id}.json", JsonSerializerService.ToJson(item));
            }

            await builder.MigrateDataAsync(m_entityDefinition, 20, m_entityDefinition.Name, Migration);
        }
    }
}