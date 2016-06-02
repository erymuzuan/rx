using System;
using System.Data;
using System.Text;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class ColumnMetadata
    {
        public string Name { get; private set; }
        public string SqlType { get; private set; }
        public bool IsPrimaryKey { get; private set; }
        public bool IsComputed { get; private set; }
        public bool IsIdentity { get; private set; }
        public bool IsNullable { get; private set; }
        public short Length { get; private set; }

        private ColumnMetadata() { }
        public ColumnMetadata(SqlColumn column)
        {
            Name = column.Name;
            SqlType = column.SqlType.ToString();
            IsNullable = column.IsNullable;
            IsIdentity = column.IsIdentity;
            IsComputed = column.IsComputed;
            Length = column.Length;
            IsPrimaryKey = column.IsPrimaryKey;


        }

        public static ColumnMetadata Read(IDataReader reader, TableDefinition table)
        {
            var mt = new ColumnMetadata();
            Console.WriteLine("");
            mt.Name = reader.GetString(1);
            mt.SqlType = reader.GetString(2);
            mt.Length = reader.GetInt16(3);
            mt.IsNullable = reader.GetBoolean(4);
            mt.IsIdentity = reader.GetBoolean(5);
            mt.IsComputed = reader.GetBoolean(6);
            mt.IsPrimaryKey = table.PrimaryKeyCollection.Contains(mt.Name);
            return mt;
        }


        public override string ToString()
        {
            var t = new StringBuilder();
            var nullable = this.IsNullable ? "NULL" : "NOT NULL";
            t.Append($"{Name}({SqlType} {Length}) {nullable}");

            if (this.IsComputed)
                t.AppendLine(" Computed");
            if (this.IsIdentity)
                t.Append(" Identity");
            if (this.IsPrimaryKey)
                t.Append(" PRIMARY KEY");

            return t.ToString();
        }
    }
}