using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Management.Automation;

namespace sqlcmd
{
    [Cmdlet(VerbsLifecycle.Invoke, "SqlCmd")]
    public class SqlCmd : PSCmdlet
    {
        [Parameter(HelpMessage = "Use trusted connection", ParameterSetName = "A")]
        [Alias("E")]
        public SwitchParameter TrustedConnection { get; set; }

        [Parameter(HelpMessage = "servername\\instance-name", ParameterSetName = "A", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        [Alias("S")]
        public string Server { get; set; }

        [Parameter(HelpMessage = "Default database", ParameterSetName = "A", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        [Alias("d")]
        public string Database { get; set; }

        [Parameter(HelpMessage = "File to read the SQL scripts", ParameterSetName = "A", Position = 0)]
        [Alias("i")]
        public string InputFile { get; set; }

        [Parameter(HelpMessage = "SQL scripts", ParameterSetName = "A", Position = 0)]
        [Alias("q")]
        public string CommandQuery { get; set; }


        protected override void ProcessRecord()
        {
            string connectionString = $"server={this.Server};database={this.Database};trusted_connection={this.TrustedConnection.IsPresent.ToString().ToLowerInvariant()}";
            WriteVerbose($"Connecting .. to {connectionString}");
            var sql = this.CommandQuery;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                WriteVerbose($"Opening database....{this.Database}");
                conn.Open();
                if (sql.ToUpper().Contains("SELECT"))
                {
                    var table = new DataTable();
                    WriteVerbose($"Executing reader {sql}");
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var name = reader.GetName(i);
                            table.Columns.Add(name);
                            WriteVerbose("Column name  = " + name.ToString());
                        }
                        while (reader.Read())
                        {
                            var row = table.NewRow();
                            foreach (var column in table.Columns)
                            {
                                row[column.ToString().ToString()] = reader[column.ToString()];
                            }
                            table.Rows.Add(row);
                        }
                        WriteObject(table,true);
                    }
                }
                else
                {
                    WriteVerbose("Executing non query...");
                    var rows = cmd.ExecuteNonQuery();
                    WriteObject($"Execute nonquery {rows}");
                }


            }

            base.ProcessRecord();
        }
    }
}
