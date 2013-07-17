using Telerik.Windows.Controls;

namespace routes.editor
{
    public class DropIndicationDetails : ViewModelBase
    {
        private object m_currentDraggedItem;
        private DropPosition m_currentDropPosition;
        private object m_currentDraggedOverItem;

        public object CurrentDraggedOverItem
        {
            get
            {
                return m_currentDraggedOverItem;
            }
            set
            {
                if (this.m_currentDraggedOverItem != value)
                {
                    m_currentDraggedOverItem = value;
                    OnPropertyChanged("CurrentDraggedOverItem");
                }
            }
        }

        public int DropIndex { get; set; }

        public DropPosition CurrentDropPosition
        {
            get
            {
                return this.m_currentDropPosition;
            }
            set
            {
                if (this.m_currentDropPosition != value)
                {
                    this.m_currentDropPosition = value;
                    OnPropertyChanged("CurrentDropPosition");
                }
            }
        }

        public object CurrentDraggedItem
        {
            get
            {
                return this.m_currentDraggedItem;
            }
            set
            {
                if (this.m_currentDraggedItem != value)
                {
                    this.m_currentDraggedItem = value;
                    OnPropertyChanged("CurrentDraggedItem");
                }
            }
        }
    }
}