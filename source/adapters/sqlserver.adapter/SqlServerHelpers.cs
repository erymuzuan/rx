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
            return GetClrType(typeName);
        }

        public static Type GetClrType(this string sqlType)
        {
            switch (sqlType)
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
                case "real":
                case "float": return typeof(double);
                case "sql_variant": return typeof(object);
            }
            var color = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cannot find mapping to \"{0}\"", sqlType);
            }
            finally
            {
                Console.ForegroundColor = color;
            }

            return null;

        }


    }
}
