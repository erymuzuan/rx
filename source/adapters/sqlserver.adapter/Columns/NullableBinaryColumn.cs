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

        public override string GenerateUpdateParameterValue(string commandName = "cmd", string itemIdentifier = "item")
        {
            return $@"{commandName}.Parameters.Add(new SqlParameter(""@{ClrName}"", SqlDbType.{SqlType}, {Length}){{ Value = {itemIdentifier}.{ClrName}.ToDbNull()}});";
        }

        public override string GenerateValueAssignmentCode(string dbValue)
        {
            return $"{dbValue}.ReadNullableByteArray()";
        }


        public override string GenerateReadAdapterCode(TableDefinition table, Adapter adapter)
        {
            if (!this.IsComplex)
                return null;
            var pks = table.ColumnCollection.Where(x => table.PrimaryKeyCollection.Contains(x.Name)).ToArray();
            var args = pks.ToString(", ", x => $"{x.GenerateParameterCode()}");
            var predicates = pks.ToString("AND ", x => $"[{x.Name}] = @{x.Name}");
            var parameters = pks.ToString("\r\n", x => $@"cmd.Parameters.AddWithValue(""@{x.Name}"", {x.Name.ToCamelCase()});");
            var code = new StringBuilder();
            code.AppendLine($"       public async Task<{ClrType.ToCSharp()}> Get{Name}Async({args})");
            code.AppendLine("       {");
            code.AppendLine($@"           var sql = $""SELECT [{Name}] FROM [{table.Schema}].[{table.Name}] WHERE {predicates}"";");
            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLine("           using(var cmd = new SqlCommand(sql, conn))");
            code.AppendLine("           {");
            code.AppendLine("               " + parameters);

            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               var dbval = await cmd.ExecuteScalarAsync();");
            code.AppendLine("               return dbval.ReadNullableByteArray();");
            code.AppendLine("           }");

            code.AppendLine("       }");
            return code.ToString();
        }

    }
}