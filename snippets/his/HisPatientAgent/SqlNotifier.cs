using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class SqlNotifier : IDisposable
    {
        public SqlCommand CurrentCommand { get; set; }
        private SqlConnection _connection;
        public SqlConnection CurrentConnection
        {
            get
            {
                this._connection = this._connection ??
                                  new SqlConnection(this.ConnectionString);
                return this._connection;
            }
        }
        public string ConnectionString
        {
            get
            {
                return @"Data Source=(localdb)\ProjectsV12;Initial Catalog=His;Integrated Security=True";
            }
        }

        public SqlNotifier()
        {
            SqlDependency.Start(this.ConnectionString);

        }

        public event EventHandler<SqlNotificationEventArgs> NewMessageArrived;

        public async Task<DataTable> RegisterDependency()
        {

            this.CurrentCommand = new SqlCommand("Select [Id], [Rowid], [Timestamp] from dbo.TransactionQueue", this.CurrentConnection) { Notification = null };

            var dependency = new SqlDependency(this.CurrentCommand);
            dependency.OnChange += QueryChanged;

            if (this.CurrentConnection.State == ConnectionState.Closed)
                await this.CurrentConnection.OpenAsync();
            try
            {

                var dt = new DataTable();
                using (var reader = await this.CurrentCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    dt.Load(reader);
                    return dt;
                }
            }
            catch { return null; }
        }

        void QueryChanged(object sender, SqlNotificationEventArgs e)
        {
            var dependency = (SqlDependency)sender;
            dependency.OnChange -= QueryChanged;

            if (null != this.NewMessageArrived)
                NewMessageArrived(sender, e);
            RegisterDependency().Wait();
            Console.WriteLine("---------");
        }



        #region IDisposable Members

        public void Dispose()
        {
            SqlDependency.Stop(this.ConnectionString);
        }

        #endregion
    }
}