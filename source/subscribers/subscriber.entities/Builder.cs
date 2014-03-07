using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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


        public string SetIdentityOn
        {
            get
            {
                return string.Format(" SET IDENTITY_INSERT [{1}].[{0}] ON      \r\n", this.Name,ConfigurationManager.ApplicationName);
            }
        }
        public string SetIdentityOff
        {
            get
            {
                return string.Format(" SET IDENTITY_INSERT [{1}].[{0}] OFF      \r\n", this.Name, ConfigurationManager.ApplicationName);
            }
        }

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

            var sql = this.SetIdentityOn +
                string.Format(@"INSERT INTO [{1}].[{0}](", name, ConfigurationManager.ApplicationName) +
                string.Join(",", m_columns.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => "[" + x.Name + "]"))
                + " ) VALUES(" +
                string.Join(",", m_columns.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => "@" + x.Name.Replace(".", "_")))
                + ")\r\n"
           + this.SetIdentityOff;

            var parms = from c in m_columns
                        select new SqlParameter("@" + c.Name.Replace(".", "_"), this.GetParameterValue(c, item));
            await SPH_CONNECTION.ExecuteNonQueryAsync(sql, parms.ToArray()).ConfigureAwait(false);


        }

        private object GetParameterValue(Column prop, Entity item)
        {
            var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + this.EntityDefinition.Name);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, this.EntityDefinition.EntityDefinitionId, this.Name);
            var entityType = edAssembly.GetType(edTypeName);

            var id = (int)item.GetType().GetProperty(entityType.Name + "Id")
                .GetValue(item, null);
            if (prop.Name == "Data")
                return item.ToXmlString(entityType);
            if (prop.Name == "Json")
                return item.ToJsonString();
            if (prop.Name == "CreatedDate")
                return id == 0 || item.CreatedDate == DateTime.MinValue ? DateTime.Now : item.CreatedDate;
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

            if (null == value) return DBNull.Value;
            return value;
        }

    }
}