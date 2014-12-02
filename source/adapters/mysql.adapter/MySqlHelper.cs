using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{


    public static class MySqlHelper
    {
        public static Type GetClrDataType(this string dbType)
        {
            var dbl = dbType.ToEmptyString().ToLowerInvariant();

            if (dbl.StartsWith("int")) return typeof(int);
            if (dbl.StartsWith("tinyint")) return typeof(short);
            if (dbl.StartsWith("smallint")) return typeof(short);
            if (dbl.StartsWith("mediumnit")) return typeof(int);
            if (dbl.StartsWith("bignit")) return typeof(long);

            if (dbl.StartsWith("decimal")) return typeof(decimal);
            if (dbl.StartsWith("double")) return typeof(double);
            if (dbl.StartsWith("float")) return typeof(float);

            if (dbl.StartsWith("binary")) return typeof(string);
            if (dbl.StartsWith("blob")) return typeof(string);
            if (dbl.StartsWith("char")) return typeof(string);
            if (dbl.StartsWith("enum")) return typeof(string);
            if (dbl.StartsWith("text")) return typeof(string);
            if (dbl.StartsWith("varbinary")) return typeof(string);
            if (dbl.StartsWith("varchar")) return typeof(string);

            if (dbl.StartsWith("date")) return typeof(DateTime);
            if (dbl.StartsWith("time")) return typeof(DateTime);
            if (dbl.StartsWith("datetime")) return typeof(DateTime);
            if (dbl.StartsWith("timestamp")) return typeof(DateTime);
            if (dbl.StartsWith("year")) return typeof(DateTime);

            throw new Exception("Cannot find entry for datatype " + dbType);
        }

        public static Type GetClrType(this SqlColumn col)
        {

            switch (col.DataType)
            {
                case "int" :return typeof (int);
                case "tinyint": return typeof(short);
                case "smallint": return typeof(short);
                case "mediumnit": return typeof(int);
                case "bignit": return typeof(long);
                case "decimal": return typeof(decimal);
                case "double": return typeof(double);
                case "float" :return typeof (float);
                
                case "binary" :return typeof (string);
                case "blob" :return typeof (string);
                case "varbinary" :return typeof (string);
                case "text" :return typeof (string);
                case "enum" :return typeof (string);
                case "char" :return typeof (string);
                case "varchar" :return typeof (string);

                case "date" :return typeof (DateTime);
                case "time" :return typeof (DateTime);
                case "datetime" :return typeof (DateTime);
                case "timestamp" :return typeof (DateTime);
                case "year" :return typeof (DateTime);
                default: throw new ArgumentException("cannot find datatype for " + col.DataType);
            }
        }
        public static string GetCSharpType(this SqlColumn col)
        {
            switch (col.DataType)
            {
                case "int": return "int";
                case "tinyint": return "short";
                case "smallint": return "short";
                case "mediumnit": return "int";
                case "bignit": return "long";
                case "decimal": return "decimal";
                case "double": return "double";
                case "float": return "float";

                case "binary": return "string";
                case "blob": return "string";
                case "varbinary": return "string";
                case "text": return "string";
                case "enum": return "string";
                case "char": return "string";
                case "varchar": return "string";

                case "date": return "DateTime";
                case "time": return "DateTime";
                case "datetime": return "DateTime";
                case "timestamp": return "DateTime";
                case "year": return "DateTime";
                default: throw new ArgumentException("cannot find datatype for " + col.DataType);
            }
        }
    }

}
