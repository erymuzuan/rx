using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository.Management
{
    public class SqlServerManagement 
    {
        public async Task<bool> GetAccesibleStatusAsync()
        {
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM [Sph].[Message]", conn))
            {
                try
                {
                    await conn.OpenAsync();
                    var rows = await cmd.ExecuteScalarAsync();
                    return (int)rows >= 0;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public void RegisterConnectionChanged(Func<bool, int> connectionStateChanged)
        {
            this.GetAccesibleStatusAsync()
                .ContinueWith(_ =>
                {
                    var wait = connectionStateChanged(_.Result);
                    Task.Delay(wait).ContinueWith(x =>
                    {
                        RegisterConnectionChanged(connectionStateChanged);
                    });
                })
                .ConfigureAwait(false);

        }

        public void OpenManagementConsole()
        {
            try
            {
                /*
                 ---------------------------
Microsoft SQL Server Management Studio
---------------------------
Usage:

ssms.exe [-S server_name[\instance_name]] [-d database] [-U user] [-P password] [-E] [-nosplash] [file_name[, file_name]*] [-log [file_name]?] [-?]

[-S  The name of the SQL Server instance to connect to]
[-d  The name of the SQL Server database to connect to]
[-E]  Use Windows Authentication to login to SQL Server
[-U  The name of the SQL Server login to connect with]
[-P  The password associated with the login]
[-nosplash] Suppress splash screen
[file_name[, file_name]*] Names of files to load
[-log [file_name]?] Logs SQL Server Management Studio activity to the specified file for troubleshooting
[-?]  Displays this usage information
---------------------------
OK   
---------------------------

 */
                Process.Start("ssms");
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Fail to start SSMS \r\n" + ex.Message, "Rx", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}