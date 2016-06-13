using System;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public static class SqlServerHelpers
    {

        public static Type GetClrType(this string sqlType)
        {
            var lowerer = sqlType.ToEmptyString().ToLowerInvariant();
            switch (lowerer)
            {
                case "xml": return typeof(System.Xml.XmlDocument);
                case "char":
                case "nchar":
                case "ntext":
                case "text":
                case "nvarchar":
                case "varchar": return typeof(string);
                case "varbinary":
                case "timestamp":
                case "rowversion":
                case "binary":
                case "image": return typeof(byte[]);
                case "uniqueidentifier": return typeof(Guid);
                case "bigint": return typeof(long);
                case "smallint": return typeof(short);
                case "tinyint": return typeof(byte);
                case "int": return typeof(int);
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime": return typeof(DateTime);
                case "datetimeoffset": return typeof(DateTimeOffset);
                case "time": return typeof(TimeSpan);
                case "bit": return typeof(bool);
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money": return typeof(decimal);
                case "real": return typeof(float);
                case "float": return typeof(double);
                case "sql_variant": return typeof(object);
            }
            throw new Exception($"No mapping for {sqlType}");

        }

        public static int GetScore(this IColumnGeneratorMetadata x, ColumnMetadata mt)
        {
            var score = 0;

            if (x.IsNullable == ThreeWayBoolean.True && mt.IsNullable)
                score++;
            if (x.IsNullable == ThreeWayBoolean.True && !mt.IsNullable)
                return -1;

            if (x.IsNullable == ThreeWayBoolean.False && !mt.IsNullable)
                score++;
            if (x.IsNullable == ThreeWayBoolean.False && mt.IsNullable)
                return -1;

            if (x.IsComputed == ThreeWayBoolean.True && mt.IsComputed)
                score += 10;
            if (x.IsComputed == ThreeWayBoolean.True && !mt.IsComputed)
                return -1;
            if (x.IsComputed == ThreeWayBoolean.False && !mt.IsComputed)
                score += 10;
            if (x.IsComputed == ThreeWayBoolean.False && mt.IsComputed)
                return -1;

            if (x.IsIdentity == ThreeWayBoolean.True && mt.IsIdentity)
                score += 10;
            if (x.IsIdentity == ThreeWayBoolean.True && !mt.IsIdentity)
                return -1;
            if (x.IsIdentity == ThreeWayBoolean.False && !mt.IsIdentity)
                score += 10;
            if (x.IsIdentity == ThreeWayBoolean.False && mt.IsIdentity)
                return -1;

            var loweredType = mt.SqlType.ToLowerInvariant();
            if (null != x.IncludeTypes)
            {
                var includes = x.IncludeTypes.Select(t => t.ToString().ToLowerInvariant()).ToArray();
                if (includes.Contains(loweredType))
                    score++;
                if (!includes.Contains(loweredType))
                    return -1;
            }

            if (null != x.ExcludeTypes)
            {
                var excludes = x.ExcludeTypes.Select(t => t.ToString().ToLowerInvariant()).ToArray();
                if (!excludes.Contains(loweredType))
                    score++;
                if (excludes.Contains(loweredType))
                    return -1;
            }

            return score;
        }

    }
}
