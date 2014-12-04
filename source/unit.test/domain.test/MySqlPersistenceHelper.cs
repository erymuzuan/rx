using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using MySql.Data.MySqlClient;

namespace sqlserver.adapter.test
{
    public static class MySqlPersistenceHelper
    {

        public static async Task<T> GetMySqlScalarAsync<T>(this string connectionString, string sql, params MySqlParameter[] parameters)
        {
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



        public static async Task<T?> GetMySqlNullableScalarAsync<T>(string sql, string connectionStringName, params MySqlParameter[] parameters) where T : struct
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




        public static List<T> GetMySqlList<T>(string command, string connectionStringName, params MySqlParameter[] parameters)
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

        public static async Task<List<T>> GetMySqlListAsync<T>(string command, string connectionStringName, params MySqlParameter[] parameters)
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


        public static Dictionary<TKey, TValue> GetMySqlKeyList<TKey, TValue>(string sql, string connectionStringName, params MySqlParameter[] parameters)
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


        public static void ExecuteMySqlNonQuery(this string connectionString, string sql, params MySqlParameter[] parameters)
        {
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static async Task ExecuteMySqlNonQueryAsync(this string connectionString , string sql, params MySqlParameter[] parameters)
        {
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
