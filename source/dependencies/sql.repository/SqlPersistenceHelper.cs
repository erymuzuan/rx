using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public static class SqlPersistenceHelper
    {
        /// <summary>
        /// Make sure the current Thread CurrentPrincipal runnin with the specified userName and role
        /// NOTE : this will not be validated against the database
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        public static void InitializeThread(string userName, params string[] roles)
        {
            var identity = new GenericIdentity(userName);
            var principal = new GenericPrincipal(identity, roles);

            Thread.CurrentPrincipal = principal;
        }

        public static T GetDatabaseScalarValue<T>(this string connectionStringName, string sql, params SqlParameter[] parameters)
        {

            var connectionString = ConfigurationManager.SqlConnectionString;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);


                conn.Open();
                var result = cmd.ExecuteScalar();

                if (result is DBNull)
                    return default(T);


                return (T)result;
            }

        }
        public static T GetDatabaseScalarValue<T>(this SqlConnection conn, string sql, params SqlParameter[] parameters)
        {
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);
                var result = cmd.ExecuteScalar();

                if (result is DBNull)
                    return default(T);


                return (T)result;
            }

        }

        public static T? GetNullableScalarValue<T>(string sql, string connectionStringName, params SqlParameter[] parameters) where T : struct
        {

            var connectionString = ConfigurationManager.SqlConnectionString;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);


                conn.Open();

                var result = cmd.ExecuteScalar();

                if (DBNull.Value == result || null == result)
                    return new T?();


                return (T)result;
            }
        }

        public static async Task<T?> GetNullableScalarValueAsync<T>(this string sql, string connectionStringName, params SqlParameter[] parameters) where T : struct
        {

            var connectionString = ConfigurationManager.SqlConnectionString;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
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
        public static async Task<T?> GetNullableScalarValueAsync<T>(this SqlConnection conn, string sql, params SqlParameter[] parameters) where T : struct
        {

            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters.Any()) cmd.Parameters.AddRange(parameters);

                if (conn.State != ConnectionState.Open)
                    await conn.OpenAsync();

                var result = await cmd.ExecuteScalarAsync();

                if (DBNull.Value == result || null == result)
                    return new T?();


                return (T)result;
            }

        }
        public static async Task<object> GetScalarValueAsync(this SqlConnection conn, string sql, params SqlParameter[] parameters) 
        {

            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters.Any()) cmd.Parameters.AddRange(parameters);

                if (conn.State != ConnectionState.Open)
                    await conn.OpenAsync();

                var result = await cmd.ExecuteScalarAsync();

                if (DBNull.Value == result || null == result)
                    return null;


                return result;
            }

        }
        public static async Task<T> GetScalarValueAsync<T>(this SqlConnection conn, string sql, params SqlParameter[] parameters) where T : struct
        {
            var val = await conn.GetNullableScalarValueAsync<T>(sql, parameters);
            return val ?? default(T);
        }



        public static List<T> GetDatabaseList<T>(string command, string connectionStringName, params SqlParameter[] parameters)
        {
            var connectionString = ConfigurationManager.SqlConnectionString;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(command, conn))
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

        public static async Task<List<T>> GetDatabaseListAsync<T>(string command, string connectionStringName, params SqlParameter[] parameters)
        {
            var connectionString = ConfigurationManager.SqlConnectionString;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(command, conn))
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


        public static Dictionary<TKey, TValue> GetDatabaseKeyList<TKey, TValue>(string sql, string connectionStringName, params SqlParameter[] parameters)
        {

            var connectionString = ConfigurationManager.SqlConnectionString;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
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


        public static void ExecuteNonQuery(this string connectionStringName, string sql, params SqlParameter[] parameters)
        {
            var connectionString = ConfigurationManager.SqlConnectionString;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static async Task ExecuteNonQueryAsync(this string connectionStringName, string sql, params SqlParameter[] parameters)
        {
            var connectionString = ConfigurationManager.SqlConnectionString;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);

                await conn.OpenAsync();
                var row = await cmd.ExecuteNonQueryAsync();
                if (row == 0)
                {
                    Console.WriteLine("WHIIIIII " + sql);
                    Debugger.Break();
                }
            }
        }

        internal static void SetCurrentIdentity(string userName)
        {
            var id = new GenericIdentity(userName);
            Thread.CurrentPrincipal = new GenericPrincipal(id, new[] { "user" });
        }
    }
}