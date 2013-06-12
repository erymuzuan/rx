namespace Bespoke.Sph.SqlRepository
{
   public class Column
    {
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