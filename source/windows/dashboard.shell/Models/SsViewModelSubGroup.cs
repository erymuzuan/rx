using System.Collections.ObjectModel;

namespace Bespoke.Sph.Windows.Models
{
    public class SsViewModelSubGroup
    {
        private readonly ObservableCollection<SsViewModel> m_itemCollection = new ObservableCollection<SsViewModel>();
        public ObservableCollection<SsViewModel> ItemCollection
        {
            get { return m_itemCollection; }
        }
        public string Caption { get; set; }
    }
}