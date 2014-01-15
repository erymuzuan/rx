using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class SqlTableSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName
        {
            get { return "ed_sql_table"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityDefinition).Name + ".changed.Publish" }; }
        }

        private string GetSqlType(string typeName)
        {
            switch (typeName)
            {
                case "System.String, mscorlib": return "VARCHAR(255)";
                case "System.Int32, mscorlib": return "INT";
                case "System.DateTime, mscorlib": return "SMALLDATETIME";
                case "System.Decimal, mscorlib": return "MONEY";
                case "System.Double, mscorlib": return "FLOAT";
                case "System.Boolean, mscorlib": return "BIT";
            }
            return "VARCHAR(255)";
        }

        public IEnumerable<Member> GetFiltarableMembers(string parent, IList<Member> members)
        {
            var filterables = members.Where(m => m.IsFilterable)
                .Where(m => m.TypeName != "System.Object, mscorlib")
                .Where(m => m.TypeName != "System.ArrayList, mscorlib")
                .ToList();
            var list = members.Where(m => m.TypeName == "System.Object, mscorlib")
                .Select(m => this.GetFiltarableMembers(parent + m.Name + ".", m.MemberCollection))
                .SelectMany(m => m)
                .ToList();
            filterables.AddRange(list);

            filterables.Where(m=> string.IsNullOrWhiteSpace(m.FullName))
                .ToList().ForEach(m => m.FullName = parent + m.Name);

            return filterables;

        }
        protected async override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["sph"].ConnectionString;
            var applicationName = ConfigurationManager.ApplicationName;

            var sql = new StringBuilder();
            sql.AppendFormat("CREATE TABLE [{0}].[{1}]", applicationName, item.Name);
            sql.AppendLine("(");
            sql.AppendLinf("  [{0}Id] INT PRIMARY KEY IDENTITY(1,1)", item.Name);
            var members = this.GetFiltarableMembers("", item.MemberCollection);
            foreach (var member in members)
            {
                sql.AppendFormat(",[{0}] {1} {2} NULL", member.FullName, this.GetSqlType(member.TypeName), member.IsNullable ? "" : "NOT");
                sql.AppendLine("");
            }
            sql.AppendLine(",[Json] VARCHAR(MAX)");
            sql.AppendLine(",[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[CreatedBy] VARCHAR(255) NULL");
            sql.AppendLine(",[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[ChangedBy] VARCHAR(255) NULL");
            sql.AppendLine(")");

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql.ToString(), conn))
            {
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

            }

        }
    }
}
