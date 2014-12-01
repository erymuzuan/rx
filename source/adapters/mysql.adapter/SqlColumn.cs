namespace Bespoke.Sph.Integrations.Adapters
{
    public class SqlColumn
    {
        public string Name { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsComputed { get; set; }
        public bool IsNullable { get; set; }
        public string DataType { get; set; }
        public int Length { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}
