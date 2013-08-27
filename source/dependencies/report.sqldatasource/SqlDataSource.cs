using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
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
            var columns = await this.GetColumnsAsync(dataSource);
            var rows = new ObjectCollection<ReportRow>();


            var query = dataSource.Query;
            if (string.IsNullOrWhiteSpace(query))
            {
                query = this.Compile(dataSource);
            }
            Console.WriteLine("======== SQL ==========");
            Console.WriteLine(query);
            Console.WriteLine("======== SQL ==========");
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

        private string Compile(DataSource dataSource)
        {
            var sql = new StringBuilder("SELECT ");

            // for normal aggregate
            foreach (var c in dataSource.EntityFieldCollection)
            {
                if (!string.IsNullOrWhiteSpace(c.Aggregate) && c.Aggregate != "GROUP")
                {
                    sql.AppendFormat("{0}([{1}])", c.Aggregate, c.Name);
                }
            }
            sql.AppendFormat("[{0}Id], [Data] FROM [Sph].[{0}] ", dataSource.EntityName);
            if (dataSource.ReportFilterCollection.Any())
                sql.AppendLine("WHERE");

            var first = true;
            foreach (var filter in dataSource.ReportFilterCollection)
            {
                var value = this.GetFilterValue(filter);
                var op = this.GetFilteroperator(filter);
                if (!first)
                    sql.AppendLine(" AND");
                sql.AppendFormat("[{0}] {1} {2}",filter.FieldName, op,value);
                // State Eq @State

                first = false;
            }

            // for GROUP
            foreach (var c in dataSource.EntityFieldCollection)
            {
                if (c.Aggregate == "GROUP")
                {
                    sql.AppendFormat("GROUP BY [{0}]", c.Name);
                }
            }

            // for ORDER
            foreach (var c in dataSource.EntityFieldCollection.OrderBy(t => t.OrderPosition))
            {
                if (!string.IsNullOrWhiteSpace(c.Order))
                {
                    sql.AppendFormat("ORDER BY [{0}] {1})", c.Name, c.Order);
                }
            }



            return sql.ToString();
        }

        public string GetFilteroperator(ReportFilter filter)
        {
            if(null == filter)throw new ArgumentNullException("filter");
            switch (filter.Operator)
            {
                case "Eq":
                    return "=";
            }
            throw new Exception("Whoaaaa");
        }

        public string GetFilterValue(ReportFilter filter)
        {
            if(null == filter)throw new ArgumentNullException("filter");
            if (filter.Value.StartsWith("@"))
                return filter.Value;
            if (filter.Value.StartsWith("="))
            {
                
            }
            // for string
            return string.Format("'{0}'", filter.Value);
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
