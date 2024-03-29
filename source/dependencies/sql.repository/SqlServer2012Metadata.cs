﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public class SqlServer2012Metadata : ISqlServerMetadata
    {
        private const string Sql = @"SELECT 
        '[' + s.name + '].[' + o.name + ']' as 'Table'
        ,c.name as 'Column'
        ,t.name as 'Type' 
        ,c.max_length as 'length'
        ,c.is_nullable as 'IsNullable'    
	    ,c.is_identity as 'IsIdentity'
        ,c.is_computed as 'IsComputed'
    FROM 
        sys.objects o INNER JOIN sys.all_columns c
        ON c.object_id = o.object_id
        INNER JOIN sys.types t 
        ON c.system_type_id = t.system_type_id
        INNER JOIN sys.schemas s
        ON s.schema_id = o.schema_id
    WHERE 
        o.type = 'U'
        --AND s.name = @Schema
        AND o.Name = @Table
        AND t.name <> N'sysname'
    ORDER 
        BY o.type";

        public SqlServer2012Metadata()
        {
            this.ConnectionString = ConfigurationManager.SqlConnectionString;
            this.Schema = "Sph";
        }

        public SqlServer2012Metadata(string connectionString, string schema)
        {
            this.ConnectionString = connectionString;
            this.Schema = schema;
        }

        public Table GetTable(string name)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(Sql, conn))
            {
                cmd.Parameters.AddWithValue("@Table", name);
                //cmd.Parameters.AddWithValue("@Schema", this.Schema);
                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    var table = new Table { Name = name };
                    var columns = new ObjectCollection<Column>();
                    while (reader.Read())
                    {
                        var column = new Column
                        {
                            Name = reader.GetString(1),
                            SqlType = reader.GetString(2),
                            Length = reader.GetInt16(3),
                            IsNullable = reader.GetBoolean(4),
                            IsIdentity = reader.GetBoolean(5),
                            CanWrite = !reader.GetBoolean(6)// computed

                        };
                        column.CanRead = true;
                        column.IsPrimaryKey = column.Name == "Id";

                        // SpatialEntity
                        if (new[] { "Path", "EncodedWkt", "Wkt" }.Contains(column.Name)) continue;

                        columns.Add(column);
                    }
                    if (columns.Count(c => c.IsPrimaryKey) != 1)
                        throw new InvalidOperationException(name + " contains " + columns.Count(c => c.IsPrimaryKey) + " primary keys");


                    table.Columns = columns.ToArray();
                    return table;
                }

            }
        }


        public string GetDataColumn<T>() where T : Entity
        {
            throw new NotImplementedException();
        }

        protected string Schema { get; set; }
        public string ConnectionString { get; set; }
    }
}