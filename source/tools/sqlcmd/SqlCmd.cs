using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Management.Automation;

namespace sqlcmd
{
    [Cmdlet(VerbsLifecycle.Invoke, "SqlCmdRx", DefaultParameterSetName = "TrustedConnection")]
    [OutputType(typeof(DataTable))]
    public class SqlCmd : PSCmdlet
    {
        [Parameter(HelpMessage = "Use trusted connection", ParameterSetName = "TrustedConnection")]
        [Alias("E")]
        public SwitchParameter TrustedConnection { get; set; }

        [Parameter(HelpMessage = "servername\\instance-name", ParameterSetName = "TrustedConnection", Mandatory = false)]
        [Parameter(HelpMessage = "SQL scripts", ParameterSetName = "SqlAuthentication", Position = 0)]
        [ValidateNotNullOrEmpty]
        [Alias("S")]
        public string Server { get; set; } = ".";

        [Parameter(HelpMessage = "Default database", ParameterSetName = "TrustedConnection", Mandatory = false)]
        [Parameter(HelpMessage = "SQL scripts", ParameterSetName = "SqlAuthentication", Position = 0)]
        [ValidateNotNullOrEmpty]
        [Alias("d")]
        public string Database { get; set; } = "master";

        [Parameter(HelpMessage = "File to read the SQL scripts", ParameterSetName = "TrustedConnection", Position = 0)]
        [Parameter(HelpMessage = "SQL scripts", ParameterSetName = "SqlAuthentication", Position = 0)]
        [Alias("i")]
        public string InputFile { get; set; }

        [Parameter(HelpMessage = "SQL scripts", ParameterSetName = "TrustedConnection", Position = 0)]
        [Parameter(HelpMessage = "SQL scripts", ParameterSetName = "SqlAuthentication", Position = 0)]
        [Alias("q")]
        public string CommandQuery { get; set; }

        [Parameter(HelpMessage = "The userid if using Sql authentication", ParameterSetName = "SqlAuthentication", Position = 0)]
        [Alias("u")]
        public string UserId { get; set; } = "sa";

        [Parameter(HelpMessage = "The password if using Sql authentication", ParameterSetName = "SqlAuthentication", Position = 0)]
        [Alias("p")]
        public string Password { get; set; }

        [Parameter(HelpMessage = "Result set type", ParameterSetName = "TrustedConnection", Position = 0)]
        [Parameter(HelpMessage = "Result set type", ParameterSetName = "SqlAuthentication", Position = 0)]
        [ValidateSet("Reader", "Scalar", "NoQuery")]
        public string ResultSetType { get; set; } = "Reader";


        protected override void ProcessRecord()
        {
            string connectionString = $"server={this.Server};database={this.Database};trusted_connection={this.TrustedConnection.IsPresent.ToString().ToLowerInvariant()}";
            WriteVerbose($"Connecting .. to {connectionString}");
            var sql = this.CommandQuery;
            if (string.IsNullOrWhiteSpace(sql) && string.IsNullOrWhiteSpace(this.InputFile))
            {
                WriteError(new ErrorRecord(new PSArgumentNullException(nameof(SqlCmd.CommandQuery), "CommandQuery or InputFile must be supplied"), "1", ErrorCategory.InvalidArgument, this));
                return;
            }

            if (string.IsNullOrWhiteSpace(sql) && !File.Exists(this.InputFile))
            {
                WriteError(new ErrorRecord(new PSArgumentNullException(nameof(SqlCmd.CommandQuery), "InputFile does not exist"), "1", ErrorCategory.InvalidArgument, this));
                return;
            }
            if (string.IsNullOrWhiteSpace(sql))
            {
                sql = File.ReadAllText(this.InputFile);
            }
            string[] commands = sql.Split(new[] { "GO\r\n", "GO ", "GO\t" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var c in commands)
            {
                try
                {
                    ExecuteSqlCommand(connectionString, c);
                }
                catch (Exception e)
                {
                    WriteError(new ErrorRecord(e,"99", ErrorCategory.ConnectionError, null));
                }
            }




            base.ProcessRecord();
        }

        private bool ExecuteSqlCommand(string connectionString, string sql)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                WriteVerbose($"Opening database....{this.Database}");
                conn.Open();
                if (this.ResultSetType == "Reader")
                {
                    var table = new DataTable();
                    WriteVerbose($"Executing reader {sql}");
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var name = reader.GetName(i);
                            table.Columns.Add(name);
                            WriteVerbose("Column name  = " + name);
                        }
                        while (reader.Read())
                        {
                            var row = table.NewRow();
                            foreach (var column in table.Columns)
                            {
                                row[column.ToString()] = reader[column.ToString()];
                            }
                            table.Rows.Add(row);
                        }
                        WriteObject(table, true);
                    }
                    return true;
                }
                if (ResultSetType == "Scalar")
                {
                    WriteVerbose("Executing scalar...");
                    var result = cmd.ExecuteScalar();
                    WriteObject(result);
                }

                if (ResultSetType == "NonQuery")
                {
                    WriteVerbose("Executing non query...");
                    var rows = cmd.ExecuteNonQuery();
                    WriteObject($"Execute nonquery {rows}");
                }
            }
            return false;
        }
    }
}
