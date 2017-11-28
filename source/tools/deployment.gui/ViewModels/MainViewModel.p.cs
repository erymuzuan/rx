using Bespoke.Sph.Domain;
using Bespoke.Sph.Mangements.Models;

namespace Bespoke.Sph.Mangements.ViewModels
{
    public partial class MainViewModel
    {
        private string m_title;
        public ObjectCollection<DeploymentHistory> DeploymentHistoryCollection { get; } = new ObjectCollection<DeploymentHistory>();
        public ObjectCollection<EntityDeployment> EntityDefinitionCollection { get; } = new ObjectCollection<EntityDeployment>();
        public ObjectCollection<EntityDeployment> SelectedCollection { get; } = new ObjectCollection<EntityDeployment>();
        private EntityDeployment m_selected;
        public string SqlServerConnection => ConfigurationManager.SqlConnectionString;
        public string RxHomePath => ConfigurationManager.Home;
        public string RxSourcePath => ConfigurationManager.SphSourceDirectory;
        public string RxOutputPath => ConfigurationManager.CompilerOutputPath;
        private bool m_isRepositoryAccessible;
        private bool m_isReadOnlyAccesible;

        public bool IsReadOnlyAccesible
        {
            get => m_isReadOnlyAccesible;
            set
            {
                m_isReadOnlyAccesible = value;
                RaisePropertyChanged("IsElasticsearchAccesible");
            }
        }

        public bool IsRepositoryAccessible
        {
            get => m_isRepositoryAccessible;
            set
            {
                m_isRepositoryAccessible = value;
                RaisePropertyChanged("IsSqlServerAccessible");
            }
        }

        private bool m_isBusy;
        private string m_busyMessage;

        public string BusyMessage
        {
            get => m_busyMessage;
            set
            {
                m_busyMessage = value;
                RaisePropertyChanged("BusyMessage");
            }
        }

        public bool IsBusy
        {
            get => m_isBusy;
            set
            {
                m_isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }
        public EntityDeployment Selected
        {
            get => m_selected;
            set
            {
                m_selected = value;
                RaisePropertyChanged("Selected");
            }
        }
        public string Title
        {
            get => m_title;
            set
            {
                m_title = value;
                RaisePropertyChanged("Title");
            }
        }
    }
}
