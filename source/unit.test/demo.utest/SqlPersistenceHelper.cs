using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Threading;
using System.Linq;

namespace web.test
{
    public static class SqlPersistenceHelper
    {
        /// <summary>
        /// Make sure the current Thread CurrentPrincipal runnin with the specified username and role
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

        public static T GetDatabaseScalarValue<T>( this ISqlConnectionConsumer helper, string sql,  params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(helper.ConnectionString))
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

        public static T? GetNullableScalarValue<T>(this ISqlConnectionConsumer helper, string sql, params SqlParameter[] parameters) where T : struct
        {

            using (var conn = new SqlConnection(helper.ConnectionString))
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



        public static List<T> GetDatabaseList<T>(this ISqlConnectionConsumer helper, string command, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(helper.ConnectionString))
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


        public static Dictionary<TKey, TValue> GetDatabaseKeyList<TKey, TValue>(this ISqlConnectionConsumer helper, string sql, params SqlParameter[] parameters)
        {
            
            using (var conn = new SqlConnection(helper.ConnectionString))
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


        public static void ExecuteNonQuery(this ISqlConnectionConsumer helper, string sql, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(helper.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
