using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

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

        protected  async Task<int?> GetSqlServerProductVersionAsync()
        {
            int? version;
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand("SELECT SERVERPROPERTY('ProductVersion')", conn))
            {
                await conn.OpenAsync();
                var pv = await cmd.ExecuteScalarAsync();
                version = Strings.RegexInt32Value($"{pv}", @"(?<version>[0-9]{1,2})..*", "version");
            }
            return version;
        }

    }
}