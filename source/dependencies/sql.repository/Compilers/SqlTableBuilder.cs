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
            var attachedProperties = (await repos.GetAttachedPropertiesAsync(this, item)).ToArray();

            var sql = new StringBuilder();
            sql.Append($"CREATE TABLE [{applicationName}].[{item.Name}]");
            sql.AppendLine("(");
            sql.AppendLine("  [Id] VARCHAR(50) PRIMARY KEY NOT NULL");
            var members = item.GetFilterableMembers(this).ToArray();
            foreach (var member in members.OfType<SimpleMember>())
            {
                var properties = attachedProperties.Where(x => x.AttachedTo == member.WebId);
                sql.AppendLine("," + member.GenerateColumnExpression(properties.ToArray(), version));
            }
            sql.AppendLine(",[Json] VARCHAR(MAX)");
            sql.AppendLine(",[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[CreatedBy] VARCHAR(255) NULL");
            sql.AppendLine(",[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[ChangedBy] VARCHAR(255) NULL");
            sql.AppendLine(")");

            sources.Add(new Class(sql.ToString()) { FileName = $"{project.Name}.sql" });

            var indices = from m in members
                          let properties = attachedProperties.Where(x => x.AttachedTo == m.WebId).ToArray()
                          let index = m.CreateIndex(item, properties.ToArray(), version)
                          select new Class(index) { FileName = $"{item.Name}.Index.{m.Name}.sql" };
            sources.AddRange(indices);

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