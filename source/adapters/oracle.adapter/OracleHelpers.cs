using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Oracle.ManagedDataAccess.Client;

namespace Bespoke.Sph.Integrations.Adapters
{
    public static class OracleHelpers
    {
        public static IList<Column> AddWithValue(this IList<Column> columns, string name, object value)
        {
            var type = OracleDbType.Char;
            if (value is bool)
                type = OracleDbType.Char;
            if (value is int)
                type = OracleDbType.Int32;
            if (value is decimal)
                type = OracleDbType.Decimal;
            if (value is DateTime)
                type = OracleDbType.Date;

            var col = new Column
            {
                Name = name,
                Value = value,
                Type = type
            };
            columns.Add(col);
            return columns;
        }
        public static IList<Column> AddWithValue(this IList<Column> columns, string name, object value, OracleDbType type)
        {
            var col = new Column
            {
                Name = name,
                Value = value,
                Type = type
            };
            columns.Add(col);
            return columns;
        }

        public static string GetCSharpType(this Column column)
        {
            return column.GetClrType().ToCSharp();
        }

        public static Type GetClrType(this Column column)
        {

            var lowered = column.DataType.ToLowerInvariant();

            if (lowered == "number" && column.Scale == 2
                && column.Precision == 8)
                return typeof(double);
            if (lowered == "number" && column.Scale == 0
                && column.Precision == 4)
                return typeof(short);
            if (lowered == "number" && column.Scale == 2
                && column.Precision == 2)
                return typeof(float);
            if (lowered == "number" && column.Scale > 0)
                return typeof(decimal);

            switch (lowered)
            {
                case "xml":
                case "char":
                case "char2":
                case "varchar2":
                case "nchar":
                case "ntext":
                case "text":
                case "uniqueidentifier":
                case "nvarchar":
                case "varchar": return typeof(string);
                case "bigint": return typeof(long);
                case "tinyint": return typeof(short);
                case "number":
                case "int": return typeof(int);
                case "datetimeoffset":
                case "time":
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime": return typeof(DateTime);
                case "bit": return typeof(bool);
                case "numeric":
                case "smallmoney":
                case "money": return typeof(decimal);
                case "real":
                case "float": return typeof(double);
            }
            return null;
        }
    }
}