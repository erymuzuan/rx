using System.Windows.Threading;
using Bespoke.Station.Windows.RabbitMqDeadLetter.Models;
using RabbitMQ.Client;

namespace Bespoke.Station.Windows.RabbitMqDeadLetter.ViewModels
{
    public partial class MainViewModel
    {
        private string m_message;
        private BasicGetResult m_result;
        private string m_routingKey;
        private bool m_isBusy;
        private XDeathHeader m_deathHeader;
        private bool m_isCompress;
        private bool m_isAfterProcessGetNextMessage;
        private bool m_isNextMessageDecompress;
        private bool m_isNextMessageReformat;


        public bool IsNextMessageReformat
        {
            get { return m_isNextMessageReformat; }
            set
            {
                m_isNextMessageReformat = value;
                RaisePropertyChanged("IsNextMessageReformat");
            }
        }
        public bool IsNextMessageDecompress
        {
            get { return m_isNextMessageDecompress; }
            set
            {
                m_isNextMessageDecompress = value;
                RaisePropertyChanged("IsNextMessageDecompress");
            }
        }
        public bool IsAfterProcessGetNextMessage
        {
            get { return m_isAfterProcessGetNextMessage; }
            set
            {
                m_isAfterProcessGetNextMessage = value;
                RaisePropertyChanged("IsAfterProcessGetNextMessage");
            }
        }

        public bool IsCompress
        {
            get { return m_isCompress; }
            set
            {
                m_isCompress = value;
                RaisePropertyChanged("IsCompress");
            }
        }

        public XDeathHeader DeathHeader
        {
            get { return m_deathHeader; }
            set
            {
                m_deathHeader = value;
                RaisePropertyChanged("DeathHeader");
            }
        }

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public string RoutingKey
        {
            get { return m_routingKey; }
            set
            {
                m_routingKey = value;
                RaisePropertyChanged("RoutingKey");
            }
        }

        public BasicGetResult Result
        {
            get { return m_result; }
            set
            {
                m_result = value;
                RaisePropertyChanged("Result");
            }
        }


        public string Message
        {
            get { return m_message; }
            set
            {
                m_message = value;
                RaisePropertyChanged("Message");
            }
        }

        public DispatcherObject View { get; set; }
    }
}
