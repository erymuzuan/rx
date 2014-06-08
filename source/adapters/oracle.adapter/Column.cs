using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class Column
    {
        public Column()
        {
            this.Direction = ParameterDirection.Input;
        }

        public string Name { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsComputed { get; set; }
        public bool IsIdentity { get; set; }
        public int Length { get; set; }
        public decimal? Scale { get; set; }
        public decimal? Precision { get; set; }
        public object Value { get; set; }
        public OracleDbType Type { get; set; }
        public ParameterDirection Direction { get; set; }
    }
}