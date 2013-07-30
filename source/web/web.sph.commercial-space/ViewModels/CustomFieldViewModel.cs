using System.Collections.Generic;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.ViewModels
{
    public class CustomFieldViewModel
    {
        public CustomFieldViewModel(IEnumerable<CustomField> fields)
        {
            this.CustomFieldCollection.AddRange(fields);
        }
        private readonly ObjectCollection<CustomField> m_customFieldCollection = new ObjectCollection<CustomField>();

        public string RootObject { get; set; }

        public ObjectCollection<CustomField> CustomFieldCollection
        {
            get { return m_customFieldCollection; }
        }
    }
}