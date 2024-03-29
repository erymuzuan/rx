﻿using System.IO;
using Bespoke.Sph.ControlCenter.Model;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class InMemoryBrokerViewModel : ViewModelBase
    {
        private bool m_isSetup;
        private bool m_elasticSearchServiceStarted;
        private bool m_sqlServiceStarted;
        private string m_elasticSearchStatus;
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
            }
            get { return m_elasticSearchServiceStarted; }
        }


   



        private bool m_iisServiceStarted;
        public bool IisServiceStarted
        {
            set
            {
                m_iisServiceStarted = value;
                OnPropertyChanged();
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
            }
            get { return m_sqlServiceStarted; }
        }

        private bool m_isBusy;

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                OnPropertyChanged();
            }
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
