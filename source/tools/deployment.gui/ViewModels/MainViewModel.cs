using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        private IReadOnlyRepositoryManagement ReadOnlyRepositoryManagement { get; }

        public MainViewModel()
        {
            this.LoadCommand = new RelayCommand(Load, () => true);
            this.DeploySelectedCommand = new RelayCommand<IList<EntityDeployment>>(DeploySelectedItems, list => SelectedCollection.Count > 0 && IsReadOnlyAccesible && IsRepositoryAccessible);
            this.CompileCommand = new RelayCommand<EntityDeployment>(Compile, ed => ed.CanCompile);
            this.DiffCommand = new RelayCommand<EntityDeployment>(Diff, ed => ed.CanDiff);

            this.ReadOnlyRepositoryManagement = ObjectBuilder.GetObject<IReadOnlyRepositoryManagement>();
            this.ReadOnlyRepositoryManagement.RegisterConnectionChanged(status => 500);

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


        public async Task InitializeAsync()
        {
            const string SQL = @"
CREATE TABLE [Sph].[DeploymentMetadata](
     [Id] INT PRIMARY KEY NOT NULL IDENTITY(1,1)
    ,[Name] VARCHAR(255)  NULL
    ,[EdId] VARCHAR(255) NOT NULL
    ,[Tag] VARCHAR(255) NOT NULL
    ,[Revision] VARCHAR(255)  NULL
    ,[DateTime] SMALLDATETIME NOT NULL DEFAULT GETDATE()
    ,[Source] VARCHAR(MAX)
)

                ";
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var checkTableCommand = new SqlCommand(@"SELECT COUNT(*)
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Sph' 
                 AND  TABLE_NAME = 'DeploymentMetadata'", conn))
            {
                await conn.OpenAsync();
                var exist = (int)await checkTableCommand.ExecuteScalarAsync() == 1;
                if (!exist)
                {
                    using (var createTableCommand = new SqlCommand(SQL, conn))
                    {
                        await createTableCommand.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        private async void LoadHelperAsync()
        {
            var context = new SphDataContext();
            var list = context.LoadFromSources<EntityDefinition>()
                .Select(x => new EntityDeployment { EntityDefinition = x })
                .ToList();

            await this.InitializeAsync();
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
                using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM [Sph].[Message]", conn))
                {
                    try
                    {
                        await conn.OpenAsync();
                        var rows = await cmd.ExecuteScalarAsync();
                        this.IsRepositoryAccessible = (int)rows >= 0;
                    }
                    catch (SqlException)
                    {
                        this.IsRepositoryAccessible = false;
                    }
                }

                var esHost =
                    Environment.GetEnvironmentVariable(
                        $"RX_{ConfigurationManager.ApplicationNameToUpper}_ElasticSearchHost")
                    ?? "http://localhost:9200";

                using (var client = new HttpClient { BaseAddress = new Uri(esHost) })
                {
                    try
                    {
                        var cat = await client.GetStringAsync("_cat/indices");
                        this.IsReadOnlyAccesible = cat.Contains(ConfigurationManager.ApplicationName.ToLowerInvariant());
                    }
                    catch
                    {
                        //ignore
                        this.IsReadOnlyAccesible = false;
                    }
                }
                await Task.Delay(5000);

            }

        }

        public DispatcherObject View { get; set; }
    }
}
