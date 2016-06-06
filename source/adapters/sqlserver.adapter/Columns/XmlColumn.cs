using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Xml;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.Xml, }, IsNullable = ThreeWayBoolean.False)]
    public class XmlColumn : SqlColumn
    {
        public override Type ClrType => typeof(XmlDocument);

        public override string GenerateReadCode()
        {
            return $@"                         
                    var xml{Name} = new System.Xml.XmlDocument();
                    xml{Name}.LoadXml((string)reader[""{Name}""]);
                    item.{Name} = xml{Name};";

        }

        public override string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            return $"{commandName}.Parameters.AddWithValue(\"@{Name}\", item.{Name}.OuterXml);";

        }
    }
}