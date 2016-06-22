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
    public class TableValuedFunction : FunctionOperationDefinition
    {
        public override async Task InitializeRequestMembersAsync(SqlServerAdapter adapter)
        {
            await base.InitializeRequestMembersAsync(adapter);
            if (ObjectType == "TF")
            {
                // TODO : set the response member
                this.ResponseMemberCollection.Clear();

                string selectParametersSql = $@"
SELECT 
    name,TYPE_NAME(user_type_id) as 'data_type', max_length, precision, scale, is_nullable
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

                    var resultSet = new ComplexMember { AllowMultiple = true, Name = $"{MethodName}Results", TypeName = $"{MethodName}Resultset" };
                    this.ResponseMemberCollection.Add(resultSet);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dt = (string)reader["data_type"];
                            var max = reader["max_length"].ReadNullable<short>();
                            var name = (string)reader["name"];
                            var nullable = (bool)reader["is_nullable"];
                            
                            SqlDbType t;
                            Enum.TryParse(dt, true, out t);
                            var rm = new SprocResultMember
                            {
                                Name = name,
                                SqlDbType = t,
                                Type = dt.GetClrType(),
                                MaxLength = max,
                                IsNullable = nullable
                            };
                            resultSet.MemberCollection.Add(rm);

                        }
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