using System.ComponentModel;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.Windows.Infrastructure
{
    public class CommandItem<T> : INotifyPropertyChanged
    {
        private ViewGroup m_group;
        private string m_tooltip;
        private string m_caption;
        private string m_image;
        private RelayCommand<T> m_command;
        private T m_commandParameter;
        private string m_subgroup;
        private int m_order;

        public string Subgroup
        {
            get { return m_subgroup; }
            set
            {
                m_subgroup = value;
                RaisePropertyChanged("Subgroup");
            }
        }

        public T CommandParameter
        {
            get { return m_commandParameter; }
            set
            {
                m_commandParameter = value;
                RaisePropertyChanged("CommandParameter");
            }
        }

        public RelayCommand<T> Command
        {
            get { return m_command; }
            set
            {
                m_command = value;
                RaisePropertyChanged("Command");
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

        public string Caption
        {
            get { return m_caption; }
            set
            {
                m_caption = value;
                RaisePropertyChanged("Caption");
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

        public ViewGroup Group
        {
            get { return m_group; }
            set
            {
                m_group = value;
                RaisePropertyChanged("Group");
            }
        }

        public int Order
        {
            get { return m_order; }
            set { m_order = value; 
                RaisePropertyChanged("Order");}
        }

        private void RaisePropertyChanged(string prop)
        {
            if(null != this.PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}