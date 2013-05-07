using GalaSoft.MvvmLight;

namespace Bespoke.Cycling.Windows.Infrastructure
{
    public class StatusBarMessage : ViewModelBase
    {
        private string m_text;
        private int m_progress;
        private bool m_isBusy;

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public string Text
        {
            get { return m_text; }
            set
            {
                m_text = value;
                RaisePropertyChanged("Text");
            }
        }


        public int Progress
        {
            get { return m_progress; }
            set
            {
                m_progress = value;
                RaisePropertyChanged("Progress");
            }
        }
    }
}
