using System;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class FuncOperationDefinition : OperationDefinition
    {
        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            var code = new StringBuilder();
            var parameters = new[] {"TODO: get t he parameters here from Request object"};

            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLine($@"           using(var cmd = new SqlCommand(""SELECT @Value FROM [{Schema}].[{MethodName}]({parameters})"", conn))");
            code.AppendLine("           {");
            code.AppendLine("               cmd.CommandType = CommandType.Text;");
            foreach (var m in this.RequestMemberCollection.OfType<SprocParameter>())
            {
                code.AppendLinf("               cmd.Parameters.AddWithValue(\"{0}\", request.{0});", m.Name);
            }
            foreach (var m in this.ResponseMemberCollection.OfType<SprocResultMember>())
            {
                if (m.Type == typeof(Array)) continue;
                if (m.Type == typeof(object)) continue;
                if (m.Name == "@return_value") continue;
                code.AppendLinf(
                    "               cmd.Parameters.Add(\"{0}\", SqlDbType.{1}).Direction = ParameterDirection.Output;",
                    m.Name, m.SqlDbType);
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               var row = await cmd.ExecuteNonQueryAsync();");
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

            code.AppendLine("               return response;");
            code.AppendLine("           }");
            return code.ToString();
        }
    }
}