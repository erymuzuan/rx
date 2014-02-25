using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Models
{
    public class ApplicationConfigurationViewModel
    {
        public string StateOptions { get; set; }
        public string ApplicationName { get { return ConfigurationManager.ApplicationName; } }
        public string ApplicationFullName { get { return ConfigurationManager.ApplicationFullName; } }
        public string DepartmentOptions { get; set; }
        private readonly ObjectCollection<JsRoute> m_routesCollection = new ObjectCollection<JsRoute>();

        public ObjectCollection<JsRoute> Routes
        {
            get { return m_routesCollection; }
        }

        public string StartModule { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}