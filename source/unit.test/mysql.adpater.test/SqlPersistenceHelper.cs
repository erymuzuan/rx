using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Integrations.Adapters;
using MySql.Data.MySqlClient;

namespace sqlserver.adapter.test
{
    public static class SqlPersistenceHelper
    {


        public static async Task<T> GetDatabaseScalarAsync<T>(this MySqlAdapter adapter, string sql, params MySqlParameter[] parameters)
        {
            var connectionString = adapter.ConnectionString;
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);


                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();

                if (result is DBNull)
                    return default(T);


                return (T)result;
            }
        }



        public static async Task<T?> GetNullableScalarAsync<T>(string sql, string connectionStringName, params MySqlParameter[] parameters) where T : struct
        {

            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);


                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();

                if (DBNull.Value == result || null == result)
                    return new T?();


                return (T)result;
            }
        }




        public static List<T> GetDatabaseList<T>(string command, string connectionStringName, params MySqlParameter[] parameters)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(command, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                var list = new List<T>();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        list.Add((T)reader[0]);
                    }
                }

                return list;

            }
        }

        public static async Task<List<T>> GetDatabaseListAsync<T>(string command, string connectionStringName, params MySqlParameter[] parameters)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(command, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);

                await conn.OpenAsync();
                var list = new List<T>();
                using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        list.Add((T)reader[0]);
                    }
                }

                return list;

            }
        }


        public static Dictionary<TKey, TValue> GetDatabaseKeyList<TKey, TValue>(string sql, string connectionStringName, params MySqlParameter[] parameters)
        {

            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);
                var list = new Dictionary<TKey, TValue>();

                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                        list.Add((TKey)reader[0], (TValue)reader[1]);

                }

                return list;
            }



        }


        public static void ExecuteNonQuery(this MySqlAdapter adapter, string sql, params MySqlParameter[] parameters)
        {
            var connectionString = adapter.ConnectionString;
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static async Task ExecuteNonQueryAsync(this MySqlAdapter adapter, string sql, params MySqlParameter[] parameters)
        {
            var connectionString = adapter.ConnectionString;
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }


    }
}
