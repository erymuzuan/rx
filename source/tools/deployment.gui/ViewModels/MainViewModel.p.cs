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
