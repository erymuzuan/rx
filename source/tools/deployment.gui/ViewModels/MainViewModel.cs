using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Management;
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
        public RelayCommand<EntityDeployment> CompileCommand { get; set; }
        public RelayCommand<EntityDeployment> DiffCommand { get; set; }
        [Import]
        public MigrationScriptViewModel MigrationScriptViewModel { get; set; }

        [Import(typeof(IReadOnlyRepositoryManagement))]
        private IReadOnlyRepositoryManagement ReadOnlyRepositoryManagement { get; set; }
        [Import(typeof(IRepositoryManagement))]
        private IRepositoryManagement RepositoryManagement { get; set; }

        public MainViewModel()
        {
            this.LoadCommand = new RelayCommand(Load, () => true);
            this.DeploySelectedCommand = new RelayCommand<IList<EntityDeployment>>(DeploySelectedItems, list => SelectedCollection.Count > 0 && IsReadOnlyRepositoryRunning && IsRepositoryRunning);
            this.CompileCommand = new RelayCommand<EntityDeployment>(Compile, ed => ed.CanCompile);
            this.DiffCommand = new RelayCommand<EntityDeployment>(Diff, ed => ed.CanDiff);
            

        }

        private void Diff(EntityDeployment deployment)
        {
            var ed = deployment.EntityDefinition;
            var info = new ProcessStartInfo
            {
                FileName = "sph.builder.exe",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Arguments = $"/e:{ed.Name} /diff"

            };
            var agent = Process.Start(info);
            agent?.WaitForExit();

            this.Load();
            this.MigrationScriptViewModel.Load().Wait();
        }

        private void Compile(EntityDeployment deployment)
        {
            var ed = deployment.EntityDefinition;
            var info = new ProcessStartInfo
            {
                FileName = "sph.builder.exe",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Arguments = $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\/{ed.Id}.json"

            };
            var agent = Process.Start(info);
            agent?.WaitForExit();

            this.Load();
        }

        private void DeploySelectedItems(IList<EntityDeployment> obj)
        {
            foreach (var ed in this.SelectedCollection)
            {
                var info = new ProcessStartInfo
                {
                    FileName = "deployment.agent.exe",
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    Arguments = $"/e:{ed.EntityDefinition.Name} /deploy /plan:{ed}"

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
            var metadataRepository = ObjectBuilder.GetObject<IDeploymentMetadataRepository>();
            await metadataRepository.InitializeAsync();
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
            this.Post(() => this.IsBusy = false);

            // now check sql server and elasticsearch
            while (true)
            {
                this.IsRepositoryRunning = await this.RepositoryManagement.GetAccesibleStatusAsync();
                this.IsReadOnlyRepositoryRunning = await this.ReadOnlyRepositoryManagement.GetAccesibleStatusAsync();
              
                await Task.Delay(5000);

            }

        }

        public DispatcherObject View { get; set; }
    }
}
