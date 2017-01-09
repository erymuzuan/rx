using System;
using Bespoke.Sph.Domain;
using System.Data;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SqlColumn : Column
    {
        public virtual SqlDbType SqlType { get; set; }
        public override string GenerateUpdateParameterValue(string commandName = "cmd", string itemIdentifier = "item")
        {
            var nullable = this.IsNullable ? ".ToDbNull()" : "";
            return $"{commandName}.Parameters.Add(\"{ClrName.ToSqlParameter()}\",SqlDbType.{SqlType} , {Length}).Value = {itemIdentifier}.{ClrName}{nullable};";

        }

        public override Column Initialize(Adapter adapter, TableDefinition td, ColumnMetadata mt)
        {
            var col = (SqlColumn)base.Initialize(adapter, td, mt);
            col.Name = string.IsNullOrWhiteSpace(mt.Name) ? "@return_value" : mt.Name;

            SqlDbType st;
            if (Enum.TryParse(mt.DbType, true, out st))
                col.SqlType = st;

            return col;
        }

        public override string ToString()
        {
            return new ColumnMetadata(this) + " // - " + this.GetType().FullName;
        }

        public override string GenerateReadAdapterCode(TableDefinition table, Adapter adapter)
        {
            if (!this.IsComplex)
                return null;
            var pks = table.ColumnCollection.Where(x => table.PrimaryKeyCollection.Contains(x.Name)).ToArray();
            var args = pks.ToString(", ", x => $"{x.GenerateParameterCode()}");
            var predicates = pks.ToString("AND ", x => $"[{x.Name}] = @{x.Name}");
            var parameters = pks.OfType<SqlColumn>().ToString("\r\n", x => $@"cmd.Parameters.Add(""@{x.Name}"", SqlDbType.{x.SqlType} , {x.Length}).Value = {x.Name.ToCamelCase()};");
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
            code.AppendLine("               if(dbval == System.DBNull.Value)");
            code.AppendLine("                   return null;");
            code.AppendLine($"               {this.GenerateValueStatementCode("dbval")}");
            code.AppendLine($"               return {this.GenerateValueAssignmentCode("dbval")};");
            code.AppendLine("           }");

            code.AppendLine("       }");
            return code.ToString();
        }
        [JsonIgnore]
        public override Type ClrType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.DbType))
                    throw new Exception($"[{Name}]Cannot get CLR type for " + this);
                return this.DbType.GetClrType();
            }
        }
    }


}