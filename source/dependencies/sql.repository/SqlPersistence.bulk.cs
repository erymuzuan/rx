using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public partial class SqlPersistence
    {

        public async Task<SubmitOperation> BulkInsertAsync(IEnumerable<Entity> items)
        {
            if (null == items) throw new ArgumentNullException(nameof(items));
            var list = items.ToArray();
            if (list.Length == 0)
                return new SubmitOperation();

            var variations = list.Where(x => null != x).Select(x => x.GetType()).Distinct().ToList();
            if (variations.Count > 1) throw new ArgumentException($"All the items must be of the same type : {string.Join(",", variations)}", nameof(items));

            var metadataProvider = ObjectBuilder.GetObject<ISqlServerMetadata>();
            var entityType = list.First().GetType();

            var metadataType = metadataProvider.GetTable(entityType.Name);
            if (null == metadataType) throw new InvalidOperationException("Cannot find the Metadata type in SQL Server :" + entityType.Name);


            var count = 0;
            var sql = new StringBuilder("BEGIN TRAN ");
            sql.AppendLine();

            var columns = metadataType.Columns
                .Where(p => p.CanRead && p.CanWrite)
                .ToArray();

            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                var idParameters = string.Join(",", list.Select(x => $"'{x.Id}'"));
                var existing = new List<string>();
                using (var cmd2 = new SqlCommand($"SELECT [ID] FROM [{ConfigurationManager.ApplicationName}].[{entityType.Name}] WHERE [Id] IN ({idParameters})", conn))
                {
                    if (conn.State != ConnectionState.Open)
                        await conn.OpenAsync();
                    using (var reader = await cmd2.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            existing.Add(reader.GetString(0));
                        }
                    }


                }
                foreach (var item in list)
                {
                    if (string.IsNullOrWhiteSpace(item.WebId)) item.WebId = Strings.GenerateId();
                    if (string.IsNullOrWhiteSpace(item.Id)) item.Id = Strings.GenerateId();

                    count++;
                    int count1 = count;
                    item.ChangedBy = "";
                    item.ChangedDate = DateTime.Now;
                    var exist = existing.Contains(item.Id);
                    if (!exist)
                    {
                        item.CreatedBy = "";
                        item.CreatedDate = DateTime.Now;
                        this.AppendInsertStatement(sql, entityType, columns, count1);
                    }
                    else
                        this.AppendUpdateStatement(sql, entityType, columns, count1, cmd, item.Id);


                    foreach (var c in columns)
                    {
                        var name = c.Name.Replace(".", "_");
                        var parameterName = $"@{name}{count1}";
                        var parameterValue = this.GetParameterValue(c, item, "");

                        cmd.Parameters.AddWithValue(parameterName, parameterValue);
                    }

                }

                sql.AppendLine();
                sql.AppendLine("COMMIT");
                Debug.WriteLine(sql);

                cmd.CommandText = sql.ToString();

                if (conn.State == ConnectionState.Closed)
                    await conn.OpenAsync();
                try
                {
                    var rows = await cmd.ExecuteNonQueryAsync();
                    var so = new SubmitOperation { RowsAffected = rows };
                    return so;
                }
                catch (Exception e)
                {
                    ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(e));
                    throw;
                }

            }

        }

    }
}
