using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Mangements.Models;
using Dapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.Mangements.ViewModels
{
    [Export]
    public partial class MainViewModel : ViewModelBase, IView
    {
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand<IList<EntityDeployment>> DeploySelectedCommand { get; set; }


        public MainViewModel()
        {
            this.LoadCommand = new RelayCommand(Load, () => true);
            this.DeploySelectedCommand = new RelayCommand<IList<EntityDeployment>>(DeploySelectedItems, list => this.SelectedCollection.Count > 0);
        }

        private void DeploySelectedItems(IList<EntityDeployment> obj)
        {
            foreach (var ed in this.SelectedCollection)
            {
                var info = new ProcessStartInfo
                {
                    FileName = "deployment.agent.exe",
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    Arguments = $"/e:{ed.EntityDefinition.Name}"

                };
                var agent = Process.Start(info);
                agent?.WaitForExit();
            }
            this.Load();
        }

        private void Load()
        {
            this.IsBusy = true;
            this.BusyMessage = "Loading your assets....";
            this.QueueUserWorkItem(LoadHelperAsync);

        }

        private async void LoadHelperAsync()
        {
            var context = new SphDataContext();
            var list = context.LoadFromSources<EntityDefinition>()
                .Select(x => new EntityDeployment { EntityDefinition = x })
                .ToList();

            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            {
                await conn.OpenAsync();
                var histories = (await conn.QueryAsync<DeploymentHistory>("SELECT * FROM [Sph].[DeploymentMetadata]")).ToList();
                foreach (var model in list)
                {
                    model.HistoryCollection.ClearAndAddRange(histories.Where(x => x.Name == model.EntityDefinition.Name));
                }
            }

            foreach (var model in list)
            {
                var dll = new FileInfo($"{ConfigurationManager.CompilerOutputPath}\\{ConfigurationManager.ApplicationName}.{model.EntityDefinition.Name}.dll");
                if (dll.Exists)
                    model.CompiledDateTime = dll.CreationTime;
            }

            this.Post(() => this.EntityDefinitionCollection.ClearAndAddRange(list));
            this.Post(()=> this.IsBusy = false);
        }

        public DispatcherObject View { get; set; }
    }
}
