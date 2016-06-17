using System;
using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.Char, SqlDbType.NChar, SqlDbType.VarChar, SqlDbType.NVarChar, SqlDbType.Text, SqlDbType.NText },
        IsNullable = ThreeWayBoolean.True)]
    public class NullableStringColumn : SqlColumn
    {
        public override Type ClrType => typeof(string);
        public override string GenerateValueAssignmentCode(string dbValue)
        {
            return $"{dbValue}.ReadNullableString()";

        }
    }
}