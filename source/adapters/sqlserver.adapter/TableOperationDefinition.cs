using System;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class TableOperationDefinition : OperationDefinition
    {
        public string Table { get; set; }
        public string Schema { get; set; }
        public string Crud { get; set; }
        private string CreateMethodCode(SqlServerAdapter adapter)
        {
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<{0}> {1}{0}Async({0} request)",
                this.MethodName.ToCsharpIdentitfier());
            code.AppendLine("       {");

            return code.ToString();
        }

        public string GenerateActionCode(SqlServerAdapter adapter, string methodName)
        {
            var code = new StringBuilder();
            code.AppendLine(CreateMethodCode(adapter));


            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(\"[{0}].[{1}]\", conn))", adapter.Schema, this.MethodName);
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
                code.AppendLinf("               cmd.Parameters.Add(\"{0}\", SqlDbType.{1}).Direction = ParameterDirection.Output;", m.Name, m.SqlDbType);
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               var row = await cmd.ExecuteNonQueryAsync();");
            code.AppendLinf("               var response = new {0}Response();", this.MethodName.ToCsharpIdentitfier());
            foreach (var m in this.ResponseMemberCollection.OfType<SprocResultMember>())
            {
                if (m.Type == typeof(Array))
                {
                    code.AppendLinf("               using(var reader = await cmd.ExecuteReaderAsync())");
                    code.AppendLine("               {");
                    code.AppendLine("                   while(await reader.ReadAsync())");
                    code.AppendLine("                   {");
                    code.AppendLinf("                       var item = new {0}();", m.Name.Replace("Collection", ""));
                    foreach (var rm in m.MemberCollection.OfType<SprocResultMember>())
                    {

                        code.AppendLinf("                       item.{0} = ({1})reader[\"{0}\"];", rm.Name, rm.Type.ToCSharp());
                    }
                    code.AppendLinf("                       response.{0}.Add(item);", m.Name);
                    code.AppendLine("                   }");
                    code.AppendLine("               }");
                    continue;
                }
                if (m.Name == "@return_value") continue;
                code.AppendLinf("               response.{0} = ({1})cmd.Parameters[\"{0}\"].Value;", m.Name, m.Type.ToCSharp());
            }

            code.AppendLine("               return response;");
            code.AppendLine("           }");
            code.AppendLine("       }");
            return code.ToString();
        }

    }
}