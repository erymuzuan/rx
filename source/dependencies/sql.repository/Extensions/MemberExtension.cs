using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class MemberExtension
    {

        public static string GenerateColumnExpression(this SimpleMember member, int? sqlVersion = 13)
        {
            if (sqlVersion < 13)
            {
                return $"[{member.FullName}] {member.GetSqlType()} {(member.IsNullable ? "" : "NOT")} NULL";
            }
            /*
             
ALTER TABLE [DevV1].[Patient]
ADD [NextOfKin.FullName] AS CAST(JSON_VALUE([Json], '$.NextOfKin.FullName') AS NVARCHAR(50))
GO

ALTER TABLE [DevV1].[Patient]
ADD [IsMalaysian] AS CASE WHEN (CAST(JSON_VALUE([Json], '$.Race') AS NVARCHAR(50))) = 'Others' THEN 0 ELSE 1 END */

            return $"[{member.FullName}] AS CAST(JSON_VALUE([Json], '$.{member.FullName}') AS {member.GetSqlType()})";
        }


        //TODO : allow attach properties to configured
        public static string GetSqlType(this SimpleMember member)
        {
            switch (member.TypeName)
            {
                case "System.String, mscorlib": return "VARCHAR(255)";
                case "System.Int32, mscorlib": return "INT";
                case "System.DateTime, mscorlib": return "SMALLDATETIME";
                case "System.Decimal, mscorlib": return "MONEY";
                case "System.Double, mscorlib": return "FLOAT";
                case "System.Boolean, mscorlib": return "BIT";
            }
            return "VARCHAR(255)";
        }

        public static string CreateIndex(this Member m, EntityDefinition ed, int? sqlVersion = 13)
        {
            var column = m.FullName ?? m.Name;

            // TODO : index may attached additional columns
            if (sqlVersion < 13)
                return $@"
CREATE NONCLUSTERED INDEX [idx_{ed.Name}{column}_index]
ON [{ConfigurationManager.ApplicationName}].[{ed.Name}] ([{column}]) ";

            return $@"CREATE INDEX idx_{ed.Name}_Json_{column.Replace(".", "")}
ON [{ConfigurationManager.ApplicationName}].[{ed.Name}]({column})  ";

        }





    }
}
