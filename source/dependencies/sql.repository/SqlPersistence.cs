using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public class SqlPersistence : IPersistence
    {
        private readonly string m_connectionString;

        public SqlPersistence()
        {
            m_connectionString = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
        }

        public SqlPersistence(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public Task<SubmitOperation> SubmitChanges(Entity item)
        {
            return this.SubmitChanges(new[] { item }, new Entity[] { }, null);
        }

        public async Task<SubmitOperation> SubmitChanges(IEnumerable<Entity> addedOrUpdatedItems, IEnumerable<Entity> deletedItems, PersistenceSession session)
        {
            var ad = ObjectBuilder.GetObject<IDirectoryService>();
            var metadataProvider = ObjectBuilder.GetObject<ISqlServerMetadata>();

            var addedList = addedOrUpdatedItems.ToList();
            var deletedList = deletedItems.ToList();
            if (addedList.Count + deletedList.Count > 100)
                throw new ArgumentException("You cannot have more than 100 items in 1 transaction");

            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;


                var addedItemsIdentityParameters = new List<Tuple<Entity, SqlParameter>>();
                var count = 0;
                var sql = new StringBuilder("BEGIN TRAN ");
                sql.AppendLine();
                foreach (var item in addedList)
                {
                    if (string.IsNullOrWhiteSpace(item.WebId)) item.WebId = Guid.NewGuid().ToString();

                    count++;
                    int count1 = count;
                    var entityType = this.GetEntityType(item);
                    var metadataType = metadataProvider.GetTable(entityType.Name);
                    if (null == metadataType) throw new InvalidOperationException("Cannot find the Metadata type in SQL Server :" + entityType.Name);

                    var columns = metadataType.Columns
                        .Where(p => p.CanRead && p.CanWrite)
                        .ToArray();

                    item.ChangedBy = ad.CurrentUserName;
                    item.ChangedDate = DateTime.Now;
                    bool exist;
                    using (var cmd2 = new SqlCommand("SELECT COUNT([ID]) FROM [" + this.GetSchema(entityType) + "].[" + entityType.Name + "] WHERE [Id] = '" +item.Id+"'", conn))
                    {
                        if (conn.State != ConnectionState.Open)
                            await conn.OpenAsync();
                        exist = (int)(await cmd2.ExecuteScalarAsync()) > 0;

                    }
                    if (!exist)
                    {
                        item.CreatedBy = ad.CurrentUserName;
                        item.CreatedDate = DateTime.Now;
                        this.AppendInsertStatement(sql, entityType, columns, count1);
                    }
                    else
                        this.AppendUpdateStatement(sql, entityType, columns, count1, cmd, item.Id);


                    foreach (var c in columns)
                    {
                        var parameterName = string.Format("@{0}{1}", c.Name.Replace(".", "_"), count1);
                        var parameterValue = this.GetParameterValue(c, entityType, item);

                        cmd.Parameters.AddWithValue(parameterName, parameterValue);
                    }

                }

                foreach (var item in deletedList)
                {
                    count++;
                    var entityType = this.GetEntityType(item);
                    var typeName = entityType.Name;
                    var schema = this.GetSchema(entityType);
                    var id = item.Id;
                    sql.AppendFormat("DELETE FROM [{2}].[{0}] WHERE [Id] = @{0}Id{1}", typeName, count, schema);
                    sql.AppendLine();
                    cmd.Parameters.AddWithValue("@" + typeName + "Id" + count, id);
                }
                sql.AppendLine();
                sql.AppendLine("COMMIT");
                Console.WriteLine(sql);
                cmd.CommandText = sql.ToString();
                var rows = await cmd.ExecuteNonQueryAsync();

                var so = new SubmitOperation { RowsAffected = rows };
                // get the @@IDENTITY
                foreach (var t in addedItemsIdentityParameters)
                {
                    var item = t.Item1;
                    var id = (string)t.Item2.Value;
                    item.Id = id;
                    so.Add(item.WebId, id);
                }

                return so;

            }

        }

        private void AppendUpdateStatement(StringBuilder sql, Type entityType, IEnumerable<Column> columns, int count1,
                                                    SqlCommand cmd, string id)
        {
            var schema = this.GetSchema(entityType);
            sql.AppendFormat("UPDATE [{1}].[{0}]", entityType.Name, schema);

            var updates = columns
                .Where(p => p.Name != "Id")
                .Where(p => p.Name != "CreatedDate")
                .Where(p => p.Name != "CreatedBy")
                .Select(p => string.Format("[{0}]=@{1}{2}", p.Name, p.Name.Replace(".", "_"), count1));
            sql.AppendLine();
            sql.AppendFormat("SET {0}", string.Join(",\r\n", updates));

            sql.AppendLine();
            sql.AppendFormat("WHERE [Id] = @{0}Id{1}", entityType.Name, count1);
            sql.AppendLine();
            cmd.Parameters.AddWithValue(string.Format("@{0}Id{1}", entityType.Name, count1), id);
        }

        private void AppendInsertStatement(StringBuilder sql, Type entityType, Column[] columns, int count1)
        {
            var schema = this.GetSchema(entityType);
            sql.AppendFormat("INSERT INTO [{1}].[{0}]", entityType.Name, schema);
            sql.AppendFormat("({0})", string.Join(",", columns.Select(p => string.Format("[{0}]", p.Name))));

            sql.AppendLine();
            sql.AppendFormat("VALUES");
            sql.AppendFormat("({0})", string.Join(",", columns.Select(p => string.Format("@{0}{1}", p.Name.Replace(".", "_"), count1))));
            sql.AppendLine();

        }

        private string GetSchema(Type type)
        {
            if (type.Namespace.StartsWith(typeof(Entity).Namespace)) return "Sph";
            return ConfigurationManager.ApplicationName;
        }

        private Type GetEntityType(Entity item)
        {
            var type = item.GetType();
            var attr = type.GetCustomAttribute<EntityTypeAttribute>();
            return null != attr ? attr.Type : type;
        }


        private object GetParameterValue(Column prop, Type entityType, Entity item)
        {
            var ad = ObjectBuilder.GetObject<IDirectoryService>();

            var id = (string)item.GetType().GetProperty("Id")
                .GetValue(item, null);
            if (prop.Name == "Data")
                return item.ToXmlString(entityType);
            if (prop.Name == "Json")
                return item.ToJsonString();
            if (prop.Name == "CreatedDate")
                return string.IsNullOrWhiteSpace(id) || item.CreatedDate == DateTime.MinValue ? DateTime.Now : item.CreatedDate;
            if (prop.Name == "CreatedBy")
                return ad.CurrentUserName;
            if (prop.Name == "ChangedDate")
                return DateTime.Now;
            if (prop.Name == "ChangedBy")
                return ad.CurrentUserName;

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
