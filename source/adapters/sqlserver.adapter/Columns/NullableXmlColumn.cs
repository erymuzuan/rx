using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Xml;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.Xml }, IsNullable = ThreeWayBoolean.True)]
    public class NullableXmlColumn : SqlColumn
    {
        [JsonIgnore]
        public override Type ClrType => typeof(XmlDocument);
        public override string GenerateValueAssignmentCode(string dbValue)
        {
            return $"{dbValue}.ReadNullableXmlDocument()";
        }


        public override string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            return $"{commandName}.Parameters.AddWithValue(\"@{Name}\", item.{ClrName}?.OuterXml.ToDbNull());";

        }
       
    }
}
