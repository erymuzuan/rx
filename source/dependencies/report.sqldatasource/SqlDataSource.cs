using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.SqlReportDataSource
{

    public class SqlDataSource : IReportDataSource
    {
        private void GetColumns(ObjectCollection<ReportColumn> columns, Type type, string root = "")
        {
            var nativeTypes = new[] { typeof(string), typeof(int), typeof(decimal), typeof(bool) };
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
               .Where(p => nativeTypes.Contains(p.PropertyType))
               .Where(p => p.Name != "Item")
               .Where(p => p.Name != "WebId")
               .Where(p => p.Name != "Dirty")
               .Where(p => p.Name != "Bil")
               .Where(p => p.Name != "Error")
               .Select(p => new ReportColumn
               {
                   Name = root + p.Name
               });

            var aggregates = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
               .Where(p => p.PropertyType.Namespace == typeof(Entity).Namespace)
               .Where(p => p.Name != "Item")
               .ToList();
            foreach (var p in aggregates)
            {
                this.GetColumns(columns, p.PropertyType, p.Name + ".");
            }

            columns.AddRange(props);
        }

        public Task<ObjectCollection<ReportColumn>> GetColumnsAsync(DataSource dataSource)
        {

            // ReSharper disable PossibleNullReferenceException
            var type = Type.GetType(typeof(Entity).AssemblyQualifiedName.Replace("Entity", dataSource.EntityName));
            var columns = new ObjectCollection<ReportColumn>();
            this.GetColumns(columns, type);
            // ReSharper restore PossibleNullReferenceException

            return Task.FromResult(columns);
        }

        public async Task<ObjectCollection<ReportRow>> GetRowsAsync(DataSource dataSource)
        {
            // ReSharper disable PossibleNullReferenceException
            var type = Type.GetType(typeof(Entity).AssemblyQualifiedName.Replace("Entity", dataSource.EntityName));
            var columns = await this.GetColumnsAsync(dataSource);
            // ReSharper restore PossibleNullReferenceException
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
                    r.ReportColumnCollection.AddRange(columns.Clone());
                    var xml = XElement.Parse(reader["Data"].ToString());

                    FillColumnValue(xml, r);


                    rows.Add(r);
                }

            }


            return rows;
        }


        public void FillColumnValue(XElement xml, ReportRow r)
        {
            XNamespace x = Strings.DEFAULT_NAMESPACE;
            foreach (var c in r.ReportColumnCollection)
            {
                if (!c.Name.Contains("."))
                {
                    var attribute = xml.Attribute(c.Name);
                    if (null != attribute)
                    {
                        c.Value = attribute.Value;
                        continue;
                    }

                    var element = xml.Attribute(c.Name);
                    if (null != element) c.Value = element.Value;
                    continue;
                }

                var aggregate = c.Name.Split(new[] { '.' });
                var prop = aggregate.Last();

                var node = xml;
                var currentPath = aggregate.First();
                while (true)
                {
                    var xe = node.Element(x + currentPath);
                    if (null == xe) break;
                    node = xe;
                } 
                var attr = node.Attribute(prop);
                if (null != attr)
                {
                    c.Value = attr.Value;
                }

            }
        }

    }
}
