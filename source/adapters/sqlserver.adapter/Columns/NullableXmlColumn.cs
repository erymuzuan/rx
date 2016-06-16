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
        public override string GenerateReadCode()
        {
            return $"item.{Name} = reader[\"{Name}\"].ReadNullableXmlDocument();";
        }

        public override string GeneratedCode(string padding = "      ")
        {
            var code = base.GeneratedCode(padding);
            const string IGNORE = "[Newtonsoft.Json.JsonIgnore]\r\n";
            if (this.IsComplex)
                return IGNORE + code;
            return code;

        }

        public override string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            return $"{commandName}.Parameters.AddWithValue(\"@{Name}\", item.{Name}?.OuterXml.ToDbNull());";

        }
       
    }
}
