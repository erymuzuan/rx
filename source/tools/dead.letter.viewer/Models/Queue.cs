namespace Bespoke.Station.Windows.RabbitMqDeadLetter.Models
{
    public class Queue:ModelBase
    {
        public override string ToString()
        {
            return string.Format("{1}:{0}\t:{2}", this.Name, this.VirtualHost, this.MessagesCount);
        }

        private string m_name;
        private string m_virtualHost;
        private bool m_isDurable;
        private bool m_isAutoDelete;
        private long m_messagesCount;
        private long m_pendingAcks;

        public long PendingAcks
        {
            get { return m_pendingAcks; }
            set
            {
                m_pendingAcks = value;
                RaisePropertyChanged();
            }
        }

        public long MessagesCount
        {
            get { return m_messagesCount; }
            set
            {
                m_messagesCount = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAutoDelete
        {
            get { return m_isAutoDelete; }
            set
            {
                m_isAutoDelete = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDurable
        {
            get { return m_isDurable; }
            set
            {
                m_isDurable = value;
                RaisePropertyChanged();
            }
        }

        public string VirtualHost
        {
            get { return m_virtualHost; }
            set
            {
                m_virtualHost = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                RaisePropertyChanged();
            }
        }
    }
}