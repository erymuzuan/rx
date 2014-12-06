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
        public virtual async Task RestoreAllAsync()
        {
            var tasks = from t in this.GetItems()
                        select InsertAsync(t);
            await Task.WhenAll(tasks);
        }

        public virtual async Task RestoreAsync(T item)
        {
            await this.InsertAsync(item);
        }

        private Column[] m_columns;
        public const string SPH_CONNECTION = "sph";
        public IEnumerable<T> GetItems()
        {
            var folder = Path.Combine(ConfigurationManager.SphSourceDirectory, typeof(T).Name);

            if (!Directory.Exists(folder))
                return new List<T>();

            var list = from f in Directory.GetFiles(folder, "*.json")
                       let json = File.ReadAllText(f)
                       select json.DeserializeFromJson<T>();
            return list;
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



        }

        public void Clean()
        {
            var name = typeof(T).Name;
            SPH_CONNECTION.ExecuteNonQuery(string.Format("TRUNCATE TABLE [Sph].[{0}]", name));

            // TODO : clear the ElasticSearch too
        }

        public async Task InsertAsync(T item)
        {
            var name = typeof(T).Name;
            await SPH_CONNECTION.ExecuteNonQueryAsync(string.Format("DELETE FROM [Sph].[{0}] WHERE [Id] = '{1}'", name, item.Id));

            var sql = 
                string.Format(@"INSERT INTO [Sph].[{0}](", name) +
                string.Join(",", m_columns.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => "[" + x.Name + "]"))
                + " ) VALUES(" +
                string.Join(",", m_columns.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => "@" + x.Name.Replace(".", "_")))
                + ")\r\n";

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
                    Console.WriteLine("Failed to insert {1} : {0}", item, typeof(T).Name);
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
            var id = item.Id;
            if (prop.Name == "Data")
                throw new InvalidOperationException("Xml [Data] column is no longer supporterd");
            if (prop.Name == "Json")
                return item.ToJsonString();
            if (prop.Name == "CreatedDate")
                return string.IsNullOrWhiteSpace(id) || item.CreatedDate == DateTime.MinValue ? DateTime.Now : item.CreatedDate;
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