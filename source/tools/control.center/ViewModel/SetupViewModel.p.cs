using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class SetupViewModel 
    {
        private SphSettings m_settings;
        private Visibility m_generalVisible;
        private Visibility m_sqlServerVisible;
        private Visibility m_elasticsearchVisible;
        private Visibility m_rabbitMqVisible;
        private string m_currentTab;
        private Visibility m_setupVisible;
        private bool m_isBusy;

        public ObservableCollection<LogEntry> LogCollection { get; } = new ObservableCollection<LogEntry>();

        public double Progress
        {
            get { return m_progress; }
            set
            {
                m_progress = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                OnPropertyChanged();
                this.FinishCommand.RaiseCanExecuteChanged();
                this.PreviousCommand.RaiseCanExecuteChanged();
                this.NextCommand.RaiseCanExecuteChanged();
            }
        }

        public Visibility SetupVisible
        {
            get { return m_setupVisible; }
            set
            {
                m_setupVisible = value;
                OnPropertyChanged();
            }
        }

        public string CurrentTab
        {
            get { return m_currentTab; }
            set
            {
                m_currentTab = value;
                OnPropertyChanged();
            }
        }

        public Visibility RabbitMqVisible
        {
            get { return m_rabbitMqVisible; }
            set
            {
                m_rabbitMqVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility ElasticsearchVisible
        {
            get { return m_elasticsearchVisible; }
            set
            {
                m_elasticsearchVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility SqlServerVisible
        {
            get { return m_sqlServerVisible; }
            set
            {
                m_sqlServerVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility GeneralVisible
        {
            get { return m_generalVisible; }
            set
            {
                m_generalVisible = value;
                OnPropertyChanged();
            }
        }
        public SphSettings Settings
        {
            get { return m_settings; }
            set
            {
                m_settings = value;
                OnPropertyChanged();
            }
        }

   
        private double m_progress;
        private string m_status;



        public DispatcherObject View { get; set; }

        public string Status
        {
            get { return m_status; }
            set
            {
                m_status = value;
                OnPropertyChanged();
            }
        }
    }
}
