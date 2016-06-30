using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Xml;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.Xml, }, IsNullable = ThreeWayBoolean.False)]
    public class XmlColumn : SqlColumn
    {
        public override Type ClrType => typeof(XmlDocument);
        private string VariableName => Name.ToCamelCase()+"Xml";

        public override string GenerateValueStatementCode(string dbValue)
        {
            return $@"                         
                    var {VariableName} = new System.Xml.XmlDocument();
                    {VariableName}.LoadXml((string){dbValue});";
        }

        public override string GenerateValueAssignmentCode(string dbValue)
        {
            return VariableName;
        }
        
        public override string GenerateUpdateParameterValue(string commandName = "cmd", string itemIdentifier = "item")
        {
            return $"{commandName}.Parameters.AddWithValue(\"@{ClrName}\", {itemIdentifier}.{ClrName}.OuterXml);";

        }
    }
}