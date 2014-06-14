using System;
using System.Threading.Tasks;
using Bespoke.Sph.Integrations.Adapters;
using Oracle.ManagedDataAccess.Client;

namespace oracle.adapter.test
{
    public static class OracleHelpers
    {
        public static async Task<T> GetScalarAsync<T>(this OracleAdapter adapter, string sql)
        {
            using (var conn = new OracleConnection(adapter.ConnectionString))
            using (var cmd = new OracleCommand(sql, conn))
            {
                await conn.OpenAsync();
                var val = (await cmd.ExecuteScalarAsync());
                Console.WriteLine(val.GetType().FullName);
                return (T)val;
            }
        }
    }
}