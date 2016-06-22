using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var response = this.ResponseMemberCollection.OfType<SprocResultMember>().Single();

            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLine($@"           using(var cmd = new SqlCommand(""SELECT {response.ToSqlParameter()} = [{Schema}].[{MethodName}]({parameters})"", conn))");
            code.AppendLine("           {");
            code.AppendLine("               cmd.CommandType = CommandType.Text;");

            code.JoinAndAppendLine(this.RequestMemberCollection, "\r\n",
                m => $@"cmd.Parameters.AddWithValue(""{m.ToSqlParameter()}"", request.{m.Name});");

            code.AppendLine($@"var result = cmd.Parameters.Add(""{response.ToSqlParameter()}"", SqlDbType.{response.SqlDbType}, {response.MaxLength});");
            code.AppendLine("result.Direction = ParameterDirection.Output;");

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               var row = await cmd.ExecuteNonQueryAsync();");
            code.AppendLine(response.Type == typeof(string)
                ? "var response = result.Value.ReadNullableString();"
                : $"var response = ({response.Type.ToCSharp()})result.Value;");


            code.AppendLine($"               return new {MethodName.ToCsharpIdentitfier()}Response{{ {response.Name} = response}};");
            code.AppendLine("           }");
            return code.ToString();
        }
    }
}