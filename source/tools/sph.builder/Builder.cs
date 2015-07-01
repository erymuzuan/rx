using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
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
            SPH_CONNECTION.ExecuteNonQuery($"TRUNCATE TABLE [Sph].[{name}]");
            
            using (var client = new HttpClient())
            {
                client.DeleteAsync(
                    $"{ConfigurationManager.ElasticSearchHost}/{ConfigurationManager.ElasticSearchIndex}/_mapping/{name.ToLowerInvariant()}")
                    .Wait(5000);
            }
        }

        public async Task InsertAsync(T item)
        {
            var name = typeof(T).Name;
            await SPH_CONNECTION.ExecuteNonQueryAsync($"DELETE FROM [Sph].[{name}] WHERE [Id] = '{item.Id}'");

            var sql =
                $@"INSERT INTO [Sph].[{name}](" +
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

        private object GetParameterValue(Column col, Entity item)
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

            return value ?? DBNull.Value;
        }

    }
}