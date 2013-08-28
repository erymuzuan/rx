using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;

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

                    var id = (int)item.GetType().GetProperty(entityType.Name + "Id").GetValue(item);


                    var edmxType = metadataProvider.GetTable(entityType.Name);
                    if (null == edmxType) throw new InvalidOperationException("Cannot find the EF type in edmx :" + entityType.Name);

                    var columns = edmxType.Columns
                        .Where(p => p.Name != entityType.Name + "Id")
                        .Where(p => p.CanRead && p.CanWrite)
                        .ToArray();

                    item.ChangedBy = ad.CurrentUserName;
                    item.ChangedDate = DateTime.Now;
                    if (id == 0)
                    {
                        item.CreatedBy = ad.CurrentUserName;
                        item.CreatedDate = DateTime.Now;
                        this.AppendInsertStatement(sql, entityType, columns, count1, cmd, addedItemsIdentityParameters, item);
                    }
                    else
                        this.AppendUpdateStatement(sql, entityType, columns, count1, cmd, id);


                    foreach (var c in columns)
                    {
                        var parameterName = string.Format("@{0}{1}", c.Name, count1);
                        var parameterValue = this.GetParameterValue(c, entityType, item);

                        cmd.Parameters.AddWithValue(parameterName, parameterValue);
                    }

                }

                foreach (var item in deletedList)
                {
                    count++;
                    var entityType = this.GetEntityType(item);
                    var typeName = entityType.Name;
                    var id = (int)item.GetType().GetProperty(typeName + "Id").GetValue(item, null);
                    sql.AppendFormat("DELETE FROM [Sph].[{0}] WHERE [{0}Id] = @{0}Id{1}", typeName, count);
                    sql.AppendLine();
                    cmd.Parameters.AddWithValue("@" + typeName + "Id" + count, id);
                }
                sql.AppendLine();
                sql.AppendLine("COMMIT");
                /**/
                cmd.CommandText = sql.ToString();
                await conn.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();

                var so = new SubmitOperation { RowsAffected = rows };
                // get the @@IDENTITY
                foreach (var t in addedItemsIdentityParameters)
                {
                    var item = t.Item1;
                    var id = (int)t.Item2.Value;
                    var type = this.GetEntityType(item);
                    type.GetProperty(type.Name + "Id")
                        .SetValue(item, id);
                    so.Add(item.WebId, id);
                }

                return so;

            }

        }

        private void AppendUpdateStatement(StringBuilder sql, Type entityType, Column[] columns, int count1,
                                                    SqlCommand cmd, int id)
        {
            sql.AppendFormat("UPDATE [Sph].[{0}]", entityType.Name);

            var updates = columns
                .Where(p => p.Name != "CreatedDate")
                .Where(p => p.Name != "CreatedBy")
                .Select(p => string.Format("[{0}]=@{0}{1}", p.Name, count1));
            sql.AppendLine();
            sql.AppendFormat("SET {0}", string.Join(",\r\n", updates));

            sql.AppendLine();
            sql.AppendFormat("WHERE [{0}Id] = @{0}Id{1}", entityType.Name, count1);
            sql.AppendLine();
            cmd.Parameters.AddWithValue(string.Format("@{0}Id{1}", entityType.Name, count1), id);
        }

        private void AppendInsertStatement(StringBuilder sql, Type entityType, Column[] columns, int count1,
                                                    SqlCommand cmd, List<Tuple<Entity, SqlParameter>> parameters, Entity item)
        {
            sql.AppendFormat("INSERT INTO [Sph].[{0}]", entityType.Name);
            sql.AppendFormat("({0})", string.Join(",", columns.Select(p => string.Format("[{0}]", p.Name))));

            sql.AppendLine();
            sql.AppendFormat("VALUES");
            sql.AppendFormat("({0})", string.Join(",", columns.Select(p => string.Format("@{0}{1}", p.Name, count1))));
            sql.AppendLine();
            sql.AppendFormat("SELECT @id{0} = @@IDENTITY", count1);
            sql.AppendLine();

            var idParam = new SqlParameter("@id" + count1, SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(idParam);
            parameters.Add(new Tuple<Entity, SqlParameter>(item, idParam));
        }

        private Type GetEntityType(Entity item)
        {
            var type = item.GetType();
            var attr = type.GetCustomAttribute<EntityTypeAttribute>();
            if (null != attr) return attr.Type;
            return type;
        }


        private object GetParameterValue(Column prop, Type entityType, Entity item)
        {
            var ad = ObjectBuilder.GetObject<IDirectoryService>();

            var id = (int)item.GetType().GetProperty(entityType.Name + "Id")
                .GetValue(item, null);
            if (prop.Name == "Data")
                return item.ToXmlString(entityType);
            if (prop.Name == "Json")
                return item.ToJsonString();
            if (prop.Name == "CreatedDate")
                return id == 0 || item.CreatedDate == DateTime.MinValue ? DateTime.Now : item.CreatedDate;
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


        public async Task<SubmitOperation> SubmitChanges(Entity item)
        {
            using (var conn = new SqlConnection(m_connectionString))
            {
                await conn.OpenAsync();
            }
            return new SubmitOperation();
        }
    }
}
