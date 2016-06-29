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
	'{Schema}-{Name}-' + cast(P.parameter_id as varchar(3))  as 'Id',
    P.is_output AS 'IsOutPutParameter',
    P.name AS 'Column',
    TYPE_NAME(P.user_type_id) AS 'Type',
    P.max_length AS [Length],
	p.is_nullable as 'IsNullable',
	cast(0 as bit) as 'IsIdentity',
	cast(0 as bit) as 'IsComputed'
FROM 
    sys.objects AS SO
INNER JOIN 
    sys.parameters AS P 
ON SO.OBJECT_ID = P.OBJECT_ID
WHERE 
   SO.OBJECT_ID IN ( SELECT OBJECT_ID FROM sys.objects WHERE TYPE = '{ObjectType}')
AND 
    SO.name = @Name
AND
	SCHEMA_NAME(SCHEMA_ID)  = @Schema
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
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Schema", Schema);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var parameter = await reader.ReadColumnAsync(adapter);
                        if ((bool)reader["IsOutPutParameter"])
                            this.ResponseMemberCollection.Add(parameter);
                        else
                            this.RequestMemberCollection.Add(parameter);

                    }
                }

            }
        }
    }
}