using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class ColumnGeneratorMetadata : ExportAttribute, IColumnGeneratorMetadata
    {
        public string Name { get; set; }
        public SqlDbType[] IncludeTypes { get; set; }
        public SqlDbType[] ExcludeTypes { get; set; }
        public ThreeWayBoolean IsComputed { get; set; }
        public ThreeWayBoolean IsIdentity { get; set; }
        public ThreeWayBoolean IsNullable { get; set; }

    }
}