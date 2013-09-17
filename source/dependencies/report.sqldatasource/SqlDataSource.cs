using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlReportDataSource
{

    public class SqlDataSource : IReportDataSource
    {

        public void GetColumns(ObjectCollection<ReportColumn> columns, Type type, string root = "")
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
               .Where(p => p.Name != "Wkt")
               .Where(p => p.Name != "EncodedWkt")
               .Select(p => new ReportColumn
               {
                   Name = root + p.Name,
                   Type = p.PropertyType,
                   IsNullable = p.PropertyType.FullName.Contains("Nullable")
               });

            var aggregates = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
               .Where(p => p.PropertyType.Namespace == typeof(Entity).Namespace)
               .Where(p => p.Name != "Item")
               .Where(p => !p.Name.EndsWith("Collection"))
               .ToList();
            foreach (var p in aggregates)
            {
                this.GetColumns(columns, p.PropertyType, root + p.Name + ".");
            }

            columns.AddRange(props);
        }

        private async Task<ReportColumn[]> GetCustomFieldColumns(string table)
        {
            var list = new ObjectCollection<ReportColumn>();
            if (table == typeof(Land).Name) return list.ToArray();
            if (table == typeof(Payment).Name) return list.ToArray();
            if (table == typeof(Tenant).Name) return list.ToArray();
            if (table == typeof(Inventory).Name) return list.ToArray();
            if (table == typeof(Invoice).Name) return list.ToArray();
            if (table == typeof(Rent).Name) return list.ToArray();
            if (table == typeof(RentalApplication).Name) table = "Application";

            var name = "Name";
            if (table == typeof(Contract).Name)
                name = "Type";

            XNamespace x = Strings.DEFAULT_NAMESPACE;
            var sql = string.Format("SELECT [{0}], [Data] FROM [Sph].[{1}Template]", name, table);
            var cs = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
            using (var conn = new SqlConnection(cs))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var template = reader.GetString(0);
                        var xml = XElement.Parse(reader.GetString(1));
                        var element = xml.Element(x + "CustomFieldCollection");
                        if (null == element) continue;

                        var customFieldCollection =
                            XmlSerializerService.DeserializeFromXml<ObjectCollection<CustomField>>(element.ToString()
                            .Replace("CustomFieldCollection", "ArrayOfCustomField"));
                        var columns = from e in customFieldCollection
                                      where !string.IsNullOrWhiteSpace(e.Name)
                                      select new ReportColumn
                                      {
                                          IsCustomField = true,
                                          Name = string.Format("({0}) {1}", template, e.Name),
                                          TypeName = e.Type
                                      };
                        list.AddRange(columns);

                    }
                }
            }
            list.ForEach(Console.WriteLine);

            return list.ToArray();
        }

        private async Task<string[]> GetDatabaseColumns(string table)
        {
            const string sql = @"SELECT 
        '[' + s.name + '].[' + o.name + ']' as 'Table'
        ,c.name as 'Column'
        ,t.name as 'Type' 
        ,c.max_length as 'length'
        ,c.is_nullable as 'IsNullable'    
	    ,c.is_identity as 'IsIdentity'
        ,c.is_computed as 'IsComputed'
    FROM 
        sys.objects o INNER JOIN sys.all_columns c
        ON c.object_id = o.object_id
        INNER JOIN sys.types t 
        ON c.system_type_id = t.system_type_id
        INNER JOIN sys.schemas s
        ON s.schema_id = o.schema_id
    WHERE 
        o.type = 'U'
        AND s.name = @Schema
        AND o.Name = @Table
        AND t.name <> N'sysname'
    ORDER 
        BY o.type";
            var list = new ObjectCollection<string>();
            var cs = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
            using (var conn = new SqlConnection(cs))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Schema", "Sph");
                cmd.Parameters.AddWithValue("@Table", table);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetString(1));
                    }
                }
            }


            return list.ToArray();
        }

        public async Task<ObjectCollection<ReportColumn>> GetColumnsAsync(Type type)
        {
            var columns = new ObjectCollection<ReportColumn>();
            this.GetColumns(columns, type);
            var databaseColumns = await this.GetDatabaseColumns(type.Name);
            var customColumns = await this.GetCustomFieldColumns(type.Name);

            foreach (var column in columns)
            {
                var column1 = column;
                column1.IsFilterable = databaseColumns.Any(c => c == column1.Name);
            }
            columns.AddRange(customColumns);

            // custom field


            return columns;
        }

        public async Task<ObjectCollection<ReportRow>> GetRowsAsync(ReportDefinition rdl)
        {

            // ReSharper disable PossibleNullReferenceException
            var type = Type.GetType(typeof(Entity).GetShortAssemblyQualifiedName().Replace("Entity", rdl.DataSource.EntityName));
            // ReSharper restore PossibleNullReferenceException

            var dataSource = rdl.DataSource;
            var columns = await this.GetColumnsAsync(type);
            var rows = new ObjectCollection<ReportRow>();


            var query = dataSource.Query;
            if (string.IsNullOrWhiteSpace(query))
            {
                var compiler = new SqlCompiler(rdl);
                query = compiler.Compile(dataSource);
            }
            Console.WriteLine(" ---------- SQL ------------------");
            Console.WriteLine(query);
            Console.WriteLine(" ---------- .SQL ------------------");
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
                        c.Value = attribute.Value;
                        continue;
                    }


                    // custom fields
                    if (c.IsCustomField)
                    {
                        var ce = xml.Element(x + "CustomFieldValueCollection");
                        if (null == ce) continue;
                        foreach (var cv in ce.Elements(x + "CustomFieldValue"))
                        {
                            var cvName = cv.Attribute("Name");
                            if (null == cvName) continue;
                            if (cvName.Value == c.Name)
                            {
                                var valueAtribute = cv.Attribute("Value");
                                if (null == valueAtribute) continue;
                                c.Value = valueAtribute.Value;
                            }
                        }
                        continue;
                    }

                    var element = xml.Element(x + c.Name);
                    if (null != element) c.Value = element.Value;
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
                    c.Value = attr.Value;
                }

            }
        }

    }
}
