using Bespoke.Cycling.Domain;

namespace Bespoke.Station.Windows.ViewModels.Utils
{
    public partial class SettingAndOptionsViewModel 
    {
        private readonly ObjectCollection<Setting> m_settingCollection = new ObjectCollection<Setting>();
     
        public ObjectCollection<Setting> SettingCollection
        {
            get { return m_settingCollection; }
        }
        private string m_gmailUsername;
        private string m_gmailPassword;
        private string m_sapUsername;
        private string m_sapPassword;
        private string m_mystationUsername;
        private string m_mystationPassword;
        private string m_stationName;
        private int m_pdbAutoDebitAccountId;

        public int PdbAutoDebitAccountId
        {
            get { return m_pdbAutoDebitAccountId; }
            set
            {
                m_pdbAutoDebitAccountId = value;
                RaisePropertyChanged("PdbAutoDebitAccountId");
            }
        }

        public string StationName
        {
            get { return m_stationName; }
            set
            {
                m_stationName = value;
                RaisePropertyChanged("StationName");
            }
        }

        public string MystationPassword
        {
            get { return m_mystationPassword; }
            set
            {
                m_mystationPassword = value;
                RaisePropertyChanged("MystationPassword");
            }
        }
        public string MystationUsername
        {
            get { return m_mystationUsername; }
            set
            {
                m_mystationUsername = value;
                RaisePropertyChanged("MystationUsername");
            }
        }
        public string SapPassword
        {
            get { return m_sapPassword; }
            set
            {
                m_sapPassword = value;
                RaisePropertyChanged("SapPassword");
            }
        }
        public string SapUsername
        {
            get { return m_sapUsername; }
            set
            {
                m_sapUsername = value;
                RaisePropertyChanged("SapUsername");
            }
        }

        public string GmailPassword
        {
            get { return m_gmailPassword; }
            set
            {
                m_gmailPassword = value;
                RaisePropertyChanged("GmailPassword");
            }
        }

        public string GmailUsername
        {
            get { return m_gmailUsername; }
            set
            {
                m_gmailUsername = value;
                RaisePropertyChanged("GmailUsername");
            }
        }
    }
}
