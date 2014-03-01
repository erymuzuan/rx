namespace Bespoke.Station.Windows.RabbitMqDeadLetter.Models
{
    public class Exchange :ModelBase
    {
        private string m_name;
        private string m_type;
        private string m_virtualHost;

        public override string ToString()
        {
            return string.Format("{1}:{0}({2})", this.Name, this.VirtualHost, this.Type);
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

        public string Type
        {
            get { return m_type; }
            set
            {
                m_type = value;
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