using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Xml;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.Xml}, IsNullable = ThreeWayBoolean.True)]
    public class NullableXmlColumn : SqlColumn
    {
        public override Type ClrType => typeof(XmlDocument);
        public override string GenerateReadCode()
        {
            return $"item.{Name} = reader[\"{Name}\"].ReadNullableXmlDocument();";
        }

        public override string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            return  $"{commandName}.Parameters.AddWithValue(\"@{Name}\", item.{Name}?.OuterXml.ToDbNull());";

        }
    }
}