using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    public partial class ColumnMetadata : DomainObject
    {
        public string Name { get; private set; }
        public string DbType { get; private set; }
        public bool IsPrimaryKey { get; private set; }
        public bool IsComputed { get; private set; }
        public bool IsIdentity { get; private set; }
        public bool IsNullable { get; private set; }
        public short Length { get; private set; }

        private ColumnMetadata() { }
        public ColumnMetadata(Column column, string type = null)
        {
            Name = column.Name;
            WebId = column.WebId;
            IsNullable = column.IsNullable;
            IsIdentity = column.IsIdentity;
            IsComputed = column.IsComputed;
            Length = column.Length;
            IsPrimaryKey = column.IsPrimaryKey;
            if (!string.IsNullOrWhiteSpace(type))
                this.DbType = type;
        }

        public static ColumnMetadata Read(IDataReader reader)
        {
            var mt = new ColumnMetadata();
            Console.WriteLine("");
            mt.WebId = reader["Id"].ToString();
            mt.Name = (string)reader["Column"];
            mt.DbType = (string)reader["Type"];
            mt.Length = Convert.ToInt16(reader["Length"]);
            mt.IsNullable = (bool)reader["IsNullable"];
            mt.IsIdentity = (bool)reader["IsIdentity"];
            mt.IsComputed = (bool)reader["IsComputed"];
            return mt;
        }
        public static ColumnMetadata Read(IDictionary<string, object> reader)
        {
            var mt = new ColumnMetadata();
            Console.WriteLine("");
            mt.WebId = reader["Id"].ToString();
            mt.Name = (string)reader["Column"];
            mt.DbType = (string)reader["Type"];
            mt.Length = Convert.ToInt16(reader["Length"]);
            mt.IsNullable = (bool)reader["IsNullable"];
            mt.IsIdentity = (bool)reader["IsIdentity"];
            mt.IsComputed = (bool)reader["IsComputed"];
            return mt;
        }


        public override string ToString()
        {
            var t = new StringBuilder();
            var nullable = this.IsNullable ? "NULL" : "NOT NULL";
            t.Append($"{Name}({DbType} {Length}) {nullable}");

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