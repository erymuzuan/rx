using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.SqlRepository.Extensions;

namespace Bespoke.Sph.SqlRepository.Compilers
{
    [Export(typeof(IProjectBuilder))]
    public class SqlTableBuilder : SqlTableTool, IProjectBuilder
    {
        public string Name => "SqlServer2016";
        public string Description => @"Compile EntityDefintion to Sql Server 2016 schema";
        public SqlTableBuilder() : base(null)
        {
        }

        public SqlTableBuilder(Action<string> writeMessage, Action<string> writeWarning = null,
            Action<Exception> writeError = null) : base(writeMessage, writeWarning, writeError)
        {
        }

        public Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(IProjectDefinition project)
        {
            var properties = new List<AttachedProperty>
            {
                new AttachedProperty
                {
                    Name = "InMemory",
                    Type = typeof(bool),
                    Help = "Create the table in memory OLT"
                }
            };
            return Task.FromResult(properties.AsEnumerable());
        }

        public Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(Member member)
        {
            var properties = member.GenerateAttachedProperties();
            return Task.FromResult(properties.AsEnumerable());
        }



        public async Task<IEnumerable<Class>> GenerateCodeAsync(IProjectDefinition project)
        {
            var sources = new List<Class>();
            if (!(project is EntityDefinition item)) return Array.Empty<Class>();

            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var applicationName = ConfigurationManager.ApplicationName;
            var version = await GetSqlServerProductVersionAsync();

            var sql = new StringBuilder();
            // TODO : AttachedProperty allow InMemory table
            sql.AppendLine($@"
CREATE TABLE [{applicationName}].[{item.Name}]
(
    [Id] VARCHAR(50) PRIMARY KEY NOT NULL");
            var members = item.GetFilterableMembers(this).ToArray();
            foreach (var member in members.OfType<SimpleMember>())
            {
                var properties = await repos.GetAttachedPropertiesAsync(this, item, member);
                sql.AppendLine("    ," + member.GenerateColumnExpression(properties.ToArray(), version));
            }
            
            // TODO : JSON should be VARCHAR(MAX) or NVARCHAR(MAX), if AttachedProperty AllowUnicode is true
                sql.AppendLine(@"    ,[Json] VARCHAR(MAX)
    ,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
    ,[CreatedBy] VARCHAR(255) NULL
    ,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
    ,[ChangedBy] VARCHAR(255) NULL
)");

            sources.Add(new Class(sql.ToString()) { FileName = $"{project.Name}.sql", TrackedInSourceControl = true });

            var indices = from m in members
                          let properties = repos.GetAttachedPropertiesAsync(this, item, m).Result.ToArray()
                          select m.CreateIndex(item, properties.ToArray(), version);
            var index = new Class(indices.ToString("\r\nGO\r\n"))
            {
                FileName = $"{item.Name}.Indices.sql",
                TrackedInSourceControl = true
            };
            sources.Add(index);

            return sources;
        }

        public Task<RxCompilerResult> BuildAsync(IProjectDefinition project, Func<IProjectDefinition, CompilerOptions2> getOptions)
        {
            return RxCompilerResult.TaskEmpty;
        }

        public bool IsAvailableInDesignMode => true;
        public bool IsAvailableInBuildMode => true;
        public bool IsAvailableInDeployMode => false;
    }
}