using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.SqlReportDataSource
{

    public class SqlDataSource : IReportDataSource
    {
        public IEnumerable<Member> GetFilterableMembers(string parent, IList<Member> members)
        {
            var filterables = members
                .Where(m => m.Type != typeof(object))
                .Where(m => m.Type != typeof(Array))
                .ToList();
            var list = members.Where(m => m.Type == typeof(object))
                .Select(m => this.GetFilterableMembers(parent + m.Name + ".", m.MemberCollection))
                .SelectMany(m => m)
                .ToList();
            filterables.AddRange(list);

            filterables.Where(m => string.IsNullOrWhiteSpace(m.FullName))
                .ToList().ForEach(m => m.FullName = parent + m.Name);

            return filterables;
        }

        public async Task<ObjectCollection<ReportColumn>> GetColumnsAsync(string type)
        {
            var context = new SphDataContext();
            var columns = new ObjectCollection<ReportColumn>();
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Name == type);
            if (null == ed)
                return new ObjectCollection<ReportColumn>();

            var databaseColumns = this.GetFilterableMembers("", ed.MemberCollection)
                .Select(m => new ReportColumn
                {
                    IsFilterable = m.IsFilterable,
                    Name = m.FullName,
                    TypeName = m.TypeName,
                    Type = m.Type,
                    Header = m.Name,
                    IsNullable = m.IsNullable,
                    WebId = m.WebId

                });


            columns.AddRange(databaseColumns);

            return columns;
        }

        public async Task<ObjectCollection<ReportRow>> GetRowsAsync(ReportDefinition rdl)
        {

            var type = rdl.DataSource.EntityName;

            var dataSource = rdl.DataSource;
            var columns = await this.GetColumnsAsync(type);
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
                    Debug.WriteLine("PARAM {0} = {1}", parameter.ParameterName, parameter.Value);
                }
                await conn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();

                var sqlcolumns = new ObjectCollection<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    sqlcolumns.Add(name);
                    Debug.WriteLine("Column name  = " + name);
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
                    else if (sqlcolumns.Contains("Json"))
                    {
                        r.ReportColumnCollection.AddRange(columns.Clone());
                        var json = reader["Json"].ToString();
                        FillColumnValue(json, r);
                    }
                    else
                    {
                        foreach (var c in sqlcolumns)
                        {
                            var column = new ReportColumn { Name = c, Value = reader[c] };
                            r.ReportColumnCollection.Add(column);
                        }
                    }


                    rows.Add(r);
                }

            }


            return rows;
        }

        
        public void FillColumnValue(string json, ReportRow r)
        {
            JObject o = JObject.Parse(json);
            foreach (var c in r.ReportColumnCollection)
            {
                c.SetValue(o.SelectToken("$." + c.Name).Value<string>());
            }
        }


        public void FillColumnValue(XElement xml, ReportRow r)
        {
            XNamespace x = Strings.DEFAULT_NAMESPACE;
            foreach (var c in r.ReportColumnCollection)
            {
                if (!c.Name.Contains("."))
                {
                    if (c.Name.StartsWith("(")) continue; // custom field
                    var attribute = xml.Attribute(c.Name);
                    if (null != attribute)
                    {
                        c.SetValue(attribute.Value);
                        continue;
                    }

                    var element = xml.Element(x + c.Name);
                    if (null != element) c.SetValue(element.Value);
                    continue;
                }

                var propertyPathList = c.Name.Split(new[] { '.' });
                var prop = propertyPathList.Last();

                var node = xml;
                foreach (var currentPath in propertyPathList)
                {
                    var xe = node.Element(x + currentPath);
                    if (null == xe) break;
                    node = xe;
                }

                var attr = node.Attribute(prop);
                if (null != attr)
                {
                    c.SetValue(attr.Value);
                }

            }
        }

    }
}
