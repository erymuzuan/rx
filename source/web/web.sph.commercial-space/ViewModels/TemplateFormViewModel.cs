using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.ViewModels
{
    public class TemplateFormViewModel
    {
        private readonly ObjectCollection<FormElement> m_nameCollection = new ObjectCollection<FormElement>();
        public string Entity { get; set; }
        public ObjectCollection<FormElement> FormElements
        {
            get { return m_nameCollection; }
        }
    }
}