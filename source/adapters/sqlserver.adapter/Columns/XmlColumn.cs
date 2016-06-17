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
            if (this.IsComplex) return null;
            return $@"                         
                    var xml{Name} = new System.Xml.XmlDocument();
                    xml{Name}.LoadXml((string)reader[""{Name}""]);
                    item.{Name} = xml{Name};";

        }

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
            return $"{commandName}.Parameters.AddWithValue(\"@{Name}\", item.{Name}.OuterXml);";

        }
    }
}