﻿using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
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
        private bool m_isSqlServerAccessible;
        private bool m_isElasticsearchAccesible;

        public bool IsElasticsearchAccesible
        {
            get => m_isElasticsearchAccesible;
            set
            {
                m_isElasticsearchAccesible = value;
                RaisePropertyChanged("IsElasticsearchAccesible");
            }
        }

        public bool IsSqlServerAccessible
        {
            get => m_isSqlServerAccessible;
            set
            {
                m_isSqlServerAccessible = value;
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
        public string ElasticsearchServer =>
            $"{EsConfigurationManager.Host}/{EsConfigurationManager.Index}";
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