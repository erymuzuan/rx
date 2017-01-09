using System;
using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.DateTime, SqlDbType.DateTime2, SqlDbType.SmallDateTime, }, IsNullable = ThreeWayBoolean.False)]
    public class DateTimeColumn : NonNullableColumn
    {
        public override Type ClrType => typeof(DateTime);
        public override string GenerateUpdateParameterValue(string commandName = "cmd", string itemIdentifier = "item")
        {
            if (this.IsModifiedDate)
                return $"{commandName}.Parameters.Add(\"{ClrName.ToSqlParameter()}\",SqlDbType.{SqlType} , {Length}).Value = System.DateTime.Now;";
            return base.GenerateUpdateParameterValue(commandName,itemIdentifier);
        }
        
    }
}