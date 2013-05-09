using Bespoke.Sph.Windows.Infrastructure;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Bespoke.Sph.Windows.Models
{
    public class SsViewModel : ViewModelBase
    {
        public RelayCommand ShowViewCommand { get; set; }
        public RelayCommand<object> CommandVmCommand { get; set; }

        public SsViewModel()
        {
            this.ShowViewCommand = new RelayCommand(ShowView);
        }

        private void ShowView()
        {
            Messenger.Default.Send(new ChangeViewArgs(this.ViewName));
        }

        private string m_viewName;
        private string m_caption;
        private string m_tooltip;
        private string m_image;
        private object m_commandParameter;
        private bool m_isHidden;
        private int m_order;

        public object CommandParameter
        {
            get { return m_commandParameter; }
            set
            {
                m_commandParameter = value;
                RaisePropertyChanged("CommandParameter");
            }
        }

        public string Image
        {
            get { return m_image; }
            set
            {
                m_image = value;
                RaisePropertyChanged("Image");
            }
        }

        public string Tooltip
        {
            get { return m_tooltip; }
            set
            {
                m_tooltip = value;
                RaisePropertyChanged("Tooltip");
            }
        }

        public string Caption
        {
            get { return m_caption; }
            set
            {
                m_caption = value;
                RaisePropertyChanged("Caption");
            }
        }

        public string ViewName
        {
            get { return m_viewName; }
            set
            {
                m_viewName = value;
                RaisePropertyChanged("ViewName");
            }
        }

        public bool IsHidden
        {
            get { return m_isHidden; }
            set { m_isHidden = value; 
            RaisePropertyChanged("IsHidden");}
        }

        public int Order
        {
            get { return m_order; }
            set { m_order = value; 
            RaisePropertyChanged("Order");}
        }
    }
}