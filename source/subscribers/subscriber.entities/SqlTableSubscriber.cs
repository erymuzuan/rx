﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class SqlTableSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName
        {
            get { return "ed_sql_table"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityDefinition).Name + ".changed.Publish" }; }
        }

        private string GetSqlType(string typeName)
        {
            switch (typeName)
            {
                case "System.String, mscorlib": return "VARCHAR(255)";
                case "System.Int32, mscorlib": return "INT";
                case "System.DateTime, mscorlib": return "SMALLDATETIME";
                case "System.Decimal, mscorlib": return "MONEY";
                case "System.Double, mscorlib": return "FLOAT";
                case "System.Boolean, mscorlib": return "BIT";
            }
            return "VARCHAR(255)";
        }

        public IEnumerable<Member> GetFiltarableMembers(string parent, IList<Member> members)
        {
            var filterables = members.Where(m => m.IsFilterable)
                .Where(m => m.Type != typeof(object))
                .Where(m => m.Type != typeof(Array))
                .ToList();
            var list = members.Where(m => m.Type == typeof(object))
                .Select(m => this.GetFiltarableMembers(parent + m.Name + ".", m.MemberCollection))
                .SelectMany(m => m)
                .ToList();
            filterables.AddRange(list);

            filterables.Where(m => string.IsNullOrWhiteSpace(m.FullName))
                .ToList().ForEach(m => m.FullName = parent + m.Name);

            return filterables;

        }

        protected async override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["sph"].ConnectionString;
            var applicationName = ConfigurationManager.ApplicationName;
            var tableExistSql =
                string.Format(
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{0}'  AND  TABLE_NAME = '{1}'",
                    applicationName, item.Name);
            var createTable = this.CreateTableSql(item, header, applicationName);
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var count = 0;
                using (var cmd = new SqlCommand(tableExistSql, conn))
                {
                    count = (int)(await cmd.ExecuteScalarAsync());

                } if (count > 0)
                {
                    // rename table for migration
                    using (var renameTableCommand = new SqlCommand("sp_rename", conn) { CommandType = CommandType.StoredProcedure })
                    {
                        renameTableCommand.Parameters.AddWithValue("@objname", string.Format("[{0}].[{1}]", applicationName, item.Name));
                        renameTableCommand.Parameters.AddWithValue("@newname", string.Format("{0}_{1:yyyyMMdd_HHmmss}",  item.Name, DateTime.Now));
                        //renameTableCommand.Parameters.AddWithValue("@objtype", "OBJECT");

                        await renameTableCommand.ExecuteNonQueryAsync();
                    }
                    
                }

                using (var createTableCommand = new SqlCommand(createTable, conn))
                {
                    await createTableCommand.ExecuteNonQueryAsync();
                }



            }

        }



        private string CreateTableSql(EntityDefinition item, MessageHeaders header, string applicationName)
        {

            var sql = new StringBuilder();
            sql.AppendFormat("CREATE TABLE [{0}].[{1}]", applicationName, item.Name);
            sql.AppendLine("(");
            sql.AppendLinf("  [{0}Id] INT PRIMARY KEY IDENTITY(1,1)", item.Name);
            var members = this.GetFiltarableMembers("", item.MemberCollection);
            foreach (var member in members)
            {
                sql.AppendFormat(",[{0}] {1} {2} NULL", member.FullName, this.GetSqlType(member.TypeName), member.IsNullable ? "" : "NOT");
                sql.AppendLine("");
            }
            sql.AppendLine(",[Json] VARCHAR(MAX)");
            sql.AppendLine(",[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[CreatedBy] VARCHAR(255) NULL");
            sql.AppendLine(",[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()");
            sql.AppendLine(",[ChangedBy] VARCHAR(255) NULL");
            sql.AppendLine(")");

            return sql.ToString();
        }
    }
}
