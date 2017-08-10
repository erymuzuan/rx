using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
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
            this.DeploySelectedCommand = new RelayCommand<IList<EntityDeployment>>(list => { }, list => this.SelectedCollection.Count > 0);
        }

        private void Load()
        {
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

            this.Post(() => this.EntityDefinitionCollection.ClearAndAddRange(list));
        }

        public DispatcherObject View { get; set; }
    }
}
