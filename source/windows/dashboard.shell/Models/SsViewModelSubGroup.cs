using Bespoke.Cycling.Domain;

namespace Bespoke.Station.Windows.Models
{
    public class SsViewModelSubGroup
    {
        private readonly ObjectCollection<SsViewModel> m_itemCollection = new ObjectCollection<SsViewModel>();
        public ObjectCollection<SsViewModel> ItemCollection
        {
            get { return m_itemCollection; }
        }
        public string Caption { get; set; }
    }
}