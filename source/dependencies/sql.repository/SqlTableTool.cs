using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.SqlRepository
{
    public class SqlTableTool
    {

        private readonly Action<string> m_writeMessage;
        private readonly Action<string> m_writeWarning;
        private readonly Action<Exception> m_writeError;

        public SqlTableTool(Action<string> writeMessage, Action<string> writeWarning = null,
            Action<Exception> writeError = null)
        {
            m_writeMessage = writeMessage;
            m_writeWarning = writeWarning;
            m_writeError = writeError;
        }
        protected void WriteMessage(string message, params object[] args)
        {
            m_writeMessage?.Invoke(string.Format(message, args));
        }

        protected void WriteWarning(string message, params object[] args)
        {
            m_writeWarning?.Invoke(string.Format(message, args));
        }

        protected void WriteError(Exception exception)
        {
            m_writeError?.Invoke(exception);
        }

        protected async Task<int?> GetSqlServerProductVersionAsync()
        {
            try
            {
                ObjectBuilder.GetObject<ILogger>()
                    .WriteInfo("Getting SQL Server Product Version .....");

                int? version;
                var cs = ConfigurationManager.SqlConnectionString;
                if (!cs.Contains(";Connection Timeout"))
                    cs += ";Connection Timeout=5";

                using (var conn = new SqlConnection(cs))
                using (var cmd = new SqlCommand("SELECT SERVERPROPERTY('ProductVersion')", conn))
                {
                    await conn.OpenAsync();
                    var pv = await cmd.ExecuteScalarAsync();
                    version = Strings.RegexInt32Value($"{pv}", @"(?<version>[0-9]{1,2})..*", "version");
                }
                return version;

            }
            catch (SqlException)
            {
                ObjectBuilder.GetObject<ILogger>()
                    .WriteWarning("Fail to connect to target SQL Server to get version info, revert to SqlServerProductVersion environment else use 13");
                return ConfigurationManager.GetEnvironmentVariableInt32("SqlServerProductVersion", 13);
            }
        }

    }
}