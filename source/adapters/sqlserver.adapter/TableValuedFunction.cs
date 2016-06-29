using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class TableValuedFunction : FunctionOperationDefinition
    {
        public override async Task InitializeRequestMembersAsync(SqlServerAdapter adapter)
        {
            await base.InitializeRequestMembersAsync(adapter);

            // TODO : set the response member
            this.ResponseMemberCollection.Clear();

            string selectParametersSql = $@"
SELECT 
	'{Schema}-{Name}-' + cast(column_id as varchar(3))  as 'Id',
    name as 'Column',
    TYPE_NAME(user_type_id) as 'Type', 
    max_length as 'Length',
    precision, 
    scale, 
    is_nullable as 'IsNullable',
	cast(0 as bit) as 'IsIdentity',
	cast(0 as bit) as 'IsComputed'
FROM 
    sys.columns
WHERE 
    object_id=object_id('{Schema}.{Name}')
";


            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand(selectParametersSql, conn))
            {
                cmd.CommandType = CommandType.Text;

                await conn.OpenAsync();

                var resultSet = new ComplexMember { AllowMultiple = true, Name = "_results", TypeName = $"{MethodName}Row", WebId = $"{Schema}-{Name}-results" };
                this.ResponseMemberCollection.Add(resultSet);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var rm = await reader.ReadColumnAsync(adapter);
                        resultSet.MemberCollection.Add(rm);

                    }
                }

            }


        }

        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            var code = new StringBuilder();
            var parameters = this.RequestMemberCollection.ToString(", ", x => x.ToSqlParameter());

            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLine($@"           using(var cmd = new SqlCommand(""SELECT * FROM [{Schema}].[{MethodName}]({parameters})"", conn))");
            code.AppendLine("           {");
            code.AppendLine("               cmd.CommandType = CommandType.Text;");

            code.JoinAndAppendLine(this.RequestMemberCollection, "\r\n",
                m => $@"cmd.Parameters.AddWithValue(""{m.ToSqlParameter()}"", request.{m.Name});");

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLinf("               var response = new {0}Response();", this.MethodName.ToCsharpIdentitfier());
            foreach (var m in this.ResponseMemberCollection)
            {
                var cm = m as ComplexMember;
                if (null != cm && cm.AllowMultiple)
                {
                    code.AppendLinf("               using(var reader = await cmd.ExecuteReaderAsync())");
                    code.AppendLine("               {");
                    code.AppendLine("                   while(await reader.ReadAsync())");
                    code.AppendLine("                   {");
                    code.AppendLine($"                       var item = new {cm.TypeName}();");
                    var statementsCode = m.MemberCollection.OfType<SqlColumn>()
                                    .Select(x => x.GenerateValueStatementCode($@"reader[""{x.Name}""]"))
                                    .Where(x => !string.IsNullOrWhiteSpace(x))
                                    .ToString("\r\n");
                    var assignmentCodes = m.MemberCollection.OfType<SqlColumn>()
                                    .Select(x => $"item.{x.ClrName} = {x.GenerateValueAssignmentCode($@"reader[""{x.Name}""]")};")
                                    .Where(x => !string.IsNullOrWhiteSpace(x))
                                    .ToString("\r\n");

                    code.AppendLine(statementsCode);
                    code.AppendLine(assignmentCodes);
                    code.AppendLine($"                       response.{m.Name}.Add(item);");
                    code.AppendLine("                   }");
                    code.AppendLine("               }");
                    continue;
                }
                if (m.Name == "@return_value") continue;
                var srm = m as SqlColumn;
                if (null != srm)
                    code.AppendLinf("               response.{0} = ({1})cmd.Parameters[\"{0}\"].Value;", m.Name, srm.Type.ToCSharp());
            }

            code.AppendLine("               return response;");
            code.AppendLine("           }");
            return code.ToString();
        }

    }
}