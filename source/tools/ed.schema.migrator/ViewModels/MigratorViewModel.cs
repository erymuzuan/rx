using System;
using System.Data.SqlClient;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ed.schema.migrator.ViewModels
{
    public partial class MigratorViewModel : ViewModelBase
    {
        public RelayCommand ConnectCommand { get; set; }

        public MigratorViewModel()
        {
            this.ConnectCommand = new RelayCommand(Connect, () => !string.IsNullOrWhiteSpace(this.SqlServerName) && !this.IsBusy);
        }

        private async void Connect()
        {
            this.IsBusy = true;
            this.Message = "Connecting....";

            var connString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False",
                this.SqlServerName, this.ApplicationName);
            using (var conn = new SqlConnection(connString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    this.Message = "Fail to connect to " + this.SqlServerName;
                    this.IsBusy = false;
                }
            }


        }

        protected override void RaisePropertyChanged(string propertyName = null)
        {
            this.ConnectCommand.RaiseCanExecuteChanged();
            base.RaisePropertyChanged(propertyName);
        }
    }
}