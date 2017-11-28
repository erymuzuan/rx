using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;

namespace Bespoke.Sph.SqlRepository
{
    [Export(typeof(IProjectBuilder))]
    public class SqlTableBuilder : SqlTableTool, IProjectBuilder
    {
        public SqlTableBuilder() : base(null)
        {
        }

        public SqlTableBuilder(Action<string> writeMessage, Action<string> writeWarning = null,
            Action<Exception> writeError = null) : base(writeMessage, writeWarning, writeError)
        {
        }


        public async Task<Dictionary<string, string>> GenerateCodeAsync(IProjectDefinition project)
        {
            var sources = new Dictionary<string, string>();
            if (!(project is EntityDefinition item)) return sources;


            var applicationName = ConfigurationManager.ApplicationName;
            var version = await GetSqlServerProductVersionAsync();

            var sql = new StringBuilder();
            sql.Append($"CREATE TABLE [{applicationName}].[{item.Name}]");
            sql.AppendLine("(");
            sql.AppendLine("  [Id] VARCHAR(50) PRIMARY KEY NOT NULL");
            var members = item.GetFilterableMembers().ToArray();
            foreach (var member in members.OfType<SimpleMember>())
            {
                sql.AppendLine("," + member.GenerateColumnExpression(version));
            }
            sql.AppendLine(",[Json] VARCHAR(MAX)");
            sql.AppendLine(",[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[CreatedBy] VARCHAR(255) NULL");
            sql.AppendLine(",[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[ChangedBy] VARCHAR(255) NULL");
            sql.AppendLine(")");

            sources.Add($"{project.Name}.sql", sql.ToString());

            var indices = from m in members
                let index = m.CreateIndex(item, version)
                select (FileName:$"{item.Name}.Index.{m.Name}.sql",  Source:index);
            foreach (var id in indices)
            {
                sources.Add(id.FileName, id.Source);
            }

            return sources;
        }

        public Task<RxCompilerResult> BuildAsync(IProjectDefinition project, string[] sources)
        {
            var cr = new RxCompilerResult
            {
                Result =  true,
                Output = $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\{project.Name}.sql"
            };
            return Task.FromResult(cr);
        }

        public bool IsAvailableInDesignMode => true;
        public bool IsAvailableInBuildMode => true;
        public bool IsAvailableInDeployMode => false;
    }
}