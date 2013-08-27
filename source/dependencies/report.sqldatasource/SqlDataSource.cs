using System;
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
            var nativeTypes = new[] { typeof(string), typeof(int),typeof(DateTime), typeof(decimal), typeof(double), typeof(float), typeof(bool) ,
                typeof(int?),typeof(DateTime?), typeof(decimal?), typeof(double?), typeof(float?), typeof(bool?) };
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

        public Task<ObjectCollection<ReportColumn>> GetColumnsAsync(ReportDefinition rdl)
        {
            var dataSource = rdl.DataSource;

            // ReSharper disable PossibleNullReferenceException
            var type = Type.GetType(typeof(Entity).AssemblyQualifiedName.Replace("Entity", dataSource.EntityName));
            var columns = new ObjectCollection<ReportColumn>();
            this.GetColumns(columns, type);
            // ReSharper restore PossibleNullReferenceException

            return Task.FromResult(columns);
        }

        public async Task<ObjectCollection<ReportRow>> GetRowsAsync(ReportDefinition rdl)
        {
            var dataSource = rdl.DataSource;
            var columns = await this.GetColumnsAsync(rdl);
            var rows = new ObjectCollection<ReportRow>();


            var query = dataSource.Query;
            if (string.IsNullOrWhiteSpace(query))
            {
                var compiler = new SqlCompiler(rdl);
                query = compiler.Compile(dataSource);
            }
            var cs = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
            using (var conn = new SqlConnection(cs))
            using (var cmd = new SqlCommand(query, conn))
            {
                foreach (var p in dataSource.ParameterCollection)
                {
                    var parameter = new SqlParameter("@" + p.Name, p.Value ?? p.DefaultValue);
                    cmd.Parameters.Add(parameter);
                    Console.WriteLine("PaRAM {0} = {1}", parameter.ParameterName, parameter.Value);
                }
                await conn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();

                var sqlcolumns = new ObjectCollection<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    sqlcolumns.Add(name);
                    Console.WriteLine("Column name  = " + name);
                }
                while (await reader.ReadAsync())
                {
                    var r = new ReportRow();

                    if (sqlcolumns.Contains("Data"))
                    {
                        r.ReportColumnCollection.AddRange(columns.Clone());
                        var xml = XElement.Parse(reader["Data"].ToString());
                        FillColumnValue(xml, r);
                    }
                    else
                    {
                        foreach (var c in sqlcolumns)
                        {
                            var column = new ReportColumn { Name = c, Value = string.Format("{0}", reader[c]) };
                            r.ReportColumnCollection.Add(column);
                        }
                    }


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

                    var element = xml.Element(x + c.Name);
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
