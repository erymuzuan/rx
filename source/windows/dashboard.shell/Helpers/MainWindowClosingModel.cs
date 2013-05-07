using Bespoke.Cycling.Domain;

namespace Bespoke.Station.Windows.Helpers
{
    class MainWindowClosingModel
    {
        private bool m_cancel;
        public bool Cancel
        {
            get { return m_cancel; }
            set
            {
                if (value)
                    m_cancel = true;
            }
        }

        private readonly ObjectCollection<string> m_messsageCollection = new ObjectCollection<string>();

        public ObjectCollection<string> MesssageCollection
        {
            get { return m_messsageCollection; }
        }
    }
}
