using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class SqlServerRecordSet : DomainObject { }
    public class SqlServerFieldFieldMapping : TextFieldMapping { }
    public partial class SqlServerRecordSetFormatter : TextFormatter
    {
        public override async Task<Class> GetPortClassAsync(ReceivePort port)
        {
            var type = await base.GetPortClassAsync(port);
            type.AddNamespaceImport<SqlDataReader>();

            var processCode = GenerateProcessCode(port);
            type.AddMethod(new Method { Code = processCode });


            return type;
        }

        private string GenerateProcessCode(ReceivePort port)
        {
            var code = new StringBuilder();

            code.Append($"public async Task<IEnumerable<{port.Entity}>> Process(SqlDataReader reader)");
            code.AppendLine("{");

            code.AppendLine($@"    var list = new List<{port.Entity}>();");
            code.AppendLine("    while(await reader.ReadAsync())");
            code.AppendLine("    { ");
            code.AppendLine($@"      var record  = new {port.Entity}();");
            foreach (var mp in port.FieldMappingCollection)
            {
                //TODO : use column provider in SQL server adapter
                if (typeof(string) == mp.Type && mp.IsNullable)
                    code.AppendLine($@"record.{mp.Name} = reader[""{mp.Name}""].ReadNullableString();");
                if (typeof(string) == mp.Type && !mp.IsNullable)
                    code.AppendLine($@"record.{mp.Name} = (string)reader[""{mp.Name}""];");

                if (typeof(int) == mp.Type && mp.IsNullable)
                    code.AppendLine($@"record.{mp.Name} = reader[""{mp.Name}""].ReadNullable<int>();");
                if (typeof(int) == mp.Type && !mp.IsNullable)
                    code.AppendLine($@"record.{mp.Name} = (int)reader[""{mp.Name}""];");

                if (typeof(decimal) == mp.Type && mp.IsNullable)
                    code.AppendLine($@"record.{mp.Name} = reader[""{mp.Name}""].ReadNullable<decimal>();");
                if (typeof(decimal) == mp.Type && !mp.IsNullable)
                    code.AppendLine($@"record.{mp.Name} = (decimal)reader[""{mp.Name}""];");

                if (typeof(DateTime) == mp.Type && mp.IsNullable)
                    code.AppendLine($@"record.{mp.Name} = reader[""{mp.Name}""].ReadNullable<DateTime>();");
                if (typeof(DateTime) == mp.Type && !mp.IsNullable)
                    code.AppendLine($@"record.{mp.Name} = (DateTime)reader[""{mp.Name}""];");

                if (typeof(bool) == mp.Type && mp.IsNullable)
                    code.AppendLine($@"record.{mp.Name} = reader[""{mp.Name}""].ReadNullable<bool>();");
                if (typeof(bool) == mp.Type && !mp.IsNullable)
                    code.AppendLine($@"record.{mp.Name} = (bool)reader[""{mp.Name}""];");
            }
            code.AppendLine("       list.Add(record);");
            code.AppendLine("    } ");
            code.AppendLine("    return list;");
            code.AppendLine("} ");
            return code.ToString();
        }

        public override async Task<TextFieldMapping[]> GetFieldMappingsAsync()
        {
            var connectionString = this.Trusted ?
                    $@"Data Source={this.Server};Initial Catalog={this.Database};Integrated Security=True;MultipleActiveResultSets=True" :
                    $"Server={this.Server};Database={this.Database};User Id={this.UserId};Password={this.Password};;MultipleActiveResultSets=True";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(this.Query, conn))

            {
                var fields = new List<TextFieldMapping>();
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var name = reader.GetName(i);
                        var field = new SqlServerFieldFieldMapping
                        {
                            Name = name,
                            Type = reader.GetFieldType(i),
                        };
                        fields.Add(field);
                    }
                    while (await reader.ReadAsync())
                    {
                        var col = 0;
                        foreach (var f in fields)
                        {
                            f.SampleValue = reader[col].ToEmptyString();
                            f.IsNullable = reader.IsDBNull(col);
                            col++;
                        }
                    }
                }
                return fields.ToArray();
            }
        }
    }
}