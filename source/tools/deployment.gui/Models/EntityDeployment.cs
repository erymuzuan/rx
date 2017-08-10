using System;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Mangements.Models
{
    public class EntityDeployment : DomainObject
    {
        private EntityDefinition m_entityDefinition;
        public ObjectCollection<DeploymentHistory> HistoryCollection { get; } = new ObjectCollection<DeploymentHistory>();
        

        public DateTime? LastDeployedDateTime => this.HistoryCollection.OrderBy(x => x.DateTime).LastOrDefault()?.DateTime;
        public string LastDeployedRevision => this.HistoryCollection.OrderBy(x => x.DateTime).LastOrDefault()?.Revision;
        public string LastDeployedTag => this.HistoryCollection.OrderBy(x => x.DateTime).LastOrDefault()?.Tag;
        private bool m_isSelected;

        public bool IsSelected
        {
            get => m_isSelected;
            set
            {
                m_isSelected = value;
                RaisePropertyChanged();
            }
        }
        public EntityDefinition EntityDefinition
        {
            get => m_entityDefinition;
            set
            {
                m_entityDefinition = value;
                RaisePropertyChanged();
            }
        }
    }
}
