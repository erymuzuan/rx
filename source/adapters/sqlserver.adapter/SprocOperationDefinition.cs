using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters.Columns;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SprocOperationDefinition : SqlOperationDefinition
    {
        public override async Task InitializeRequestMembersAsync(SqlServerAdapter adapter)
        {/*
            if the type is unicode, the length is twice 
            select * from information_schema.PARAMETERS
where SPECIFIC_NAME = @name
order by ORDINAL_POSITION*/
            string sql = $@"
select  
	-- DO NOT USE parameter_id as this value changes, and doesnot track the old object 
	'{Schema}-{Name}-' + name  as 'Id',
    name as 'Column',
    TYPE_NAME(user_type_id) as 'Type',
    max_length as 'Length',
    is_output,
    has_default_value,
    is_nullable as 'IsNullable',
    cast(0 as bit) as 'IsIdentity',
	cast(0 as bit) as 'IsComputed'
 from sys.parameters where object_id = object_id('[{Schema}].[{Name}]')

";

            var uuid = Guid.NewGuid().ToString();

            MethodName = Name.ToCsharpIdentitfier();
            Uuid = uuid;
            CodeNamespace = this.CodeNamespace;
            WebId = uuid;

            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var col = await reader.ReadColumnAsync(adapter);
                        if ((bool)reader["is_output"])
                            this.ResponseMemberCollection.Add(col);
                        else
                            this.RequestMemberCollection.Add(col);
                    }
                }
            }

            var retVal = new IntColumn
            {
                Name = "@return_value",
                DisplayName = "@return_value",
                Type = typeof(int),
                WebId = $"{Schema}-{Name}-@return_value"
            };
            this.ResponseMemberCollection.Add(retVal);
            await this.SuggestReturnResultset(adapter);
        }


        private async Task SuggestReturnResultset(SqlServerAdapter adapter)
        {
            var parameters = this.RequestMemberCollection.Concat(this.ResponseMemberCollection.Where(x => x.Name != "@return_value"));
            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand($@"
set fmtonly on
exec [{Schema}].[{Name}] {parameters.ToString(", ", x => x.Name)}
set fmtonly off
", conn))
            {
                cmd.CommandType = CommandType.Text;
                foreach (var p in this.RequestMemberCollection)
                {
                    cmd.Parameters.AddWithValue(p.Name, 1);
                }

                foreach (var p in this.ResponseMemberCollection.Where(x => x.Name != "@return_value"))
                {
                    cmd.Parameters.AddWithValue(p.Name, 442).Direction = ParameterDirection.Output;
                }

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var dt = reader.GetSchemaTable();
                    if (null == dt) return;
                    var resultSet = new ComplexMember { Name = "_results", TypeName = $"{MethodName}Row", AllowMultiple = true, WebId = $"{Schema}-{Name}-results" };
                    foreach (DataRow colMetadata in dt.Rows)
                    {
                        var colReader = new Dictionary<string, object>
                        {
                            {"Id", colMetadata["ColumnName"]},
                            {"Column", colMetadata["ColumnName"]},
                            {"Type", colMetadata["DataTypeName"]},
                            {"Length", colMetadata["NumericPrecision"]},
                            {"IsNullable", colMetadata["AllowDBNull"]},
                            {"IsIdentity", false},
                            {"IsComputed", false},
                        };
                        var member = await colReader.ReadColumnAsync(adapter);
                        resultSet.MemberCollection.Add(member);
                    }
                    this.ResponseMemberCollection.AddRange(resultSet);
                }
            }
        }

        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            var code = new StringBuilder();


            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLine($"           using(var cmd = new SqlCommand(\"[{Schema}].[{Name}]\", conn))");
            code.AppendLine("           {");
            code.AppendLine("               cmd.CommandType = CommandType.StoredProcedure;");
            foreach (var member in this.RequestMemberCollection)
            {
                var m = (SqlColumn)member;
                code.AppendLinf(m.GenerateUpdateParameterValue(itemIdentifier: "request"));
            }

            // TODO : OUT parameter
            foreach (var m in this.ResponseMemberCollection.Where(x => x.Name != "@return_value").OfType<SqlColumn>())
            {
                code.Append($"cmd.Parameters.Add(\"{m.ToSqlParameter()}\", SqlDbType.{m.SqlType}).Direction = ParameterDirection.Output;");
            }
            var returnParameter = (SqlColumn)this.ResponseMemberCollection.Single(x => x.Name == "@return_value");

            code.AppendLine($@"
            SqlParameter retval = cmd.Parameters.Add(""{returnParameter.Name}"", SqlDbType.Int);
            retval.Direction = ParameterDirection.ReturnValue;");
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               var row = await cmd.ExecuteNonQueryAsync();");
            code.AppendLinf("               var response = new {0}Response();", this.MethodName.ToCsharpIdentitfier());
            code.AppendLine($@"             response.{returnParameter.ClrName} = (int)cmd.Parameters[""{returnParameter.Name}""].Value;");
            foreach (var m in this.ResponseMemberCollection)
            {
                if (m.Name == "@return_value") continue;
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
                    code.AppendLinf("                       response.{0}.Add(item);", m.Name);
                    code.AppendLine("                   }");
                    code.AppendLine("               }");
                    continue;
                }
                var srm = m as SqlColumn;
                if (null != srm)
                    code.AppendLine($"               response.{m.Name} = ({srm.Type.ToCSharp()})cmd.Parameters[\"{m.Name}\"].Value;");
            }

            code.AppendLine("               return response;");
            code.AppendLine("           }");
            return code.ToString();
        }
    }
}