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
        private bool m_truncate;
        private bool m_skipElasticsearch;
        private bool m_canSkipElasticsearch;
        private bool m_canTruncate;
        public bool Outdated => LastDeployedDateTime == null || LastDeployedDateTime < EntityDefinition.ChangedDate;
        private DateTime? m_compiledDateTime;
        private bool m_canDeploy;

        public bool CanDeploy => (!LastDeployedDateTime.HasValue && this.CompiledDateTime.HasValue) || ( LastDeployedDateTime.HasValue && EntityDefinition.ChangedDate > LastDeployedDateTime);

        public DateTime? CompiledDateTime
        {
            get => m_compiledDateTime;
            set
            {
                m_compiledDateTime = value;
                RaisePropertyChanged();
            }
        }
        public bool CanTruncate
        {
            get => m_canTruncate;
            set
            {
                m_canTruncate = value;
                RaisePropertyChanged();
            }
        }

        public bool CanSkipElasticsearch
        {
            get => m_canSkipElasticsearch;
            set
            {
                m_canSkipElasticsearch = value;
                RaisePropertyChanged();
            }
        }

        public bool SkipElasticsearch
        {
            get => m_skipElasticsearch;
            set
            {
                m_skipElasticsearch = value;
                RaisePropertyChanged();
            }
        }

        public bool Truncate
        {
            get => m_truncate;
            set
            {
                m_truncate = value;
                RaisePropertyChanged();
            }
        }
        public bool IsSelected
        {
            get => m_isSelected;
            set
            {
                m_isSelected = value;

                this.CanSkipElasticsearch = !EntityDefinition.TreatDataAsSource && value;
                this.CanTruncate = EntityDefinition.TreatDataAsSource && value;
                RaisePropertyChanged(nameof(CanSkipElasticsearch));
                RaisePropertyChanged(nameof(CanTruncate));
                RaisePropertyChanged();
            }
        }
        public EntityDefinition EntityDefinition
        {
            get => m_entityDefinition;
            set
            {
                m_entityDefinition = value;
                this.CanSkipElasticsearch = !value.TreatDataAsSource && this.IsSelected;
                this.CanTruncate = value.TreatDataAsSource && this.IsSelected;
                RaisePropertyChanged(nameof(CanSkipElasticsearch));
                RaisePropertyChanged(nameof(CanTruncate));
                RaisePropertyChanged();
            }
        }
    }
}
