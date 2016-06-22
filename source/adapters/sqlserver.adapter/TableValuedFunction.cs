using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class TableValuedFunction : OperationDefinition
    {
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
                    var readerCodes = m.MemberCollection.OfType<SprocResultMember>()
                                    .Select(x => x.GenerateReaderCode())
                                    .Where(x => !string.IsNullOrWhiteSpace(x))
                                    .ToString("\r\n");
                    code.AppendLine(readerCodes);
                    code.AppendLinf("                       response.{0}.Add(item);", m.Name);
                    code.AppendLine("                   }");
                    code.AppendLine("               }");
                    continue;
                }
                if (m.Name == "@return_value") continue;
                var srm = m as SprocResultMember;
                if (null != srm)
                    code.AppendLinf("               response.{0} = ({1})cmd.Parameters[\"{0}\"].Value;", m.Name, srm.Type.ToCSharp());
            }



            code.AppendLine($"               return response;");
            code.AppendLine("           }");
            return code.ToString();
        }
    }
}