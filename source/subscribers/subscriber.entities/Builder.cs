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
                         select new SqlParameter("@" + c.Name.Replace(".", "_"), this.GetParameterValue(c, item))
                        ).ToList();
            var paramsValue = string.Join("\r\n",
                parms.Select(p => string.Format("{0}\t=> {1}", p.ParameterName, p.Value)));
            Debug.WriteLine(sql + "\r\n" + paramsValue);
            await SPH_CONNECTION.ExecuteNonQueryAsync(sql, parms.ToArray());


        }

        private object GetParameterValue(Column prop, Entity item)
        {
            if (prop.Name == "Data")
                throw new InvalidOperationException("Xml [Data] column is no longer supporterd");
            if (prop.Name == "Json")
                return item.ToJsonString();
            if (prop.Name == "CreatedDate")
                return item.IsNewItem || item.CreatedDate == DateTime.MinValue ? DateTime.Now : item.CreatedDate;
            if (prop.Name == "CreatedBy")
                return "admin";
            if (prop.Name == "ChangedDate")
                return DateTime.Now;
            if (prop.Name == "ChangedBy")
                return "admin";

            var itemProp = item.GetType().GetProperty(prop.Name);
            if (null == itemProp) return item.MapColumnValue(prop.Name);
            var value = itemProp.GetValue(item, null);
            if (itemProp.PropertyType.IsEnum)
                return value.ToString();
            if (itemProp.PropertyType.IsGenericType)
            {
                if (itemProp.PropertyType.GenericTypeArguments[0].IsEnum)
                    return value.ToString();
            }
            if (!prop.IsNullable && null == value)
            {
                // get the default value
                switch (prop.SqlType)
                {
                    case "varchar":
                    case "nvarchar": return "";
                    case "int": return 0;
                    case "bit": return 0;
                    case "money": return 0;
                    case "float": return 0;
                    case "smalldatetime": return DateTime.Today;
                    default: throw new Exception("No default value for " + prop.SqlType);
                }
            }
            if (null == value) return DBNull.Value;
            return value;
        }

    }
}