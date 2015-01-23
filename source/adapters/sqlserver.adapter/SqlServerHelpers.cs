using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public static class SqlServerHelpers
    {

        public static string GetCSharpType(this SqlColumn column)
        {
            return column.GetClrType().ToCSharp();
        }

        public static Type GetClrType(this SqlColumn column)
        {
            var typeName = column.DataType.ToLowerInvariant();
            switch (typeName)
            {
                case "xml"://return typeof(XElement);
                case "char":
                case "nchar":
                case "ntext":
                case "text":
                case "nvarchar":
                case "varchar": return typeof(string);
                case "uniqueidentifier": return typeof(Guid);
                case "bigint": return typeof(long);
                case "tinyint": return typeof(short);
                case "int": return typeof(int);
                case "datetimeoffset":
                case "time":
                case "datetime":
                case "datetime2":
                case "smalldatetime": return typeof(DateTime);
                case "bit": return typeof(bool);
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money": return typeof(decimal);
                case "real": return typeof(float);
                case "float": return typeof(double);
            }
            ConsoleColor color = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cannot find mapping to \"{0}\"", typeName);
            }
            finally
            {
                Console.ForegroundColor = color;
            }

            return null;
        }

        public static Type GetClrType(this string sqlType)
        {
            switch (sqlType)
            {
                case "xml"://return typeof(XElement);
                case "char":
                case "nchar":
                case "ntext":
                case "text":
                case "nvarchar":
                case "varchar": return typeof(string);
                case "uniqueidentifier": return typeof(Guid);
                case "bigint": return typeof(long);
                case "tinyint": return typeof(short);
                case "int": return typeof(int);
                case "datetimeoffset":
                case "time":
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
            return typeof(object);
        }


    }
}
