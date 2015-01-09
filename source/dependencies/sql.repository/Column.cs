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
    }
}