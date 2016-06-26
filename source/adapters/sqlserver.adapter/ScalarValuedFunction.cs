using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class ScalarValuedFunction : FunctionOperationDefinition
    {
        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            var code = new StringBuilder();
            var parameters = this.RequestMemberCollection.ToString(", ", x => x.ToSqlParameter());
            var response = (SqlColumn)this.ResponseMemberCollection.Single();

            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLine($@"           using(var cmd = new SqlCommand(""SELECT {response.ToSqlParameter()} = [{Schema}].[{MethodName}]({parameters})"", conn))");
            code.AppendLine("           {");
            code.AppendLine("               cmd.CommandType = CommandType.Text;");

            code.JoinAndAppendLine(this.RequestMemberCollection, "\r\n",
                m => $@"cmd.Parameters.AddWithValue(""{m.ToSqlParameter()}"", request.{m.Name});");

            code.AppendLine($@"var result = cmd.Parameters.Add(""{response.ToSqlParameter()}"", SqlDbType.{response.SqlType}, {response.Length});");
            code.AppendLine("result.Direction = ParameterDirection.Output;");

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               var _row = await cmd.ExecuteNonQueryAsync();");
            code.AppendLine($"              var retVal = {response.GenerateValueAssignmentCode("result.Value")};");


            code.AppendLine($"               return new {MethodName.ToCsharpIdentitfier()}Response{{ {response.ClrName} = retVal}};");
            code.AppendLine("           }");
            return code.ToString();
        }
    }
}