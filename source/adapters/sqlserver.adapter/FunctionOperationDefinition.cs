using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public abstract class FunctionOperationDefinition : SqlOperationDefinition
    {

        public override async Task InitializeRequestMembersAsync(SqlServerAdapter adapter)
        {

            string selectParametersSql = $@"
SELECT 
    SCHEMA_NAME(SCHEMA_ID) AS [Schema], 
    SO.name AS [ObjectName],
    SO.Type_Desc AS [ObjectType (UDF/SP)],
    P.parameter_id AS [ParameterID],
    P.name AS [ParameterName],
    TYPE_NAME(P.user_type_id) AS [ParameterDataType],
    P.max_length AS [ParameterMaxBytes],
    P.is_output AS [IsOutPutParameter]
FROM 
    sys.objects AS SO
INNER JOIN 
    sys.parameters AS P 
ON SO.OBJECT_ID = P.OBJECT_ID
WHERE 
    SO.OBJECT_ID IN ( SELECT OBJECT_ID FROM sys.objects WHERE TYPE = '{ObjectType}')
AND 
    SO.name = @name
ORDER 
    BY [Schema], SO.name, P.parameter_id
";

            var uuid = Guid.NewGuid().ToString();
            MethodName = Name.ToCsharpIdentitfier();
            Uuid = uuid;
            CodeNamespace = this.CodeNamespace;
            WebId = uuid;
            ErrorRetry = new ErrorRetry { Attempt = 3, Wait = 500, Algorithm = WaitAlgorithm.Linear };

            using (var conn = new SqlConnection(adapter.ConnectionString))
            using (var cmd = new SqlCommand(selectParametersSql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@name", Name);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var dt = (string)reader["ParameterDataType"];
                        var cml = reader["ParameterMaxBytes"].ReadNullable<short>();
                        var mode = ((bool)reader["IsOutPutParameter"]) ? "OUT" : "IN";
                        var pname = (string)reader["ParameterName"];
                        var position = reader["ParameterID"].ReadNullable<int>();

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
                                Name = string.IsNullOrWhiteSpace(pname) ? "Result" : pname,
                                SqlDbType = t,
                                Type = dt.GetClrType(),
                                MaxLength = cml
                            };
                            this.ResponseMemberCollection.Add(rm);
                        }
                    }
                }

            }
        }
    }
}