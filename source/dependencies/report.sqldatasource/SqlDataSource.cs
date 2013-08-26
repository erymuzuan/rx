using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.SqlReportDataSource
{

    public class SqlDataSource : IReportDataSource
    {
        public Task<ObjectCollection<ReportColumn>> GetColumnsAsync(DataSource dataSource)
        {
            var nativeTypes = new[] { typeof(string), typeof(int), typeof(decimal), typeof(bool) };

            var columns = new ObjectCollection<ReportColumn>();
            var type = Type.GetType(typeof(Entity).AssemblyQualifiedName.Replace("Entity", dataSource.EntityName));
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => nativeTypes.Contains(p.PropertyType))
                .Select(p => new ReportColumn
                {
                    Name = p.Name
                });

            columns.AddRange(props);


            return Task.FromResult(columns);
        }

        public async Task<ObjectCollection<ReportRow>> GetRowsAsync(DataSource dataSource)
        {
            var columns = await this.GetColumnsAsync(dataSource);
            var rows = new ObjectCollection<ReportRow>();

            var cs = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
            using (var conn = new SqlConnection(cs))
            using (var cmd = new SqlCommand(dataSource.Query, conn))
            {
                await conn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var r = new ReportRow();
                    r.ReportColumnCollection.AddRange(columns);
                    var sqlcolumns = new List<string>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        sqlcolumns.Add(reader.GetName(i));
                    }

                    foreach (var c in sqlcolumns)
                    {
                        if (null == r[c]) continue;
                        r[c].Value = string.Format("{0}",reader[c]);

                    }

                    rows.Add(r);
                }

            }


            return rows;
        }
    }
}
