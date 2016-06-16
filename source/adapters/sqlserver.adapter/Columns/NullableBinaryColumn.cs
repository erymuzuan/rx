using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.VarBinary, SqlDbType.Timestamp, SqlDbType.Binary, SqlDbType.Image },
        IsNullable = ThreeWayBoolean.True)]
    public class NullableBinaryColumn : SqlColumn
    {
        public override Type ClrType => typeof(byte[]);
        public override string GenerateReadCode()
        {
            return null;
        }

        public override string GeneratedCode(string padding = "      ")
        {
            var code = base.GeneratedCode(padding);
            const string IGNORE = "[Newtonsoft.Json.JsonIgnore]\r\n";
            if (this.IsComplex)
                return IGNORE + code;
            return code;

        }
    
    }
}