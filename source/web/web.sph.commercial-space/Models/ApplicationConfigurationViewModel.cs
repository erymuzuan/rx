using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Models
{
    public class ApplicationConfigurationViewModel
    {
        private readonly ObjectCollection<JsRoute> m_routesCollection = new ObjectCollection<JsRoute>();

        public ObjectCollection<JsRoute> Routes
        {
            get { return m_routesCollection; }
        }

        public string StartModule { get; set; }
    }
}