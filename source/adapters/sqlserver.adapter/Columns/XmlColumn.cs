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

        public override string GenerateValueStatementCode(string dbValue)
        {
            return $@"                         
                    var xml{Name} = new System.Xml.XmlDocument();
                    xml{Name}.LoadXml((string){dbValue});";
        }

        public override string GenerateValueAssignmentCode(string dbValue)
        {
            return $"xml{Name}";
        }
        
        public override string GenerateUpdateParameterValue(string commandName = "cmd", string itemIdentifier = "item")
        {
            return $"{commandName}.Parameters.AddWithValue(\"@{ClrName}\", {itemIdentifier}.{ClrName}.OuterXml);";

        }
    }
}