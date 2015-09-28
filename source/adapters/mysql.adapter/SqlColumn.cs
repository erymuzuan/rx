using System.Data;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SqlColumn
    {
        public SqlColumn()
        {

        }

        public SqlColumn(IDataRecord reader)
        {
            var dt = reader.GetString(1);
            var dt2 = dt;
            var length = 0;
            if (dt.Contains("("))
            {
                dt2 = Strings.RegexSingleValue(dt, "(?<dt>[a-z]{3,12})\\(", "dt");
                length = Strings.RegexInt32Value(dt, "\\((?<l>[0-9]{1,5})", "l") ?? 0;
            }

            this.Name = reader.GetString(0);
            this.DataType = dt2;
            this.Length = length;
            this.IsNullable = reader.GetString(2) == "YES";
            this.IsIdentity = reader.GetString(5) == "auto_increment";
            //this.IsComputed = reader.GetBoolean(6)


        }
        public string Name { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsComputed { get; set; }
        public bool IsNullable { get; set; }
        public string DataType { get; set; }
        public int Length { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}
