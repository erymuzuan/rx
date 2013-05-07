using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Station.Windows.Infrastructure;

namespace Bespoke.Station.Windows.Models
{
    public class SsViewModelGroup
    {
        private readonly ObjectCollection<SsViewModelSubGroup> m_itemCollection = new ObjectCollection<SsViewModelSubGroup>();

        public ObjectCollection<SsViewModelSubGroup> ItemCollection
        {
            get { return m_itemCollection; }
        }

        public ViewGroup Caption { get; set; }
    }
}