using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;

namespace sph.builder
{
    public class Builder<T> where T : Entity
    {
        public virtual async Task Restore()
        {
            foreach (var item in this.GetItems())
            {
                await this.InsertAsync(item);

            }
        }

        private Column[] m_columns;
        public const string SPH_CONNECTION = "sph";
        public IEnumerable<T> GetItems()
        {
            var folder = Path.Combine(ConfigurationManager.WorkflowSourceDirectory, typeof(T).Name);

            if (!Directory.Exists(folder))
                return new List<T>();

            var list = from f in Directory.GetFiles(folder, "*.json")
                       let json = File.ReadAllText(f)
                       select json.DeserializeFromJson<T>();
            return list;
        }

        public string SetIdentityOn
        {
            get
            {
                return string.Format(" SET IDENTITY_INSERT [Sph].[{0}] ON      \r\n", typeof(T).Name);
            }
        }
        public string SetIdentityOff
        {
            get
            {
                return string.Format(" SET IDENTITY_INSERT [Sph].[{0}] OFF      \r\n", typeof(T).Name);
            }
        }

        public void Initialize()
        {
            var name = typeof(T).Name;

            var metadataProvider = new SqlServer2012Metadata();
            var meta = metadataProvider.GetTable(name);
            m_columns = meta.Columns
                        .Where(x => null != x)
                       .Where(p => p.CanRead && p.CanWrite)
                       .ToArray();


            SPH_CONNECTION.ExecuteNonQuery(string.Format("TRUNCATE TABLE [Sph].[{0}]", name));

        }

        public async Task InsertAsync(T item)
        {
            var name = typeof(T).Name;

            var sql = this.SetIdentityOn +
                string.Format(@"INSERT INTO [Sph].[{0}](", name) +
                string.Join(",", m_columns.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => "[" + x.Name + "]"))
                + " ) VALUES(" +
                string.Join(",", m_columns.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => "@" + x.Name.Replace(".", "_")))
                + ")\r\n"
           + this.SetIdentityOff;

            var parms = from c in m_columns
                        select new SqlParameter("@" + c.Name.Replace(".", "_"), this.GetParameterValue(c, item));

            try
            {
                await SPH_CONNECTION.ExecuteNonQueryAsync(sql, parms.ToArray()).ConfigureAwait(false);
            }
            catch (SqlException e)
            {
                if (e.Message.Contains("PRIMARY KEY"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to insert {1} : {0}",item, typeof(T).Name);
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
                else
                {
                    throw;
                }
            }

        }

        private object GetParameterValue(Column prop, Entity item)
        {
            var entityType = typeof(T);
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