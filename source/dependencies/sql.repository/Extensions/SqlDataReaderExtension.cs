using System;
using System.Data.SqlClient;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository.Extensions
{
    public static class SqlDataReaderExtension
    {
        public static object GetValue(this SqlDataReader reader, string path, EntityDefinition ed)
        {
            var member = ed.GetMember(path);

            var rvalue = reader[path];
            if (rvalue is DBNull)
                return null;
            if (member is SimpleMember sm && rvalue is string value)
            {
                if (sm.Type == typeof(string))
                    return value.ReadNullableString();

                if (sm.Type == typeof(DateTime) && sm.IsNullable)
                {
                    if (DateTime.TryParse(value, out var dv))
                        return dv;
                    return new DateTime?();
                }
                if (sm.Type == typeof(DateTime))
                    return DateTime.Parse(value);

                if (sm.Type == typeof(int) && sm.IsNullable)
                {
                    if (int.TryParse(value, out var dv))
                        return dv;
                    return new int?();
                }
                if (sm.Type == typeof(int))
                    return int.Parse(value);

                if (sm.Type == typeof(bool) && sm.IsNullable)
                {
                    if (bool.TryParse(value, out var dv))
                        return dv;
                    return new bool?();
                }
                if (sm.Type == typeof(bool))
                {
                    if (value == "1") return true;
                    if (value == "0") return false;
                    return bool.Parse(value);
                }

                if (sm.Type == typeof(decimal) && sm.IsNullable)
                {
                    if (decimal.TryParse(value, out var dv))
                        return dv;
                    return new decimal?();
                }
                if (sm.Type == typeof(decimal))
                    return (decimal.Parse(value));


                if (sm.Type == typeof(double) && sm.IsNullable)
                {
                    if (double.TryParse(value, out var dv))
                        return dv;
                    return new double?();
                }
                if (sm.Type == typeof(double))
                    return (double.Parse(value));

                if (sm.Type == typeof(float) && sm.IsNullable)
                {
                    if (float.TryParse(value, out var dv))
                        return dv;
                    return new float?();
                }
                if (sm.Type == typeof(float))
                    return float.Parse(value);
            }
            return rvalue;
        }
    }
}