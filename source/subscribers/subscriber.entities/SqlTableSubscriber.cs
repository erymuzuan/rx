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
            get { return new[] { typeof(EntityDefinition).Name + ".added.#" }; }
        }

        protected async override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["sph"].ConnectionString;
            var applicationName = ConfigurationManager.ApplicationName;

            var sql = new StringBuilder();
            sql.AppendFormat("CREATE TABLE [{0}].[{1}]", applicationName, item.Name);
            sql.AppendLine("(");
            sql.AppendLinf("  [{0}Id] INT PRIMARY KEY IDENTITY(1,1)");
            var members = item.MemberCollection.Where(m => m.IsFilterable);
            foreach (var member in members)
            {
                sql.AppendFormat(",[{0}] {1} {2} NULL", member.Name, member.TypeName, member.IsRequired ? "NOT" : "");
                sql.AppendLine("");
            }
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
