using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;

namespace subscriber.entities
{
    public class Builder
    {
        public EntityDefinition EntityDefinition { get; set; }
        public string Name { get; set; }
        private Column[] m_columns;
        public const string SPH_CONNECTION = "sph";

        public void Initialize()
        {
            var name = this.Name;

            var metadataProvider = new SqlServer2012Metadata();
            var meta = metadataProvider.GetTable(name);
            m_columns = meta.Columns
                        .Where(x => null != x)
                        .Where(p => p.CanRead && p.CanWrite)
                        .ToArray();


        }

        public async Task InsertAsync(Entity item)
        {
            var name = this.Name;

            var sql = string.Format(@"INSERT INTO [{1}].[{0}](", name, ConfigurationManager.ApplicationName) +
                string.Join(",", m_columns.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => "[" + x.Name + "]"))
                + " ) VALUES(" +
                string.Join(",", m_columns.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => "@" + x.Name.Replace(".", "_")))
                + ")\r\n";

            var parms = (from c in m_columns
                         select new SqlParameter("@" + c.Name.Replace(".", "_"), GetParameterValue(c, item))
                        ).ToList();
            var paramsValue = string.Join("\r\n",
                parms.Select(p => $"{p.ParameterName}\t=> {p.Value}"));
            Debug.WriteLine(sql + "\r\n" + paramsValue);
            await SPH_CONNECTION.ExecuteNonQueryAsync(sql, parms.ToArray());


        }

        private static object GetParameterValue(Column col, Entity item)
        {
            if (col.Name == "Data")
                throw new InvalidOperationException("Xml [Data] column is no longer supporterd");
            if (col.Name == "Json")
                return item.ToJsonString();
            if (col.Name == "CreatedDate")
                return item.IsNewItem || item.CreatedDate == DateTime.MinValue ? DateTime.Now : item.CreatedDate;
            if (col.Name == "CreatedBy")
                return "admin";
            if (col.Name == "ChangedDate")
                return DateTime.Now;
            if (col.Name == "ChangedBy")
                return "admin";

            var prop = item.GetType().GetProperty(col.Name);

            if (null == prop)
            {
                return item.MapColumnValue(col.Name)
                    ?? col.GetDefaultValue();
            }

            var value = prop.GetValue(item, null);
            if (prop.PropertyType.IsEnum)
                return value.ToString();
            if (prop.PropertyType.IsGenericType)
            {
                if (prop.PropertyType.GenericTypeArguments[0].IsEnum)
                    return value.ToString();
            }
            if (!col.IsNullable && null == value)
            {
                return col.GetDefaultValue();
            }
            return value ?? DBNull.Value;
        }

    }
}