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


        public MainViewModel()
        {
            this.LoadCommand = new RelayCommand(Load, () => true);
            this.DeploySelectedCommand = new RelayCommand<IList<EntityDeployment>>(DeploySelectedItems, list => SelectedCollection.Count > 0 &&  IsElasticsearchAccesible && IsSqlServerAccessible);
            this.CompileCommand = new RelayCommand<EntityDeployment>(Compile, ed => ed.CanCompile);
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
                        this.IsSqlServerAccessible = (int)rows >= 0;
                    }
                    catch (SqlException)
                    {
                        this.IsSqlServerAccessible = false;
                    }
                }

                using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) })
                {
                    try
                    {
                        var cat = await client.GetStringAsync("_cat/indices");
                        this.IsElasticsearchAccesible = cat.Contains(ConfigurationManager.ApplicationName.ToLowerInvariant());
                    }
                    catch
                    {
                        //ignore
                        this.IsElasticsearchAccesible = false;
                    }
                }
                await Task.Delay(5000);

            }

        }

        public DispatcherObject View { get; set; }
    }
}
