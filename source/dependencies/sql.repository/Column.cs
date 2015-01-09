using System;

namespace Bespoke.Sph.SqlRepository
{
    public class Column
    {
        public override string ToString()
        {
            return string.Format("[{0}] as {1}({2})", this.Name, this.SqlType, this.Length);
        }

        public string Name { get; set; }
        public string SqlType { get; set; }
        public int Length { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsIdentity { get; set; }
        public bool CanWrite { get; set; }
        public bool CanRead { get; set; }


        public object GetDefaultValue()
        {
            if (this.IsNullable)
                return DBNull.Value;
            switch (this.SqlType.ToLowerInvariant())
            {
                case "int":return default(int);
                case "float": return default(float);
                case "money": return default(decimal);
                case "bit":return default(bool);
                case "smalldatetime":return DateTime.Now;
                case "nvarchar(255)":
                case "nvarchar":
                case "varchar":
                case "varchar(255)": return string.Empty;
            }
            throw new Exception("No default value for " + this.SqlType);
        }
    }
}