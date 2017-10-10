using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public partial class SqlPersistence : IPersistence
    {
        private readonly string m_connectionString;

        public SqlPersistence()
        {
            m_connectionString = ConfigurationManager.SqlConnectionString;
        }

        public SqlPersistence(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public Task<SubmitOperation> SubmitChanges(Entity item, string user)
        {
            return this.SubmitChanges(new[] { item }, new Entity[] { }, null, user);
        }

        public async Task<SubmitOperation> SubmitChanges(IEnumerable<Entity> addedOrUpdatedItems, IEnumerable<Entity> deletedItems, PersistenceSession session, string user)
        {
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


                var count = 0;
                var sql = new StringBuilder("BEGIN TRAN ");
                sql.AppendLine();
                foreach (var item in addedList)
                {
                    var option = item.GetPersistenceOption();
                    if (!option.IsSqlDatabase)
                        continue;

                    if (string.IsNullOrWhiteSpace(item.WebId)) item.WebId = Strings.GenerateId();
                    if (string.IsNullOrWhiteSpace(item.Id)) item.Id = Strings.GenerateId();

                    count++;
                    var count1 = count;
                    var entityType = item.GetEntityType();

                    var metadataType = metadataProvider.GetTable(entityType.Name);
                    if (null == metadataType) throw new InvalidOperationException("Cannot find the Metadata type in SQL Server :" + entityType.Name);

                    var columns = metadataType.Columns
                        .Where(p => p.CanRead && p.CanWrite)
                        .ToArray();

                    item.ChangedBy = user;
                    item.ChangedDate = DateTime.Now;
                    bool exist;
                    var existSql = $@"SELECT COUNT([ID]) FROM [{this.GetSchema(entityType)}].[{entityType.Name}] WHERE [Id] = '{item.Id}'";
                    using (var existCmd = new SqlCommand(
                        existSql, conn))
                    {
                        if (conn.State != ConnectionState.Open)
                            await conn.OpenAsync();
                        exist = (int)await existCmd.ExecuteScalarAsync() > 0;

                    }
                    if (!exist)
                    {
                        item.CreatedBy = user;
                        item.CreatedDate = DateTime.Now;
                        this.AppendInsertStatement(sql, entityType, columns, count1);
                    }
                    else
                        this.AppendUpdateStatement(sql, entityType, columns, count1, cmd, item.Id);


                    foreach (var c in columns)
                    {
                        var parameterName = $"@{c.Name.Replace(".", "_")}{count1}";
                        var parameterValue = this.GetParameterValue(c, item, user);
                        if (cmd.Parameters.Contains(parameterName))
                            cmd.Parameters[parameterName].Value = parameterValue;
                        else
                            cmd.Parameters.AddWithValue(parameterName, parameterValue);
                    }

                }

                foreach (var item in deletedList)
                {
                    var entityType = this.GetEntityType(item);
                    var option = entityType.GetCustomAttribute<PersistenceOptionAttribute>();
                    if (!option.IsSqlDatabase)
                        continue;

                    count++;
                    var typeName = entityType.Name;
                    var schema = this.GetSchema(entityType);
                    var id = item.Id;
                    var idField = $"{typeName}Id{count}";
                    sql.Append($"DELETE FROM [{schema}].[{typeName}] WHERE [Id] = @{idField}");
                    sql.AppendLine();
                    cmd.Parameters.Add($"@{idField}", SqlDbType.VarChar, 50).Value = id;
                }
                sql.AppendLine();
                sql.AppendLine("COMMIT");
                Debug.WriteLine(sql);

                cmd.CommandText = sql.ToString();

                if (conn.State == ConnectionState.Closed)
                    await conn.OpenAsync();
                try
                {
                    var rows = await cmd.ExecuteNonQueryAsync();
                    var so = new SubmitOperation { RowsAffected = rows };
                    return so;
                }
                catch (Exception e)
                {
                    ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(e));
                    throw;
                }

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
                .Select(p => $"[{p.Name}]=@{p.Name.Replace(".", "_")}{count1}");
            sql.AppendLine();
            sql.AppendLine($"SET {string.Join(",\r\n", updates)}");

            sql.AppendLine();
            sql.Append($"WHERE [Id] = @Id{count1}");
            sql.AppendLine();
            if (cmd.Parameters.Contains($"@Id{count1}"))
                cmd.Parameters[$"@Id{count1}"].Value = id;
            else
                cmd.Parameters.AddWithValue($"@Id{count1}", id);
        }

        private void AppendInsertStatement(StringBuilder sql, Type entityType, Column[] columns, int count1)
        {
            var schema = this.GetSchema(entityType);
            sql.AppendFormat("INSERT INTO [{1}].[{0}]", entityType.Name, schema);
            sql.AppendFormat("({0})", string.Join(",", columns.Select(p => $"[{p.Name}]")));

            sql.AppendLine();
            sql.AppendFormat("VALUES");
            sql.AppendFormat("({0})", string.Join(",", columns.Select(p => $"@{p.Name.Replace(".", "_")}{count1}")));
            sql.AppendLine();

        }

        private string GetSchema(Type type)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));
            var domainNamespace = $"{type.Namespace}".StartsWith($"{typeof(Entity).Namespace}");
            return domainNamespace ? "Sph" : ConfigurationManager.ApplicationName;
        }

        private Type GetEntityType(Entity item)
        {
            var type = item.GetType();
            var attr = type.GetCustomAttribute<EntityTypeAttribute>();
            return null != attr ? attr.Type : type;
        }

        private object GetParameterValue(Column col, Entity item, string user)
        {
            if (col.Name == "Data")
                throw new InvalidOperationException("Xml [Data] column is no longer supported");
            if (col.Name == "Json")
                return item.ToJsonString();
            if (col.Name == "CreatedDate")
                return item.IsNewItem || item.CreatedDate == DateTime.MinValue ? DateTime.Now : item.CreatedDate;
            if (col.Name == "CreatedBy")
                return user;
            if (col.Name == "ChangedDate")
                return DateTime.Now;
            if (col.Name == "ChangedBy")
                return user;

            var itemProp = item.GetType().GetProperty(col.Name);
            if (null == itemProp)
            {
                return item.MapColumnValue(col.Name)
                    ?? col.GetDefaultValue();
            }
            var value = itemProp.GetValue(item, null);
            if (itemProp.PropertyType.IsEnum)
                return value.ToString();
            if (itemProp.PropertyType.IsGenericType)
            {
                if (itemProp.PropertyType.GenericTypeArguments[0].IsEnum)
                    return value.ToString();
            }

            return value ?? DBNull.Value;
        }


    }
}
