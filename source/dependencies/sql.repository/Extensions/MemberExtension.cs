using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository.Extensions
{
    public static class MemberExtension
    {
        public static AttachedProperty[] GenerateAttachedProperties(this Member member)
        {
            switch (member)
            {
                case SimpleMember sm:
                    return GenerateAttachedProperties(sm);
            }

            return Array.Empty<AttachedProperty>();
        }

        private static AttachedProperty[] GenerateAttachedProperties(this SimpleMember sm)
        {
            var properties = new List<AttachedProperty>();

            const string SQL_INDEX = "SqlIndex";
            properties.Add<bool>(sm, SQL_INDEX, "Create SQL Index", "Without this option you will not be able to filter the data using standard SQL WHERE predicates, use JSON_VALUE([Json], '$.Path') instead");
            properties.Add<string>(sm, "IndexedFields", "Attach other fields to the index", "Use commad to seperate the fields, if the index is used and other fields needed to fetch together, attaching them will avoid JSON operation")
                .WithEnableExpression(SQL_INDEX);

            if (sm.Type == typeof(string))
            {
                properties.Add<int>(sm, "Length", "NVARCHAR/VARCHAR Length")
                    .WithEnableExpression(SQL_INDEX);
                properties.Add<bool>(sm, "AllowUnicode", "NVARCHAR/VARCHAR Length", "Choosing Unicode, will create a column and index of type NVARCHAR")
                    .WithEnableExpression(SQL_INDEX);

            }

            if (sm.Type == typeof(int))
            {
                properties.Add<string>(sm, "DataType", "BIGINT, SMALLINT, TINYINT, INT")
                    .WithValidOptions("INT", "SMALLINT", "TINYINT", "BIGINT")
                    .WithEnableExpression(SQL_INDEX);
            }
            if (sm.Type == typeof(DateTime))
            {
                properties.Add<string>(sm, "DataType", "SQL DateTime data type", "If your data type required specific range and needs")
                     .WithEnableExpression(SQL_INDEX)
                     .WithValidOptions("DATETIME2", "SMALLDATETIME", "TIME", "DATE", "DATETIME", "DATETIMEOFFSET");
            }

            return properties.ToArray();
        }

        public static string GenerateColumnExpression(this SimpleMember member, AttachedProperty[] properties, int? sqlVersion = 13)
        {
            if (sqlVersion < 13)
            {
                return $"[{member.FullName}] {member.GetSqlType()} {(member.IsNullable ? "" : "NOT")} NULL";
            }

            var length = properties.GetPropertyValue<int?>("Length", default);
            return $"[{member.FullName}] AS CAST(JSON_VALUE([Json], '$.{member.FullName}') AS {member.GetSqlType(length)})";
        }


        public static string GetSqlType(this SimpleMember member, int? length = null)
        {

            switch (member.TypeName)
            {
                case "System.String, mscorlib": return $"VARCHAR({(length ?? 255)})";
                case "System.Int32, mscorlib": return "INT";
                case "System.DateTime, mscorlib": return "DATETIME2";
                case "System.Decimal, mscorlib": return "MONEY";
                case "System.Double, mscorlib": return "FLOAT";
                case "System.Boolean, mscorlib": return "BIT";
            }
            return "VARCHAR(255)";
        }

        public static string CreateIndex(this Member m, EntityDefinition ed, AttachedProperty[] properties = null, int? sqlVersion = 13)
        {
            var column = m.FullName ?? m.Name;
            var otherFields = "";

            if (null != properties)
            {
                var indexed = properties.GetPropertyValue("SqlIndex", true);
                if (!indexed)
                {
                    return "-- Indexed is set to false for " + column;
                }
                var of = properties.GetPropertyValue("IndexedFields", "");
                if (!string.IsNullOrWhiteSpace(of))
                {
                    var fields = of.Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    otherFields = ", " + fields.ToString(", ", x => $"[{x}]");
                }
            }

            if (sqlVersion < 13)
                return $@"
CREATE NONCLUSTERED INDEX [idx_{ed.Name}{column}_index]
ON [{ConfigurationManager.ApplicationName}].[{ed.Name}] ([{column}]) ";

            return $@"CREATE INDEX idx_{ed.Name}_Json_{column.Replace(".", "")}
ON [{ConfigurationManager.ApplicationName}].[{ed.Name}]([{column}]{otherFields})  ";

        }

        private static T GetPropertyValue<T>(this AttachedProperty[] properties, string name, T defaultValue)
        {
            if (null != properties)
            {
                var prop = properties.SingleOrDefault(x => x.Name == name);
                return null == prop ? default : prop.GetValue(defaultValue);
            }
            return default;
        }





    }
}
