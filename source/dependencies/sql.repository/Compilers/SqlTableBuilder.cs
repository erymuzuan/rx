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

        //TODO : persist to disk in source
        private readonly Dictionary<Member, List<AttachedProperty>> m_memberProperties = new Dictionary<Member, List<AttachedProperty>>();
        public Task SaveAttachPropertiesAsycn(Member member, params AttachedProperty[] properties)
        {
            m_memberProperties.AddOrReplace(member, properties.ToList());
            return Task.FromResult(0);
        }

        public Task SaveAttachPropertiesAsycn(IProjectDefinition project, params AttachedProperty[] properties)
        {
            return Task.FromResult(0);
        }

        public Task<IEnumerable<AttachedProperty>> GetAttachPropertiesAsycn(IProjectDefinition project)
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

        public Task<IEnumerable<AttachedProperty>> GetAttachPropertiesAsycn(Member member)
        {
            var properties = new List<AttachedProperty>();
            properties.AddRange(member.GenerateAttachedProperties());
            foreach (var prop in properties)
            {
                if (!m_memberProperties.ContainsKey(member)) continue;
                var persistedProperties = m_memberProperties[member];
                var pp = persistedProperties.SingleOrDefault(x => x.Name == prop.Name);


                if (null == pp) continue;

                prop.ValueAsString = pp.ValueAsString;
                prop.Value = pp.Value;
            }
            return Task.FromResult(properties.AsEnumerable());
        }

        public async Task<IEnumerable<Class>> GenerateCodeAsync(IProjectDefinition project)
        {
            var sources = new List<Class>();
            if (!(project is EntityDefinition item)) return Array.Empty<Class>();


            var applicationName = ConfigurationManager.ApplicationName;
            var version = await GetSqlServerProductVersionAsync();

            var sql = new StringBuilder();
            sql.Append($"CREATE TABLE [{applicationName}].[{item.Name}]");
            sql.AppendLine("(");
            sql.AppendLine("  [Id] VARCHAR(50) PRIMARY KEY NOT NULL");
            var members = item.GetFilterableMembers().ToArray();
            foreach (var member in members.OfType<SimpleMember>())
            {
                var properties = await this.GetAttachPropertiesAsycn(member);
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
                          let properties = this.GetAttachPropertiesAsycn(m).Result
                          let index = m.CreateIndex(item, properties.ToArray(), version)
                          select new Class(index){FileName = $"{item.Name}.Index.{m.Name}.sql"};
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