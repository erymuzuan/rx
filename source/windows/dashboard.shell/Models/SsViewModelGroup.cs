using Bespoke.SphCommercialSpaces.Domain;
using Bespoke.Sph.Windows.Infrastructure;

namespace Bespoke.Sph.Windows.Models
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