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
    public class SprocOperationDefinition : SqlOperationDefinition
    {
        public override async Task InitializeRequestMembersAsync(SqlServerAdapter adapter)
        {
            const string SQL = @"
select * from information_schema.PARAMETERS
where SPECIFIC_NAME = @name
order by ORDINAL_POSITION";

            var uuid = Guid.NewGuid().ToString();

            MethodName = Name.ToCsharpIdentitfier();
            Uuid = uuid;
            CodeNamespace = this.CodeNamespace;
            WebId = uuid;

            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand(SQL, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@name", Name);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var dt = (string)reader["DATA_TYPE"];
                        var cml = reader["CHARACTER_MAXIMUM_LENGTH"].ReadNullable<int>();
                        var mode = (string)reader["PARAMETER_MODE"];
                        var pname = (string)reader["PARAMETER_NAME"];
                        var position = reader["ORDINAL_POSITION"].ReadNullable<int>();

                        var member = new SprocParameter
                        {
                            Name = pname,
                            FullName = pname,
                            SqlType = dt,
                            Type = dt.GetClrType(),
                            IsNullable = cml == 0,
                            MaxLength = cml,
                            Mode = mode == "IN" ? ParameterMode.In : ParameterMode.Out,
                            Position = position ?? 0,
                            WebId = Guid.NewGuid().ToString()
                        };
                        if (mode == "IN" || mode == "INOUT")
                            this.RequestMemberCollection.Add(member);
                        if (mode == "OUT" || mode == "INOUT")
                        {
                            SqlDbType t;
                            Enum.TryParse(dt, true, out t);
                            var rm = new SprocResultMember
                            {
                                Name = pname,
                                SqlDbType = t,
                                Type = dt.GetClrType()
                            };
                            this.ResponseMemberCollection.Add(rm);
                        }
                    }
                }

            }


            var retVal = new SprocResultMember
            {
                Name = "@return_value",
                Type = typeof(int),
                SqlDbType = SqlDbType.Int
            };
            this.ResponseMemberCollection.Add(retVal);
        }

        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            var code = new StringBuilder();


            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLine($"           using(var cmd = new SqlCommand(\"[{Schema}].[{MethodName}]\", conn))");
            code.AppendLine("           {");
            code.AppendLine("               cmd.CommandType = CommandType.StoredProcedure;");
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