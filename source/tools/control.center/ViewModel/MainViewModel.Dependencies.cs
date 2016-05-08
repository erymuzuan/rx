using System.IO;
using Bespoke.Sph.ControlCenter.Model;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private bool m_isSetup;
        private bool m_elasticSearchServiceStarted;
        private bool m_rabbitMqServiceStarted;
        private bool m_rabbitMqServiceStarting;
        private bool m_sqlServiceStarted;
        private string m_elasticSearchStatus;
        private string m_rabbitMqStatus;
        private string m_iisStatus;
        private string m_sqlServiceStatus;
        private string m_iisServiceStatus;
        private string m_sphWorkersStatus;
        private SphSettings m_settings;

        public SphSettings Settings
        {
            get { return m_settings; }
            set
            {
                m_settings = value;
                OnPropertyChanged();
            }
        }

        public bool IsSetup
        {
            get { return m_isSetup; }
            set
            {
                m_isSetup = value;
                OnPropertyChanged();
                this.StartIisServiceCommand.RaiseCanExecuteChanged();
                this.StartSphWorkerCommand.RaiseCanExecuteChanged();
            }
        }






        private TextWriter m_writer;
        public TextWriter TextWriter
        {
            set
            {
                m_writer = value;
                OnPropertyChanged();
            }
            get { return m_writer; }
        }




        public bool ElasticSearchServiceStarted
        {
            set
            {
                m_elasticSearchServiceStarted = value;
                OnPropertyChanged();
                this.StopRabbitMqCommand.RaiseCanExecuteChanged();
            }
            get { return m_elasticSearchServiceStarted; }
        }


        public bool RabbitMqServiceStarting
        {
            get { return m_rabbitMqServiceStarting; }
            set
            {
                m_rabbitMqServiceStarting = value;
                OnPropertyChanged();
                this.StartElasticSearchCommand.RaiseCanExecuteChanged();
                this.StartSphWorkerCommand.RaiseCanExecuteChanged();
                this.StartIisServiceCommand.RaiseCanExecuteChanged();
            }
        }

        private bool m_webConsoleStarted;

        public bool WebConsoleStarted
        {
            get { return m_webConsoleStarted; }
            set
            {
                m_webConsoleStarted = value;
                RaisePropertyChanged("WebConsoleStarted");
                this.StartWebConsoleCommand.RaiseCanExecuteChanged();
                this.StopWebConsoleCommand.RaiseCanExecuteChanged();
            }
        }
        public bool RabbitMqServiceStarted
        {
            set
            {
                m_rabbitMqServiceStarted = value;
                OnPropertyChanged();
            }
            get { return m_rabbitMqServiceStarted; }
        }



        private bool m_iisServiceStarted;
        public bool IisServiceStarted
        {
            set
            {
                m_iisServiceStarted = value;
                OnPropertyChanged();
                this.StopRabbitMqCommand.RaiseCanExecuteChanged();
                this.StartIisServiceCommand.RaiseCanExecuteChanged();
                this.StopIisServiceCommand.RaiseCanExecuteChanged();
            }
            get { return m_iisServiceStarted; }
        }

        public bool SqlServiceStarted
        {
            set
            {
                m_sqlServiceStarted = value;
                OnPropertyChanged();
                this.StartSphWorkerCommand.RaiseCanExecuteChanged();
            }
            get { return m_sqlServiceStarted; }
        }

        private bool m_sphWorkerServiceStarted;
        private bool m_isBusy;
        private string m_busyMessage;

        public string BusyMessage
        {
            get { return m_busyMessage; }
            set
            {
                m_busyMessage = value;
                RaisePropertyChanged("BusyMessage");
            }
        }

        public void StartBusy(string message)
        {
            this.BusyMessage = message;
            this.IsBusy = true;
        }
        public void StopBusy()
        {
            this.IsBusy = false;
            this.BusyMessage = "";
        }

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                OnPropertyChanged();
                this.StopSphWorkerCommand.RaiseCanExecuteChanged();
            }
        }
        public bool SphWorkerServiceStarted
        {
            set
            {
                m_sphWorkerServiceStarted = value;
                OnPropertyChanged();
                this.StopRabbitMqCommand.RaiseCanExecuteChanged();
                this.StartSphWorkerCommand.RaiseCanExecuteChanged();
                this.StopSphWorkerCommand.RaiseCanExecuteChanged();
            }
            get { return m_sphWorkerServiceStarted; }
        }

        public string ElasticSearchStatus
        {
            set
            {
                m_elasticSearchStatus = value;
                OnPropertyChanged();
            }
            get { return m_elasticSearchStatus; }
        }

        public string RabbitMqStatus
        {
            set
            {
                m_rabbitMqStatus = value;
                OnPropertyChanged();
            }
            get { return m_rabbitMqStatus; }
        }

        public string IisStatus
        {
            set
            {
                m_iisStatus = value;
                OnPropertyChanged();
            }
            get { return m_iisStatus; }
        }


        public string SqlServiceStatus
        {
            set
            {
                m_sqlServiceStatus = value;
                OnPropertyChanged();
            }
            get { return m_sqlServiceStatus; }
        }

        public string IisServiceStatus
        {
            set
            {
                m_iisServiceStatus = value;
                OnPropertyChanged();
            }
            get { return m_iisServiceStatus; }
        }


        public string SphWorkersStatus
        {
            set
            {
                m_sphWorkersStatus = value;
                OnPropertyChanged();
            }
            get { return m_sphWorkersStatus; }
        }
        
    }
}
